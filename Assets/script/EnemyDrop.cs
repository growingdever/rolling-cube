using UnityEngine;
using System.Collections;

public class EnemyDrop : Enemy, IInitializableEnemy {

	private int _count;
	private GameObject _effect;

	public EnemyDrop(EnemyData data, GameObject prefab)
		: base(EnemyType.Drop, prefab, new TileCoordinate( data._x, data._y ), MapManager.MoveDirection.Stop ) {
		Init (data, prefab);
	}

	override public void Move() {
		base.Move ();

		_count--;
		if (_count <= 0) {
			Color color = _model.renderer.material.color;
			color.a = 1.0f;
			_model.renderer.material.color = color;

			Vector3 dest = new Vector3(_coord._x, 1, _coord._y);
			iTween.MoveTo( _model, 
				iTween.Hash( 
				"position", dest, 
				"time", 0.2f,
				"easetype", iTween.EaseType.easeInCubic
			) );
		}
	}

	public void Init(EnemyData data, GameObject prefab) {
		_model = MapManager.Instantiate (prefab,
			new Vector3 (data._x, 1, data._y),
			Quaternion.identity) as GameObject;

		_count = data._wait;

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

	override public void AfterMove(MapManager manager) {
		base.AfterMove (manager);

		if( _count > 0 )
			manager.Twinkle (_coord);

		if (_count <= 0) {
			_alive = false;
		}
	}
}