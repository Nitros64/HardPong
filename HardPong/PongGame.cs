using Microsoft.Xna.Framework;

namespace HardPong;

public class PongGame : Game
{
    private readonly GraphicsDeviceManager _graphics;

    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Position = new Point(500, 200);// posiciona la ventana en la pantalla
        GetMenuPong = new MainMenuScreen(this);
        GetSpriteManager = new GameplayScreen(this);
    }

    protected override void Initialize()
    {
        Components.Add(GetMenuPong);
        base.Initialize();
    }

    protected override void Update(GameTime gameTime) => base.Update(gameTime);

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    public MainMenuScreen GetMenuPong { get; }
    public GameplayScreen GetSpriteManager { get; }
}
