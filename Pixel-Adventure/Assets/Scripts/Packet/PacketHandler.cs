using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;
		Managers.Object.Add(enterGamePacket.Player, myPlayer: true);

		Debug.Log("S_EnterGameHandler");
	}
	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGamePacket = packet as S_LeaveGame;
		Managers.Object.RemoveMyPlayer();

		Debug.Log("S_LeaveGameHandler");
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = packet as S_Spawn;

		foreach (PlayerInfo players in spawnPacket.Players)
			Managers.Object.Add(players, myPlayer: false);
			Debug.Log(spawnPacket.Players);	

		Debug.Log("S_SpawnHandler");
	}
	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;

		foreach (int id in despawnPacket.PlayerId)
			Managers.Object.Remove(id);

		Debug.Log("S_DespawnHandler");
	}
	public static void S_MoveHandler(PacketSession session, IMessage packet)
	{
		S_Move movePacket = packet as S_Move;
		ServerSession serverSession = session as ServerSession;

		// 이동 중인 Player 추출
		GameObject go = Managers.Object.FindById(movePacket.PlayerId);
		if (go == null)
			return;

		// PlayerController
		Controller c = go.GetComponent<Controller>();
		if (c == null)
			return;

		c.PosInfo = movePacket.PosInfo;

		Debug.Log(c.PosInfo.PosX);
	}
}
