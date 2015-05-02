using MetroDigger.Gameplay.Entities.Terrains;

namespace MetroDigger.Gameplay.Entities.Others
{
    public abstract class Metro : StaticEntity
    {
        public bool IsCleared { get; set; }

        public Metro()
        {
            IsCleared = false;
            IsVisitedInSequence = true;
            ClearedOf = Accessibility.Free;
        }

        public virtual int Clear(ref int stationsCount)
        {
            IsCleared = true;
            return 0;
        }

        public bool IsVisitedInSequence { get; set; }
        public Accessibility ClearedOf { get; set; }
    }
}
