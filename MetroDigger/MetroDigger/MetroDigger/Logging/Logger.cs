using System;
using System.IO;

namespace MetroDigger.Logging
{
    /// <summary>
    /// Narzędzie pomocnicze służące do zapisywania logów z gry do pliku. Działa jedynie w trybie DEBUG
    /// </summary>
    static class Logger
    {
        private const string Header = "{0}";

        private const string Directory = "logs/";

        private static string _fileName;
        /// <summary>
        /// Konfiguruje ustawienia loggera
        /// </summary>
        /// <param name="fileName">nazwa pliku, do którego zapisywane są logi z gry</param>
        public static void Config(string fileName)
        {
            _fileName = fileName;
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);
            File.Delete(Directory+_fileName);
        }
        /// <summary>
        /// Loguje podaną wiadomość wraz z bieżacą datą i czasem
        /// </summary>
        /// <param name="message">Wiadomość do zalogowania.</param>
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
