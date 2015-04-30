using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MetroDigger.Gameplay.CollisionDetection;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Gameplay.GameObjects;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Buffer = MetroDigger.Gameplay.Entities.Terrains.Buffer;

namespace MetroDigger.Gameplay
{
    internal class Level
    {
        //private readonly List<Bullet> _bullets;
        private readonly CollisionDetector _collisionDetector;
        private readonly int _height;
        private readonly TopBar _topBar;
        private readonly int _width;

        public static Vector2 GravityVector = new Vector2(0, 1);

        public List<Character> Enemies = new List<Character>();
        public List<Character> Characters = new List<Character>();
        public List<Character> NewlyAddedCharacters = new List<Character>();
        public Board Board;
        private bool _isStarted;
        private Player _player;

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

        public int StationsCount;

        #region LoadFromSave

        public Level(int width, int height)
        {
            StationsCount = 0;
            _width = width;
            _height = height;
            float h = (float)MediaManager.Instance.Height / height;
            float w = (float)MediaManager.Instance.Width / width;
            float min = Math.Min(h, w);
            Tile.Size = new Vector2(min, min);
            MediaManager.Instance.Scale = new Vector2(min / 300, min / 300);
            _isStarted = false;
            Board = new Board(Width, Height);
            //_bullets = new List<Bullet>();
            _topBar = new TopBar();
            _collisionDetector = new RectangleDetector();
        }

        public void RegisterPlayer(Player p)
        {
            _player = p;
            Player.Shoot += (sender, bullet) =>
            {
                bullet = new Bullet(new StraightDriver(Tile.Size, Board), sender);
                bullet.Update();
                bullet.Hit += (bullet1, tile) =>
                {
                    bool b;
                    Player.Score += tile.Clear(ref StationsCount, out b);
                    bullet1.IsToRemove = b;
                };
                NewlyAddedCharacters.Add(bullet);
            };

            Player.Visited += (character, tile1, tile2) =>
            {
                if (tile2.Item != null)
                    if (character is ICollector)
                        tile2.Item.GetCollected(character as ICollector);
                int score = tile2.Clear(ref StationsCount);
                if (tile1.Metro is Tunnel && score>0)
                    score += 50;
                Player.Score += score;
            };

            Player.Drilled += (character, tile) => { /*tile.Clear();*/ };
            Board.StartTile = Player.OccupiedTile;
        }

        public void RegisterEnemies()
        {
            foreach (Character enemy in Enemies) //TODO to jeswt nie tak
            {
                enemy.Drilled += (character, tile) => Player.Score += tile.Clear(ref StationsCount);
            }
            Characters.AddRange(Enemies);
            //Characters.AddRange(_bullets);
            Characters.Add(_player);
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            if (!_isStarted)
                return;

            foreach (Character character in Characters)
            {
                character.Update();
            }

            //_player.Update();

            //foreach (Bullet bullet in _bullets)
            //{
            //    bullet.Update();
            //}

            //foreach (Character enemy in Enemies)
            //{
            //    enemy.Update();
            //}


            _topBar.Update(Player.LivesCount,Player.Score,Player.PowerCellCount);
            for (int i=0;i<Characters.Count;i++)
            {
                for (int j = i+1; j < Characters.Count; j++)
                {
                    Character c1 = Characters[i];
                    Character c2 = Characters[j];
                    if (_collisionDetector.CheckCollision(c1, c2))
                    {
                        Debug.WriteLine("Collision Detected");
                        c1.CollideWith(c2);
                        c2.CollideWith(c1);
                        //Player.Score += enemy.Value;
                    }
                }
            }

            Characters.AddRange(NewlyAddedCharacters);
            NewlyAddedCharacters.Clear();

            Characters.RemoveAll(character => character.IsToRemove);
            //Enemies.RemoveAll(enemy => enemy.IsToRemove);

            CheckProgress();
        }

        private void CheckProgress()
        {
            if (StationsCount == 0)
                RaiseLevelAccomplished(true);
            if(Player.LivesCount==0)
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
            foreach (Character character in Characters)
                character.Draw(gameTime, spriteBatch);
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
        private readonly int _width;
        private readonly int _height;
        private Tile[,] _tiles; 
        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new Tile[width+2,height+2];
            for (int i = 0; i < width+2; i++)
            {
                for (int j = 0; j < height+2; j++)
                {
                    _tiles[i,j] = new Tile(i-1,j-1,new Buffer());
                }
            }
        }
        public Tile this[int x, int y]
        {
            get
            {
                if (x + 1 < 0 || x > _width || y + 1 < 0 || y > _height)
                    return null;
                return _tiles[x+1,y+1];
            }
            set
            {
               _tiles[x+1,y+1] = value;
            }
        }

        public Tile StartTile
        { get; set; }

        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _width;
            return _height;
        }

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

    }
}