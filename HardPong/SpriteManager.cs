using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using HardPong.Interfaces;
using HardPong.Dependencies;
using HardPong.SpriteClass;
using static HardPong.GameEnum;

namespace HardPong;

public class SpriteManager : DrawableGameComponent {     
           
    private int _player1PositionX, _player1PositionY, _player2PositionX, _player2PositionY;        
    private int _ballPositionX, _ballPositionY;
 
    private readonly GameStateController _gameState;
    
    //Menu Simple

    //SpriteBatch for drawing
    private SpriteBatch _spriteBatch;

    //A sprite for the player and a list of automated sprites
    private Paddle _player, _player2;
    private Ball _ball;

    //Audio de la partida
    private PongAudio _pongAudio;

    //Fuentes para las letras
    private SpriteFont _fontScore;
    private SpriteFont _greatScore;
    private const string ScoreString = "SCORE:";

    //Puntuacion y reglas de victoria
    private readonly MatchScore _matchScore = new();

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

    //Collision Detector
    private readonly CollisionDetector _collisionManager;

    //Interfaces
    private readonly GameInputReader _inputReader;
    private readonly Game1 _gameEngine;

    public SpriteManager(Game game) : base(game)
    {
        _gameState = new GameStateController();
       
        var gameEngine = (Game1) game;
        _gameEngine = gameEngine;
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
        _collisionManager = new CollisionDetector();

        _inputReader = new GameInputReader();
    }

    public override void Initialize()
    {
        base.Initialize();
        Begin();
    }

    public void Begin()
    {
        reset_primitives();                        
        ResetPosition();
        reset_soundEffects();
        MediaPlayer.Stop();
        MediaPlayer.Play(_music);
        EscapeMenu.Initialize();
    }

    private void reset_primitives() {
        _gameState.Ready();
        _matchScore.Reset();
    }

    public void ResetPosition()//ball and players
    { 
        _player.SetPosition(_player1PositionX, _player1PositionY);
        _player2.SetPosition(_player2PositionX, _player2PositionY);
        _ball.SetPosition(_ballPositionX, _ballPositionY);
        _ball.SetSpeed(Ball.BallSpeedX, Ball.BallSpeedY);
    }

    public static void End()
    {
        MediaPlayer.Stop();
    }    

    public void reset_soundEffects() {
        _pongAudio.StopScore();
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
    protected override void UnloadContent()
    {
        // Solo recursos creados a mano; lo cargado via ContentManager lo libera el framework.
        _whiteRectangle?.Dispose();
        _pongAudio?.Dispose();
    }

    private void LoadContentSprites() {
        _player1PositionX = 10;
        _player1PositionY = Game.Window.ClientBounds.Height / 2 - 30;

        _player2PositionX = Game.Window.ClientBounds.Width - 25;
        _player2PositionY = Game.Window.ClientBounds.Height / 2 - 30;

        _ballPositionX = Game.Window.ClientBounds.Width / 2 - Ball.BallWidth / 2;
        _ballPositionY = Game.Window.ClientBounds.Height / 2 - Ball.BallHeight / 2;        

        //Load the player sprite y asignando valores
        _player = new Paddle(Game.Content.Load<Texture2D>(@"Images/rect"),
                 new Vector2(_player1PositionX, _player1PositionY), //start position
                 new Point(Paddle.BrickWidth, Paddle.BrickHeight), //brick width and height
                 0, //Colisionador
                 new Point(0, 0),
                 new Point(0, 0),
                 new Vector2(Paddle.BrickSpeedX, Paddle.BrickSpeedY),//speed
                 new PaddleInputMovement(new KeyboardReader(), Keys.W, Keys.S));

        _player.PlayerNumber = 1;
        
        //Load the player2 sprite y asignando valores
        _player2 = new Paddle(Game.Content.Load<Texture2D>(@"Images/rect"),
                  new Vector2(_player2PositionX, _player2PositionY), //start position
                  new Point(Paddle.BrickWidth, Paddle.BrickHeight), //brick width and height
                  0, //Colisionador
                  new Point(0, 0),
                  new Point(0, 0),
                  new Vector2(Paddle.BrickSpeedX, Paddle.BrickSpeedY), //speed
                  new PaddleInputMovement(new KeyboardReader(), Keys.Up, Keys.Down));

        _player2.PlayerNumber = 2;

        _ball = new Ball(
                Game.Content.Load<Texture2D>(@"Images/circulo"),
                new Vector2(_ballPositionX, _ballPositionY), //Position X,Y
                new Point(Ball.BallWidth, Ball.BallHeight), //ball width and height
                0, //Colisionador
                new Point(0, 0),
                new Point(0, 0),
                new Vector2(Ball.BallSpeedX, Ball.BallSpeedY));            

        _ball.SetColor(Color.Red);
        //El audio de la partida vive en PongAudio, no en la pelota
        _pongAudio = new PongAudio(
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/paddleSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/wallSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio/cheer")));
        _ball.SoloMovementDependency(new BallMain());
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
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3f;
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
        Execute(_inputReader.ReadAction(_gameState.GetGameState(), () => EscapeMenu.Update()));
        if (_gameState.GetGameState() == GameStates.Stop)
        {
            // Los efectos de fin de partida solo se muestran en Stop: avanzan solo alli
            _scWinScale.Advance();
            _scScoreScale.Advance();
            _ccWinnerColor.Advance();
        }
        if (_gameState.GetGameState() == GameStates.Playing)// If the user hasn't paused, Update normally
        {
            //Update movement
            _ball.Update();            
            _player.Update();
            _player2.Update();
            
            //Sprites Collisions
            ResolveCollisions();
            UpdateScores();
        }
    }

    private void Execute(GameAction action)
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
            case GameAction.PrepareNextRound:
                _gameState.PrepareNextRound();
                if (IsMatchOver)
                    Begin();
                else{
                    reset_soundEffects();
                    ResetPosition();
                    StartNextRound();
                }
                break;
            case GameAction.OpenExitMenu:
                _gameState.OpenExitMenu();
                _inputReader.ResetGracePeriod();
                break;
            case GameAction.ContinueGame:
                _gameState.ResumeFromExitMenu();
                EscapeMenu.InputManager.Exit();
                break;
            case GameAction.ReturnToMainMenu:
                Game.Components.Remove(this);
                Game.Components.Add(_gameEngine.GetMenuPong);
                EscapeMenu.InputManager.Exit();
                _inputReader.ResetGracePeriod();
                break;
            case GameAction.QuitGame:
                Game.Exit();
                break;
        }
    }

    private void ResolveCollisions()
    {
        _collisionManager.ResolvePaddleBoundary(_player, Game.Window.ClientBounds);
        _collisionManager.ResolvePaddleBoundary(_player2, Game.Window.ClientBounds);

        CollisionSound sound;
        if (_ball.Direction.X > 0)
            sound = _collisionManager.ResolveBallPaddle(_ball, _player2);// Right Paddle (Player 2)
        else if (_ball.Direction.X < 0)
            sound = _collisionManager.ResolveBallPaddle(_ball, _player);// Left Paddle (Player 1)
        else
            sound = CollisionSound.None;

        BallBoundaryOutcome boundary = _collisionManager.ResolveBallBoundary(_ball, Game.Window.ClientBounds);
        sound |= boundary.Sound;
        if (boundary.Scorer != PlayerId.None)
            _matchScore.AwardPoint(boundary.Scorer);

        // Orden historico de audio: pala, puntuacion, muro
        if (sound.HasFlag(CollisionSound.Paddle))
            _pongAudio.PlayPaddle();
        if (sound.HasFlag(CollisionSound.Score))
            _pongAudio.PlayScore();
        if (sound.HasFlag(CollisionSound.Wall))
            _pongAudio.PlayWall();
    }
    private void UpdateScores() {
        if (_matchScore.LastPointWinner != PlayerId.None)
            _gameState.EndRound();//enter to stop state
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);            
        // Draw the player
        _player.Draw(gameTime,_spriteBatch);
        _player2.Draw(gameTime,_spriteBatch);
        // Draw all sprites
        if (GameStates.ExitMenu != _gameState.GetGameState()){
            _ball.Draw(gameTime,_spriteBatch);
            DrawRectangles();
        }
        else EscapeMenu.Draw(null, _spriteBatch);
            
        DrawScores(_spriteBatch);
        _spriteBatch.End();
    }        
        
    private void DrawScores(SpriteBatch spriteBatch) {
        float winScale = 0.6f; // El score en numeros GRANDES
        float score_scale = 1.0f;

        spriteBatch.DrawString(_fontScore,ScoreString + _matchScore.Player1,new Vector2(100, 10), Color.White);
        spriteBatch.DrawString(_fontScore,ScoreString + _matchScore.Player2,new Vector2(Game.Window.ClientBounds.Width - 200, 10), Color.White);

        if (_gameState.GetGameState() == GameStates.Stop)
        {
            Color winner_color = _ccWinnerColor.Current;
            winScale    = _scWinScale.Current;  //Alterna cada 10 frames (0.6f, 0.57f)
            score_scale = _scScoreScale.Current; //Alterna cada 10 frames (1.0f, 1.2f)

            if (_matchScore.MatchWinner != PlayerId.None) {
                string winner = _matchScore.MatchWinner == PlayerId.Player1 ? "1" : "2";
                spriteBatch.DrawString(_greatScore, "PLAYER " + winner + "\n WINS",
                                       _pcWinner.VectorSwitch(IsNear(winScale, 0.57f)), winner_color, 0,
                                       Vector2.Zero, winScale, SpriteEffects.None, 0);
            }
            else{
                DrawScoreCounters(_matchScore.LastPointWinner == PlayerId.Player1, "" + _matchScore.Player1,
                    _pcScoreCounter1, winner_color, score_scale, 1.2f);
                DrawScoreCounters(_matchScore.LastPointWinner == PlayerId.Player2, "" + _matchScore.Player2,
                    _pcScoreCounter2, winner_color, score_scale, 1.2f);
            }
        }
        else if (_gameState.GetGameState() == GameStates.Paused)             
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

    public bool IsMatchOver => _matchScore.MatchWinner != PlayerId.None;

    public void StartNextRound() => _matchScore.StartNextRound();

    public GameStateController getGameStateController => _gameState;

    internal MenuSimple EscapeMenu { get; set; }
}