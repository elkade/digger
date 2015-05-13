using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using MetroDigger.Gameplay;
using MetroDigger.Manager.Settings;
using MetroDigger.Serialization;

namespace MetroDigger.Manager
{
    /// <summary>
    /// Odpowiada za przekazywanie, zapisywanie i odczytywanie informacji związanych z grą i użytkownikiem.
    /// Realizuje wzorzec projektowy singleton.
    /// </summary>
    class GameManager
    {
        #region Singleton
        /// <summary>
        /// Zwraca instancję klasy.
        /// </summary>
        public static GameManager Instance { get { return _instance; } }

        private static readonly GameManager _instance = new GameManager();
        #endregion

        private GameManager()
        {
            _levelAssembler = new LevelAssembler();
        }

        private LevelDto _levelDto;

        private readonly IAssembler<Level,LevelDto> _levelAssembler;
        /// <summary>
        /// Nazwa aktualnie zalogowanego gracza
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// Zwraca numer poziomu o najwyższym numerze
        /// </summary>
        /// <returns>Numer poziomu</returns>
        public int GetMaxLevel()
        {
            if (!Directory.Exists(LevelDirectory))
                Directory.CreateDirectory(LevelDirectory);
            int i;
            for (i = 0; i < int.MaxValue; i++)
            {
                string path = LevelDirectory + "level_" + i;
                if (!File.Exists(path))
                    break;
            }
            return i;
        }
        /// <summary>
        /// Loguje gracza do gry
        /// </summary>
        /// <param name="name">Nazwa gracza</param>
        public void SignIn(string name)
        {
            UserName = name;
        }

        private const string SaveDirectory = "saved_games/";
        private const string BestDirectory = "best_scores/";
        private const string UserDirectory = "users/";
        private const string LevelDirectory = "levels/";

        /// <summary>
        /// Zapisuje poziom wraz z aktualnym stanem do pliku
        /// </summary>
        /// <param name="text">nazwa pliku</param>
        public void SaveGameToFile(string text)
        {
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);

            if(File.Exists(SaveDirectory+text))
                throw new Exception("File with specified name already exists.");
            XmlSerializer xmlSerializer = new XmlSerializer(_levelDto.GetType());
            using (TextWriter writer = new StreamWriter(SaveDirectory+text))
                xmlSerializer.Serialize(writer, _levelDto);
        }
        /// <summary>
        /// Pobiera zapisany poziom z pliku
        /// </summary>
        /// <param name="filename">nazwa pliku</param>
        /// <param name="isFromSave">określa gdzie szukać pliku:
        /// czy w folderze z zapisanymi grami, czy w folderze z poziomami</param>
        public void LoadLevelFromFile(string filename, bool isFromSave = false)
        {
            string directory = isFromSave ? SaveDirectory : LevelDirectory;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string path = directory + filename;
            if (!File.Exists(path))
                throw new Exception("Unable to load file of specified name.");
            XmlSerializer serializer = new XmlSerializer(typeof(LevelDto));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                _levelDto = (LevelDto) serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// Konwertuje bieżący poziom  i zapisuje w pamięci do momentu zapisania do pliku
        /// </summary>
        /// <param name="level"></param>
        public void SaveToMemory(Level level)
        {
            _levelDto = _levelAssembler.GetDto(level);
        }
        /// <summary>
        /// Pobiera Poziom zapisany w pamięci
        /// </summary>
        /// <returns>Poziom w stanie gotowym do rozgrywki</returns>
        public Level LoadSavedLevelFromMemory()
        {
            return _levelAssembler.GetPlain(_levelDto);
        }
        /// <summary>
        /// Ładuje poziom o konkretnym numerze z uwzględnieniem dotychczasowych wyników.
        /// </summary>
        /// <param name="lvlNo">Numer poziomu</param>
        /// <param name="level">Poziom do zwrócenia</param>
        /// <param name="isNew">czy bieżące wyniki mają być dla żądanego poziomu uwzględniane</param>
        /// <returns>czy żądany numer poziomu nie przekracza najwyższego możliwego</returns>
        public bool GetLevel(int lvlNo, out Level level, bool isNew=false)
        {
            level = null;
            if (lvlNo == GetMaxLevel())
                return true;
            LoadLevelFromFile("level_" + lvlNo);
            level = _levelAssembler.GetPlain(_levelDto);
            if (isNew && level.Number > 0)
            {
                UserData ud = LoadUserData();
                level.InitLives = ud.Levels[level.Number-1].MaxLives;
                level.InitScore = ud.Levels[level.Number-1].BestScore;
            }
            return false;
        }
        /// <summary>
        /// Pobiera z pliku najlepsze wyniki poziomu lub całej gry
        /// </summary>
        /// <param name="lvlNo">numer poziomu. null = cała gra</param>
        /// <returns>lista 10 najlepszych wyników</returns>
        public List<ScoreInfo> LoadBestScores(int? lvlNo=null)
        {
            string num = lvlNo == null ? "" : lvlNo.ToString();
            string fileName = BestScoresFileName + "_" + num;
            List<ScoreInfo> bestScores;

            XmlSerializer serializer = new XmlSerializer(typeof(List<ScoreInfo>));

            if (!Directory.Exists(BestDirectory))
                Directory.CreateDirectory(BestDirectory);
            fileName = BestDirectory + fileName;

            if (!File.Exists(fileName))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ScoreInfo>));
                using (TextWriter writer = new StreamWriter(fileName))
                    xmlSerializer.Serialize(writer, new List<ScoreInfo>());
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                bestScores = (List<ScoreInfo>)serializer.Deserialize(reader);
            }
            return bestScores;
        }
        /// <summary>
        /// Dodaje wynik do listy najlepszych wyników
        /// </summary>
        /// <param name="scoreValue">wynik</param>
        /// <param name="lvlNo">poziom, na którym zdobyto wynik</param>
        public void AddToBestScores(int scoreValue, int? lvlNo=null)
        {
            string num = lvlNo == null ? "" : lvlNo.ToString();
            string fileName = BestScoresFileName + "_" + num;
            if (!Directory.Exists(BestDirectory))
                Directory.CreateDirectory(BestDirectory);
            fileName = BestDirectory + fileName;
            List<ScoreInfo> scores;
            try
            {
                scores = LoadBestScores(lvlNo);
            }
            catch
            {
                scores = new List<ScoreInfo>();
                File.Delete(fileName);
            }

            ScoreInfo score = new ScoreInfo {Score = scoreValue, Name = UserName};
            scores.Add(score);
            scores = scores.OrderByDescending((info => info.Score)).Take(SavedScoresCount).ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(scores.GetType());
            using (TextWriter writer = new StreamWriter(fileName))
                xmlSerializer.Serialize(writer, scores);
        }

        private UserData LoadUserData()
        {
            UserData userData;
            string path = UserName+"_data";

            if (!Directory.Exists(UserDirectory))
                Directory.CreateDirectory(UserDirectory);
            path = UserDirectory + path;


            if (!File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
                using (TextWriter writer = new StreamWriter(path))
                    xmlSerializer.Serialize(writer, new UserData(UserName,new List<UserLevel>()));
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof (UserData));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    XmlReader reader = XmlReader.Create(fs);
                    userData = (UserData) serializer.Deserialize(reader);
                }
            }
            catch
            {
                File.Delete(path);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
                using (TextWriter writer = new StreamWriter(path))
                    xmlSerializer.Serialize(writer, new UserData(UserName, new List<UserLevel>()));
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    XmlReader reader = XmlReader.Create(fs);
                    userData = (UserData)serializer.Deserialize(reader);
                }
            }
            return userData;
        }

        private const string BestScoresFileName = "bestScores";

        private const int SavedScoresCount = 10;

        /// <summary>
        /// Zwraca listę odblokowanych przez gracza poziomów wraz z najlepszym wynikiem
        /// </summary>
        /// <returns>lista informacji o poziomach</returns>
        public IEnumerable<int> UnlockedLevels()//które odblokowane i z jakim wynikiem
        {
            UserData userData = LoadUserData();
            return userData.Levels.Where(ll=>ll.IsUnlocked).Select(l=>l.Number).ToArray();
        }

        private void SaveUserData(UserData userData)
        {
            var path = UserName + "_data";
            if (!Directory.Exists(UserDirectory))
                Directory.CreateDirectory(UserDirectory);
            path = UserDirectory + path;
            XmlSerializer xmlSerializer = new XmlSerializer(userData.GetType());
            using (TextWriter writer = new StreamWriter(path))
                xmlSerializer.Serialize(writer, userData);
        }
        /// <summary>
        /// Zapisuje do pliku informację, że dany poziom został ukończony oraz wynik i liczbę żyć.
        /// </summary>
        /// <param name="lvlNo">numer poziomu</param>
        /// <param name="score">Wynik uzyskany na poziomie</param>
        /// <param name="lives">pozostała liczba żyć.</param>
        public void SaveAccomplishedLevel(int lvlNo, int score, int lives)
        {
            UserData userData = LoadUserData();
            var lvl = userData.Levels.SingleOrDefault(l => l.Number == lvlNo);
            if (lvl == null)
            {
                lvl = new UserLevel {Number = lvlNo};
                userData.Levels.Add(lvl);
            }
            if (lvl.BestScore > score && lvl.IsUnlocked) return;
            lvl.BestScore = score;
            lvl.IsUnlocked = true;
            lvl.MaxLives = lives;
            SaveUserData(userData);
        }
        /// <summary>
        /// Czyści ranking najlepszych wyników.
        /// </summary>
        /// <param name="lvlNo">numer poziomu null=ranking dla wszystkich</param>
        public void ClearRanking(int? lvlNo)
        {
            string num = lvlNo == null ? "" : lvlNo.ToString();
            string fileName = BestScoresFileName + "_" + num;

            if (!Directory.Exists(BestDirectory))
                Directory.CreateDirectory(BestDirectory);
            fileName = BestDirectory + fileName;
            if(File.Exists(fileName))
                File.Delete(fileName);
        }
    }

    public class ScoreInfo
    {
        public int Score { get; set; }
        public string Name { get; set; }
    }
}
