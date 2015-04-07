using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Manager
{
    public class GraphicResourceContainer
    {
        #region Singleton
        public static GraphicResourceContainer Instance { get { return _instance; } }

        private static readonly GraphicResourceContainer _instance = new GraphicResourceContainer();
        #endregion
        private GraphicResourceContainer()
        {
            DrillingPracticles = new List<Texture2D>();
            RedBullet = new Texture2D[2];
        }
        public Texture2D Free { get; set; }
        public Texture2D Rock { get; set; }
        public Texture2D Soil { get; set; }
        public Texture2D PlayerIdle { get; set; }
        public List<Texture2D> DrillingPracticles { get; set; }
        public Texture2D MetroStation { get; set; }
        public Texture2D MetroTunnel { get; set; }

        public Texture2D PowerCell { get; set; }
        public Texture2D Drill { get; set; }

        public Texture2D[] RedBullet;

        public SpriteFont Font { get; set; }
        public Texture2D PlayerWithDrill { get; set; }
        public Texture2D Miner { get; set; }
        public Texture2D Ranger { get; set; }
    }
}
