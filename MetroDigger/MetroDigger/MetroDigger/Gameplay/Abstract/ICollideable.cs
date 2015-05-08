using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.Abstract
{
    public interface ICollideable : IBoardObject
    {
        Aggressiveness Aggressiveness { get; }
        void CollideWith(ICollideable collideable);
        void Harm();
        bool IsWaterProof { get; set; }
    }
}