using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong;
public class Sprite {
    // Stuff needed to draw the sprite
    protected readonly Texture2D TextureImage; //Sprite or sprite sheet of image being drawn
    private readonly Point _frameSize; //Size of each individual frame in sprite sheet
    private readonly Point _currentFrame; //Index of current frame in sprite sheet
    private Point _sheetSize; //Number of columns/rows in sprite sheet
    private float _rotation;

    // Collision data
    private readonly int _collisionOffset; //Offset used to modify frame-size rectangle for collision checks against this sprite

    // Movement data
    protected Vector2 Speed; //Speed at which sprite will move in both X and Y directions
    protected Vector2 Position; //Position at which to draw sprite

    private float _scale = 1f;
    private Color _spriteColor;

    private readonly ISpriteAutomaticMovement _movement;

    public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
        int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
        ISpriteAutomaticMovement movement)
    {
        _movement = movement ?? throw new ArgumentNullException(nameof(movement));
        TextureImage = textureImage;
        Position = position;
        _frameSize = frameSize;
        _collisionOffset = collisionOffset;
        _currentFrame = currentFrame;
        _sheetSize = sheetSize;
        Speed = speed;
        SetColor(Color.White);
        SetRotation(0);
    }

    public virtual Vector2 Direction
    {
        get => Speed;
        set => Speed = value;
    }

    public virtual Vector2 SpritePosition {
        get => Position;
        set => Position = value;
    }

    public void MoveSprite(Vector2 move)
    {
        Position += move;
    }

    public Point SpriteFrameSize => _frameSize;

    public void SetColor(Color spriteColor)
    {
        _spriteColor = spriteColor;
    }

    public virtual void Update(GameTime gameTime, Rectangle clientBounds)
    {
        _movement.AutomaticMovement(this, clientBounds);
    }

    public virtual void Update()
    {
        _movement.AutomaticMovement(this);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // Draw the sprite
        spriteBatch.Draw(TextureImage, Position,
                         new Rectangle(_currentFrame.X * _frameSize.X,
                                       _currentFrame.Y * _frameSize.Y,
                                       _frameSize.X, _frameSize.Y
                                       ),
                                       _spriteColor,
                                       _rotation,
                                       Vector2.Zero,
                                       _scale,
                                       SpriteEffects.None,
                                       0);
    }

    // Gets the collision rect based on position, framesize and collision offset
    public Rectangle CollisionRect =>
        new(
            (int) Position.X + _collisionOffset,
            (int) Position.Y + _collisionOffset,
            _frameSize.X - (_collisionOffset * 2),
            _frameSize.Y - (_collisionOffset * 2));

    protected void SetScale(float s){
        if (s <= 0) return;

        _scale = s;
    }

    public void SetPosition(float x, float y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public void SetRotation(float rot) {
        _rotation = rot;
    }

    public float PositionX
    {
        set => Position.X = value;
        get => Position.X;
    }
    public float PositionY
    {
        set => Position.Y = value;
        get => Position.Y;
    }

}
