using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Buffer = MetroDigger.Gameplay.Entities.Terrains.Buffer;

namespace MetroDigger.Gameplay.Entities.Tiles
{
    public class Tile
    {
        public Item Item;

        public Terrain Terrain;

        public Metro Metro;

        public int Clear(ref int stationsCount, out bool isCollision)
        {
            isCollision = true;
            if (Item == null && Accessibility == Accessibility.Free)
                isCollision = false;
            return Clear(ref stationsCount);
        }
        public int Clear(ref int stationsCount)
        {
            int points = 0;
            Item = null;
            if (Metro != null)
                points = Metro.Clear(ref stationsCount);
            if (Accessibility == Accessibility.Free)
                points *= 2;
            else if (Accessibility == Accessibility.Water)
                points *= 3;

            Terrain = new Free();
            return points;
        }
        public virtual int Value
        {
            get { return _value; }
        }

        private readonly int _x;
        private readonly int _y;
        private readonly Terrain _terrain;
        private static ulong _c;

        public ulong Number { get; set; }

        public static Vector2 Size = new Vector2(300*0.3f,300*0.3f);
        public static int Width
        {
            get { return (int)Size.X; }
        }

        public static int Height
        {
            get { return (int) Size.Y; }
        }

        Vector2 _position;
        private const int _value = 0;

        public Tile(int x, int y, Terrain terrain = null, Item item = null, Metro metro = null)
        {
            _x = x;
            _y = y;
            Terrain = terrain ?? new Soil();
            Item = item;
            Metro = metro;
            Metro = null;
            Item = null;
            Position = new Vector2(X*Width+Width/2f, Y*Height+Height/2f);

            Number = _c++;

           
        }

        public Vector2 Position { get; set; }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Terrain != null)
                Terrain.Draw(gameTime, spriteBatch, Position);
            if (Metro != null)
                Metro.Draw(gameTime, spriteBatch, Position);
            if(Item!=null)
                Item.Draw(gameTime, spriteBatch, Position);
        }

        public virtual Accessibility Accessibility
        {
            get
            {
                return Terrain != null ? Terrain.Accessibility : Accessibility.Rock;
            }
        }

        //public static event Action<Tile, Character> Destroyed;

        //public void RaiseDestroyed(Character destroyer)
        //{
        //    if (Destroyed != null)
        //        Destroyed(this, destroyer);
        //}
    }
}
