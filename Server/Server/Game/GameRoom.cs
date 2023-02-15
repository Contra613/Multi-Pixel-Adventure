using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        List<Player> _players = new List<Player>();

        public void EnterGame(Player newPlayer)
        {
            if (newPlayer == null)
                return;

            lock (_lock)
            {
                _players.Add(newPlayer);
                newPlayer.Room = this;

                // Send MyPlayer
                {
                    S_EnterGame enterPacke = new S_EnterGame();
                    enterPacke.Player = newPlayer.Info;

                    newPlayer.Session.Send(enterPacke);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in _players)
                    {
                        if (newPlayer != p)
                            spawnPacket.Players.Add(p.Info);
                    }
                    newPlayer.Session.Send(spawnPacket);
                }

                // Send Other Players
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Players.Add(newPlayer.Info);

                    foreach (Player p in _players)
                    {
                        if (newPlayer != p)
                            p.Session.Send(spawnPacket);
                    }
                }
            }

            
        }

        public void LeaveGame(int playerId)
        {
            lock (_lock)
            {
                Player player = _players.Find(p => p.Info.PlayerId == playerId);
                if (player == null)
                    return;

                _players.Remove(player);
                player.Room = null;

                // Send MyPlayer
                {
                    S_LeaveGame leavePcket = new S_LeaveGame();
                    player.Session.Send(leavePcket);
                }

                // Send Other Players
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.PlayerId.Add(player.Info.PlayerId);

                    foreach(Player p in _players)
                    {
                        if (player != p)
                            p.Session.Send(despawnPacket);
                    }
                }                
            }
        }

        public void Broadcast(IMessage packet)
        {
            lock(_lock)
            {
                foreach (Player p in _players)
                    p.Session.Send(packet);
            }
        }
    }
}
