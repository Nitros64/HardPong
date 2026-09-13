using HardPong.Dependencies;
using Microsoft.Xna.Framework;
using Xunit;

namespace HardPong.Tests;

// Geometria pura: nada de sprites, texturas ni sonido.
public class CollisionDetectionTests
{
    private static readonly Rectangle Court = new(0, 0, 800, 480);

    // ---- CollisionBallPaddle ----

    [Fact]
    public void BallPaddle_WhenSeparated_ReturnsNoHit()
    {
        var contact = CollisionBallPaddle.Detect(new Rectangle(100, 100, 12, 12), new Rectangle(700, 200, 15, 60));

        Assert.False(contact.Hit);
    }

    [Fact]
    public void BallPaddle_WhenOverlapping_ReturnsHitWithRects()
    {
        var ballRect = new Rectangle(690, 200, 12, 12);
        var paddleRect = new Rectangle(700, 200, 15, 60);

        var contact = CollisionBallPaddle.Detect(ballRect, paddleRect);

        Assert.True(contact.Hit);
        Assert.Equal(ballRect, contact.BallRect);
        Assert.Equal(paddleRect, contact.PaddleRect);
    }

    [Fact]
    public void BallPaddle_WhenTouchingByEdge_ReturnsNoHit()
    {
        // MonoGame Rectangle.Intersects excluye el contacto exacto por borde
        var contact = CollisionBallPaddle.Detect(new Rectangle(688, 200, 12, 12), new Rectangle(700, 200, 15, 60));

        Assert.False(contact.Hit);
    }

    [Fact]
    public void BallPaddle_WhenTouchingByCorner_ReturnsNoHit()
    {
        var contact = CollisionBallPaddle.Detect(new Rectangle(688, 188, 12, 12), new Rectangle(700, 200, 15, 60));

        Assert.False(contact.Hit);
    }

    [Fact]
    public void BallPaddle_WhenOverlappingByCorner_ReturnsHit()
    {
        var contact = CollisionBallPaddle.Detect(new Rectangle(692, 194, 12, 12), new Rectangle(700, 200, 15, 60));

        Assert.True(contact.Hit);
    }

    [Fact]
    public void BallPaddle_WithAlignedCenters_ReturnsZeroAngleAndSlope()
    {
        var contact = CollisionBallPaddle.Detect(new Rectangle(40, 0, 12, 12), new Rectangle(50, 0, 15, 12));

        Assert.True(contact.Hit);
        Assert.Equal(0f, contact.Angle);
        Assert.Equal(0f, contact.Slope);
    }

    // ---- CollisionBallWall ----

    [Fact]
    public void BallWall_InsideCourt_ReturnsNoViolation()
    {
        var contact = CollisionBallWall.Detect(new Rectangle(400, 200, 12, 12), Court);

        Assert.False(contact.OutLeft);
        Assert.False(contact.OutRight);
        Assert.False(contact.OutTop);
        Assert.False(contact.OutBottom);
    }

    [Fact]
    public void BallWall_AtRightBoundary_ReturnsOutRight()
    {
        var contact = CollisionBallWall.Detect(new Rectangle(788, 200, 12, 12), Court);

        Assert.True(contact.OutRight);
        Assert.False(contact.OutLeft);
    }

    [Fact]
    public void BallWall_AtLeftBoundary_ReturnsOutLeft()
    {
        var contact = CollisionBallWall.Detect(new Rectangle(0, 200, 12, 12), Court);

        Assert.True(contact.OutLeft);
        Assert.False(contact.OutRight);
    }

    [Fact]
    public void BallWall_AtBottomBoundary_ReturnsOutBottom()
    {
        var contact = CollisionBallWall.Detect(new Rectangle(400, 468, 12, 12), Court);

        Assert.True(contact.OutBottom);
        Assert.False(contact.OutTop);
    }

    [Fact]
    public void BallWall_AtTopBoundary_ReturnsOutTop()
    {
        var contact = CollisionBallWall.Detect(new Rectangle(400, 0, 12, 12), Court);

        Assert.True(contact.OutTop);
        Assert.False(contact.OutBottom);
    }

    // ---- CollisionPaddleWall ----

    [Fact]
    public void PaddleWall_InsideCourt_ReturnsFalse()
    {
        Assert.False(CollisionPaddleWall.Detect(new Rectangle(100, 100, 15, 60), Court));
    }

    [Fact]
    public void PaddleWall_TouchingTop_ReturnsTrue()
    {
        Assert.True(CollisionPaddleWall.Detect(new Rectangle(100, 0, 15, 60), Court));
    }

    [Fact]
    public void PaddleWall_TouchingBottom_ReturnsTrue()
    {
        Assert.True(CollisionPaddleWall.Detect(new Rectangle(100, 420, 15, 60), Court));
    }
}
