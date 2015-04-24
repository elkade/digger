using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetroDigger.Gameplay;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Serialization;

namespace MetroDigger.Utils
{
    class LevelAssembler : IAssembler<Level, LevelDto>
    {
        public LevelDto GetDto(Level plain)
        {
            LevelDto dto = new LevelDto
            {
                Drills = new List<EntityDto>(),
                PowerCells = new List<EntityDto>(),
                MetroTunnels = new List<EntityDto>(),
                MetroStations = new List<EntityDto>(),
                Terrains = new List<TerrainDto>(),
                Width = plain.Width,
                Height = plain.Height,
            };
            foreach (var tile in plain.Tiles)
            {
                #region Terrain
                TerrainDto terrainDto = new TerrainDto();
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
                terrainDto.Position = new Position { X = tile.X, Y = tile.Y };
                dto.Terrains.Add(terrainDto);
                #endregion
                #region Metro
                if (tile.Metro is Station)
                    dto.MetroStations.Add(new EntityDto { Position = new Position { X = tile.X, Y = tile.Y } });
                else if (tile.Metro is Tunnel)
                    dto.MetroTunnels.Add(new EntityDto { Position = new Position { X = tile.X, Y = tile.Y } });
                #endregion
                #region Item
                if (tile.Item is PowerCell)
                    dto.PowerCells.Add(new EntityDto { Position = new Position { X = tile.X, Y = tile.Y } });
                else if (tile.Item is Drill)
                    dto.Drills.Add(new EntityDto { Position = new Position { X = tile.X, Y = tile.Y } });

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
            return dto;
        }

        public Level GetPlain(LevelDto dto)
        {
            Level plain = new Level(dto.Width,dto.Height);
            for (int i = 0; i < dto.Width; i++)
                for (int j = 0; j < dto.Height; j++)
                    plain.Tiles[i, j] = new Tile(i, j);
            foreach (var item in dto.Terrains)
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
                plain.Tiles[item.Position.X, item.Position.Y].Terrain = terrain;
            }
            #region items & metro
            foreach (var item in dto.MetroStations)
            {
                plain.Tiles[item.Position.X, item.Position.Y].Metro = new Station();
                plain.StationsCount++;
            }
            foreach (var item in dto.MetroTunnels)
                plain.Tiles[item.Position.X, item.Position.Y].Metro = new Tunnel();
            foreach (var item in dto.PowerCells)
                plain.Tiles[item.Position.X, item.Position.Y].Item = new PowerCell();
            foreach (var item in dto.Drills)
                plain.Tiles[item.Position.X, item.Position.Y].Item = new Drill();
            #endregion
            #region Enemy

            #endregion
            #region Player

            Player player = new Player(new KeyboardDriver(Tile.Size,plain.Tiles), plain.Tiles[dto.Player.Position.X, dto.Player.Position.Y])
            {
                PowerCellCount = dto.Player.PowerCells,
                LivesCount = dto.Player.Lives,
                Score = dto.Player.Score
            };
            plain.RegisterPlayer(player);

            plain.RegisterEnemies();

            plain.InitEvents();

            #endregion

            return plain;
        }

    }
}
