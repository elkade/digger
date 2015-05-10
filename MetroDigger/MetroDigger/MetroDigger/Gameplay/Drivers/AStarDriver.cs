using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Logging;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    internal class AStarDriver : Driver
    {
        private readonly IDynamicEntity _chasedEntity;
        private readonly bool _isDriller;

        public AStarDriver(Vector2 unit, Board board, IDynamicEntity chasedEntity, bool isDriller = true)
            : base(unit, board)
        {
            _chasedEntity = chasedEntity;
            _isDriller = isDriller;
            _nextUpdate = DateTime.Now;
        }

        private DateTime _nextUpdate;

        private Tile[] _path;
        private int _i;
        private int _soilWeight = 5;
        int _doShoot = 0;

        public override void UpdateMovement(IMover mh, EntityState state)
        {

            if (state == EntityState.Idle)
            {
                if (_chasedEntity.OccupiedTile == Board.StartTile)
                {
                    _nextUpdate = DateTime.Now + TimeSpan.FromSeconds((new Random()).Next(1, 3));
                    return;
                }
                Tile destTile;
                if (_nextUpdate<DateTime.Now)
                {
                    _nextUpdate = DateTime.Now + TimeSpan.FromSeconds( (new Random()).Next(1, 3));
                    _path = FindPath(Board, mh.StartTile, _chasedEntity.OccupiedTile);
                    _i = 0;
                }
                if (_path!=null&&_i < _path.Length)
                    destTile = _path[_i++];
                else
                {
                    if (!_isDriller)
                        _soilWeight = 5;
                    return;
                }
                if (destTile != null && destTile != mh.StartTile && destTile != Board.StartTile)
                {
                    if (AreNeighbours(destTile, mh.StartTile))
                        //destTile = Board[mh.StartTile.X + (new Random()).Next(2) - 1, mh.StartTile.Y + (new Random()).Next(2) - 1];
                        switch (destTile.Accessibility)
                        {
                            case Accessibility.Free:
                                RaiseMove(destTile);
                                break;
                            case Accessibility.Water:
                                RaiseMove(destTile);
                                break;
                            case Accessibility.Rock:
                                break;
                            case Accessibility.Soil:
                                if (!_isDriller)
                                {
                                    _soilWeight += 5;
                                    _doShoot = 2;
                                }
                                RaiseDrill(destTile);
                                break;
                        }
                }
                if (_doShoot==2)
                {
                    _doShoot = 0;
                    RaiseShoot();
                }
                if (IsStraight(_path))
                    _doShoot ++;

            }
        }

        private bool IsStraight(Tile[] path)
        {
            if (path.Length == 0) return false;
            int x = path[0].X;
            int y = path[0].Y;
            bool isX = true, isY = true;
            for (int i = 1; i < path.Length; i++)
            {
                isX &= path[i].X == x;
                isY &= path[i].Y == y;
            }
            return isX || isY;
        }

        private bool AreNeighbours(Tile destTile, Tile startTile)
        {
            return Math.Abs(destTile.X - startTile.X) <= 1 && Math.Abs(destTile.Y - startTile.Y) == 0 ||
                   Math.Abs(destTile.X - startTile.X) <= 0 && Math.Abs(destTile.Y - startTile.Y) == 1;
        }

        Tile[] FindPath(Board map, Tile s, Tile t)
        {
            int m = map.GetLength(0);
            int n = map.GetLength(1);
            List<Tile> T = new List<Tile>();
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    T.Add(map[i, j]);
            int[,] distance = new int[m, n];
            Tile[,] previous = new Tile[m, n];
            foreach (var tile in map)
            {
                distance[tile.X, tile.Y] = Int16.MaxValue;
                previous[tile.X, tile.Y] = null;
            }
            distance[s.X, s.Y] = 0;
            previous[s.X, s.Y] = s;
            while (T.Count != 0)
            {
                Tile u = null;
                foreach (var uu in T)
                {
                    bool b = T.TrueForAll(w => distance[uu.X, uu.Y] + Estimation(uu, t) <= distance[w.X, w.Y] + Estimation(w, t));
                    if (!b) continue;
                    u = uu;
                    break;
                }
                if (u != null)
                    T.Remove(u);
                if (u == t) break;
                var vicinity = T.Where(v => Math.Abs(u.X - v.X) + Math.Abs(u.Y - v.Y)==1).ToList();
                foreach (var w in vicinity)
                {
                    if (distance[w.X, w.Y] > distance[u.X, u.Y] + Weight(u, w))
                    {
                        distance[w.X, w.Y] = distance[u.X, u.Y] = Weight(u, w);
                        previous[w.X, w.Y] = u;
                    }
                }
            }
            var path = new List<Tile>();
            Tile next = t;
            do
            {
                path.Add(next);
                next = previous[next.X, next.Y];
            } while (next != null && next != s);
            path.Reverse();
            Logger.Log("A* new path created");
            return path.ToArray();
        }

        private int Weight(Tile tile1, Tile tile2)
        {
            if (tile2.Accessibility == Accessibility.Rock)
                return 2500;
            if (tile2.Accessibility == Accessibility.Buffer)
                return 5000;
            if (tile2 == Board.StartTile)
                return 10000;
            if (tile2.Accessibility == Accessibility.Soil)
                return _isDriller?2:_soilWeight;
            return 1;
        }

        private int Estimation(Tile t1, Tile t2)
        {
            return Math.Abs(t1.X - t2.X) + Math.Abs(t1.Y - t2.Y);
        }

    }
}