using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Effects;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Manager
{
    /// <summary>
    /// Manager odpowiedzialny za zarządzanie grafikami, czcionkami i dźwiękami używanymi w grze.
    /// Realizuje wzorzec projektowy Singleton
    /// </summary>
    public class MediaManager
    {
        #region Singleton
        /// <summary>
        /// Zwraca instancję obiektu
        /// </summary>
        public static MediaManager Instance { get { return _instance; } }

        private static readonly MediaManager _instance = new MediaManager();
        #endregion

        private MediaManager()
        {
            _soundInfos = new List<SoundInfo>();
            _gameOptions = GameOptions.Instance;
            DrillingPracticles = new List<Texture2D>();
        }
        /// <summary>
        /// Szerokość ekranu
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Wysokość ekranu
        /// </summary>
        public int Height { get; set; }

        private readonly GameOptions _gameOptions ;

        private readonly List<SoundInfo> _soundInfos;
        /// <summary>
        /// Zapisuje dźwięki używane w grze w słowniku.
        /// </summary>
        /// <param name="name">Nazwa dźwiąku</param>
        /// <param name="sound">Obiekt dźwięku</param>
        /// <param name="soundType">typ dźwięku</param>
        public void LoadSound(string name, SoundEffect sound, SoundType soundType)
        {
            if(_soundInfos.Any(si=>si.Name == name))
                throw new ArgumentException("Sound with this name already exists in the container.");
            var soundEffectInstance = sound.CreateInstance();
            soundEffectInstance.IsLooped = soundType==SoundType.Music;
            _soundInfos.Add(new SoundInfo
            {
                Name = name,
                SoundEffectInstance = soundEffectInstance,
                SoundType = soundType
            });
        }
        /// <summary>
        /// Odtwarza dźwięk zapisaany w słowniku
        /// </summary>
        /// <param name="name">Nazwa dźwięku do odtworzenia</param>
        public void PlaySound(string name)
        {
            SoundInfo soundInfo = _soundInfos.SingleOrDefault(si => si.Name == name);
            if(soundInfo==null)
                throw new Exception("There's no sound with this name.");
            SoundEffectInstance player = soundInfo.SoundEffectInstance;
            if (player.State == SoundState.Playing || player.State == SoundState.Paused)
            {
                player.Stop();
                player = soundInfo.SoundEffectInstance;
            }
            if (soundInfo.SoundType == SoundType.Music)
                player.Volume = _gameOptions.IsMusicEnabled ? 1f : 0f;
            else if (soundInfo.SoundType == SoundType.SoundEffect)
                player.Volume = _gameOptions.IsSoundEnabled ? 1f : 0f;
            player.Play();
        }
        /// <summary>
        /// Włącza lub wyłącza dźwiąki o określonym typie
        /// </summary>
        /// <param name="soundType">Typ dźwięków do przełączenia</param>
        public void Switch(SoundType soundType)
        {
            var set = _soundInfos.Where(si => si.SoundType == soundType);
            foreach (var soundInfo in set)
            {
                SoundEffectInstance player = soundInfo.SoundEffectInstance;
                switch (soundInfo.SoundType)
                {
                    case SoundType.Music:
                        player.Volume = _gameOptions.IsMusicEnabled ? 1f : 0f;
                        break;
                    case SoundType.SoundEffect:
                        player.Volume = _gameOptions.IsSoundEnabled ? 1f : 0f;
                        break;
                }
            }
        }

        private class SoundInfo
        {
            public SoundEffectInstance SoundEffectInstance { get; set; }
            public string Name { get; set; }
            public SoundType SoundType { get; set; }
        }
        #region Graphics

        readonly Dictionary<string,Texture2D> _graphicsDictionary = new Dictionary<string, Texture2D>();
        private const float ImageWidth = 300;
        private const float ImageHeight = 300;
        /// <summary>
        /// Zwraca obiekt animacji z jedną klatką
        /// </summary>
        /// <param name="name">nazwa animacji</param>
        /// <param name="h">wysokość animacji</param>
        /// <returns>obiekt animacji</returns>
        public Animation GetStaticAnimation(string name, float h = ImageHeight)
        {
            if (!_graphicsDictionary.ContainsKey(name))
                return null;
            return new Animation(_graphicsDictionary[name], 1, false, (int)h, Scale);
        }
        /// <summary>
        /// Zwraca obiekt animacji z wieloma klatkami
        /// </summary>
        /// <param name="name">nazwa animacji</param>
        /// <returns>obiekt animacji</returns>
        public Animation GetDynamicAnimation(string name)
        {
            if (!_graphicsDictionary.ContainsKey(name))
                return null;
            return new Animation(_graphicsDictionary[name], 1, true, (int)ImageHeight, Scale);
        }
        /// <summary>
        /// Zapisuje teksturę do słownika
        /// </summary>
        /// <param name="name">nazwa tekstury</param>
        /// <param name="texture">Obiekt tekstury</param>
        public void LoadGraphics(string name, Texture2D texture)
        {
            //if (!_graphicsDictionary.ContainsKey(name)) throw new Exception("Such name already exists.");
            _graphicsDictionary[name] = texture;
        }

        //public Texture2D Free { get; set; }
        //public Texture2D Rock { get; set; }
        //public Texture2D Soil { get; set; }
        //public Texture2D PlayerIdle { get; set; }
        public List<Texture2D> DrillingPracticles { get; set; }
        //public Texture2D MetroStation { get; set; }
        //public Texture2D MetroTunnel { get; set; }

        //public Texture2D PowerCell { get; set; }
        //public Texture2D Drill { get; set; }

        //public Texture2D[] RedBullet;

        public SpriteFont Font { get; set; }
        public SpriteFont TopBarFont { get; set; }
        //public Texture2D PlayerWithDrill { get; set; }
        //public Texture2D Miner { get; set; }
        //public Texture2D Ranger { get; set; }
        //public Texture2D Stone { get; set; }
        public Vector2 Scale { get; set; }
        //public Texture2D Water { get; set; }

        #endregion
        /// <summary>
        /// Dopasowuje wymiary wyświatlanych elementów do wielkości okna
        /// </summary>
        /// <param name="width">szerokośc okna</param>
        /// <param name="height">wysokość okna</param>
        public void SetDimensions(int width, int height)
        {
            double h = (float)Height / height;
            double w = (float)Width / width;
            float min = (float)Math.Min(h, w);
            Tile.Size = new Vector2(min, min);
            Scale = new Vector2(min / ImageWidth, min / ImageHeight);
        }
    }
    /// <summary>
    /// Typ dźwięku
    /// </summary>
    public enum SoundType
    {
        SoundEffect,
        Music
    }
}
