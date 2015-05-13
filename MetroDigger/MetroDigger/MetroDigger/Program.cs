namespace MetroDigger
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// G³ówny punk wejœcia aplikacji.
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

