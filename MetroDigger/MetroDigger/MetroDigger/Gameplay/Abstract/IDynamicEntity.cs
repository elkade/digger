namespace MetroDigger.Gameplay.Abstract
{
    public interface IDynamicEntity : ICollideable, IDrawable
    {
        int Value { get; set; }
    }
}