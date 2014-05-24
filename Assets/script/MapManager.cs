using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

	public const int DIR_LEFT = 0;
	public const int DIR_TOP = 1;
	public const int DIR_RIGHT = 2;
	public const int DIR_BOTTOM = 3;
	
	public const int TYPE_PLAYER = 1;
	public const int TYPE_ENEMY = 2;
	
	public const int MAP_WIDTH = 5;
	public const int MAP_HEIGHT = 5;
	private int[,] _map;

	private List<MovableTileMapObject> _movableObjects;
    private Player _player;
	
	public GameObject PlayerPrefab;
	public GameObject EnemyPrefab;
	public GameObject FloorBlock1;
	public GameObject FloorBlock2;

	private bool _isMoving;

	// Use this for initialization
	void Start () {
		_map = new int[MAP_HEIGHT, MAP_WIDTH];

		_movableObjects = new List<MovableTileMapObject> ();

		GameObject playerGameObject = Instantiate (PlayerPrefab,
			new Vector3 (MAP_WIDTH/2, 1, MAP_HEIGHT/2),
			Quaternion.identity) as GameObject;
		_player = new Player (playerGameObject, 
			new TileCoordinate(MAP_WIDTH/2, MAP_HEIGHT/2) );


		_movableObjects.Add (_player);

		int i, j;
		for (i = 0; i < 5; i ++) {
			for (j = 0; j < 5; j++) {
				if( (i+j)%2 == 0 ) {
					GameObject clone = Instantiate (FloorBlock1, 
					             new Vector3 (i, 0, j), 
					             Quaternion.identity) as GameObject;
					clone.transform.localScale.Set( 1.0f, 0.1f, 1.0f );
				}
				else {
					GameObject clone = Instantiate (FloorBlock2, 
					             new Vector3 (i, 0, j), 
					             Quaternion.identity) as GameObject;
					clone.transform.localScale.Set( 1.0f, 0.1f, 1.0f );
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("w")) {

		} else if (Input.GetKeyDown ("a")) {

		} else if (Input.GetKeyDown ("s")) {
			
		} else if (Input.GetKeyDown ("d")) {

		}

		if( Input.GetKeyUp("w") ) {
			MovePlayer(DIR_BOTTOM);
		} else if( Input.GetKeyUp("a") ) {
			MovePlayer(DIR_LEFT);
		} else if( Input.GetKeyUp("s") ) {
			MovePlayer(DIR_TOP);
		} else if( Input.GetKeyUp("d") ) {
			MovePlayer(DIR_RIGHT);
		} else if( Input.GetKeyUp("space") ) {
			AddEnemy();
		}
	}


	public void MoveTileMapObject() {
		foreach( MovableTileMapObject obj in _movableObjects ) {
			TileCoordinate prevCoord = obj.GetCurrCoordinate();
			if( IsValidCoordinate(prevCoord) )
				_map[prevCoord._y, prevCoord._x] = 0;

			obj.Move();
			
			TileCoordinate currCoord = obj.GetCurrCoordinate();
			if( IsValidCoordinate(currCoord) )
				_map[currCoord._y, currCoord._x] = 0;

			Vector3 dest = new Vector3(currCoord._x, 1, currCoord._y);
			switch( obj.GetType() ) {
				case TYPE_PLAYER:
					iTween.MoveTo( obj.GetModel(), 
						iTween.Hash( 
						"position", dest, 
						"time", 1.0f,
						"easetype", iTween.EaseType.easeOutCubic
					) );
					break;
				case TYPE_ENEMY:
					iTween.MoveTo( obj.GetModel(), 
						iTween.Hash( 
						"position", dest, 
						"time", 1.0f,
						"easetype", iTween.EaseType.easeOutCubic
					) );
					break;
			}
		}
	}
	
	public void MovePlayer(int dir) {
		_player.SetDirection(dir);
		TileCoordinate coord = _player.GetNextCoordinate();
		if( ! IsValidCoordinate(coord._x, coord._y) )
			return;
		
		MoveTileMapObject();
		_isMoving = true;
	}
	
	public void CheckMap() {
		
	}
	
	public bool IsValidCoordinate(int x, int y) {
		if( x < 0 || x >= MAP_WIDTH )
			return false;
		
		if( y < 0 || y >= MAP_HEIGHT )
			return false;
		
		return true;
	}
	public bool IsValidCoordinate(TileCoordinate coord) {
		return IsValidCoordinate (coord._x, coord._y);
	}
	
	public void GameOver() {
		
	}

	public void AddEnemy() {
		GameObject clone = Instantiate (EnemyPrefab,
			new Vector3 (0, 1, 0),
			Quaternion.identity) as GameObject;
		Missile missile = new Missile(clone,
			new TileCoordinate(Random.Range(0, 4), Random.Range(0, 4)),
			Random.Range (0, 3));
		_movableObjects.Add(missile);
	}
	
}
