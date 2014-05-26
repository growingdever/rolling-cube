using UnityEngine;
using System.Collections;

public class Missile : MovableTileMapObject {
	public Missile(GameObject gameObject, TileCoordinate coord, int dir)
		: base(Type.Enemy, gameObject, coord) {
		_moveDir = dir;
	}
}