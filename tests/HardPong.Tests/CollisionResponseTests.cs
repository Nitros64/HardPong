using Microsoft.Xna.Framework;
using Xunit;

namespace HardPong.Tests;

public class CollisionResponseTests
{
    [Theory]
    [InlineData(101, 124, 5f, 3f, false)]
    [InlineData(101, 114, 9f, 7f, true)]
    [InlineData(101, 134, 9f, 7f, true)]
    [InlineData(90, 124, 5f, 3f, false)]
    [InlineData(90, 113, 5f, 1f, false)]
    [InlineData(100, 120, 7.596376f, 4f, true)]
    public void PaddleImpact_PreservesExpectedReboundForEveryIncomingDirection(
        int ballX, int ballY, float expectedSpeedX, float expectedSpeedY, bool invertVertical)
    {
        foreach (int signX in new[] { -1, 1 })
        foreach (int signY in new[] { -1, 1 })
        {
            var ball = new Ball(new Rectangle(0, 0, 800, 480));
            ball.SetPosition(ballX, ballY);
            if (signX < 0) ball.InvertDirectionHorizontal();
            if (signY < 0) ball.InvertDirectionVertical();
            var paddle = new Paddle(new Vector2(100, 100), new StationaryController());

            var events = new CollisionDetector().ResolveBallPaddle(ball, paddle);

            Assert.Equal(CollisionEvents.PaddleHit, events);
            Assert.Equal(-signX * expectedSpeedX, ball.Direction.X, 4);
            Assert.Equal(signY * (invertVertical ? -1 : 1) * expectedSpeedY, ball.Direction.Y, 4);
            ball.Update();
            Assert.True(float.IsFinite(ball.Position.X));
            Assert.True(float.IsFinite(ball.Position.Y));
        }
    }

    private sealed class StationaryController : IPaddleController
    {
        public float ReadMovementAxis() => 0f;
    }
}
