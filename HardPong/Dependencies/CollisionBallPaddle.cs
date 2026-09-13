using HardPong.Interfaces;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Dependencies;

internal class CollisionBallPaddle : IBallPaddleCollision
{
	public void Resolve(Ball ball, Paddle paddle)
	{
		Rectangle rectBall = ball.CollisionRect;
		Rectangle rectPaddle = paddle.CollisionRect;

		if (rectBall.Intersects(rectPaddle))
		{
			float m;
			float angle = (float)
				MathHelper.Angle(rectBall.X + rectBall.Width / 2, rectBall.Y + rectBall.Height / 2,
							rectPaddle.X + rectPaddle.Width / 2, rectPaddle.Y + rectPaddle.Height / 2,
							out m);

			ball.Direction = new Vector2((angle / 10), m);

			if (angle >= 70){
				ball.ChangeDirection();
				ball.SoundBrick();
			}
			else{
				ball.InvertDirectionHorizontal();//invierte el desplzamiento horizontal
				ball.SoundBrick();
			}

			if (rectBall.Y + rectBall.Width >= rectPaddle.Y && rectBall.Y + rectBall.Width < rectPaddle.Y + 10) // 7 es el original
				ball.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y - rectBall.Width);
			else if (rectBall.Y < rectPaddle.Y + rectPaddle.Height && rectBall.Y >= rectPaddle.Y + rectPaddle.Height - 10)
				ball.SpritePosition = new Vector2(rectBall.X, rectPaddle.Y + rectPaddle.Height);


			// si la pelota ha tocado los bordes el brick no bajara o subira
			//(Garantiza la no penetracion de la bola a traves de los ladrillos)
            if(BallBorderCollision(rectBall, rectPaddle))
                paddle.GetBackToOldPosition();
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