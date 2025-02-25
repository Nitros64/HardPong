using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using static HardPong.SpriteClass.Ball;

namespace HardPong.Dependencies;
class CollisionBallWall : ISpriteCollisionEnvironment
{
    public void SpriteCollisionEnviroment(Sprite s1, Rectangle r2)
    {
        Ball mainBall = (Ball) s1;
        Rectangle rectball = mainBall.CollisionRect;

        if(rectball.X + rectball.Width >= r2.Width)
        {//Pierde el jugador izquierdo
            mainBall.PositionX = r2.Width - rectball.Width;
            mainBall.InvertDirectionHorizontal();//Invertir direccion Horizontal
            mainBall.Winner = PlayerNumber.Player1;
            mainBall.ScorePlaySoundEffects();
        }
        else if (rectball.X <= 0)
        {//Pierde el jugador derecho
            mainBall.PositionX = 0;
            mainBall.InvertDirectionHorizontal();
            mainBall.Winner = PlayerNumber.Player2;
            mainBall.ScorePlaySoundEffects();
        }
        if (rectball.Y + rectball.Height >= r2.Height)
        {
            mainBall.PositionY = r2.Height - mainBall.SpriteFrameSize.Y;
            mainBall.InvertDirectionVertical();
            mainBall.SoundWall();
        }
        else if (rectball.Y<= 0)
        {
            mainBall.PositionY = 0;
            mainBall.InvertDirectionVertical();
            mainBall.SoundWall();
        }            
    }
}