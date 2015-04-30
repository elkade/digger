using System.Collections.Generic;
using MetroDigger.Gameplay;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Utils;
using Microsoft.Xna.Framework;

namespace MetroDigger.Serialization
{
    internal class LevelAssembler : IAssembler<Level, LevelDto>
    {
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
                Stones = new List<StoneDto>(),
                Width = plain.Width,
                Height = plain.Height,
                Number = plain.Number,
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
                PowerCells = plain.Player.PowerCellCount
            };

            #endregion

            #region Enemies

            foreach (Character enemy in plain.Enemies)
            {
                if (enemy is Miner)
                    dto.Miners.Add(new MinerDto
                    {
                        Position = new Position {X = enemy.OccupiedTile.X, Y = enemy.OccupiedTile.Y}
                    });
                else if (enemy is Stone)
                    dto.Stones.Add(new StoneDto
                    {
                        Position = new Position { X = enemy.OccupiedTile.X, Y = enemy.OccupiedTile.Y }
                    });
            }

            #endregion

            return dto;
        }

        public Level GetPlain(LevelDto dto)
        {
            var plain = new Level(dto.Width, dto.Height)
            {
                Number = dto.Number
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
                }
                plain.Board[item.Position.X, item.Position.Y].Terrain = terrain;
            }

            #region items & metro

            foreach (EntityDto item in dto.MetroStations)
            {
                plain.Board[item.Position.X, item.Position.Y].Metro = new Station();
                if (plain.Board[item.Position.X, item.Position.Y].Terrain.Accessibility != Accessibility.Free)
                    plain.StationsCount++;
                else
                    plain.Board[item.Position.X, item.Position.Y].Metro.IsCleared = true;
            }
            foreach (EntityDto item in dto.MetroTunnels)
                plain.Board[item.Position.X, item.Position.Y].Metro = new Tunnel();
            foreach (EntityDto item in dto.PowerCells)
                plain.Board[item.Position.X, item.Position.Y].Item = new PowerCell();
            foreach (EntityDto item in dto.Drills)
                plain.Board[item.Position.X, item.Position.Y].Item = new Drill();

            #endregion

            #region Player

            var player = new Player(new KeyboardDriver(Tile.Size, plain.Board),
                plain.Board[dto.Player.Position.X, dto.Player.Position.Y])
            {
                PowerCellCount = dto.Player.PowerCells,
                LivesCount = dto.Player.Lives,
                Score = dto.Player.Score
            };
            plain.RegisterPlayer(player);

            #endregion

            #region Enemy

            foreach (MinerDto item in dto.Miners)
                plain.Enemies.Add(new Miner(new AStarDriver(Tile.Size, plain.Board, plain.Player),plain.Board[item.Position.X,item.Position.Y] ));

            foreach (StoneDto item in dto.Stones)
                plain.Enemies.Add(new Stone(new GravityDriver(Tile.Size, plain.Board, Level.GravityVector), plain.Board[item.Position.X, item.Position.Y]));


            plain.RegisterEnemies();
            #endregion

            

            return plain;
        }
    }
}