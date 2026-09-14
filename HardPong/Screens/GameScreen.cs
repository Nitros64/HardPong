using System;
using Microsoft.Xna.Framework;

namespace HardPong;

// El ciclo de MonoGame carga/libera recursos; Enter y Leave activan la pantalla.
public abstract class GameScreen : DrawableGameComponent
{
    private bool _disposed;

    protected GameScreen(Game game) : base(game)
    {
        Enabled = false;
        Visible = false;
    }

    public bool IsActive { get; private set; }

    public sealed override void Initialize()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        base.Initialize(); // DrawableGameComponent carga el contenido una sola vez.
    }

    public void Enter()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (IsActive)
            return;

        Initialize();
        OnEnter();
        IsActive = true;
        Enabled = true;
        Visible = true;
    }

    public void Leave()
    {
        if (!IsActive)
            return;

        IsActive = false;
        Enabled = false;
        Visible = false;
        OnLeave();
    }

    protected abstract void OnEnter();
    protected abstract void OnLeave();

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;
        try
        {
            if (disposing)
                Leave();
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
