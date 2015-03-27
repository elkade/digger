using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Digger
{
    interface IActor
    {
        void LoadContent(ContentManager content);
        void UnloadContent();

        void Think(float seconds);
        void Update(float seconds);

        void Draw(SpriteBatch spriteBatch);

        void Touched(IActor by);

        Vector2 Position { get; }
        Bounding Bounding { get; }
        Model Model { get; }
    
    }

    internal class Bounding
    {
    }
    internal class Bounding<T> : Bounding
    {
    }
}
/*
 void Touched(IActor by)
{
    if(by is Ball)
         ((Ball)by).BounceOff(this.BoundingBox);
    if(by is Snake)
         ((Snake)by).Kill();
}
 */