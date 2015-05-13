using System.Collections.Generic;

namespace MetroDigger.Manager.Settings
{
    /// <summary>
    /// Ieprezentuje informację o postępie użytkownia w przechodzeniu gry 
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// Domyślny konstruktor bezparametrowy potrzebny w celu serializacji
        /// </summary>
        public UserData()
        {
            
        }
        /// <summary>
        /// Tworzy nowy okbiekt UserData
        /// </summary>
        /// <param name="name">Nazwa użytkownika</param>
        /// <param name="levels">Lista obiektów UserLevel mówiąca,
        /// które poziomy i z jakim wynikiem ukończył użytkownik.</param>
        public UserData(string name, List<UserLevel> levels)
        {
            Name = name;
            Levels = levels;
        }
        /// <summary>
        /// Lista poziomów, które ukończył użytkownik wraz ze stanem ukończenia.
        /// </summary>
        public List<UserLevel> Levels { get; set; }

        private string Name { get; set; }
    }
    /// <summary>
    /// Reprezentuje opis stan ukończenia poziomu
    /// </summary>
    public class UserLevel
    {
        /// <summary>
        /// Tworzy nowy obiekt typu UserLEvel z wertościamy domyślnymi
        /// </summary>
        public UserLevel()
        {
            BestScore = 0;
            IsUnlocked = true;
            MaxLives = 2;
            Number = 0;
        }
        /// <summary>
        /// Numer poziomu
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Najlepszy uzyskany wynik w tym poziomie
        /// </summary>
        public int BestScore { get; set; }
        /// <summary>
        /// Czy poziom został już odblokowany
        /// </summary>
        public bool IsUnlocked { get; set; }
        /// <summary>
        /// liczba żyć przy najlepszym wyniku
        /// </summary>
        public int MaxLives { get; set; }
    }
}
