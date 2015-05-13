namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Teren buforowy, na który nikt nie ma wstępu. Okala planszę.
    /// </summary>
    class Buffer : Terrain
    {
        /// <summary>
        /// Tworzy nowy teren buforowy.
        /// </summary>
        public Buffer()
        {
            _accessibility = Accessibility.Buffer;
        }
    }
}
