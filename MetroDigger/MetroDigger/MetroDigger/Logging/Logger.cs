using System;
using System.IO;
using MetroDigger.Manager;

namespace MetroDigger.Logging
{
    class Logger
    {
        private const string Header = "{0}";

        private const string Directory = "logs/";

        private static string _fileName;


        public static void Config(string fileName)
        {
            _fileName = fileName;
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
            File.Delete(Directory+_fileName);
        }

        public static void Log(string message)
        {
#if DEBUG
            if (_fileName == null) return;
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
            using (var writer = File.AppendText(Directory + _fileName))
            {
                writer.WriteLine(Header, DateTime.Now);

                foreach (var line in message.Split(new[] { '\n' }))
                {
                    writer.WriteLine(line);
                }

                writer.WriteLine();
            }
#endif
        }
    }
}
