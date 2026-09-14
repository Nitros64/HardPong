using Microsoft.Xna.Framework;
using static HardPong.GameEnum;

namespace HardPong;

// Resultado de preparar la ronda siguiente.
internal enum MatchEndResult
{
    NextRound,
    MatchWon,
    Ignored
}

// Coordina la partida: estado, marcador, entidades, fisica y audio.
// No dibuja ni lee entrada; expone lo necesario para renderizar y leer acciones.
internal class MatchSession
{
    private readonly GameStateController _gameState = new();
    private readonly MatchScore _score = new();
    private readonly CollisionDetector _collisionManager = new();

    private readonly Ball _ball;
    private readonly Paddle _player1, _player2;
    private readonly PongAudio _audio;

    private readonly Vector2 _player1Spawn, _player2Spawn, _ballSpawn;

    public MatchSession(Ball ball, Paddle player1, Paddle player2, PongAudio audio, Rectangle court)
    {
        _ball = ball;
        _player1 = player1;
        _player2 = player2;
        _audio = audio;

        _player1Spawn = new Vector2(10, court.Height / 2 - Paddle.BrickHeight / 2);
        _player2Spawn = new Vector2(court.Width - 25, court.Height / 2 - Paddle.BrickHeight / 2);
        _ballSpawn = new Vector2(court.Width / 2 - Ball.BallWidth / 2, court.Height / 2 - Ball.BallHeight / 2);
        ResetPositions();
    }

    public GameStates State => _gameState.GetGameState();
    public bool IsMatchOver => _score.MatchWinner != PlayerId.None;
    public MatchScore Score => _score;
    public Ball Ball => _ball;
    public Paddle Player1 => _player1;
    public Paddle Player2 => _player2;

    public void Execute(GameAction action)
    {
        switch (action)
        {
            case GameAction.StartMatch:
                _gameState.Play();
                break;
            case GameAction.Pause:
                _gameState.Pause();
                break;
            case GameAction.Resume:
                _gameState.Resume();
                break;
            case GameAction.OpenExitMenu:
                _gameState.OpenExitMenu();
                break;
            case GameAction.ContinueGame:
                _gameState.ResumeFromExitMenu();
                break;
        }
    }

    public void OpenExitMenu() => _gameState.OpenExitMenu();

    // Prepara la ronda siguiente; SOLO procede desde Stop (la transicion y sus
    // efectos se aceptan o rechazan juntos). Si el partido tenia campeon, lo
    // resetea y lo comunica para que la pantalla dispare su presentacion.
    public MatchEndResult PrepareNextRound()
    {
        if (_gameState.GetGameState() != GameStates.Stop)
            return MatchEndResult.Ignored;

        _gameState.PrepareNextRound();
        if (!IsMatchOver)
        {
            StartNextRound();
            return MatchEndResult.NextRound;
        }

        ResetMatch();
        return MatchEndResult.MatchWon;
    }

    // Simula un frame de juego; SOLO tiene efecto en Playing.
    public void SimulateFrame(Rectangle court)
    {
        if (_gameState.GetGameState() != GameStates.Playing)
            return;

        _ball.Update();
        _player1.Update();
        _player2.Update();

        _collisionManager.ResolvePaddleBoundary(_player1, court);
        _collisionManager.ResolvePaddleBoundary(_player2, court);

        CollisionSound sound;
        if (_ball.Direction.X > 0)
            sound = _collisionManager.ResolveBallPaddle(_ball, _player2);// Right Paddle (Player 2)
        else if (_ball.Direction.X < 0)
            sound = _collisionManager.ResolveBallPaddle(_ball, _player1);// Left Paddle (Player 1)
        else
            sound = CollisionSound.None;

        BallBoundaryOutcome boundary = _collisionManager.ResolveBallBoundary(_ball, court);
        sound |= boundary.Sound;
        if (boundary.Scorer != PlayerId.None)
            _score.AwardPoint(boundary.Scorer);

        if (_score.LastPointWinner != PlayerId.None)
            _gameState.EndRound();

        PlaySounds(sound);
    }

    public void ResetMatch()
    {
        _gameState.Ready();
        _score.Reset();
        ResetPositions();
        _audio.StopScore();
    }

    private void StartNextRound()
    {
        _score.StartNextRound();
        ResetPositions();
        _audio.StopScore();
    }

    private void ResetPositions()
    {
        _player1.SetPosition(_player1Spawn.X, _player1Spawn.Y);
        _player2.SetPosition(_player2Spawn.X, _player2Spawn.Y);
        _ball.SetPosition(_ballSpawn.X, _ballSpawn.Y);
        _ball.SetSpeed(Ball.BallSpeedX, Ball.BallSpeedY);
    }

    private void PlaySounds(CollisionSound sound)
    {
        // Orden historico de audio: pala, puntuacion, muro
        if (sound.HasFlag(CollisionSound.Paddle))
            _audio.PlayPaddle();
        if (sound.HasFlag(CollisionSound.Score))
            _audio.PlayScore();
        if (sound.HasFlag(CollisionSound.Wall))
            _audio.PlayWall();
    }
}
