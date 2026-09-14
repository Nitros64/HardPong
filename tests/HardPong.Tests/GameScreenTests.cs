using Xunit;

namespace HardPong.Tests;

public class GameScreenTests
{
    [Fact]
    public void Initialize_LoadsOnceWithoutEntering()
    {
        using var screen = new ProbeScreen();

        screen.Initialize();
        screen.Initialize();

        Assert.Equal(new[] { "load" }, screen.Events);
        Assert.False(screen.IsActive);
        Assert.False(screen.Enabled);
        Assert.False(screen.Visible);
    }

    [Fact]
    public void Enter_LoadsBeforeActivating_AndDuplicateEntryDoesNotReset()
    {
        using var screen = new ProbeScreen();

        screen.Enter();
        screen.Enter();

        Assert.Equal(new[] { "load", "enter" }, screen.Events);
        Assert.True(screen.IsActive);
        Assert.True(screen.Enabled);
        Assert.True(screen.Visible);
    }

    [Fact]
    public void Leave_DeactivatesWithoutReleasingResources()
    {
        using var screen = new ProbeScreen();
        screen.Enter();
        var resource = screen.Resource;

        screen.Leave();
        screen.Leave();

        Assert.Equal(new[] { "load", "enter", "leave" }, screen.Events);
        Assert.Same(resource, screen.Resource);
        Assert.False(screen.IsActive);
        Assert.False(screen.Enabled);
        Assert.False(screen.Visible);
    }

    [Fact]
    public void RepeatedMenuAndGameplayEntries_ReuseResourcesAndRunEntryEachTime()
    {
        using var menu = new ProbeScreen();
        using var gameplay = new ProbeScreen();
        menu.Initialize();
        gameplay.Initialize();
        var menuResource = menu.Resource;
        var gameplayResource = gameplay.Resource;

        for (int visit = 0; visit < 5; visit++)
        {
            menu.Enter();
            Assert.True(menu.IsActive);
            Assert.False(gameplay.IsActive);
            menu.Leave();
            gameplay.Enter();
            Assert.False(menu.IsActive);
            Assert.True(gameplay.IsActive);
            gameplay.Leave();
        }

        Assert.Same(menuResource, menu.Resource);
        Assert.Same(gameplayResource, gameplay.Resource);
        Assert.Equal(1, menu.Events.Count(e => e == "load"));
        Assert.Equal(1, gameplay.Events.Count(e => e == "load"));
        Assert.Equal(5, menu.Events.Count(e => e == "enter"));
        Assert.Equal(5, gameplay.Events.Count(e => e == "enter"));
        Assert.DoesNotContain("unload", menu.Events);
        Assert.DoesNotContain("unload", gameplay.Events);
    }

    [Fact]
    public void Dispose_ActiveScreen_LeavesBeforeUnloadingExactlyOnce()
    {
        var screen = new ProbeScreen();
        screen.Enter();

        screen.Dispose();
        screen.Dispose();

        Assert.Equal(new[] { "load", "enter", "leave", "unload" }, screen.Events);
        Assert.Null(screen.Resource);
        Assert.False(screen.IsActive);
        Assert.False(screen.Enabled);
        Assert.False(screen.Visible);
        Assert.Throws<ObjectDisposedException>(() => screen.Enter());
        Assert.Throws<ObjectDisposedException>(() => screen.Initialize());
    }

    [Fact]
    public void Dispose_InactiveScreen_UnloadsWithoutLeavingAgain()
    {
        var screen = new ProbeScreen();
        screen.Enter();
        screen.Leave();

        screen.Dispose();

        Assert.Equal(new[] { "load", "enter", "leave", "unload" }, screen.Events);
        Assert.Null(screen.Resource);
    }

    [Fact]
    public void Dispose_NeverOpenedScreen_DoesNotLoadOrEnter()
    {
        var screen = new ProbeScreen();

        screen.Dispose();

        Assert.Equal(new[] { "unload" }, screen.Events);
        Assert.Null(screen.Resource);
    }

    // DrawableGameComponent admite Game nulo; esta pantalla no usa dispositivos.
    private sealed class ProbeScreen() : GameScreen(null)
    {
        public List<string> Events { get; } = [];
        public object Resource { get; private set; }

        protected override void LoadContent()
        {
            Resource = new object();
            Events.Add("load");
        }

        protected override void OnEnter()
        {
            Assert.NotNull(Resource);
            Events.Add("enter");
        }

        protected override void OnLeave() => Events.Add("leave");

        protected override void UnloadContent()
        {
            Resource = null;
            Events.Add("unload");
        }
    }
}
