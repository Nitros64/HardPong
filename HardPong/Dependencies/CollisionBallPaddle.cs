using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

internal class CollisionBallPaddle : ISpriteCollision
{
	public void SpriteCollision(Sprite s1, Sprite s2)
	{
		Ball _refBall = (Ball) s1;
	    Paddle _refPaddle = (Paddle) s2;	
		Rectangle rectBall = _refBall.CollisionRect;
		Rectangle rectPaddle = _refPaddle.CollisionRect;

		if (rectBall.Intersects(rectPaddle))
		{
			float m;
			float angle = (float)
				MathHelper.Angle(rectBall.X + rectBall.Width / 2, rectBall.Y + rectBall.Height / 2,
							rectPaddle.X + rectPaddle.Width / 2, rectPaddle.Y + rectPaddle.Height / 2,
							out m);

			_refBall.Direction = new Vector2((angle / 10), m);

			if (angle >= 70){
				_refBall.ChangeDirection();
				_refBall.SoundBrick();
			}
			else{
				_refBall.InvertDirectionHorizontal();//invierte el desplzamiento horizontal
				_refBall.SoundBrick();
			}

			if (rectBall.Y + rectBall.Width >= rectPaddle.Y && rectBall.Y + rectBall.Width < rectPaddle.Y + 10) // 7 es el original
				s1.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y - rectBall.Width);
			else if (rectBall.Y < rectPaddle.Y + rectPaddle.Height && rectBall.Y >= rectPaddle.Y + rectPaddle.Height - 10)
				s1.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y + rectPaddle.Height);


			// si la pelota ha tocado los bordes el brick no bajara o subira
			//(Garantiza la no penetracion de la bola a traves de los ladrillos)
            if(BallBorderCollision(rectBall, rectPaddle))
                _refPaddle.GetBackToOldPosition();
		}
	}

	private static bool BallBorderCollision(Rectangle ball, Rectangle paddle)
	{        
		if(paddle.Y + paddle.Height >= ball.Y && ball.Y > paddle.Y + paddle.Height/2)//bajando
            return true;       
        else if (paddle.Y <= ball.Y + ball.Height && paddle.Y > ball.Y) //subiendo
            return true;
		
		return false;
	}
}//END of the Class