using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Player : MovableTileMapObject
{
	public Player(GameObject playerGameObject, TileCoordinate coord) 
		: base(MapManager.TYPE_PLAYER, playerGameObject, coord) {
	}

	public void SetDirection(int dir) {
		_moveDir = dir;
	}

	override public void Move() {
		base.Move();

		Vector3 copy = _model.transform.position;
		Vector3 currPos = new Vector3(copy.x, copy.y, copy.z);
		float cubeSize = 1.0f;
		Vector3 axis = Vector3.right;
		switch( _moveDir ) {
			case 3:
				currPos.y += -cubeSize/2;
				currPos.z += cubeSize/2;
				axis = Vector3.right;
				break;
			case 1:
				currPos.y += -cubeSize/2;
				currPos.z += -cubeSize/2;
				axis = -Vector3.right;
				break;
			case 0:
				currPos.x += -cubeSize/2;
				currPos.y += -cubeSize/2;
				axis = Vector3.forward;
				break;
			case 2:
				currPos.x += cubeSize/2;
				currPos.y += -cubeSize/2;
				axis = -Vector3.forward;
				break;
		}

		float cubeSpeed = 0.7f;
		StartCoroutine( DoRoll( _model.transform.position, axis, 90.0f, cubeSpeed ) );
	}

	public IEnumerator DoRoll(Vector3 aPoint, Vector3 aAxis, float aAngle, float aDuration) {
		float tSteps = Mathf.Ceil(aDuration * 30.0f);
		float tAngle = aAngle / tSteps;
		Vector3 pos;

		for (var i = 1; i <= tSteps; i++) {
			_model.transform.RotateAround( aPoint, aAxis, aAngle );
			yield return new WaitForSeconds(0.0033333f);
		}

		pos = _model.transform.position;
		pos.y = 1;
		_model.transform.position = pos;

		Vector3 euler = _model.transform.eulerAngles;
		euler.x = Mathf.Round (euler.x / 90.0f) * 90.0f;
		euler.y = Mathf.Round (euler.y / 90.0f) * 90.0f;
		euler.z = Mathf.Round (euler.z / 90.0f) * 90.0f;
		_model.transform.eulerAngles = euler;
	}
}