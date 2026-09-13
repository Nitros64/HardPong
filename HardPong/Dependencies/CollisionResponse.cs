using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using static HardPong.SpriteClass.Ball;

namespace HardPong.Dependencies;

// Aplica las respuestas fisicas y sonoras a los contactos detectados.
// No detecta nada por si mismo: solo responde a hechos.
internal class CollisionResponse
{
    public void ResolveBallPaddle(Ball ball, Paddle paddle, in BallPaddleContact contact)
    {
        Rectangle rectBall = contact.BallRect;
        Rectangle rectPaddle = contact.PaddleRect;
        float angle = contact.Angle;
        float m = contact.Slope;

        ball.Direction = new Vector2((angle / 10), m);

        if (angle >= 70)
        {
            ball.ChangeDirection();
            ball.SoundBrick();
        }
        else
        {
            ball.InvertDirectionHorizontal();//invierte el desplzamiento horizontal
            ball.SoundBrick();
        }

        if (rectBall.Y + rectBall.Height >= rectPaddle.Y && rectBall.Y + rectBall.Height < rectPaddle.Y + 10) // 7 es el original
            ball.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y - rectBall.Height);
        else if (rectBall.Y < rectPaddle.Y + rectPaddle.Height && rectBall.Y >= rectPaddle.Y + rectPaddle.Height - 10)
            ball.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y + rectPaddle.Height);

        // si la pelota ha tocado los bordes el brick no bajara o subira
        //(Garantiza la no penetracion de la bola a traves de los ladrillos)
        if (BallBorderCollision(rectBall, rectPaddle))
            paddle.GetBackToOldPosition();
    }

    private static bool BallBorderCollision(Rectangle ball, Rectangle paddle)
    {
        if (paddle.Y + paddle.Height >= ball.Y && ball.Y > paddle.Y + paddle.Height / 2)//bajando
            return true;
        else if (paddle.Y <= ball.Y + ball.Height && paddle.Y > ball.Y) //subiendo
            return true;

        return false;
    }

    public void ResolveBallBoundary(Ball ball, in BallBoundaryContact contact, Rectangle bounds)
    {
        Rectangle rectball = ball.CollisionRect;

        if (contact.OutRight)
        {//Pierde el jugador izquierdo
            ball.PositionX = bounds.Width - rectball.Width;
            ball.InvertDirectionHorizontal();//Invertir direccion Horizontal
            ball.Winner = PlayerNumber.Player1;
            ball.ScorePlaySoundEffects();
        }
        else if (contact.OutLeft)
        {//Pierde el jugador derecho
            ball.PositionX = 0;
            ball.InvertDirectionHorizontal();
            ball.Winner = PlayerNumber.Player2;
            ball.ScorePlaySoundEffects();
        }
        if (contact.OutBottom)
        {
            ball.PositionY = bounds.Height - ball.SpriteFrameSize.Y;
            ball.InvertDirectionVertical();
            ball.SoundWall();
        }
        else if (contact.OutTop)
        {
            ball.PositionY = 0;
            ball.InvertDirectionVertical();
            ball.SoundWall();
        }
    }

    public void ResolvePaddleBoundary(Paddle paddle, Rectangle bounds)
    {
        if (paddle.PositionY <= 0)
            paddle.PositionY = 0;
        if (paddle.PositionY + paddle.SpriteFrameSize.Y >= bounds.Height)
            paddle.PositionY = bounds.Height - paddle.SpriteFrameSize.Y;
    }
}
