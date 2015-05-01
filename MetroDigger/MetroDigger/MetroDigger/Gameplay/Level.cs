using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MetroDigger.Gameplay.CollisionDetection;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.GameObjects;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Buffer = MetroDigger.Gameplay.Entities.Terrains.Buffer;

namespace MetroDigger.Gameplay
{
    internal class Level
    {
        public static Vector2 GravityVector = new Vector2(0, 1);
        private readonly ICollisionDetector _collisionDetector;
        private readonly int _height;
        private readonly TopBar _topBar;
        private readonly int _width;
        public readonly Board Board;

        public readonly List<IDynamicEntity> DynamicEntities = new List<IDynamicEntity>();
        public readonly List<Character> Enemies = new List<Character>();
        public readonly List<IDynamicEntity> NewlyAddedDynamicEntities = new List<IDynamicEntity>();
        public int StationsCount;
        private bool _isStarted;
        private Player _player;
        private WaterSpiller _ws;
        #region LoadFromSave

        public Level(int width, int height)
        {
            StationsCount = 0;
            _width = width;
            _height = height;
            float h = (float) MediaManager.Instance.Height/height;
            float w = (float) MediaManager.Instance.Width/width;
            float min = Math.Min(h, w);
            Tile.Size = new Vector2(min, min);
            MediaManager.Instance.Scale = new Vector2(min/300, min/300);
            _isStarted = false;
            Board = new Board(Width, Height);
            _topBar = new TopBar();
            _collisionDetector = new RectangleDetector();
            _ws = new WaterSpiller(Board, GravityVector);
        }

        public void RegisterPlayer(Player p)
        {
            _player = p;
            Player.Shoot += (sender) =>
            {
                var bullet = new Bullet(new StraightDriver(Tile.Size, Board), sender);
                bullet.Update();
                bullet.Hit += (bullet1, tile) =>
                {
                    bool b;
                    Player.Score += tile.Clear(ref StationsCount, out b);
                    bullet1.IsToRemove = b;
                    if (tile.Accessibility!=Accessibility.Water)
                        _ws.Spill(tile.X, tile.Y);
                };
                NewlyAddedDynamicEntities.Add(bullet);
            };

            Player.Visited += (collector, tile1, tile2) =>
            {
                if (tile2.Item != null)
                    tile2.Item.GetCollected(collector);
                int score = tile2.Clear(ref StationsCount);
                if (tile1.Metro is Tunnel && score > 0)
                    score += 50;
                Player.Score += score;
            };

            Player.Drilled += (character, tile) =>
            {
                tile.Clear(ref StationsCount);
                _ws.Spill(tile.X,tile.Y);
            };
            Board.StartTile = Player.OccupiedTile;
        }

        public void RegisterEnemies()
        {
            var drillers = Enemies.OfType<IDriller>();
            foreach (var enemy in drillers) //TODO to jeswt nie tak
            {
                enemy.Drilled += (character, tile) =>
                {
                    Player.Score += tile.Clear(ref StationsCount);
                    _ws.Spill(tile.X, tile.Y);
                };
            }
            DynamicEntities.AddRange(Enemies);
            DynamicEntities.Add(_player);
        }

        #endregion

        //public Level(int width, int height, bool isStarted)
        //{
        //    StationsCount = 0;
        //    _width = width;
        //    _height = height;
        //    _isStarted = isStarted;
        //    Board = new Tile[Width, Height];
        //    _bullets = new List<Bullet>();
        //    InitMap();
        //    _topBar = new TopBar();
        //    _player = new Player(new KeyboardDriver(Tile.Size, Board), Board[0, 0]);

        //    _collisionDetector = new RectangleDetector();
        //    InitEvents();
        //}

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Player Player
        {
            get { return _player; }
        }


        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; }
        }

        public int Number { get; set; }

        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
                return;

            foreach (IDynamicEntity dynamicEntity in DynamicEntities)
            {
                dynamicEntity.Update();
            }

            _topBar.Update(Player.LivesCount, Player.Score, Player.PowerCellCount);
            for (int i = 0; i < DynamicEntities.Count; i++)
            {
                IDynamicEntity u1 = DynamicEntities[i];
                if(!u1.IsWaterProof && u1.OccupiedTile.Accessibility==Accessibility.Water)
                    u1.Harm();
                for (int j = i + 1; j < DynamicEntities.Count; j++)
                {
                    IDynamicEntity u2 = DynamicEntities[j];
                    if (_collisionDetector.CheckCollision(u1, u2))
                    {
                        Debug.WriteLine("Collision Detected");
                        u1.CollideWith(u2);
                        u2.CollideWith(u1);
                    }
                }
            }

            DynamicEntities.AddRange(NewlyAddedDynamicEntities);
            NewlyAddedDynamicEntities.Clear();

            DynamicEntities.RemoveAll(character => character.IsToRemove);

            CheckProgress();
        }

        private void CheckProgress()
        {
            if (StationsCount == 0)
                RaiseLevelAccomplished(true);
            if (Player.LivesCount == 0)
                RaiseLevelAccomplished(false);
        }

        private void DrawTiles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Board[x, y].Draw(gameTime, spriteBatch);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(gameTime, spriteBatch);
            foreach (IDynamicEntity dynamicEntity in DynamicEntities)
                dynamicEntity.Draw(gameTime, spriteBatch);
            _topBar.Draw(spriteBatch);
        }


        //private void InitMap()
        //{
        //    for (int i = 0; i < Width; i++)
        //    {
        //        for (int j = 0; j < Height; j++)
        //        {
        //            Board[i, j] = new Tile(i, j, new Soil());
        //        }
        //    }
        //    for (int i = 1; i < Width - 1; i++)
        //    {
        //        int j = 1;
        //        Board[j, i] = new Tile(j, i, new Rock());
        //    }
        //    for (int i = 1; i < Width; i++)
        //    {
        //        int j = 2;
        //        Board[j, i].Metro = new Tunnel();
        //    }
        //    Board[4, 4].Item = new Drill();
        //    Board[5, 5].Item = new PowerCell();
        //    Board[0, 6].Metro = new Station();
        //    StationsCount++;
        //    Board[4, 6].Metro = new Station();
        //    StationsCount++; //TODO WTF
        //}

        private void RaiseLevelAccomplished(bool b)
        {
            if (LevelAccomplished != null)
                LevelAccomplished(this, b);
        }

        public event Action<Level, bool> LevelAccomplished;
    }

    public class Board : IEnumerable<Tile>
    {
        private readonly int _height;
        private readonly Tile[,] _tiles;
        private readonly int _width;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new Tile[width + 2, height + 2];
            for (int i = 0; i < width + 2; i++)
            {
                for (int j = 0; j < height + 2; j++)
                {
                    _tiles[i, j] = new Tile(i - 1, j - 1, new Buffer());
                }
            }
        }

        public Tile this[int x, int y]
        {
            get
            {
                if (x + 1 < 0 || x > _width || y + 1 < 0 || y > _height)
                    return null;
                return _tiles[x + 1, y + 1];
            }
            set { _tiles[x + 1, y + 1] = value; }
        }

        public Tile StartTile { get; set; }

        public IEnumerator<Tile> GetEnumerator()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    yield return this[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _width;
            return _height;
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
    }
}