using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MetroDigger.Gameplay;
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
            _maxLevel = 1;
        }

        private LevelDto _levelDto;

        private readonly IAssembler<Level,LevelDto> _levelAssembler;

        private int _maxLevel;

        public string UserName { get; set; }

        public void SignIn(string text)
        {
            UserName = text;
        }

        public void SaveGameToFile(string text)
        {
            text = Directory.GetCurrentDirectory() + "\\" + text + ".xml";
            if (File.Exists(text)) return;
            XmlSerializer xmlSerializer = new XmlSerializer(_levelDto.GetType());
            using (TextWriter writer = new StreamWriter(text))
                xmlSerializer.Serialize(writer, _levelDto);
        }

        public void LoadLevelFromFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LevelDto));

            using (FileStream fs = new FileStream(filename+".xml", FileMode.Open))
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
            if (lvlNo == _maxLevel)
                return true;
            LoadLevelFromFile("level_" + lvlNo);
            level = _levelAssembler.GetPlain(_levelDto);
            return false;
        }

        public bool NextLevel(ref Level level)
        {
            Level bufLevel = level;
            bool b = GetLevel(level.Number + 1, out level);
            level.Player.Score = bufLevel.Player.Score;
            level.Player.LivesCount = bufLevel.Player.LivesCount;
            return b;
        }
    }
}
