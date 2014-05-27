using UnityEngine;
using System.Collections;

public class EnemyMissile : Enemy {

	protected int _length;

	public EnemyMissile(GameObject gameObject, TileCoordinate coord, MapManager.MoveDirection dir, int length)
		: base(EnemyType.Missile, gameObject, coord, dir) {
		_length = length;
	}
}