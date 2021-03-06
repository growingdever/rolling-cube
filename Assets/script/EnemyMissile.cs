﻿using UnityEngine;
using System.Collections;

public class EnemyMissile : Enemy, IInitializableEnemy {

	protected int _length;

	public EnemyMissile(EnemyData data, GameObject prefab)
		: base(EnemyType.Missile, prefab, new TileCoordinate( data._x, data._y ), data._dir ) {
		Init (data, prefab);
	}

	override public void Move() {
		base.Move();

		Vector3 dest = new Vector3(_coordList[0]._x, 1, _coordList[0]._y);
		iTween.MoveTo( _model, 
			iTween.Hash( 
			"position", dest, 
			"time", MapManager.MoveTime,
			"easetype", iTween.EaseType.easeOutCubic
		) );
	}

	public void Init(EnemyData data, GameObject prefab) {
		_model = MapManager.Instantiate (prefab,
			new Vector3 (data._x, 1, data._y),
			Quaternion.identity) as GameObject;

		// create child like tail to head
		_length = data._length;
		for( int i = 1; i < _length; i ++ ) {
			Vector3 pos = new Vector3(0, 0, 0);
			switch( data._dir ) {
			case MapManager.MoveDirection.Left:
				pos.x = -i;
				break;
			case MapManager.MoveDirection.Right:
				pos.x = i;
				break;
			case MapManager.MoveDirection.Top:
				pos.z = i;
				break;
			case MapManager.MoveDirection.Bottom:
				pos.z = -i;
				break;
			}
			GameObject clone2 = MapManager.Instantiate (prefab,
				new Vector3(),
				Quaternion.identity) as GameObject;
			clone2.transform.parent = _model.transform;
			clone2.transform.localPosition = pos;

			TileCoordinate coord = new TileCoordinate(_coordList[0]._x + (int)pos.x, _coordList[0]._y + (int)pos.z);
			_coordList.Add(coord);
		}
	}
}