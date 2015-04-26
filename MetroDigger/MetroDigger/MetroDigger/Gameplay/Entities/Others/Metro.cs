namespace MetroDigger.Gameplay.Entities.Others
{
    public abstract class Metro : StaticEntity
    {
        public bool IsCleared { get; set; }

        public Metro()
        {
            IsCleared = false;
        }

        public virtual int Clear(ref int stationsCount)
        {
            IsCleared = true;
            return 0;
        }
    }
}
