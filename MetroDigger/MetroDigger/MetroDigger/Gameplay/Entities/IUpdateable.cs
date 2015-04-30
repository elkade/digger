namespace MetroDigger.Gameplay.Entities
{
    public interface IUpdateable
    {
        void Update();
        bool IsToRemove { get; set; }
    }
}
