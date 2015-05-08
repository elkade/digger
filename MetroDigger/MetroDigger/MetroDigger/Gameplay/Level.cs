using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.CollisionDetection;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.GameObjects;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Logging;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private readonly List<IDynamicEntity> _dynamicEntities = new List<IDynamicEntity>();
        public readonly List<IDynamicEntity> Enemies = new List<IDynamicEntity>();
        private readonly List<IDynamicEntity> _newlyAddedDynamicEntities = new List<IDynamicEntity>();
        private int _stationsCount;
        private bool _isStarted;
        private Player _player;
        private readonly ISpiller _ws;

        public readonly List<Tile> StationTiles = new List<Tile>();
        public readonly List<Tile> TunnelTiles = new List<Tile>(); 
        #region LoadFromSave

        public Level(int width, int height)
        {
            _stationsCount = 0;
            _width = width;
            _height = height;
            MediaManager.Instance.SetDimensions(_width,_height);
            _isStarted = false;
            Board = new Board(Width, Height);
            _topBar = new TopBar();
            _collisionDetector = new RectangleDetector();
            _ws = new WaterSpiller(Board, GravityVector);

            Logger.Log("New level created");

        }

        public void RegisterPlayer(Player p)
        {
            _player = p;
            Player.Shoot += sender =>
            {
                var bullet = new Bullet(new StraightDriver(Tile.Size, Board), sender);
                bullet.Update();
                bullet.Hit += (bullet1, tile) =>
                {
                    bool b;
                    Player.Score += tile.Clear(ref _stationsCount, out b);
                    bullet1.IsToRemove = b;
                    if (tile.Accessibility!=Accessibility.Water)
                        Player.Score += _ws.Spill(tile.X, tile.Y);
                };
                _newlyAddedDynamicEntities.Add(bullet);
            };

            Player.Visited += (collector, tile1, tile2) =>
            {
                if (tile2.Item != null)
                    tile2.Item.GetCollected(collector);
                int score = tile2.Clear(ref _stationsCount);
                if (tile1.Metro is Tunnel && score > 0)
                    score += 50;
                Player.Score += score;
            };

            Player.Drilled += (character, tile) =>
            {
                Player.Score += tile.Clear(ref _stationsCount);
                Player.Score += _ws.Spill(tile.X, tile.Y);
            };
        }

        public void RegisterEnemies()
        {
            var drillers = Enemies.OfType<IDriller>();
            foreach (var enemy in drillers) //TODO to jeswt nie tak
            {
                enemy.Drilled += (character, tile) =>
                {
                    Player.Score += tile.Clear(ref _stationsCount);
                    Player.Score+=_ws.Spill(tile.X, tile.Y);
                };
            }
            _dynamicEntities.AddRange(Enemies);
            _dynamicEntities.Add(_player);
        }

        #endregion

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

        public int GainedScore
        {
            get { return Player.Score; }
        }

        public int TotalScore
        {
            get { return InitScore + Player.Score; }
        }

        public int InitLives { get; set; }
        public int InitScore { get; set; }

        public int TotalLives
        {
            get { return Player.LivesCount + InitLives; }
        }

        public void Update()
        {
            if (!_isStarted)
                return;

            foreach (IDynamicEntity dynamicEntity in _dynamicEntities)
                dynamicEntity.Update();

            _topBar.Update(TotalLives, TotalScore, Player.PowerCellsCount);
            for (int i = 0; i < _dynamicEntities.Count; i++)
            {
                IDynamicEntity u1 = _dynamicEntities[i];
                if(!u1.IsWaterProof && u1.OccupiedTile.Accessibility==Accessibility.Water)
                    u1.Harm();
                for (int j = i + 1; j < _dynamicEntities.Count; j++)
                {
                    IDynamicEntity u2 = _dynamicEntities[j];
                    if (_collisionDetector.CheckCollision(u1, u2))
                    {
                        Logger.Log("Collision Detected");
                        u1.CollideWith(u2);
                        u2.CollideWith(u1);
                    }
                }
            }
            if (_newlyAddedDynamicEntities.Count != 0)
            {
                _dynamicEntities.AddRange(_newlyAddedDynamicEntities);
                _dynamicEntities.Sort(new ZIndexComparer());
            }
            _newlyAddedDynamicEntities.Clear();

            _dynamicEntities.RemoveAll(character => character.IsToRemove);

            CheckProgress();
        }

        private void CheckProgress()
        {
            if (StationTiles.TrueForAll(t=>t.Accessibility==Accessibility.Free))
                RaiseLevelAccomplished(true);
            if (TotalLives == 0)
                RaiseLevelAccomplished(false);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in Board)
                tile.Draw(gameTime, spriteBatch);
            foreach (var dynamicEntity in _dynamicEntities)
                dynamicEntity.Draw(gameTime, spriteBatch);
            _topBar.Draw(spriteBatch);
        }

        private void RaiseLevelAccomplished(bool b)
        {
            if (LevelAccomplished != null)
                LevelAccomplished(this, b);
            Logger.Log(b ? "level won" : "level lost");
        }

        public event Action<Level, bool> LevelAccomplished;
    }

    class ZIndexComparer : IComparer<IDynamicEntity>
    {
        public int Compare(IDynamicEntity x, IDynamicEntity y)
        {
            if (x.ZIndex < y.ZIndex)
                return -1;
            return 1;
        }
    }
}