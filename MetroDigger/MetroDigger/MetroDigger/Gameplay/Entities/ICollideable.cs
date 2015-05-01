using MetroDigger.Gameplay.Entities.Characters;

namespace MetroDigger.Gameplay.Entities
{
    public interface ICollideable : IBoardObject
    {
        Aggressiveness Aggressiveness { get; }
        void CollideWith(ICollideable collideable);
        void Harm();
        bool IsWaterProof { get; set; }
    }
}