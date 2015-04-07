namespace MetroDigger.Gameplay.Entities.Others
{
    public abstract class Metro : StaticEntity
    {
        public bool IsCleared { get; set; }

        public Metro()
        {
            IsCleared = false;
        }

        public virtual void Clear()
        {
            IsCleared = true;
        }
    }
}
