using HardPong.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong.SpriteClass;
public class Ball : Sprite {
    public const float BallSpeedX = 5;
    public const float BallSpeedY = 4;
    public const int BallWidth = 12;
    public const int BallHeight = 12;

    private const float MyScale = 1f;
    public enum PlayerNumber {NoOne = 0, Player1, Player2 };

    private ISoundEffect _soundBrick;
    private ISoundEffect _soundWall;
    private ISoundEffect _soundScore;

    public Ball(Texture2D textureImage, Vector2 position, Point frameSize,
        int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
        sheetSize, speed)
    {
        this.SetScale(MyScale);
        Winner = PlayerNumber.NoOne;
    }

    public Ball(Texture2D textureImage, Vector2 position, Point frameSize,
        int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
        int millisecondsPerFrame)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
        sheetSize, speed, millisecondsPerFrame)
    {
        this.SetScale(MyScale);
        Winner = PlayerNumber.NoOne;
    }

    public void SoundDependencies(ISoundEffect soundBrick1, ISoundEffect soundWall1, 
                                  ISoundEffect soundScore) {
        _soundBrick = soundBrick1;
        _soundWall = soundWall1;
        _soundScore = soundScore;
    }

    //Sprite is automated. Direction is same as speed
    public override Vector2 Direction
    {
        set {
            if (Speed.X < 0)
            {
                if (value.X < BallSpeedX)
                    Speed.X = -BallSpeedX;
                else
                    Speed.X = -value.X;
            }
            else if (value.X < BallSpeedX)
                Speed.X = BallSpeedX;
            else
                Speed.X = value.X;

            if (value.Y > 7)
                value.Y = 7;

            if (value.Y == 0 || value.Y < 1)
                value.Y = 3f;

            if (Speed.Y < 0)
            {
                switch (value.Y)
                {
                    case < 0:
                        Speed.Y = value.Y;
                        break;
                    case > 0:
                        value.Y *= -1;
                        Speed.Y = value.Y;
                        break;
                }
            }
            else                
                Speed.Y = value.Y;                
        }
    }

    public void SetSpeed(float x, float y) {
        if (Speed.X < 0)
        {
            Speed.X = x < BallSpeedX ? BallSpeedX : x;
            Speed.X *= -1;
        }
        else
        {
            Speed.X = x < BallSpeedX ? BallSpeedX : x;
        }

        if (y > 7)
            y = 7;
       
        if (y is 0 or < 1) 
            y = 0.5f;

        if (Speed.Y < 0)
        {
            switch (y)
            {
                case < 0:
                    Speed.Y = y;
                    break;
                case > 0:
                    y *= -1;
                    Speed.Y = y;
                    break;
            }
        }
        else
            Speed.Y = y;
        
    }

    public void DefaultDirection() {
        Speed.X = BallSpeedX;
        Speed.Y = BallSpeedY;
    }

    public void ChangeDirection() {
        InvertDirectionHorizontal();
        InvertDirectionVertical();
    }

    public void InvertDirectionVertical(){
        Speed.Y *= -1;
    }

    public void InvertDirectionHorizontal() {
        Speed.X *= -1;
    }

    public PlayerNumber Winner { get; set; }

    public void SoundWall()
    {
        _soundWall.PlaySoundEffect();
    }

    public void SoundBrick() {
        _soundBrick.PlaySoundEffect();
    }

    public void ScorePlaySoundEffects() {
        _soundScore.PlaySoundEffect();
    }

    public void ScoreStopSoundEffects()
    {
        _soundScore.StopSoundEffect();
    }
}