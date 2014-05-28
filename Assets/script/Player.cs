using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Player : MovableTileMapObject
{
	public Player(GameObject playerGameObject, TileCoordinate coord) 
	: base(Type.Player, coord, MapManager.MoveDirection.Left) {
		_model = playerGameObject;
		_model.transform.position = new Vector3 (coord._x, 1, coord._y);
	}

	public void SetDirection(MapManager.MoveDirection dir) {
		_moveDir = dir;
	}
}