using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Tiles
{
    public class Tile
    {
        public Item Item;

        public ITerrain Terrain;

        public Metro Metro;

        public int Clear(ref int stationsCount, out bool isCollision)
        {
            isCollision = true;
            if (Item == null && (Accessibility == Accessibility.Free || Accessibility == Accessibility.Water))
                isCollision = false;
            return Clear(ref stationsCount);
        }

        public int Clear(ref int stationsCount)
        {
            if (Accessibility == Accessibility.Buffer)
                return 0;
            int points = 0;
            Item = null;
            if (Metro != null)
                points = Metro.Clear(ref stationsCount);
            if (Accessibility == Accessibility.Free)
                points *= 2;
            //else if (Accessibility == Accessibility.Water)
            //    points *= 3;
            if (Accessibility != Accessibility.Water && Accessibility != Accessibility.Free)
            Terrain = new Free();
            return points;
        }

        private readonly int _x;
        private readonly int _y;

        public static Vector2 Size = new Vector2(300*0.3f,300*0.3f);
        public static int Width
        {
            get { return (int)Size.X; }
        }

        public static int Height
        {
            get { return (int) Size.Y; }
        }

        public Tile(int x, int y, ITerrain terrain = null, Item item = null, Metro metro = null)
        {
            _x = x;
            _y = y;
            Terrain = terrain ?? new Soil();
            Item = item;
            Metro = metro;
            Metro = null;
            Item = null;
            Position = new Vector2(X*Width+Width/2f, Y*Height+Height/2f);          
        }

        public Vector2 Position { get; private set; }

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
            {
                Terrain.Position = Position;
                Terrain.Draw(gameTime, spriteBatch);
            }
            if (Metro != null)
            {
                Metro.Position = Position;
                Metro.Draw(gameTime, spriteBatch);
            }
            if (Item != null)
            {
                Item.Position = Position;
                Item.Draw(gameTime, spriteBatch);
            }
        }

        public Accessibility Accessibility
        {
            get
            {
                return Terrain != null ? Terrain.Accessibility : Accessibility.Rock;
            }
        }
    }
}
