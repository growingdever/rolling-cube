using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableTileMapObject {
	public enum Type {
		Player,
		Enemy
	};
	private Type _type;
	protected List<TileCoordinate> _coordList;
	protected MapManager.MoveDirection _moveDir;
	protected GameObject _model;

	public MovableTileMapObject(Type type, TileCoordinate coord, MapManager.MoveDirection dir)
		: base() {
		_type = type;
		_coordList = new List<TileCoordinate> ();
		_coordList.Add(coord);
		_moveDir = dir;
	}

	public Type GetType() {
		return _type;
	}

	public GameObject GetModel() {
		return _model;
	}

	public virtual void Move() {
		for( int i = 0; i < _coordList.Count; i ++ ) {
			TileCoordinate next = GetNextCoordinate( _coordList[i] );
			_coordList[i]._x = next._x;
			_coordList[i]._y = next._y;
		}
	}

	public virtual void AfterMove (MapManager manager) {

	}

	public List<TileCoordinate> GetCurrCoordinate() {
		return _coordList;
	}
	
	public TileCoordinate GetNextCoordinate(TileCoordinate coord) {
		TileCoordinate ret = new TileCoordinate (coord);
		switch( _moveDir ) {
		case MapManager.MoveDirection.Left:
			ret._x -= 1;
			break;
		case MapManager.MoveDirection.Top:
			ret._y -= 1;
			break;
		case MapManager.MoveDirection.Right:
			ret._x += 1;
			break;
		case MapManager.MoveDirection.Bottom:
			ret._y += 1;
			break;
		}
		
		return ret;
	}

	public bool IsIntersectCoordinate(List<TileCoordinate> list) {
		foreach( TileCoordinate c1 in _coordList ) {
			foreach( TileCoordinate c2 in list ) {
				if( c1.Equals( c2 ) )
					return true;
			}
		}
		return false;
	}
	public bool IsIntersectCoordinate(TileCoordinate coord) {
		foreach( TileCoordinate c1 in _coordList ) {
			if( c1.Equals( coord ) )
				return true;
		}
		return false;
	}
}