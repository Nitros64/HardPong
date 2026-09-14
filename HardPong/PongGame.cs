using Microsoft.Xna.Framework;

namespace HardPong;

public class PongGame : Game, IScreenNavigation
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly MainMenuScreen _mainMenu;
    private readonly GameplayScreen _gameplay;
    private DrawableGameComponent _activeScreen;

    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Position = new Point(500, 200);// posiciona la ventana en la pantalla
        _mainMenu = new MainMenuScreen(this, this);
        _gameplay = new GameplayScreen(this, this);
    }

    protected override void Initialize()
    {
        ShowMainMenu();
        base.Initialize();
    }

    protected override void Update(GameTime gameTime) => base.Update(gameTime);

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }

    public void ShowMainMenu() => SwitchScreen(_mainMenu);

    public void StartGame() => SwitchScreen(_gameplay);

    public void QuitGame() => Exit();

    private void SwitchScreen(DrawableGameComponent screen)
    {
        if (_activeScreen == screen)
            return;

        if (_activeScreen != null)
            Components.Remove(_activeScreen);

        _activeScreen = screen;
        // MonoGame llama a Initialize al añadir una pantalla durante el juego.
        // GameplayScreen mantiene ahí el reinicio de la partida en cada entrada.
        Components.Add(screen);
    }
}
