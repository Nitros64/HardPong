using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using static HardPong.SpriteClass.Ball;

namespace HardPong.Dependencies;

// Aplica las respuestas fisicas y sonoras a los contactos detectados.
// No detecta nada por si mismo: solo responde a hechos.
internal class CollisionResponse
{
    public CollisionSound ResolveBallPaddle(Ball ball, Paddle paddle, in BallPaddleContact contact)
    {
        Rectangle rectBall = contact.BallRect;
        Rectangle rectPaddle = contact.PaddleRect;
        float angle = contact.Angle;
        float m = contact.Slope;

        ball.SetDirection(new Vector2((angle / 10), m));

        if (angle >= 70)
            ball.ChangeDirection();
        else
            ball.InvertDirectionHorizontal();//invierte el desplzamiento horizontal

        if (rectBall.Y + rectBall.Height >= rectPaddle.Y && rectBall.Y + rectBall.Height < rectPaddle.Y + 10) // 7 es el original
            ball.Position = new Vector2(rectBall.X, rectPaddle.Y - rectBall.Height);
        else if (rectBall.Y < rectPaddle.Y + rectPaddle.Height && rectBall.Y >= rectPaddle.Y + rectPaddle.Height - 10)
            ball.Position = new Vector2(rectBall.X, rectPaddle.Y + rectPaddle.Height);

        // si la pelota ha tocado los bordes el brick no bajara o subira
        //(Garantiza la no penetracion de la bola a traves de los ladrillos)
        if (BallBorderCollision(rectBall, rectPaddle))
            paddle.GetBackToOldPosition();

        return CollisionSound.Paddle;
    }

    private static bool BallBorderCollision(Rectangle ball, Rectangle paddle)
    {
        if (paddle.Y + paddle.Height >= ball.Y && ball.Y > paddle.Y + paddle.Height / 2)//bajando
            return true;
        else if (paddle.Y <= ball.Y + ball.Height && paddle.Y > ball.Y) //subiendo
            return true;

        return false;
    }

    public BallBoundaryOutcome ResolveBallBoundary(Ball ball, in BallBoundaryContact contact, Rectangle bounds)
    {
        Rectangle rectball = ball.CollisionRect;
        var sound = CollisionSound.None;
        var scorer = PlayerId.None;

        if (contact.OutRight)
        {//Pierde el jugador izquierdo
            ball.Position = new Vector2(bounds.Width - rectball.Width, ball.Position.Y);
            ball.InvertDirectionHorizontal();//Invertir direccion Horizontal
            scorer = PlayerId.Player1;
            sound |= CollisionSound.Score;
        }
        else if (contact.OutLeft)
        {//Pierde el jugador derecho
            ball.Position = new Vector2(0, ball.Position.Y);
            ball.InvertDirectionHorizontal();
            scorer = PlayerId.Player2;
            sound |= CollisionSound.Score;
        }
        if (contact.OutBottom)
        {
            ball.Position = new Vector2(ball.Position.X, bounds.Height - Ball.BallHeight);
            ball.InvertDirectionVertical();
            sound |= CollisionSound.Wall;
        }
        else if (contact.OutTop)
        {
            ball.Position = new Vector2(ball.Position.X, 0);
            ball.InvertDirectionVertical();
            sound |= CollisionSound.Wall;
        }

        return new BallBoundaryOutcome(sound, scorer);
    }

    public void ResolvePaddleBoundary(Paddle paddle, Rectangle bounds)
    {
        Rectangle rect = paddle.CollisionRect;
        if (rect.Y <= 0)
            paddle.SetPosition(rect.X, 0);
        if (rect.Y + rect.Height >= bounds.Height)
            paddle.SetPosition(rect.X, bounds.Height - Paddle.BrickHeight);
    }
}
