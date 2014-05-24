using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Player : MovableTileMapObject
{
	public Player(GameObject playerGameObject, TileCoordinate coord) 
		: base(MapManager.TYPE_PLAYER, playerGameObject, coord) {
	}

	public void SetDirection(int dir) {
		_moveDir = dir;
	}
}