using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static HardPong.GameEnum;

namespace HardPong;

// Pantalla de juego: construye las entidades, lee acciones y dibuja.
// La logica de la partida vive en MatchSession.
public class GameplayScreen : GameScreen {

    //Menu Simple

    //SpriteBatch for drawing
    private SpriteBatch _spriteBatch;

    //A sprite for the player and a list of automated sprites
    private Paddle _player, _player2;
    private Ball _ball;

    //Partida
    private MatchSession _match;

    //Audio de la partida
    private PongAudio _pongAudio;

    //Render de la pelota y de las palas
    private SpriteRenderer _ballRenderer;
    private SpriteRenderer _paddleRenderer;

    //Fuentes para las letras
    private SpriteFont _fontScore;
    private SpriteFont _greatScore;
    private const string ScoreString = "SCORE:";

    //Exit Menu
    private const string Continue = "CONTINUE";
    private const string MainMenu = "MAIN MENU";
    private const string Exit     = "EXIT GAME";
    private const string Pause    = "PAUSE";

    //Musica
    private Song _music;

    //Central Lines, lineas centrales que dividen la pantalla
    private Texture2D _whiteRectangle;
    private const byte CentralRectangleWidth  = 4;
    private const byte CentralRectangleHeight = 40;

    //Efectos Especiales
    private readonly ScaleChanger _scWinScale;
    private readonly ScaleChanger _scScoreScale;
    private readonly ColorChanger _ccWinnerColor;
    private readonly PositionChanger2 _pcScoreCounter1;
    private readonly PositionChanger2 _pcScoreCounter2;
    private readonly PositionChanger2 _pcWinner;

    //Input
    private readonly GameInputReader _inputReader;
    private readonly IScreenNavigation _navigation;

    public GameplayScreen(Game game, IScreenNavigation navigation) : base(game)
    {
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        _inputReader = new GameInputReader();

        int gameWidth = Game.Window.ClientBounds.Width;
        EscapeMenu = new MenuSimple(new Vector2(gameWidth / 2 - 90, 230),
                    Continue, MainMenu, Exit);

        EscapeMenu.InputManager.AddTriggerKeys(Keys.Escape);

        _scWinScale    = new ScaleChanger(10, 0.6f, 0.57f);
        _scScoreScale  = new ScaleChanger(10, 1.0f, 1.2f);
        _ccWinnerColor = new ColorChanger(10, Color.White, Color.Yellow);
        _pcScoreCounter1 =
            new PositionChanger2(new Point((gameWidth / 2) - 200, 40),
                                 new Point((gameWidth / 2) - 205, 45));
        _pcScoreCounter2 =
            new PositionChanger2(new Point(gameWidth / 2 + 120, 40),
                                 new Point(gameWidth / 2 + 115, 45));
        _pcWinner = new PositionChanger2(new Point(210, 150), new Point(220, 150));
    }

    protected override void OnEnter()
    {
        _inputReader.ResetGracePeriod();
        EscapeMenu.InputManager.Exit();
        RestartMatch();
    }

    protected override void OnLeave()
    {
        _inputReader.ResetGracePeriod();
        EscapeMenu.InputManager.Exit();
        _pongAudio.StopAll();
        MediaPlayer.Stop();
    }

    // Reinicia la partida y su presentacion (musica y menu de pausa)
    private void RestartMatch()
    {
        _match.ResetMatch();
        MediaPlayer.Stop();
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3f;
        MediaPlayer.Play(_music);
        EscapeMenu.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        LoadContentSprites(); //Load Sprites
        LoadContentFont();  //Load Font
        LoadContentMusic(); //Load Musica
        BuildCentralPaddles();//Build Central Lines

        EscapeMenu.LoadContent(Game.Content.Load<SpriteFont>(@"Font/NESfont2"),
                               Game.Content.Load<Texture2D>(@"Images/triangulo"),
                               Game.Content.Load<SoundEffect>(@"Audio/laser-shoot"));
    }

    protected override void UnloadContent() => ReleaseOwnResources();

    private void ReleaseOwnResources()
    {
        // Solo recursos creados a mano; lo cargado via ContentManager lo libera el framework.
        _spriteBatch?.Dispose();
        _spriteBatch = null;
        _whiteRectangle?.Dispose();
        _whiteRectangle = null;
        _pongAudio?.Dispose();
        _pongAudio = null;
        _ballRenderer = null;
        _paddleRenderer = null;
        _match = null;
    }

    private void LoadContentSprites() {
        //Load the player sprite y asignando valores
        _player = new Paddle(new Vector2(10, Game.Window.ClientBounds.Height / 2 - 30),
                 new PaddleInputMovement(new KeyboardReader(), Keys.W, Keys.S));

        //Load the player2 sprite y asignando valores
        _player2 = new Paddle(new Vector2(Game.Window.ClientBounds.Width - 25, Game.Window.ClientBounds.Height / 2 - 30),
                  new PaddleInputMovement(new KeyboardReader(), Keys.Up, Keys.Down));

        _ball = new Ball(Game.Window.ClientBounds);

        //El audio de la partida vive en PongAudio, no en la pelota
        _pongAudio = new PongAudio(
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/paddleSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/wallSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/cheer")));

        //El render usa las texturas; la fisica no
        _paddleRenderer = new SpriteRenderer(Game.Content.Load<Texture2D>(@"Images/rect"), Color.White);
        _ballRenderer = new SpriteRenderer(Game.Content.Load<Texture2D>(@"Images/circulo"), Color.Red);

        //La pantalla construye; la sesion coordina
        _match = new MatchSession(_ball, _player, _player2, _pongAudio, Game.Window.ClientBounds);
    }

    private void LoadContentFont()
    {
        //Load Fuente de Letras
        _fontScore = Game.Content.Load<SpriteFont>(@"Font/NESfont2");
        _greatScore = Game.Content.Load<SpriteFont>(@"Font/NESfont");
    }

    private void LoadContentMusic() {
        //Load Musica
        _music = Game.Content.Load<Song>(@"Audio/inspace");
    }

    private void BuildCentralPaddles()
    {
         //Central Lines
        _whiteRectangle = new Texture2D(GraphicsDevice, CentralRectangleWidth, CentralRectangleHeight);

        Color[] data = new Color[CentralRectangleWidth * CentralRectangleHeight];
        for (int i = 0; i < data.Length; ++i)
                data[i] = Color.Red;

        _whiteRectangle.SetData<Color>(data);
    }

    public override void Update(GameTime gameTime)
    {
        Execute(_inputReader.ReadAction(_match.State, () => EscapeMenu.Update()));
        if (!IsActive)
            return;
        if (_match.State == GameStates.Stop)
        {
            // Los efectos de fin de partida solo se muestran en Stop: avanzan solo alli
            _scWinScale.Advance();
            _scScoreScale.Advance();
            _ccWinnerColor.Advance();
        }
        if (_match.State == GameStates.Playing)// If the user hasn't paused, Update normally
            _match.SimulateFrame(Game.Window.ClientBounds);
    }

    private void Execute(GameAction action)
    {
        switch (action)
        {
            case GameAction.OpenExitMenu:
                _match.OpenExitMenu();
                _inputReader.ResetGracePeriod();
                break;
            case GameAction.PrepareNextRound:
                if (_match.PrepareNextRound() == MatchEndResult.MatchWon)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(_music);
                    EscapeMenu.Initialize();
                }
                break;
            case GameAction.ContinueGame:
                _match.Execute(action);
                EscapeMenu.InputManager.Exit();
                break;
            case GameAction.ReturnToMainMenu:
                _navigation.ShowMainMenu();
                break;
            case GameAction.QuitGame:
                _navigation.QuitGame();
                break;
            default:
                _match.Execute(action); // StartMatch, Pause, Resume
                break;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
        // Draw the players
        _paddleRenderer.Draw(_match.Player1.Position, _spriteBatch);
        _paddleRenderer.Draw(_match.Player2.Position, _spriteBatch);
        // Draw all sprites
        if (GameStates.ExitMenu != _match.State){
            _ballRenderer.Draw(_match.Ball.Position, _spriteBatch);
            DrawRectangles();
        }
        else EscapeMenu.Draw(null, _spriteBatch);

        DrawScores(_spriteBatch);
        _spriteBatch.End();
    }

    private void DrawScores(SpriteBatch spriteBatch) {
        float winScale = 0.6f; // El score en numeros GRANDES
        float score_scale = 1.0f;

        spriteBatch.DrawString(_fontScore,ScoreString + _match.Score.Player1,new Vector2(100, 10), Color.White);
        spriteBatch.DrawString(_fontScore,ScoreString + _match.Score.Player2,new Vector2(Game.Window.ClientBounds.Width - 200, 10), Color.White);

        if (_match.State == GameStates.Stop)
        {
            Color winner_color = _ccWinnerColor.Current;
            winScale    = _scWinScale.Current;  //Alterna cada 10 frames (0.6f, 0.57f)
            score_scale = _scScoreScale.Current; //Alterna cada 10 frames (1.0f, 1.2f)

            if (_match.Score.MatchWinner != PlayerId.None) {
                string winner = _match.Score.MatchWinner == PlayerId.Player1 ? "1" : "2";
                spriteBatch.DrawString(_greatScore, "PLAYER " + winner + "\n WINS",
                                       _pcWinner.VectorSwitch(IsNear(winScale, 0.57f)), winner_color, 0,
                                       Vector2.Zero, winScale, SpriteEffects.None, 0);
            }
            else{
                DrawScoreCounters(_match.Score.LastPointWinner == PlayerId.Player1, "" + _match.Score.Player1,
                    _pcScoreCounter1, winner_color, score_scale, 1.2f);
                DrawScoreCounters(_match.Score.LastPointWinner == PlayerId.Player2, "" + _match.Score.Player2,
                    _pcScoreCounter2, winner_color, score_scale, 1.2f);
            }
        }
        else if (_match.State == GameStates.Paused)
            spriteBatch.DrawString(_greatScore, Pause, new Vector2(200, 200), Color.Yellow);
    }

    void DrawScoreCounters(bool isWinner, string playerScore, PositionChanger2 pos, Color winner_color,
                           float score_scale, float max_scale) {
        if (isWinner) {
            _spriteBatch.DrawString(_greatScore,playerScore,pos.VectorSwitch(IsNear(score_scale, max_scale)),
                                   winner_color, 0, Vector2.Zero,score_scale, SpriteEffects.None, 0);
        }
        else
            _spriteBatch.DrawString(_greatScore, playerScore, pos.GetFirst(), Color.White);
    }

    private static bool IsNear(float a, float b) => MathF.Abs(a - b) < 0.0001f;

    //funcion para crear las lineas del medio
    private void DrawRectangles() {
        for (int i = 0, k = 0; i < 9; ++i, k += 55)
            _spriteBatch.Draw(_whiteRectangle,
                            new Vector2((Game.Window.ClientBounds.Width / 2) - _whiteRectangle.Width / 2, k),
                            Color.White);
    }

    //PROPERTIES

    internal MenuSimple EscapeMenu { get; set; }
}
