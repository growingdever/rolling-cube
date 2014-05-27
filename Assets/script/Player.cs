using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Player : MovableTileMapObject
{
	public Player(GameObject playerGameObject, TileCoordinate coord) 
	: base(Type.Player, playerGameObject, coord, MapManager.MoveDirection.Left) {
	}

	public void SetDirection(MapManager.MoveDirection dir) {
		_moveDir = dir;
	}
}