using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using HardPong.Dependencies;
using HardPong.SpriteClass;
namespace HardPong;

public class MainMenu : DrawableGameComponent {
    //SpriteBatch for drawing
    private SpriteBatch _spriteBatch;
    private readonly Game1 _gameEngine;
                
    //Bolitas mudas
    private readonly List <Sprite> _randomBalls;

    //Fuentes para las letras
    private SpriteFont _nesFont, _nesFont2;
    private const string Tittle = "HARD PONG";
    private readonly string[] _opcions = { "PLAYER VS PLAYER",
                                 "PLAYER VS PC",
                                 "CREDITS",
                                 "EXIT" };

    private const string ProgramedBy = "PROGRAMED BY: NITROS64";
    private readonly MenuSimple _menuSimple;
    //Musica
    private Song _music;

    //Efectos Especiales
    private readonly ScaleChanger _scaleChanger;
    private readonly ColorChanger _colorChanger;

    public MainMenu(Game game) : base(game)
    {
        _gameEngine = (Game1) game;
        _randomBalls = new List<Sprite>();
        Vector2 menuPosition = new Vector2(game.Window.ClientBounds.Width / 2 - 110, 
                                           game.Window.ClientBounds.Height / 2 + 50);
            
        _menuSimple = new MenuSimple(menuPosition, _opcions);
        _menuSimple.InputManager.AddTriggerKeys(Keys.Enter);
        _scaleChanger = new ScaleChanger(10, 1.0f, 1.01f);
        _colorChanger = new ColorChanger(10, Color.White, Color.Yellow);
    }

    public override void Initialize()
    {
        base.Initialize();
        MediaPlayer.Play(_music);
        _menuSimple.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        _nesFont = Game.Content.Load<SpriteFont>(@"Font\NESfont");
        _nesFont2 = Game.Content.Load<SpriteFont>(@"Font\NESfont2");            
            
        //Load Musica
        _music = Game.Content.Load<Song>(@"Audio\dinothunder");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3f;

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
                                new Vector2(randomSpeedX, randomSpeedY));
            addSprite.SoloMovementDependency(new BallSimple(addSprite,Game.Window.ClientBounds ));
            addSprite.SetColor(Color.Yellow);
            _randomBalls.Add(addSprite);
            
        } // fin de for
        base.LoadContent();
        _menuSimple.LoadContent(_gameEngine.Content.Load<SpriteFont>(@"Font\NESfont2"), 
                               _gameEngine.Content.Load<Texture2D>(@"Images\triangulo"),
                               _gameEngine.Content.Load<SoundEffect>(@"Audio\laser-shoot"));
    }
        
    public override void Update(GameTime gameTime)
    {
        checkMainMenuKey(Game.Window.ClientBounds);
        music_loop();
        foreach (Sprite s in _randomBalls)            
            s.Update(gameTime, Game.Window.ClientBounds); 
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();            
        // Draw all sprites
        foreach (Sprite s in _randomBalls)//Draw the balls
            s.Draw(gameTime, _spriteBatch);

        _spriteBatch.DrawString(_nesFont, Tittle, new Vector2(12, 150), 
                                _colorChanger.VisualEffect(), 0,
                                Vector2.Zero, 
                                _scaleChanger.VisualEffect(), 
                                SpriteEffects.None, 0);

        _menuSimple.Draw(gameTime, _spriteBatch);

        _spriteBatch.DrawString(_nesFont2, ProgramedBy,
            new Vector2(10, Game.Window.ClientBounds.Height - 30),
            Color.White);

        _spriteBatch.End();
    }

    private void music_loop() {
        if (MediaPlayer.PlayPosition.Minutes == 1 && MediaPlayer.PlayPosition.Seconds == 17 &&
            MediaPlayer.PlayPosition.Milliseconds >= 195)
            MediaPlayer.Play(_music, new TimeSpan(0, 0, 16));            
    }
                
    public void checkMainMenuKey(Rectangle rect)
    {
        switch (_menuSimple.Update()) {
            case 1:
                Game.Components.Remove(this);// Controlar con exception
                _menuSimple.InputManager.Exit();
                Game.Components.Add(_gameEngine.GetSpriteManager);// Controlar con exception                    
                break;
            case 2:
                break;
            case 3:
                break;
            case 0:
            case 4:
                Game.Exit();
                break;
        }
    }

}//fin de la clase MainMenu