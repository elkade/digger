using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    internal class AStarDriver : Driver
    {
        private readonly DynamicEntity _chasedEntity;

        public AStarDriver(Vector2 unit, Tile[,] board, DynamicEntity chasedEntity)
            : base(unit, board)
        {
            _chasedEntity = chasedEntity;
        }

        public override void UpdateMovement(MovementHandler mh, EntityState state)
        {

            if (state == EntityState.Idle)
            {
                var path = FindPath(Board, mh.StartTile, _chasedEntity.OccupiedTile);
                var destTile = path[0];
                if(destTile!=null && destTile != mh.StartTile)
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
                            RaiseDrill(destTile);
                            break;
                    }
            }
        }

        Tile[] FindPath(Tile[,] map, Tile s, Tile t)
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
            return path.ToArray();
        }

        private int Weight(Tile tile1, Tile tile2)
        {
            if (tile2.Accessibility == Accessibility.Rock)
                return Int16.MaxValue;
            if (tile2.Accessibility == Accessibility.Soil)
                return 2;
            return 1;
        }

        private int Estimation(Tile t1, Tile t2)
        {
            return Math.Abs(t1.X - t2.X) + Math.Abs(t1.Y - t2.Y);
        }

    }
}