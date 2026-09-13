using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using static HardPong.SpriteClass.Ball;

namespace HardPong.Dependencies;
internal class CollisionBallWall : IBallBoundaryCollision
{
    public void Resolve(Ball ball, Rectangle bounds)
    {
        Rectangle rectball = ball.CollisionRect;

        if(rectball.X + rectball.Width >= bounds.Width)
        {//Pierde el jugador izquierdo
            ball.PositionX = bounds.Width - rectball.Width;
            ball.InvertDirectionHorizontal();//Invertir direccion Horizontal
            ball.Winner = PlayerNumber.Player1;
            ball.ScorePlaySoundEffects();
        }
        else if (rectball.X <= 0)
        {//Pierde el jugador derecho
            ball.PositionX = 0;
            ball.InvertDirectionHorizontal();
            ball.Winner = PlayerNumber.Player2;
            ball.ScorePlaySoundEffects();
        }
        if (rectball.Y + rectball.Height >= bounds.Height)
        {
            ball.PositionY = bounds.Height - ball.SpriteFrameSize.Y;
            ball.InvertDirectionVertical();
            ball.SoundWall();
        }
        else if (rectball.Y<= 0)
        {
            ball.PositionY = 0;
            ball.InvertDirectionVertical();
            ball.SoundWall();
        }
    }
}