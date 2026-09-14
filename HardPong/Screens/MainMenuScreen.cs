using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace HardPong;

public class MainMenuScreen : GameScreen {
    //SpriteBatch for drawing
    private SpriteBatch _spriteBatch;
    private readonly IScreenNavigation _navigation;
                
    //Bolitas mudas
    private readonly List <Sprite> _randomBalls;

    //Fuentes para las letras
    private SpriteFont _nesFont, _nesFont2;
    private const string Title = "HARD PONG";
    private readonly string[] _options = { "PLAYER VS PLAYER",
                                 "PLAYER VS PC",
                                 "CREDITS",
                                 "EXIT" };

    private const string ProgrammedBy = "PROGRAMED BY: NITROS64";
    private readonly MenuSimple _menuSimple;
    //Musica
    private Song _music;

    //Efectos Especiales
    private readonly ScaleChanger _scaleChanger;
    private readonly ColorChanger _colorChanger;

    public MainMenuScreen(Game game, IScreenNavigation navigation) : base(game)
    {
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        _randomBalls = new List<Sprite>();
        Vector2 menuPosition = new Vector2(game.Window.ClientBounds.Width / 2 - 110, 
                                           game.Window.ClientBounds.Height / 2 + 50);
            
        _menuSimple = new MenuSimple(menuPosition, _options);
        _menuSimple.InputManager.AddTriggerKeys(Keys.Enter);
        _scaleChanger = new ScaleChanger(10, 1.0f, 1.01f);
        _colorChanger = new ColorChanger(10, Color.White, Color.Yellow);
    }

    protected override void OnEnter()
    {
        _menuSimple.InputManager.Exit();
        _menuSimple.Initialize();
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3f;
        MediaPlayer.Play(_music);
    }

    protected override void OnLeave()
    {
        _menuSimple.InputManager.Exit();
        MediaPlayer.Stop();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _nesFont = Game.Content.Load<SpriteFont>(@"Font/NESfont");
        _nesFont2 = Game.Content.Load<SpriteFont>(@"Font/NESfont2");

        //Load Musica
        _music = Game.Content.Load<Song>(@"Audio/dinothunder");

        // Las bolas decorativas y sus recursos se conservan entre entradas.
        Random random = new Random(); // generador de números aleatorios
        for (int cont = 0; cont < 30; ++cont) {
            int randomlocationX = random.Next(50, 590);
            int randomlocationY = random.Next(50, 590);
            int randomSpeedX = random.Next(4, 12);
            int randomSpeedY = random.Next(2, 12);

            Sprite addSprite = new Sprite(
                                Game.Content.Load<Texture2D>(@"Images/circulo2"),
                                new Vector2(randomlocationX, randomlocationY),
                                new Point(Ball.BallWidth, Ball.BallHeight),
                                0,
                                new Point(0, 0),
                                new Point(0, 0),
                                new Vector2(randomSpeedX, randomSpeedY),
                                new BallSimple());
            addSprite.SetColor(Color.Yellow);
            _randomBalls.Add(addSprite);
            
        } // fin de for
        base.LoadContent();
        _menuSimple.LoadContent(Game.Content.Load<SpriteFont>(@"Font/NESfont2"),
                               Game.Content.Load<Texture2D>(@"Images/triangulo"),
                               Game.Content.Load<SoundEffect>(@"Audio/laser-shoot"));
    }
        
    protected override void UnloadContent()
    {
        _spriteBatch?.Dispose();
        _spriteBatch = null;
        _randomBalls.Clear();
    }

    public override void Update(GameTime gameTime)
    {
        HandleMenuInput();
        if (!IsActive)
            return;
        _scaleChanger.Advance();
        _colorChanger.Advance();
        foreach (Sprite s in _randomBalls)
            s.Update(gameTime, Game.Window.ClientBounds);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();            
        // Draw all sprites
        foreach (Sprite s in _randomBalls)//Draw the balls
            s.Draw(gameTime, _spriteBatch);

        _spriteBatch.DrawString(_nesFont, Title, new Vector2(12, 150),
                                _colorChanger.Current, 0,
                                Vector2.Zero,
                                _scaleChanger.Current,
                                SpriteEffects.None, 0);

        _menuSimple.Draw(gameTime, _spriteBatch);

        _spriteBatch.DrawString(_nesFont2, ProgrammedBy,
            new Vector2(10, Game.Window.ClientBounds.Height - 30),
            Color.White);

        _spriteBatch.End();
    }

    private void HandleMenuInput()
    {
        switch (_menuSimple.Update()) {
            case 1:
                _navigation.StartGame();
                break;
            case 2:
                break;
            case 3:
                break;
            case 0:
            case 4:
                _navigation.QuitGame();
                break;
        }
    }

}//fin de la clase MainMenu
