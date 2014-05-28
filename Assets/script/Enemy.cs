using UnityEngine;

public class Enemy : MovableTileMapObject 
{
	public enum EnemyType {
		Missile,
		Drop,
	};

	protected EnemyType _eType;
	protected bool _alive = true;

	public Enemy(EnemyType type, GameObject gameObject, TileCoordinate coord, MapManager.MoveDirection dir)
	: base(MovableTileMapObject.Type.Enemy, coord, dir) {	
		_moveDir = dir;
		_eType = type;
	}

	public static Enemy EnemyCreate(EnemyData data, GameObject prefab) {
		Enemy ret = null;
		switch(data._type) {
			case EnemyType.Missile:
				ret = new EnemyMissile(data, prefab);
				break;
			case EnemyType.Drop:
				ret = new EnemyDrop(data, prefab);
				break;
		}
		return ret;
	}

	public bool IsDead() {
		return ! _alive;
	}
}