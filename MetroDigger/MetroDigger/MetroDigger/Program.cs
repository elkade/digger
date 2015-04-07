using System;

namespace MetroDigger
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MetroDiggerGame game = new MetroDiggerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

