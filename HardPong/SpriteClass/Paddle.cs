using HardPong.Dependencies;
using HardPong.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HardPong.SpriteClass;

internal class Paddle : Sprite
{
    public const int BrickWidth = 15;
    public const int BrickHeight = 60;
    public const float BrickSpeedX = 5;
    public const float BrickSpeedY = 5;

    // Movement stuff
    private IKeyboardInput _paddleInputMovement;

    public byte PlayerNumber { get; set; }

    public Paddle(Texture2D textureImage, Vector2 position,
        Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
        Vector2 speed)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
        sheetSize, speed){}

    public Paddle(Texture2D textureImage, Vector2 position,
        Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
        Vector2 speed, int millisecondsPerFrame)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
        sheetSize, speed, millisecondsPerFrame){}

    public override void Update()
    {
        _paddleInputMovement.CheckKeyboardInput();
    }

    public void SetInputMovementDependency(IKeyboardInput input) {
        _paddleInputMovement = input;
    }

    public void SetKeys(Keys up, Keys down)
    {
        if (_paddleInputMovement == null) return;
        var pm =  (PaddleInputMovement) _paddleInputMovement;
        pm.SetKeys(up,down);
    }

    public void GetBackToOldPosition() {
        Position = OldPosition;
    }

    public Vector2 OldPosition { get; set; }
}