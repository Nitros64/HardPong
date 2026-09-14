using Microsoft.Xna.Framework;
using Xunit;

namespace HardPong.Tests;

// La pelota es fisica pura: se construye sin texturas ni sonido.
public class BallTests
{
    private static readonly Rectangle Court = new(0, 0, 800, 480);
    private static readonly Vector2 Spawn = new(800 / 2 - Ball.BallWidth / 2, 480 / 2 - Ball.BallHeight / 2);

    private static Ball NewBall() => new(Court);

    [Fact]
    public void NewBall_StartsAtCourtCenterMovingRightAndDown()
    {
        var ball = NewBall();

        Assert.Equal(Spawn, ball.Position);
        Assert.Equal(Ball.BallSpeedX, ball.Direction.X);
        Assert.Equal(Ball.BallSpeedY, ball.Direction.Y);
    }

    [Fact]
    public void CollisionRect_MatchesSizeAndPosition()
    {
        var ball = NewBall();

        Assert.Equal(new Rectangle((int)Spawn.X, (int)Spawn.Y, Ball.BallWidth, Ball.BallHeight), ball.CollisionRect);
    }

    [Fact]
    public void Update_MovesByCurrentSpeed()
    {
        var ball = NewBall();

        ball.Update();

        Assert.Equal(Spawn + new Vector2(Ball.BallSpeedX, Ball.BallSpeedY), ball.Position);
    }

    [Fact]
    public void InvertDirectionHorizontal_FlipsXOnly()
    {
        var ball = NewBall();

        ball.InvertDirectionHorizontal();

        Assert.Equal(-Ball.BallSpeedX, ball.Direction.X);
        Assert.Equal(Ball.BallSpeedY, ball.Direction.Y);
    }

    [Fact]
    public void ChangeDirection_InvertsBoth()
    {
        var ball = NewBall();

        ball.ChangeDirection();

        Assert.Equal(-Ball.BallSpeedX, ball.Direction.X);
        Assert.Equal(-Ball.BallSpeedY, ball.Direction.Y);
    }

    [Fact]
    public void SetDirection_AppliesMinimumHorizontalMagnitude()
    {
        var ball = NewBall();

        ball.SetDirection(new Vector2(3, 4));

        Assert.Equal(Ball.BallSpeedX, ball.Direction.X);
    }

    [Fact]
    public void SetDirection_AppliesMinimumVerticalSpeed()
    {
        var ball = NewBall();

        ball.SetDirection(new Vector2(Ball.BallSpeedX, 0));

        Assert.Equal(Ball.MinVerticalSpeed, ball.Direction.Y);
    }

    [Fact]
    public void SetDirection_WhileMovingLeft_KeepsMovingLeft()
    {
        var ball = NewBall();
        ball.InvertDirectionHorizontal();

        ball.SetDirection(new Vector2(3, 4));

        Assert.Equal(-Ball.BallSpeedX, ball.Direction.X);
    }

    [Fact]
    public void SetSpeed_PreservesCurrentSigns()
    {
        var ball = NewBall();
        ball.InvertDirectionHorizontal();
        ball.InvertDirectionVertical();

        ball.SetSpeed(Ball.BallSpeedX, Ball.BallSpeedY);

        Assert.Equal(-Ball.BallSpeedX, ball.Direction.X);
        Assert.Equal(-Ball.BallSpeedY, ball.Direction.Y);
    }

    [Fact]
    public void ResetPosition_RestoresSpawnButKeepsServingSigns()
    {
        var ball = NewBall();
        ball.Update();
        ball.InvertDirectionHorizontal();

        ball.ResetPosition();

        Assert.Equal(Spawn, ball.Position);
        Assert.Equal(-Ball.BallSpeedX, ball.Direction.X);
        Assert.Equal(Ball.BallSpeedY, ball.Direction.Y);
    }

    [Theory]
    [InlineData(-8f, -4f, 5f, 3f)]
    [InlineData(0f, 0f, 5f, 3f)]
    [InlineData(4.99f, 0.99f, 5f, 3f)]
    [InlineData(5f, 1f, 5f, 1f)]
    [InlineData(5.01f, 2f, 5.01f, 2f)]
    [InlineData(8f, 7f, 8f, 7f)]
    [InlineData(8f, 7.01f, 8f, 7f)]
    public void SpeedSetters_ApplyExistingLimitsAndPreserveEveryTravelDirection(
        float x, float y, float expectedX, float expectedY)
    {
        foreach (bool useDirectionSetter in new[] { false, true })
        foreach (int signX in new[] { -1, 1 })
        foreach (int signY in new[] { -1, 1 })
        {
            var ball = NewBall();
            if (signX < 0) ball.InvertDirectionHorizontal();
            if (signY < 0) ball.InvertDirectionVertical();

            if (useDirectionSetter)
                ball.SetDirection(new Vector2(x, y));
            else
                ball.SetSpeed(x, y);

            var expectedVelocity = new Vector2(signX * expectedX, signY * expectedY);
            Assert.Equal(expectedVelocity, ball.Direction);
            ball.Update();
            Assert.Equal(Spawn + expectedVelocity, ball.Position);
        }
    }
}
