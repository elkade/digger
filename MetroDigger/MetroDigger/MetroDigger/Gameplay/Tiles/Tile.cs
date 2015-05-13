using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Tiles
{
    /// <summary>
    /// Reprezentuje kafelek - pojedyncze pole planszy
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Przedmiot przechowywany w kafelku
        /// </summary>
        public Item Item;
        /// <summary>
        /// Teren kafelka
        /// </summary>
        public ITerrain Terrain;
        /// <summary>
        /// Znacznik matra znajdujący się w kafelku
        /// </summary>
        public Metro Metro;
        /// <summary>
        /// Czyści kafelek
        /// </summary>
        /// <param name="stationsCount">liczba stacji metra pozostała do oczyszczenia</param>
        /// <param name="isCollision">określa, czy kafelek jest czyszczony przez obiekt,
        /// który ma z nim fizyczny kontakt, czy na odległość - przez pocisk</param>
        /// <returns>Liczba punktów uzyskana przez oczyszczenie kafelka</returns>
        public int Clear(ref int stationsCount, out bool isCollision)
        {
            isCollision = true;
            if (Item == null && (Accessibility == Accessibility.Free || Accessibility == Accessibility.Water))
                isCollision = false;
            Logger.Log("Tile cleared");

            return Clear(ref stationsCount);

        }
        /// <summary>
        /// Czyści kafelek
        /// </summary>
        /// <param name="stationsCount">liczba stacji metra pozostała do oczyszczenia</param>
        /// <returns>Liczba punktów uzyskana przez oczyszczenie kafelka</returns>
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
        /// <summary>
        /// Rozmiar kafelków
        /// </summary>
        public static Vector2 Size = new Vector2(300*0.3f,300*0.3f);
        /// <summary>
        /// Szerokośc kafelka
        /// </summary>
        public static int Width
        {
            get { return (int)Size.X; }
        }
        /// <summary>
        /// Wysokość Kafelka
        /// </summary>
        public static int Height
        {
            get { return (int) Size.Y; }
        }
        /// <summary>
        /// Tworzy nowy kafelek
        /// </summary>
        /// <param name="x">Indeks X na planszy</param>
        /// <param name="y">Indeks Y na plaszy</param>
        /// <param name="terrain">Teren kafelka</param>
        /// <param name="item">Przdmiot znajdujący się w kafelku</param>
        /// <param name="metro">Znacznik metra znajdujaćy się w kafelku</param>
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
        /// <summary>
        /// Położenie kafelka na ekranie
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// Indeks X kafelka na planszy
        /// </summary>
        public int X
        {
            get { return _x; }
        }
        /// <summary>
        /// Indeks Y kafelka na planszy
        /// </summary>
        public int Y
        {
            get { return _y; }
        }
        /// <summary>
        /// Rysuje na ekranie obiekty należące do kafelka: przedmiot, znacznik metra i teren
        /// </summary>
        /// <param name="gameTime">Aktualny czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
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
        /// <summary>
        /// Określa dostępność kafelka dla obiektów. Toższme z dostępnością terenu kafelka.
        /// </summary>
        public Accessibility Accessibility
        {
            get
            {
                return Terrain != null ? Terrain.Accessibility : Accessibility.Rock;
            }
        }
    }
}
