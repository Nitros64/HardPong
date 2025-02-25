using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Position = new Point(500, 200);// posiciona la ventana en la pantalla
        GetMenuPong = new MainMenu(this);
        GetSpriteManager = new SpriteManager(this);
    }

    protected override void Initialize()
    {
        Components.Add(GetMenuPong);
        //Components.Add(GetSpriteManager);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    public MainMenu GetMenuPong { get; }

    public SpriteManager GetSpriteManager { get; }
}