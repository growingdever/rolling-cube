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

	public float MoveTime = 0.5f;
	private bool _isMoving = false;
	private int _nowTurnCount = 0;
	private RandomGenerator _randomer;

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

		_randomer = new RandomGenerator();
		_randomer.AddRange( new RandomGenerator.Range(-5, -3) );
		_randomer.AddRange( new RandomGenerator.Range(7, 9) );
	}
	
	// Update is called once per frame
	void Update () {

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
						"time", MoveTime,
						"easetype", iTween.EaseType.easeOutCubic
					) );
					break;
				case TYPE_ENEMY:
					iTween.MoveTo( obj.GetModel(), 
						iTween.Hash( 
						"position", dest, 
						"time", MoveTime,
						"easetype", iTween.EaseType.easeOutCubic
					) );
					break;
			}
		}
	}
	
	public void MovePlayer(int dir) {
		if( _isMoving )
			return;

		_player.SetDirection(dir);
		TileCoordinate coord = _player.GetNextCoordinate();
		if( ! IsValidCoordinate(coord._x, coord._y) )
			return;
		
		MoveTileMapObject();
		_isMoving = true;
		Invoke("MoveFinish", MoveTime);
		_nowTurnCount++;
		if( _nowTurnCount % 2 == 0 ) {
			AddEnemy();
		}
	}

	public void MoveFinish() {
		_isMoving = false;
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

		int x, y, dir;
		int v = _randomer.NextInt();
		if( Random.Range(0, 2) == 0 ) {
			x = v;
			y = Random.Range(0, MAP_HEIGHT);
			if( v < 0 )
				dir = DIR_RIGHT;
			else
				dir = DIR_LEFT;
		} else {
			x = Random.Range(0, MAP_WIDTH);
			y = v;
			if( v < 0 )
				dir = DIR_BOTTOM;
			else
				dir = DIR_TOP;
		}

		GameObject clone = Instantiate (EnemyPrefab,
			new Vector3 (0, 1, 0),
			Quaternion.identity) as GameObject;
		Missile missile = new Missile(clone,
			new TileCoordinate(x, y),
			dir);
		_movableObjects.Add(missile);
	}
	
}
