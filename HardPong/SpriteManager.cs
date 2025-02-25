using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using HardPong.Interfaces;
using HardPong.Dependencies;
using HardPong.SpriteClass;
using static HardPong.GameEnum;
using static HardPong.SpriteClass.Ball;

namespace HardPong;

public class SpriteManager : DrawableGameComponent {     
           
    private int _player1PositionX, _player1PositionY, _player2PositionX, _player2PositionY;        
    private int _ballPositionX, _ballPositionY;
 
    private readonly GameStateController _gameState;
    
    //Menu Simple

    //Input Manager
    private readonly KeyInputManager _inputManager;
                
    //SpriteBatch for drawing
    private SpriteBatch _spriteBatch;

    //A sprite for the player and a list of automated sprites
    private Paddle _player, _player2;
    private Ball _ball;

    //Fuentes para las letras
    private SpriteFont _fontScore;
    private SpriteFont _greatScore;
    private const string ScoreString = "SCORE:";
    private int _score1, _score2;

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

    //Collision Detector
    private readonly CollisionDetector _collisionManager;

    //Interfaces
    private ISpriteCollision _spriteCollision;
    private readonly IKeyboardInput _keyboardInput;

    public SpriteManager(Game game) : base(game)
    {
        _gameState = new GameStateController();
       
        var gameEngine = (Game1) game;
        _inputManager = new KeyInputManager();
        _inputManager.AddTriggerKeys(Keys.Enter, Keys.Escape);

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
        _collisionManager = new CollisionDetector(new CollisionBallPaddle(), 
                                                 new CollisionBallWall(), 
                                                 new CollisionPaddleWall());

        _keyboardInput = new SpriteManagerInput(this,gameEngine);
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
        _ball.Winner = Ball.PlayerNumber.NoOne;
        ResetScore();
    }

    public void ResetPosition()//ball and players
    { 
        _player.SetPosition(_player1PositionX, _player1PositionY);
        _player2.SetPosition(_player2PositionX, _player2PositionY);
        _ball.SetPosition(_ballPositionX, _ballPositionY);
        _ball.SetSpeed(Ball.BallSpeedX, Ball.BallSpeedY);
    }

    private void ResetScore()
    {
        _score1 = _score2 = PlayerWinner = 0;
    }
    public static void End()
    {
        MediaPlayer.Stop();
    }    

    public void reset_soundEffects() {
        _ball.ScoreStopSoundEffects();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        LoadContentSprites(); //Load Sprites
        LoadContentFont();  //Load Font
        LoadContentMusic(); //Load Musica       
        BuildCentralPaddles();//Build Central Lines

        EscapeMenu.LoadContent(Game.Content.Load<SpriteFont>(@"Font\NESfont2"),
                               Game.Content.Load<Texture2D>(@"Images\triangulo"),
                               Game.Content.Load<SoundEffect>(@"Audio\laser-shoot"));
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
                 new Vector2(Paddle.BrickSpeedX, Paddle.BrickSpeedY));//speed
        
        _player.PlayerNumber = 1;
        _player.SetInputMovementDependency(new PaddleInputMovement(_player,Keys.W, Keys.S));
        
        //Load the player2 sprite y asignando valores
        _player2 = new Paddle(Game.Content.Load<Texture2D>(@"Images/rect"),
                  new Vector2(_player2PositionX, _player2PositionY), //start position
                  new Point(Paddle.BrickWidth, Paddle.BrickHeight), //brick width and height
                  0, //Colisionador
                  new Point(0, 0),
                  new Point(0, 0), 
                  new Vector2(Paddle.BrickSpeedX, Paddle.BrickSpeedY)); //speed
        
        _player2.PlayerNumber = 2;
        _player2.SetInputMovementDependency(new PaddleInputMovement(_player2,Keys.Up, Keys.Down));

        _ball = new Ball(
                Game.Content.Load<Texture2D>(@"Images/circulo"),
                new Vector2(_ballPositionX, _ballPositionY), //Position X,Y
                new Point(Ball.BallWidth, Ball.BallHeight), //ball width and height
                0, //Colisionador
                new Point(0, 0),
                new Point(0, 0),
                new Vector2(Ball.BallSpeedX, Ball.BallSpeedY));            

        _ball.SetColor(Color.Red);
        //La variable bola cargara sus sonidos correspondientes
        _ball.SoundDependencies(
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio\paddleSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio\wallSound")),
            new BallSound(Game.Content.Load<SoundEffect>(@"Audio\cheer")) 
            );
        _ball.SoloMovementDependency(new BallMain());
    }
    
    private void LoadContentFont()
    {
        //Load Fuente de Letras
        _fontScore = Game.Content.Load<SpriteFont>(@"Font\NESfont2");
        _score1 = 0;
        _score2 = 0;
        _greatScore = Game.Content.Load<SpriteFont>(@"Font\NESfont");
    }  

    private void LoadContentMusic() {
        //Load Musica
        _music = Game.Content.Load<Song>(@"Audio\inspace");            
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
        _keyboardInput.CheckKeyboardInput();
        if (_gameState.GetGameState() == GameStates.Playing)// If the user hasn't paused, Update normally
        {
            //Update movement
            _ball.Update();            
            _player.Update();
            _player2.Update();
            
            //Sprites Collisions
            _collisionManager.collision_paddle_wall(_player, Game.Window.ClientBounds);
            _collisionManager.collision_paddle_wall(_player2, Game.Window.ClientBounds);             
            
            if(_ball.Direction.X > 0 )
                _collisionManager.collision_ball_paddle(_ball,_player2);// Right Paddle (Player 2) 
			else if(_ball.Direction.X < 0)
			    _collisionManager.collision_ball_paddle(_ball,_player);// Left Paddle (Player 1)

            _collisionManager.collision_ball_wall(_ball,Game.Window.ClientBounds);            
            UpdateScores();
        }
    }
    private void UpdateScores() {
        switch (_ball.Winner) {
            case PlayerNumber.Player1:
                ++_score1;
                _gameState.Stop();//enter to stop state
                break;
            case PlayerNumber.Player2:
                ++_score2;
                _gameState.Stop();//enter to stop state
                break;
        }
        if (_score1 == 10)
            PlayerWinner = 1;
        else if (_score2 == 10)
            PlayerWinner = 2;
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);            
        // Draw the player
        _player.Draw(gameTime,_spriteBatch);
        _player2.Draw(gameTime,_spriteBatch);
        // Draw all sprites
        if (GameStates.ExitMenu != _gameState.GetGameState()){
            _ball.Draw(_spriteBatch);
            DrawRectangles();
        }
        else EscapeMenu.Draw(null, _spriteBatch);
            
        DrawScores(_spriteBatch);
        _spriteBatch.End();
    }        
        
    private void DrawScores(SpriteBatch spriteBatch) {
        float winScale = 0.6f; // El score en numeros GRANDES
        float score_scale = 1.0f;

        spriteBatch.DrawString(_fontScore,ScoreString + _score1,new Vector2(100, 10), Color.White);
        spriteBatch.DrawString(_fontScore,ScoreString + _score2,new Vector2(Game.Window.ClientBounds.Width - 200, 10), Color.White);

        if (_gameState.GetGameState() == GameStates.Stop)
        {
            var winner_color = _ccWinnerColor.VisualEffect();
            winScale    = _scWinScale.VisualEffect(); //Cambia la escala cada 10 segundos (0.6f, 0.57f)
            score_scale  = _scScoreScale.VisualEffect(); //Cambia la escala cada 10 segundos(1.0f, 1.2f)

            if (PlayerWinner >= 1) {
                PositionChanger2 winnerPos = new PositionChanger2(new Point(210, 150), new Point(220, 150));                  
                spriteBatch.DrawString(_greatScore, "PLAYER " + PlayerWinner + "\n WINS",
                                       winnerPos.VectorSwitch(winScale == 0.57f), winner_color, 0,
                                       Vector2.Zero, winScale, SpriteEffects.None, 0);
            }
            else{
                DrawScoreCounters(_ball.Winner == PlayerNumber.Player1,""+_score1,
                    _pcScoreCounter1, winner_color, score_scale, 1.2f);
                DrawScoreCounters(_ball.Winner == PlayerNumber.Player2,""+_score2,
                    _pcScoreCounter2,winner_color, score_scale, 1.2f);
            }
        }
        else if (_gameState.GetGameState() == GameStates.Paused)             
            spriteBatch.DrawString(_greatScore, Pause, new Vector2(200, 200), Color.Yellow);            
    }

    void DrawScoreCounters(bool isWinner, string playerScore, PositionChanger2 pos, Color winner_color, 
                           float score_scale, float max_scale) {
        if (isWinner) { 
            _spriteBatch.DrawString(_greatScore,playerScore,pos.VectorSwitch(score_scale == max_scale),
                                   winner_color, 0, Vector2.Zero,score_scale, SpriteEffects.None, 0);
        }
        else
            _spriteBatch.DrawString(_greatScore, playerScore, pos.GetFirst(), Color.White);
    }
    
    //funcion para crear las lineas del medio
    private void DrawRectangles() {            
        for (int i = 0, k = 0; i < 9; ++i, k += 55)
            _spriteBatch.Draw(_whiteRectangle,
                            new Vector2((Game.Window.ClientBounds.Width / 2) - _whiteRectangle.Width / 2, k), 
                            Color.White); 
    }
    

    //GETTING FIENDS - ATRIBUTTES

    public Ball GetBall() { return _ball;}

    public byte PlayerWinner { get; set; }

    public GameStateController getGameStateController => _gameState;

    internal MenuSimple EscapeMenu { get; set; }
}