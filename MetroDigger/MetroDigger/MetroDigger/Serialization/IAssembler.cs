namespace MetroDigger.Serialization
{
    /// <summary>
    /// Implementacja wzorca projektowego Assembler służącego
    /// do przekształcenia obiektów gry do postaci łatwo serializowalnej.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    interface IAssembler<T, TDto>
    {
        /// <summary>
        /// Konwertuje obiekt gry do obiektu serializowalnego
        /// </summary>
        /// <param name="plain">obiekt gry</param>
        /// <returns>obiekt do serializacji</returns>
        TDto GetDto(T plain);
        /// <summary>
        /// Konwertuje zdeselializowany obiekt do obiektu gry
        /// </summary>
        /// <param name="dto">obiekt zdeserializowany</param>
        /// <returns>obiekt gry</returns>
        T GetPlain(TDto dto);
    }
}
