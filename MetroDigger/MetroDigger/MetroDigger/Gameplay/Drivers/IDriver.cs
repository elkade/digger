using System;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    /// <summary>
    /// Odpowiada za sterowanie ruchem  obiektu za pomosą wywoływania odpowiednich zdarzeń.
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// Aktualizuje ścieżkę, po której ma poruszać się sterowany obiekt
        /// </summary>
        /// <param name="mover">narzędzie dokonujące zmiany położenia obiektu</param>
        /// <param name="state">okraśla stan, w jakim znajduje się obiekt</param>
        void UpdateMovement(IMover mover, EntityState state);
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma wykonać strzał
        /// </summary>
        event Action Shoot;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć wiercić
        /// </summary>
        /// <remarks>Tile to kafelek, który ma zaostć wiercony</remarks>
        event Action<Tile> Drill;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć się poruszać
        /// </summary>
        /// <remarks>Tile to kafelek, w kierunku którego ma odbyć się ruch</remarks>
        event Action<Tile> Move;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć się obracać.
        /// </summary>
        /// <remarks>Vector2 - kierunek docelowy obrotu</remarks>
        event Action<Vector2> Turn;
    }
}
