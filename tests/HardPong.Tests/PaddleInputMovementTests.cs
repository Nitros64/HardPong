using HardPong.Dependencies;
using HardPong.Interfaces;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace HardPong.Tests;

public class PaddleInputMovementTests
{
    // Teclado simulado: solo las teclas indicadas estan pulsadas.
    private sealed class FakeKeyboardReader(params Keys[] pressedKeys) : IKeyboardReader
    {
        private readonly KeyboardState _state = new(pressedKeys);

        public KeyboardState Capture() => _state;
    }

    private static PaddleInputMovement CreateController(FakeKeyboardReader reader)
        => new(reader, Keys.W, Keys.S);

    [Fact]
    public void ReadMovementAxis_WithoutKeys_ReturnsZero()
    {
        var controller = CreateController(new FakeKeyboardReader());

        Assert.Equal(0f, controller.ReadMovementAxis());
    }

    [Fact]
    public void ReadMovementAxis_WithOnlyUpKey_ReturnsNegativeOne()
    {
        var controller = CreateController(new FakeKeyboardReader(Keys.W));

        Assert.Equal(-1f, controller.ReadMovementAxis());
    }

    [Fact]
    public void ReadMovementAxis_WithOnlyDownKey_ReturnsPositiveOne()
    {
        var controller = CreateController(new FakeKeyboardReader(Keys.S));

        Assert.Equal(1f, controller.ReadMovementAxis());
    }

    [Fact]
    public void ReadMovementAxis_WithBothKeys_ReturnsZero()
    {
        var controller = CreateController(new FakeKeyboardReader(Keys.W, Keys.S));

        Assert.Equal(0f, controller.ReadMovementAxis());
    }
}
