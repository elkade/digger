using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using MetroDigger.Gameplay;
using MetroDigger.Manager.Settings;
using MetroDigger.Serialization;
using MetroDigger.Utils;

namespace MetroDigger.Manager
{
    class GameManager
    {
        #region Singleton
        public static GameManager Instance { get { return _instance; } }

        private static readonly GameManager _instance = new GameManager();
        #endregion

        private GameManager()
        {
            _levelAssembler = new LevelAssembler();
            MaxLevel = 2;
        }

        private LevelDto _levelDto;

        private readonly IAssembler<Level,LevelDto> _levelAssembler;

        public string UserName { get; set; }

        public int MaxLevel { get; set; }

        public void SignIn(string text)
        {
            UserName = text;
        }

        public void SaveGameToFile(string text)
        {
            text = Directory.GetCurrentDirectory() + "\\" + text;
            if (File.Exists(text)) return;
            XmlSerializer xmlSerializer = new XmlSerializer(_levelDto.GetType());
            using (TextWriter writer = new StreamWriter(text))
                xmlSerializer.Serialize(writer, _levelDto);
        }

        public void LoadLevelFromFile(string filename)
        {
            if (!File.Exists(filename)) return;

            XmlSerializer serializer = new XmlSerializer(typeof(LevelDto));
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                _levelDto = (LevelDto) serializer.Deserialize(reader);
            }
        }

        public void SaveToMemory(Level level)
        {
            _levelDto = _levelAssembler.GetDto(level);
        }

        public Level LoadSavedLevelFromMemory()
        {
            return _levelAssembler.GetPlain(_levelDto);
        }

        public bool GetLevel(int lvlNo, out Level level)
        {
            level = null;
            if (lvlNo == MaxLevel)
                return true;
            LoadLevelFromFile("level_" + lvlNo);
            level = _levelAssembler.GetPlain(_levelDto);
            return false;
        }

        public bool NextLevel(ref Level level)
        {
            Level bufLevel;
            bool b = GetLevel(level.Number + 1, out bufLevel);
            if(!b)
            {
                bufLevel.Player.Score = level.Player.Score;
                bufLevel.Player.LivesCount = level.Player.LivesCount;
                level = bufLevel;
            }
            return b;
        }

        public List<ScoreInfo> LoadBestScores()
        {
            List<ScoreInfo> bestScores;

            XmlSerializer serializer = new XmlSerializer(typeof(List<ScoreInfo>));

            if (!File.Exists(BestScoresFileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ScoreInfo>));
                using (TextWriter writer = new StreamWriter(BestScoresFileName))
                    xmlSerializer.Serialize(writer, new List<ScoreInfo>());
            }

            using (FileStream fs = new FileStream(BestScoresFileName, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                bestScores = (List<ScoreInfo>)serializer.Deserialize(reader);
            }
            return bestScores;
        }

        public void AddToBestScores(int scoreValue)
        {
            ScoreInfo score = new ScoreInfo {Score = scoreValue, Name = UserName};
            var scores = LoadBestScores();
            scores.Add(score);
            scores = scores.OrderByDescending((info => info.Score)).Take(SavedScoresCount).ToList();
            XmlSerializer xmlSerializer = new XmlSerializer(scores.GetType());
            using (TextWriter writer = new StreamWriter(BestScoresFileName))
                xmlSerializer.Serialize(writer, scores);
        }

        public UserData LoadUserData()
        {
            UserData userData;
            string path = UserName+"_data";
            if (!File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
                using (TextWriter writer = new StreamWriter(path))
                    xmlSerializer.Serialize(writer, new UserData(UserName,new List<UserLevel>{new UserLevel()}));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(UserData));

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                userData = (UserData)serializer.Deserialize(reader);
            }
            return userData;
        }

        public const string BestScoresFileName = "bestScores";

        public const int SavedScoresCount = 10;

        public int[] UnlockedLevels()
        {
            UserData userData = LoadUserData();
            return userData.Levels.Where(ll=>ll.IsUnlocked).Select(l=>l.Number).ToArray();
        }

        public void SaveUserData(UserData userData)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(userData.GetType());
            using (TextWriter writer = new StreamWriter(UserName + "_data"))
                xmlSerializer.Serialize(writer, userData);
        }

        public void SaveAccomplishedLevel(int lvlNo, int score, int lives)
        {
            UserData userData = LoadUserData();
            var lvl = userData.Levels.SingleOrDefault(l => l.Number == lvlNo);
            if (lvl == null)
            {
                lvl = new UserLevel {Number = lvlNo};
                userData.Levels.Add(lvl);
            }
            if (lvl.BestScore >= score && lvl.IsUnlocked) return;
            lvl.BestScore = score;
            lvl.IsUnlocked = true;
            lvl.MaxLives = lives;
            SaveUserData(userData);
        }
    }

    public class ScoreInfo
    {
        public int Score { get; set; }
        public string Name { get; set; }
    }
}
