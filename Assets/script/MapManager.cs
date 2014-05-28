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
		Stop,
	};

	public const int MAP_WIDTH = 5;
	public const int MAP_HEIGHT = 5;
	private int[,] _map;
	private GameObject[,] _tiles;

	private List<EnemyData> _enemyDataList;

	private List<MovableTileMapObject> _movableObjects;
    private Player _player;
	
	public GameObject PlayerPrefab;
	public GameObject EnemyPrefab;
	public GameObject FloorBlock1;
	public GameObject FloorBlock2;
	public GameObject FloorBlock3;

	public const float MoveTime = 0.5f;
	private bool _isMoving = false;
	private int _nowTurnCount = 0;
	private RandomGenerator _randomer;

	public GUIText _labelTimer;
	public const float TURN_TIMER = 5.0f;
	private float _turnTimer;

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
		_tiles = new GameObject[MAP_HEIGHT, MAP_WIDTH];
		for (i = -10; i <= 10; i ++) {
			for (j = -10; j <= 10; j++) {

				GameObject clone = null;

				if( 0 <= i && i < MAP_HEIGHT
					&& 0 <= j && j < MAP_WIDTH ) {
					if( (i+j)%2 == 0 ) {
						clone = Instantiate (FloorBlock1, 
							new Vector3 (i, 0, j), 
							Quaternion.identity) as GameObject;
						clone.transform.localScale.Set( 1.0f, 0.1f, 1.0f );
					}
					else {
						clone = Instantiate (FloorBlock1, 
							new Vector3 (i, 0, j), 
							Quaternion.identity) as GameObject;
						clone.transform.localScale.Set( 1.0f, 0.1f, 1.0f );
					}
					_tiles[i, j]=  clone;
				} 
				else {
					clone = Instantiate (FloorBlock3, 
							new Vector3 (i, 0, j), 
							Quaternion.identity) as GameObject;
						clone.transform.localScale.Set( 1.0f, 0.1f, 1.0f );
					clone.transform.Translate( new Vector3(0, -0.3f, 0) );
				}
			}
		}

		_randomer = new RandomGenerator();
		_randomer.AddRange( new RandomGenerator.Range(-5, -3) );
		_randomer.AddRange( new RandomGenerator.Range(7, 9) );

		ReadMap();

		_turnTimer = TURN_TIMER;
	}
	
	// Update is called once per frame
	void Update () {
		_turnTimer -= Time.deltaTime;
		if (_turnTimer < 0) {
			MovePlayer(MoveDirection.Stop);
		}
		_labelTimer.text = string.Format ("{0:0.00}", _turnTimer);
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
				Enemy.EnemyType type = (Enemy.EnemyType)int.Parse( tokens[3] );

				EnemyData d = new EnemyData( turn, x, y, (Enemy.EnemyType)type );
				switch( type ) {
					case Enemy.EnemyType.Missile:
						d._dir = (MoveDirection)int.Parse( tokens[4] );
						d._length = int.Parse( tokens[5] );
						break;
					case Enemy.EnemyType.Drop:
						d._wait = int.Parse( tokens[4] );
						break;
				}
				
				_enemyDataList.Add( d );
			}
		}

		while(true) {
			if( _enemyDataList.Count == 0 )
				break;

			EnemyData d = _enemyDataList[0];
			if( d._turn == _nowTurnCount ) {
				AddEnemy( d );
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

		_turnTimer = TURN_TIMER;

		_isMoving = true;
		Invoke("MoveFinish", MoveTime);
		
		_nowTurnCount++;
		while(true) {
			if( _enemyDataList.Count == 0 )
				break;
			
			EnemyData d = _enemyDataList[0];
			if( d._turn == _nowTurnCount ) {
				AddEnemy( d );
				_enemyDataList.RemoveAt(0);
			} else {
				break;
			}
		}
	}

	public void MoveFinish() {
		_isMoving = false;

		List<Enemy> deleteList = new List<Enemy> ();

		foreach (MovableTileMapObject obj in _movableObjects) {
			obj.AfterMove(this);
			if( obj.GetType() == MovableTileMapObject.Type.Enemy ) {
				Enemy enemy = obj as Enemy;
				if( enemy.IsDead() ) {
					deleteList.Add( enemy );
				}
			}
		}

		foreach (Enemy enemy in deleteList) {
			Destroy( enemy.GetModel() );
			_movableObjects.Remove( enemy );
		}
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
		Application.LoadLevel(0);
	}

	public void AddEnemy(EnemyData data) {
		Enemy enemy = Enemy.EnemyCreate(data, EnemyPrefab);
		_movableObjects.Add(enemy);
	}

	public void Twinkle(TileCoordinate coord) {
		if( IsValidCoordinate( coord ) )
			StartCoroutine ("TwinkleTile", coord);
	}

	public IEnumerator TwinkleTile(TileCoordinate coord) {
		GameObject target = _tiles [coord._y, coord._x];
		Material originMat = target.renderer.material;
		Material newMat = Resources.Load ("mat-warning") as Material;

		target.renderer.material = newMat;
		yield return new WaitForSeconds(0.5f);
		target.renderer.material = originMat;
	}
}
