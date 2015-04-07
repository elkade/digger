using System;
using System.Collections.Generic;
using System.Diagnostics;
using MetroDigger.Gameplay.CollisionDetection;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Gameplay.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay
{
    class Level
    {
        private readonly int _width;
        private readonly int _height;
        private readonly bool _isStarted;
        private readonly Tile[,] _tiles;
        private List<Character> _enemies;
        private List<Driver> _bulletDrivers;
        private readonly TopBar _topBar;
        private readonly Player _player;

        private readonly Driver _playerDriver;

        private readonly Driver _minerDriver;

        private readonly Miner _miner;

        private readonly CollisionDetector _collisionDetector;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Level(int width, int height, bool isStarted)
        {
            _stationsCount = 0;
            _width = width;
            _height = height;
            _isStarted = isStarted;
            _tiles = new Tile[Width, Height];
            _bulletDrivers = new List<Driver>();
            InitMap();
            _topBar = new TopBar();
            _player = new Player(_tiles[0, 0]);

            _miner = new Miner(_tiles[7, 2]);
            _minerDriver = new AStarDriver(_miner, Tile.Size, _tiles, _player);

            _collisionDetector = new RectangleDetector();

            InitEvents();
            _playerDriver = new KeyboardDriver(_player,Tile.Size,_tiles);
            InitEnemies();
        }


        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
                return;

            _playerDriver.UpdateMovement();

            foreach (var driver in _bulletDrivers)
            {
                driver.UpdateMovement();
            }

            _minerDriver.UpdateMovement();

            _bulletDrivers.RemoveAll(driver => (driver.Entity as Bullet).ToRemove);//TODO TODO

            _topBar.Update(_player.Score);

            foreach (var bd in _bulletDrivers)
            {
                if(_collisionDetector.CheckCollision(bd.Entity, _miner))
                {
                    Debug.WriteLine("Miner Detected");
                    (bd.Entity as Bullet).ToRemove = true;
                }
            }
            if(_collisionDetector.CheckCollision(_player,_miner))
                Debug.WriteLine("Detected");

            CheckProgress();
        }

        private void CheckProgress()
        {
            if (_stationsCount == 0)
                RaiseLevelAccomplished(true);
        }

        private void DrawTiles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    _tiles[x, y].Draw(gameTime, spriteBatch);
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);
            foreach (var driver in _bulletDrivers)
            {
                driver.Entity.Draw(gameTime, spriteBatch);
            }
            _miner.Draw(gameTime, spriteBatch);
            _topBar.Draw(spriteBatch);
        }

        private void InitEnemies()
        {
            
        }
        private void InitMap()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _tiles[i, j] = new Tile(i, j, new Soil());
                }
            }
            for (int i = 1; i < Width - 1; i++)
            {
                int j = 1;
                _tiles[j, i] = new Tile(j, i, new Rock());
            }
            for (int i = 1; i < Width; i++)
            {
                int j = 2;
                _tiles[j, i].Metro = new Tunnel();
            }
            _tiles[4, 4].Item = new Drill();
            _tiles[5, 5].Item = new PowerCell();
            _tiles[0, 6].Metro = new Station();
            _stationsCount++;
            _tiles[4, 6].Metro = new Station();
            _stationsCount++;//TODO WTF
        }

        private void InitEvents()
        {
            _player.Shoot += (sender, bullet) =>
            {
                bullet.Hit += (bullet1, tile) =>
                {
                    if (tile.Accessibility == Accessibility.Free)
                        return;
                    bullet1.ToRemove = true;
                    tile.Clear();//tu powinno zwrócić punkty
                    //tu dodać punkty bonusowe
                };
                _bulletDrivers.Add(new StraightDriver(bullet,Tile.Size,_tiles));
            };

            _player.Visited += (character, tile1, tile2) =>
            {
                if (tile2.Item != null)
                    if (character is ICollector)
                        tile2.Item.GetCollected(character as ICollector);
                tile2.Clear();
            };

            _player.Drilled += (character, tile) =>
            {
                tile.Clear();
            };

            _miner.Drilled += (character, tile) => tile.Clear();

            //Station.Created += AddStation;
            Station.Cleared += RemoveStation;
        }

        void AddStation()
        {
            _stationsCount++;
        }

        void RemoveStation()
        {
            _stationsCount--;
        }

        private void RaiseLevelAccomplished(bool b)
        {
            if (LevelAccomplished != null)
                LevelAccomplished(b);
        }

        public event Action<bool> LevelAccomplished;

        private int _stationsCount;
    }

}
