using UnityEngine;

public class Enemy : MovableTileMapObject 
{
	public enum EnemyType {
		Missile,
		Drop,
	};

	EnemyType _eType;

	public Enemy(EnemyType type, GameObject gameObject, TileCoordinate coord, MapManager.MoveDirection dir)
	: base(MovableTileMapObject.Type.Enemy, gameObject, coord, dir) {	
		_moveDir = dir;

		_eType = type;
	}
}