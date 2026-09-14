using Microsoft.Xna.Framework;

namespace HardPong;

public class PongGame : Game, IScreenNavigation
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly MainMenuScreen _mainMenu;
    private readonly GameplayScreen _gameplay;
    private GameScreen _activeScreen;
    private bool _screensDisposed;

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
        base.Initialize();
        ShowMainMenu();
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

    private void SwitchScreen(GameScreen screen)
    {
        if (_activeScreen == screen)
            return;

        if (_activeScreen != null)
        {
            _activeScreen.Leave();
            Components.Remove(_activeScreen);
        }

        _activeScreen = screen;
        Components.Add(screen);
        screen.Enter();
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing && !_screensDisposed)
            {
                _screensDisposed = true;
                // PongGame es dueño de ambas pantallas, incluidas las retiradas.
                // Las retiramos antes de liberar para evitar un segundo Dispose
                // desde Game y conservar disponibles el dispositivo y el contenido.
                Components.Remove(_mainMenu);
                Components.Remove(_gameplay);
                _activeScreen = null;
                try
                {
                    _mainMenu?.Dispose();
                }
                finally
                {
                    _gameplay?.Dispose();
                }
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
