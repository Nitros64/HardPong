using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Interfaces;

public interface ISpriteAutomaticMovement
{
    void AutomaticMovement(Sprite s);
    void AutomaticMovement(Sprite s, Rectangle clientBounds);
}