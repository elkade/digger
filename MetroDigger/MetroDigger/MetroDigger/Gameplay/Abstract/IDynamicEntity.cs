namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, który jest pełnoprwanym elementem gry.
    /// </summary>
    public interface IDynamicEntity : ICollideable, IDrawable
    {
        /// <summary>
        /// Wartość obiektu - liczba punktów zdobywanych przy jego zniszczeniu.
        /// </summary>
        int Value { get; }
    }
}