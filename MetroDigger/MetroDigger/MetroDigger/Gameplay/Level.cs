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
        private bool _isStarted;
        public Tile[,] Tiles;
        private List<Character> _enemies = new List<Character>();
        private List<Bullet> _bullets;
        private readonly TopBar _topBar;
        private Player _player;

        private readonly CollisionDetector _collisionDetector;

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


        public bool IsStarted { get { return _isStarted; } set { _isStarted = value; }}
        public int Number { get; set; }

        public int StationsCount { get; set; }

        #region LoadFromSave
        public Level(int width, int height)
        {
            StationsCount = 0;
            _width = width;
            _height = height;
            _isStarted = false;
            Tiles = new Tile[Width, Height];
            _bullets = new List<Bullet>();
            _topBar = new TopBar();
            _collisionDetector = new RectangleDetector();

            //----------

        }

        public void RegisterPlayer(Player p)
        {
            _player = p;
            Player.Shoot += (sender, bullet) =>
            {
                bullet = new Bullet(new StraightDriver(Tile.Size, Tiles), sender);
                bullet.Update();
                bullet.Hit += (bullet1, tile) =>
                {
                    if (tile.Accessibility == Accessibility.Free)
                        return;
                    bullet1.IsToRemove = true;
                    tile.Clear();//tu powinno zwrócić punkty
                    //tu dodać punkty bonusowe
                };
                _bullets.Add(bullet);
            };

            Player.Visited += (character, tile1, tile2) =>
            {
                if (tile2.Item != null)
                    if (character is ICollector)
                        tile2.Item.GetCollected(character as ICollector);
                tile2.Clear();
            };

            Player.Drilled += (character, tile) =>
            {
                tile.Clear();
            };
        }

        public void RegisterEnemies()
        {
            _enemies.Add(new Miner(new AStarDriver(Tile.Size, Tiles, Player), Tiles[7, 6]));

            foreach (var enemy in _enemies)//TODO to jeswt nie tak
            {
                enemy.Drilled += (character, tile) => tile.Clear();
            }

        }
        #endregion

        public Level(int width, int height, bool isStarted)
        {
            StationsCount = 0;
            _width = width;
            _height = height;
            _isStarted = isStarted;
            Tiles = new Tile[Width, Height];
            _bullets = new List<Bullet>();
            InitMap();
            _topBar = new TopBar();
            _player = new Player(new KeyboardDriver(Tile.Size, Tiles), Tiles[0, 0]);

            //_miner = new Miner(Tiles[7, 2]);
            //_minerDriver = new AStarDriver(_miner, Tile.Size, Tiles, Player);

            _collisionDetector = new RectangleDetector();
            InitEvents();
            InitEnemies();
        }


        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
                return;

            _player.Update();

            foreach (var bullet in _bullets)
            {
                bullet.Update();
            }

            foreach (var enemy in _enemies)
            {
                enemy.Update();
            }

            _bullets.RemoveAll(bullet => bullet.IsToRemove);
            _enemies.RemoveAll(enemy => enemy.IsToRemove);

            _topBar.Update(Player.Score);
            foreach (var enemy in _enemies)
            {
                foreach (var bullet in _bullets)
                {
                    if (_collisionDetector.CheckCollision(bullet, enemy))
                    {
                        Debug.WriteLine("Miner Detected");
                        bullet.IsToRemove = true;
                        enemy.IsToRemove = true;
                    }

                }
                if (_collisionDetector.CheckCollision(Player, enemy))
                    Debug.WriteLine("Detected");
            }
            CheckProgress();
        }

        private void CheckProgress()
        {
            if (StationsCount == 0)//TODO
                RaiseLevelAccomplished(true);
        }

        private void DrawTiles(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Tiles[x, y].Draw(gameTime, spriteBatch);
                }
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(gameTime, spriteBatch);
            Player.Draw(gameTime, spriteBatch);
            foreach (var bullet in _bullets)
                bullet.Draw(gameTime, spriteBatch);
            foreach (var enemy in _enemies)
                enemy.Draw(gameTime, spriteBatch);
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
                    Tiles[i, j] = new Tile(i, j, new Soil());
                }
            }
            for (int i = 1; i < Width - 1; i++)
            {
                int j = 1;
                Tiles[j, i] = new Tile(j, i, new Rock());
            }
            for (int i = 1; i < Width; i++)
            {
                int j = 2;
                Tiles[j, i].Metro = new Tunnel();
            }
            Tiles[4, 4].Item = new Drill();
            Tiles[5, 5].Item = new PowerCell();
            Tiles[0, 6].Metro = new Station();
            StationsCount++;
            Tiles[4, 6].Metro = new Station();
            StationsCount++;//TODO WTF
        }

        public void InitEvents()
        {
            //Player.Shoot += (sender, bullet) =>
            //{
            //    bullet.Hit += (bullet1, tile) =>
            //    {
            //        if (tile.Accessibility == Accessibility.Free)
            //            return;
            //        bullet1.IsToRemove = true;
            //        tile.Clear();//tu powinno zwrócić punkty
            //        //tu dodać punkty bonusowe
            //    };
            //    _bullets.Add(bullet);
            //};

            //Player.Visited += (character, tile1, tile2) =>
            //{
            //    if (tile2.Item != null)
            //        if (character is ICollector)
            //            tile2.Item.GetCollected(character as ICollector);
            //    tile2.Clear();
            //};

            //Player.Drilled += (character, tile) =>
            //{
            //    tile.Clear();
            //};

            //foreach (var enemy in _enemies)//TODO to jeswt nie tak
            //{
            //    enemy.Drilled += (character, tile) => tile.Clear();
            //}

            Station.Cleared += RemoveStation;
        }

        void RemoveStation()
        {
            StationsCount--;
        }

        private void RaiseLevelAccomplished(bool b)
        {
            if (LevelAccomplished != null)
                LevelAccomplished(this, b);
        }

        public event Action<Level, bool> LevelAccomplished;
    }

}
