using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Manager
{
    public class MediaManager
    {
        #region Singleton
        public static MediaManager Instance { get { return _instance; } }

        private static readonly MediaManager _instance = new MediaManager();
        #endregion

        private MediaManager()
        {
            _soundInfos = new List<SoundInfo>();
            _gameOptions = GameOptions.Instance;
            DrillingPracticles = new List<Texture2D>();
            RedBullet = new Texture2D[2];
        }

        public int Width { get; set; }
        public int Height { get; set; }

        private readonly GameOptions _gameOptions ;

        private readonly List<SoundInfo> _soundInfos;

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

        public void PlaySound(string name)
        {
            SoundInfo soundInfo = _soundInfos.SingleOrDefault(si => si.Name == name);
            if(soundInfo==null)
                throw new Exception("There's no sound with this name.");
            SoundEffectInstance player = soundInfo.SoundEffectInstance;
            //if(player.State==SoundState.Playing || player.State == SoundState.Paused)
            //    player.Stop();
            if (soundInfo.SoundType == SoundType.Music)
                player.Volume = _gameOptions.IsMusicEnabled ? 1f : 0f;
            else if (soundInfo.SoundType == SoundType.SoundEffect)
                player.Volume = _gameOptions.IsSoundEnabled ? 1f : 0f;
            player.Play();
        }

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
        public SpriteFont TopBarFont { get; set; }
        public Texture2D PlayerWithDrill { get; set; }
        public Texture2D Miner { get; set; }
        public Texture2D Ranger { get; set; }
        public Texture2D Stone { get; set; }
        public Vector2 Scale { get; set; }

        #endregion

    }

    public enum SoundType
    {
        SoundEffect,
        Music
    }
}
