using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace HardPong;
public class SelectArrow
{
    //Triangulo de seleccion
    private readonly Texture2D _textureImage;
    private readonly SoundEffect _soundArrow;
    private Vector2 _position;

    public SelectArrow(Texture2D textureImage, Vector2 position, SoundEffect soundTriangle)
    {
        _textureImage = textureImage;
        _position = position;
        _soundArrow = soundTriangle;
    }

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public void SetPosition(float x, float y)
    {
        _position.X = x;
        _position.Y = y;
    }

    public void PlaySelectionSound()
    {
        _soundArrow.Play();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_textureImage, _position, Color.White);
    }
}