using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework.Audio;

namespace MetroDigger.Manager
{
    public class SoundManager
    {
        #region Singleton
        public static SoundManager Instance{get { return _instance; }}

        private static readonly SoundManager _instance = new SoundManager();
        #endregion

        private SoundManager()
        {
            _soundInfos = new List<SoundInfo>();
            _gameOptions = GameOptions.Instance;
        }

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

        public void Switch(bool b, SoundType soundType)
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
    }

    public enum SoundType
    {
        SoundEffect,
        Music
    }
}
