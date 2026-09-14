using Microsoft.Xna.Framework;

namespace HardPong;

class BallSimple : ISpriteAutomaticMovement
{
    public BallSimple()
    {
    }

    public void AutomaticMovement(Sprite s)
    {
       s.SpritePosition += s.Direction;
    }

    public void AutomaticMovement(Sprite s, Rectangle clientBounds)
    {
        // Move sprite based on direction
        s.SetPosition(s.SpritePosition.X + s.Direction.X, s.SpritePosition.Y + s.Direction.Y);
        if (s.SpritePosition.X >= clientBounds.Width - s.SpriteFrameSize.X)
        {
            s.SpritePosition = new Vector2(clientBounds.Width - s.SpriteFrameSize.X, s.SpritePosition.Y);
            s.Direction = new Vector2(-s.Direction.X, s.Direction.Y); //speed.X *= -1
        }
        else if (s.SpritePosition.X <= 0)
        {
            s.SpritePosition = new Vector2(0, s.SpritePosition.Y);
            s.Direction = new Vector2(-s.Direction.X, s.Direction.Y); //speed.X *= -1
        }

        if (s.SpritePosition.Y >= clientBounds.Height - s.SpriteFrameSize.Y)
        {
            s.SpritePosition = new Vector2(s.SpritePosition.X, clientBounds.Height - s.SpriteFrameSize.Y);
            s.Direction = new Vector2(s.Direction.X, -s.Direction.Y);//speed.Y *= -1;
        }
        else if (s.SpritePosition.Y <= 0)
        {
            s.SpritePosition = new Vector2(s.SpritePosition.X, 0);
            s.Direction = new Vector2(s.Direction.X, -s.Direction.Y);//speed.Y *= -1;
        }
    }

}
