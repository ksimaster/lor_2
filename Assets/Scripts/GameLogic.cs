using Assets.Scripts.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class GameLogic
    {
        protected Player[] players;
        protected TileMap tilemap;

        public GameLogic(TileMap tilemap, Player[] players)
        {
            this.players = players;
            this.tilemap = tilemap;
        }

        public Player GetPlayerById(string id)
        {
            return players.Where(x => x.GetPlayerId() == id).First();
        }

        public Player GetCurrentPlayer() => tilemap.GameState.ActivePlayer;

        public void RunNextTurn()
        {
            var playerCntr = (Array.IndexOf(players, GetCurrentPlayer()) + 1) % players.Length;
            tilemap.GameState.ActivePlayer = players[playerCntr];
            // ToDo: check when player already loose
            Preturn();
        }

        public void Preturn()
        {
            var player = GetCurrentPlayer();
            var areas = player.GetAreas(true);
            if (areas.Count == 0 || areas.Where(area => area.Tiles.Length > 1).Count() == 0)
            {
                OnLoose();
                RunNextTurn();
            }

            player.CollectResources();
            player.PaySalaries();
            player.KillUnpaid();
            player.ActivateUnits();
        }

        public void OnLoose()
        {
            //ToDo: Call loose event here.
        }

        public bool IsWin()
        {
            var player = GetCurrentPlayer();
            var areas = player.GetAreas(true);
            if (players.Where(p => p != player).Where(p => p.GetAreas().Where(a => a.Tiles.Length > 1).Count() > 0).Count() == 0)
            {
                // console.log('Player wins!');
                return true;
            }

            return false;
        }

        public void CheckSplitMerge(Tile tile, string prevAreaId)
        {
            if (prevAreaId == tile.GetAreaId())
            {
                return;
            }

            var neighbors = tilemap.GetNeighbors(tile.MapTile.X, tile.MapTile.Y);
            var playerId = tile.GetPlayerId();

            var mergeAreas = new HashSet<string>();
            mergeAreas.Add(tile.GetAreaId());
            var tilesToCheck = new List<Tile>();
            foreach (var neighbor in neighbors)
            {
                if (playerId == neighbor.GetPlayerId() && tile.GetAreaId() != neighbor.GetAreaId())
                {
                    mergeAreas.Add(neighbor.GetAreaId());
                }

                if (neighbor.GetAreaId() == prevAreaId)
                {
                    tilesToCheck.Add(neighbor);
                }
            }

            var mergePlayer = players.Where(x => x.GetPlayerId() == tile.GetPlayerId()).First();

            // Merge processing
            if (mergeAreas.Count > 1)
            {
                mergePlayer.mergeAreas(mergeAreas, tile.GetAreaId());
            }

            var prevAreaParts = prevAreaId.Split('_');
            if (prevAreaParts.Length > 2)
            {
                var sets = new List<HashSet<Tile>>();
                for (var cntr = 0; cntr < tilesToCheck.Count; cntr++)
                {
                    sets.Add(new HashSet<Tile>());
                }

                var splitPlayer = players.Where(x => x.IsPlayerArea(prevAreaId)).First();

                if (sets.Count > 1)
                {
                    var i = -1;
                    foreach (var s in sets)
                    {
                        i++;
                        s.Add(tilesToCheck[i]);
                        var q = new Queue<Tile>();
                        q.Enqueue(tilesToCheck[i]);
                        while (q.Count > 0)
                        {
                            var qitem = q.Dequeue();
                            var neighborsLvl2 = this.tilemap.GetNeighbors(qitem);
                            foreach (var n in neighborsLvl2)
                            {
                                if (n.GetAreaId() == prevAreaId && !(s.Contains(n)))
                                {
                                    s.Add(n);
                                    q.Enqueue(n);
                                }
                            }
                        }
                    }

                    var Splits = new List<List<int>>();
                    var j = -1;
                    foreach (var t in tilesToCheck)
                    {
                        j++;
                        if (sets[0].Contains(t) && Splits.Count > 0)
                        {
                            Splits[0].Add(j);
                            continue;
                        }

                        if (Splits.Count > 1 && sets[Splits[1][0]].Contains(t))
                        {
                            Splits[1].Add(j);
                            continue;
                        }

                        Splits.Add(new List<int> { j });
                    }

                    splitPlayer.SplitAreas(Splits, sets, prevAreaId);
                }
            }
        }
    }
}
