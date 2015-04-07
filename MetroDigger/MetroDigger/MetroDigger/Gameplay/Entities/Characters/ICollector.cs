using MetroDigger.Gameplay.Entities.Others;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public interface ICollector
    {
        bool HasDrill { get; set; }
        int PowerCellCount { get; set; }
    }
}
