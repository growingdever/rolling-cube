using UnityEngine;
using System.Collections;

public class EnemyMissile : Enemy {

	protected int _length;

	public EnemyMissile(GameObject gameObject, TileCoordinate coord, MapManager.MoveDirection dir, int length)
		: base(EnemyType.Missile, gameObject, coord, dir) {
		_length = length;
	}

	override public void Move() {
		base.Move();

		Vector3 dest = new Vector3(_coord._x, 1, _coord._y);
		iTween.MoveTo( _model, 
			iTween.Hash( 
			"position", dest, 
			"time", MapManager.MoveTime,
			"easetype", iTween.EaseType.easeOutCubic
		) );
	}
}