using UnityEngine;
using System.Collections;

public class Missile : MovableTileMapObject {
	public Missile(GameObject gameObject, TileCoordinate coord, int dir)
		: base(MapManager.TYPE_ENEMY, gameObject, coord) {
		_moveDir = dir;
	}
}