using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong.SpriteClass;
public class SelectArrow : Sprite
{
    //Triangulo de seleccion
    private readonly SoundEffect _soundArrow;
   
    public SelectArrow() {
    }
    public SelectArrow(Texture2D textureImage, Vector2 position, SoundEffect soundTriangle)
        : base(textureImage, position, new Point(0, 0), 0, new Point(0, 0),
                new Point(0, 0), new Vector2(0, 0))
    {
        this._soundArrow = soundTriangle;
    }

    public override void Update(GameTime gameTime, Rectangle clientBounds)
    {
        // Method intentionally left empty.
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureImage, Position, Color.White);
    }

    public override Vector2 Direction
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override Vector2 SpritePosition
    {
        get => Position;
        set{
            _soundArrow.Play();
            Position = value;
        }
    }
}