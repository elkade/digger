using System.Collections.Generic;
using MetroDigger.Gameplay;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Logging;

namespace MetroDigger.Serialization
{
    /// <summary>
    /// Implementuje wzorzec projektowy Assembler. 
    /// Służy do konwersji poziomu gry na obiekt łatwo serializowalny i z powrotem.
    /// </summary>
    public class LevelAssembler : IAssembler<Level, LevelDto>
    {
        /// <summary>
        /// Konwertuje poziom gry do obiektu serializowalnego
        /// </summary>
        /// <param name="plain">poziom</param>
        /// <returns>obiekt do serializacji</returns>
        public LevelDto GetDto(Level plain)
        {
            var dto = new LevelDto
            {
                Drills = new List<EntityDto>(),
                PowerCells = new List<EntityDto>(),
                MetroTunnels = new List<EntityDto>(),
                MetroStations = new List<EntityDto>(),
                Terrains = new List<TerrainDto>(),
                Miners = new List<MinerDto>(),
                Rangers = new List<RangerDto>(),
                Stones = new List<StoneDto>(),
                Width = plain.Width,
                Height = plain.Height,
                Number = plain.Number,
                InitScore = plain.InitScore,
                InitLives = plain.InitLives,
                StartPosition = new Position{X=plain.Board.StartTile.X, Y=plain.Board.StartTile.Y}
            };
            foreach (Tile tile in plain.Board)
            {
                #region Terrain

                var terrainDto = new TerrainDto();
                switch (tile.Accessibility)
                {
                    case Accessibility.Free:
                        terrainDto.Type = TerrainType.Free;
                        break;
                    case Accessibility.Soil:
                        terrainDto.Type = TerrainType.Soil;
                        break;
                    case Accessibility.Buffer:
                        terrainDto.Type = TerrainType.Buffer;
                        break;
                    case Accessibility.Rock:
                        terrainDto.Type = TerrainType.Rock;
                        break;
                    case Accessibility.Water:
                        terrainDto.Type = TerrainType.Water;
                        break;
                }
                terrainDto.Position = new Position {X = tile.X, Y = tile.Y};
                dto.Terrains.Add(terrainDto);

                #endregion

                #region Metro

                if (tile.Metro is Station)
                    dto.MetroStations.Add(new EntityDto {Position = new Position {X = tile.X, Y = tile.Y}});
                else if (tile.Metro is Tunnel)
                    dto.MetroTunnels.Add(new EntityDto {Position = new Position {X = tile.X, Y = tile.Y}});

                #endregion

                #region Item

                if (tile.Item is PowerCell)
                    dto.PowerCells.Add(new EntityDto {Position = new Position {X = tile.X, Y = tile.Y}});
                else if (tile.Item is Drill)
                    dto.Drills.Add(new EntityDto {Position = new Position {X = tile.X, Y = tile.Y}});

                #endregion
            }

            #region Player

            dto.Player = new PlayerDto
            {
                HasDrill = plain.Player.HasDrill,
                Position = new Position
                {
                    X = plain.Player.OccupiedTile.X,
                    Y = plain.Player.OccupiedTile.Y,
                },
                Score = plain.Player.Score,
                Lives = plain.Player.LivesCount,
                PowerCells = plain.Player.PowerCellsCount
            };

            #endregion

            #region Enemies

            foreach (var enemy in plain.DynamicEntities)
            {
                if (enemy is Miner)
                    dto.Miners.Add(new MinerDto
                    {
                        Position = new Position {X = enemy.OccupiedTile.X, Y = enemy.OccupiedTile.Y}
                    });
                else if (enemy is Ranger)
                {
                    Ranger ranger = enemy as Ranger;
                    dto.Rangers.Add(new RangerDto
                    {

                        Position = new Position {X = enemy.OccupiedTile.X, Y = enemy.OccupiedTile.Y},
                        PowerCells = ranger.PowerCellsCount,
                        HasDrill = ranger.HasDrill,
                    });
                }
                else if (enemy is Stone)
                    dto.Stones.Add(new StoneDto
                    {
                        Position = new Position {X = enemy.OccupiedTile.X, Y = enemy.OccupiedTile.Y}
                    });
            }

            #endregion
            Logger.Log("Level Data Transfer Object Created");

            return dto;
        }
        /// <summary>
        /// Konwertuje zdeselializowany poziom do poziomu gry
        /// </summary>
        /// <param name="dto">poziom zdeserializowany</param>
        /// <returns>poziom gry</returns>
        public Level GetPlain(LevelDto dto)
        {
            var plain = new Level(dto.Width, dto.Height)
            {
                Number = dto.Number,
                InitLives = dto.InitLives,
                InitScore = dto.InitScore
            };

                for (int i = 0; i < dto.Width; i++)
                    for (int j = 0; j < dto.Height; j++)
                        plain.Board[i, j] = new Tile(i, j);
                foreach (TerrainDto item in dto.Terrains)
                {
                    Terrain terrain = null;
                    switch (item.Type)
                    {
                        case TerrainType.Free:
                            terrain = new Free();
                            break;
                        case TerrainType.Rock:
                            terrain = new Rock();
                            break;
                        case TerrainType.Soil:
                            terrain = new Soil();
                            break;
                        case TerrainType.Buffer:
                            terrain = new Buffer();
                            break;
                        case TerrainType.Water:
                            terrain = new Water();
                            break;
                    }
                    plain.Board[item.Position.X, item.Position.Y].Terrain = terrain;
                }

                #region items & metro

                foreach (EntityDto item in dto.MetroStations)
                {
                    plain.Board[item.Position.X, item.Position.Y].Metro = new Station();
                    plain.StationTiles.Add(plain.Board[item.Position.X, item.Position.Y]);
                }
            foreach (EntityDto item in dto.MetroTunnels)
                plain.Board[item.Position.X, item.Position.Y].Metro = new Tunnel();
            foreach (EntityDto item in dto.PowerCells)
                    plain.Board[item.Position.X, item.Position.Y].Item = new PowerCell();
                foreach (EntityDto item in dto.Drills)
                    plain.Board[item.Position.X, item.Position.Y].Item = new Drill();

                #endregion
                if(dto.StartPosition==null)
                    plain.Board.StartTile = plain.Board[dto.Player.Position.X,dto.Player.Position.Y];
                else plain.Board.StartTile = plain.Board[dto.StartPosition.X,dto.StartPosition.Y];

                #region Player

                var player = new Player(new KeyboardDriver(Tile.Size, plain.Board),
                    plain.Board[dto.Player.Position.X, dto.Player.Position.Y], plain.Board.StartTile)
                {
                    PowerCellsCount = dto.Player.PowerCells,
                    LivesCount = dto.Player.Lives,
                    Score = dto.Player.Score
                };
                plain.RegisterPlayer(player);

                #endregion

                #region Enemy

                var enemies = new List<IDynamicEntity>();

                foreach (MinerDto item in dto.Miners)
                    enemies.Add(new Miner(new AStarDriver(Tile.Size, plain.Board, plain.Player),
                        plain.Board[item.Position.X, item.Position.Y]));

                foreach (RangerDto item in dto.Rangers)
                    enemies.Add(new Ranger(new AStarDriver(Tile.Size, plain.Board, plain.Player,false),
                        plain.Board[item.Position.X, item.Position.Y], item.HasDrill, item.PowerCells));

                foreach (StoneDto item in dto.Stones)
                    enemies.Add(new Stone(new GravityDriver(Tile.Size, plain.Board, Level.GravityVector),
                        plain.Board[item.Position.X, item.Position.Y]));


                plain.RegisterEnemies(enemies);

                #endregion
                Logger.Log("Plain Level Created");

            return plain;
        }
    }
}