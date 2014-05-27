using UnityEngine;
using System.Collections;

public class EnemyDrop : Enemy {

	private int _count;

	public EnemyDrop(GameObject gameObject, TileCoordinate coord, int count)
		: base(EnemyType.Drop, gameObject, coord, MapManager.MoveDirection.Stop) {
		_count = count;

		// set alpha is not working....
		foreach (Material material in _model.renderer.materials) {	
			material.color = new Color(
				material.color.r, 
				material.color.g, 
				material.color.b, 
				0);
		}

		Vector3 pos = new Vector3 (_model.transform.position.x,
		                           _model.transform.position.y,
		                           _model.transform.position.z);
		pos.y = 10;
		_model.transform.position = pos;
	}

	override public void Move() {
		base.Move ();

		_count--;
		if (_count == 0) {
			Color color = _model.renderer.material.color;
			color.a = 1.0f;
			_model.renderer.material.color = color;

			Vector3 dest = new Vector3(_coord._x, 1, _coord._y);
			iTween.MoveTo( _model, 
				iTween.Hash( 
				"position", dest, 
				"time", 0.1f,
				"easetype", iTween.EaseType.easeInCubic
			) );
		}
	}
}