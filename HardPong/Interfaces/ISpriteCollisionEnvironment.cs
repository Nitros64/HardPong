using HardPong.SpriteClass;
using Microsoft.Xna.Framework;

namespace HardPong.Interfaces;

internal interface ISpriteCollisionEnvironment
{
	void SpriteCollisionEnvironment(Sprite s1, Rectangle r2);
}

