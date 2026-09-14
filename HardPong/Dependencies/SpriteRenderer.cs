using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong.Dependencies;

// Dibuja una textura en una posicion con un color fijo.
// Las entidades fisicas no conocen texturas; este es el puente de render.
internal class SpriteRenderer
{
    private readonly Texture2D _texture;
    private readonly Color _color;

    public SpriteRenderer(Texture2D texture, Color color)
    {
        _texture = texture;
        _color = color;
    }

    public void Draw(Vector2 position, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, position, _color);
    }
}
