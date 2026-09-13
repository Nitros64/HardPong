using System;
using HardPong.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong.SpriteClass;

internal class Paddle : Sprite
{
    public const int BrickWidth = 15;
    public const int BrickHeight = 60;
    public const float BrickSpeedX = 5;
    public const float BrickSpeedY = 5;

    // Movement stuff
    private readonly IPaddleController _controller;

    public byte PlayerNumber { get; set; }

    public Paddle(Texture2D textureImage, Vector2 position,
        Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
        Vector2 speed, IPaddleController controller)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
        sheetSize, speed)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public override void Update()
    {
        float axis = _controller.ReadMovementAxis();
        OldPosition = SpritePosition; // las colisiones utilizan OldPosition
        SpritePosition += new Vector2(0f, axis) * Speed;
    }

    public void GetBackToOldPosition() {
        Position = OldPosition;
    }

    public Vector2 OldPosition { get; set; }
}