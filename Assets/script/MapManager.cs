using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapManager : MonoBehaviour {
	public enum MoveDirection {
		Left = 0,
		Top,
		Right,
		Bottom,
	};

	public const int MAP_WIDTH = 5;
	public const int MAP_HEIGHT = 5;
	private int[,] _map;

	private List<EnemyData> _enemyDataList;

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

		//make targetpoint as child of player's cube
		GameObject targetpoint = new GameObject("targetpoint");
		targetpoint.transform.parent = playerGameObject.transform;
		targetpoint.transform.Translate(playerGameObject.transform.position);
		playerGameObject.AddComponent("RollCube");
		targetpoint.AddComponent ("targetPointRotation");


		// Generate floor blocks
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

		ReadMap();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ReadMap() {
		TextAsset data = Resources.Load("data") as TextAsset;
		string content = data.text;
		using( StringReader reader = new StringReader( content ) ) {
			_enemyDataList = new List<EnemyData>();
			string line;
			while( (line = reader.ReadLine()) != null ) {
				string[] tokens = line.Split(' ');
				int turn = int.Parse( tokens[0] );
				int x = int.Parse( tokens[1] );
				int y = int.Parse( tokens[2] );
				int type = int.Parse( tokens[3] );

				EnemyData d = new EnemyData( turn, x, y, type );
				_enemyDataList.Add( d );
			}
		}

		while(true) {
			if( _enemyDataList.Count == 0 )
				break;

			EnemyData d = _enemyDataList[0];
			if( d._turn == _nowTurnCount ) {
				AddEnemy( d._x, d._y, 0, d._type, 2 );
				_enemyDataList.RemoveAt(0);
			} else {
				break;
			}
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
				case MovableTileMapObject.Type.Player:
					// not animating on here
					break;
				case MovableTileMapObject.Type.Enemy:
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
	
	public void MovePlayer(MapManager.MoveDirection dir) {
		if( _isMoving )
			return;

		_player.SetDirection(dir);
		TileCoordinate coord = _player.GetNextCoordinate();
		if( ! IsValidCoordinate(coord._x, coord._y) )
			return;

		MoveTileMapObject();

		// move player on here
		GameObject.Find("prefab-player(Clone)").SendMessage("rollSetup", dir);

		_isMoving = true;
		Invoke("MoveFinish", MoveTime);
		
		_nowTurnCount++;
		while(true) {
			if( _enemyDataList.Count == 0 )
				break;
			
			EnemyData d = _enemyDataList[0];
			if( d._turn == _nowTurnCount ) {
				AddEnemy( d._x, d._y, 0, d._type, 2 );
				_enemyDataList.RemoveAt(0);
			} else {
				break;
			}
		}
	}

	public void MoveFinish() {
		_isMoving = false;
		CheckMap();
	}
	
	public void CheckMap() {
		TileCoordinate playerCoord = _player.GetCurrCoordinate();
		foreach( MovableTileMapObject obj in _movableObjects ) {
			if( obj.GetType() == MovableTileMapObject.Type.Enemy
				&& obj.GetCurrCoordinate().equal( playerCoord ) ) {
				GameOver();
			}
		}
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
		Debug.Log("Game Over");
	}

	public void AddEnemy(int x, int y, MoveDirection dir, int type, int length) {
		GameObject clone = Instantiate (EnemyPrefab,
			new Vector3 (0, 1, 0),
			Quaternion.identity) as GameObject;

		// create child like tail to head
		for( int i = 1; i < length; i ++ ) {
			Vector3 pos = new Vector3(0, 1, 0);
			switch( dir ) {
				case MoveDirection.Left:
					pos.x = -i;
					break;
				case MoveDirection.Right:
					pos.x = i;
					break;
				case MoveDirection.Top:
					pos.z = i;
					break;
				case MoveDirection.Bottom:
					pos.z = -i;
					break;
			}
			GameObject clone2 = Instantiate (EnemyPrefab,
				pos,
				Quaternion.identity) as GameObject;
			clone2.transform.parent = clone.transform;
		}

		EnemyMissile missile = new EnemyMissile(clone,
			new TileCoordinate(x, y),
			dir,
			length);
		_movableObjects.Add(missile);
	}
}
