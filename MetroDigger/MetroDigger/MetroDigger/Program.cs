namespace MetroDigger
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// G��wny punk wej�cia aplikacji.
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

