using Microsoft.Xna.Framework;

namespace HardPong;

public interface ISpriteAutomaticMovement
{
    void AutomaticMovement(Sprite s);
    void AutomaticMovement(Sprite s, Rectangle clientBounds);
}