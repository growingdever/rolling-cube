using UnityEngine;
using System.Collections;

public class Missile : MovableTileMapObject {

	protected int _length;

	public Missile(GameObject gameObject, TileCoordinate coord, int dir, int length)
		: base(Type.Enemy, gameObject, coord) {
		_moveDir = dir;
		_length = length;
	}
}