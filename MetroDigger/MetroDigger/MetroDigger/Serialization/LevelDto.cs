using System.Collections.Generic;

namespace MetroDigger.Serialization
{
    public class LevelDto
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public PlayerDto Player { get; set; }
        public List<EntityDto> Drills { get; set; }
        public List<EntityDto> PowerCells { get; set; }
        public List<TerrainDto> Terrains { get; set; }
        public List<EntityDto> MetroStations { get; set; }
        public List<EntityDto> MetroTunnels { get; set; }
        public List<MinerDto> Miners { get; set; }
        public List<RangerDto> Rangers { get; set; }
        public List<StoneDto> Stones { get; set; }
        public int Number { get; set; }
        public int InitScore { get; set; }
        public int InitLives { get; set; }

        public Position StartPosition { get; set; }
        //stone
        //bullet
        //miner
        //ranger
    }

    public class StoneDto : EntityDto
    {
        
    }

    public class RangerDto : EntityDto
    {
        public bool HasDrill { get; set; }
        public int PowerCells { get; set; }
    }

    public class MinerDto : EntityDto
    {

    }

    public class TerrainDto : EntityDto
    {
        public TerrainType Type { get; set; }
    }

    public enum TerrainType
    {
        Buffer,
        Free,
        Soil,
        Rock,
        Water
    }

    public class PlayerDto : EntityDto
    {
        public int Score { get; set; }
        public int Lives { get; set; }
        public int PowerCells { get; set; }
        public bool HasDrill { get; set; }
    }

    public class EntityDto
    {
        public Position Position { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
