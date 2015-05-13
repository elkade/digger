using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.CollisionDetection;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Logging;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay
{
    /// <summary>
    /// Reprezentuje poziom gry wraz z aktualnym stanem bohatera, obiektów i mapy.
    /// </summary>
    internal class Level
    {
        /// <summary>
        /// Wektor grawitacji działającej na poziomie
        /// </summary>
        public static Vector2 GravityVector = new Vector2(0, 1);
        private readonly ICollisionDetector _collisionDetector;
        private readonly int _height;
        private readonly TopBar _topBar;
        private readonly int _width;
        /// <summary>
        /// Plansza gry
        /// </summary>
        public readonly Board Board;
        /// <summary>
        /// Lista Dynamicznych obiektów gry
        /// </summary>
        public readonly List<IDynamicEntity> DynamicEntities = new List<IDynamicEntity>();
        private readonly List<IDynamicEntity> _newlyAddedDynamicEntities = new List<IDynamicEntity>();
        private int _stationsCount;
        private bool _isStarted;
        private Player _player;
        private readonly ISpiller _ws;
        /// <summary>
        /// Lista kafelków, na których znajduje się znacznik stacji metra
        /// </summary>
        public readonly List<Tile> StationTiles = new List<Tile>();
        #region LoadFromSave
        /// <summary>
        /// Tworzy nową instancję poziomu z planszą o padanej wielkości
        /// </summary>
        /// <param name="width">szerokość planszy</param>
        /// <param name="height">wysokośc planszy</param>
        public Level(int width, int height)
        {
            _stationsCount = 0;
            _width = width;
            _height = height;
            MediaManager.Instance.SetDimensions(_width, _height);
            _isStarted = false;
            Board = new Board(Width, Height);
            _topBar = new TopBar();
            _collisionDetector = new RectangleDetector();
            _ws = new WaterSpiller(Board, GravityVector);

            Logger.Log("New level created");

        }
        /// <summary>
        /// Rejestruje gracza w poziomie
        /// </summary>
        /// <param name="p">Gracz</param>
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

            Player.Drilled += (driller, tile1, tile2) =>
            {
                int score = tile2.Clear(ref _stationsCount);
                if (tile1.Metro is Tunnel && score > 0)
                    score += 50;
                Player.Score += score;
                Player.Score += _ws.Spill(tile2.X, tile2.Y);
            };
        }
        /// <summary>
        /// Rejestruje wrogów w poziomie
        /// </summary>
        public void RegisterEnemies(List<IDynamicEntity> enemies)
        {
            var drillers = enemies.OfType<IDriller>();
            foreach (var enemy in drillers)
            {
                enemy.Drilled += (character, tile1, tile2) =>
                {
                    Player.Score += tile2.Clear(ref _stationsCount);
                    Player.Score+=_ws.Spill(tile2.X, tile2.Y);
                };
            }
            var collectors = enemies.OfType<ICollector>();
            foreach (var enemy in collectors)
            {
                enemy.Visited += (collector, tile1, tile2) =>
                {
                    if (tile2.Item != null)
                        tile2.Item.GetCollected(collector);
                    tile2.Clear(ref _stationsCount);
                };
            }
            var shooters = enemies.OfType<IShooter>();
            foreach (var enemy in shooters)
            {
                enemy.Shoot += sender =>
            {
                var bullet = new Bullet(new StraightDriver(Tile.Size, Board), sender);
                bullet.Update();
                bullet.Hit += (bullet1, tile) =>
                {
                    bool b;
                    tile.Clear(ref _stationsCount, out b);
                    bullet1.IsToRemove = b;
                };
                _newlyAddedDynamicEntities.Add(bullet);
            };

            }
            DynamicEntities.AddRange(enemies);
            DynamicEntities.Add(_player);
        }

        #endregion
        /// <summary>
        /// Szerokość planszy poziomu
        /// </summary>
        public int Width
        {
            get { return _width; }
        }
        /// <summary>
        /// Wysokośc planszy poziomu
        /// </summary>
        public int Height
        {
            get { return _height; }
        }
        /// <summary>
        /// Obiekt gracza
        /// </summary>
        public Player Player
        {
            get { return _player; }
        }
        /// <summary>
        /// Określa, czy poziom został rozpoczęty i odbywa się aktualizacja stanu gry
        /// </summary>
        public bool IsStarted
        {
            set { _isStarted = value; }
        }
        /// <summary>
        /// Określa numer porządkowy poziomu
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Określa uzyskany podczas poziomu wynik punktowy
        /// </summary>
        public int GainedScore
        {
            get { return Player.Score; }
        }
        /// <summary>
        /// Określa cały wynik punktowy uzyskany do tej pory w rozgrywce
        /// </summary>
        public int TotalScore
        {
            get { return InitScore + Player.Score; }
        }
        /// <summary>
        /// Określa liczbę żyć głównrgo gracza w momencie rozpoczęcia poziomu
        /// </summary>
        public int InitLives { get; set; }
        /// <summary>
        /// Określa wynik punktowy w momencie rozpoczęcia poziomu
        /// </summary>
        public int InitScore { get; set; }
        /// <summary>
        /// Określa całkowitą liczbę żyć gracza
        /// </summary>
        public int TotalLives
        {
            get { return Player.LivesCount + InitLives; }
        }
        /// <summary>
        /// Aktualizuje stan poziomy, mapy i obiektów dynamicznych
        /// </summary>
        public void Update()
        {
            if (!_isStarted)
                return;

            foreach (IDynamicEntity dynamicEntity in DynamicEntities)
                dynamicEntity.Update();

            _topBar.Update(TotalLives, TotalScore, Player.PowerCellsCount);
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
                        Logger.Log("Collision Detected");
                        u1.CollideWith(u2);
                        u2.CollideWith(u1);
                        if (u1.IsToRemove) Player.Score += u1.Value;
                        if (u2.IsToRemove) Player.Score += u2.Value;
                    }
                }
            }
            if (_newlyAddedDynamicEntities.Count != 0)
            {
                DynamicEntities.AddRange(_newlyAddedDynamicEntities);
                DynamicEntities.Sort(new ZIndexComparer());
            }
            _newlyAddedDynamicEntities.Clear();

            DynamicEntities.RemoveAll(character => character.IsToRemove);

            CheckProgress();
        }

        private int _nextTreshold = 10000;

        private void CheckProgress()
        {
            if (TotalScore >= _nextTreshold)
            {
                Player.LivesCount++;
                _nextTreshold += 10000;
            }
            if (StationTiles.TrueForAll(t=>t.Accessibility==Accessibility.Free))
                RaiseLevelAccomplished(true);
            if (TotalLives == 0)
                RaiseLevelAccomplished(false);
        }
        /// <summary>
        /// Rysuje elementy mapty, górny pasek i obiekty dynamiczne
        /// </summary>
        /// <param name="gameTime">Aktualny czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in Board)
                tile.Draw(gameTime, spriteBatch);
            foreach (var dynamicEntity in DynamicEntities)
                dynamicEntity.Draw(gameTime, spriteBatch);
            _topBar.Draw(spriteBatch);
        }

        private void RaiseLevelAccomplished(bool b)
        {
            if (LevelAccomplished != null)
                LevelAccomplished(this, b);
            Logger.Log(b ? "level won" : "level lost");
        }
        /// <summary>
        /// Zdarzenie wywoływane w momencie ukończenia poziomu
        /// </summary>
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