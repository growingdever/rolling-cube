﻿using UnityEngine;
using System.Collections;

public class MovableTileMapObject {
	public enum Type {
		Player,
		Enemy
	};
	private Type _type;
	protected TileCoordinate _coord;
	protected MapManager.MoveDirection _moveDir;
	protected GameObject _model;

	public MovableTileMapObject(Type type, TileCoordinate coord, MapManager.MoveDirection dir)
		: base() {
		_type = type;
		_coord = coord;
		_moveDir = dir;
	}

	public Type GetType() {
		return _type;
	}

	public GameObject GetModel() {
		return _model;
	}

	public virtual void Move() {
		switch( _moveDir ) {
			case MapManager.MoveDirection.Left:
				_coord._x -= 1;
				break;
			case MapManager.MoveDirection.Top:
				_coord._y -= 1;
				break;
			case MapManager.MoveDirection.Right:
				_coord._x += 1;
				break;
			case MapManager.MoveDirection.Bottom:
				_coord._y += 1;
				break;
		}
	}

	public virtual void AfterMove () {

	}

	public TileCoordinate GetCurrCoordinate() {
		return _coord;
	}

	public void SetCoordinate(TileCoordinate coord) {
		_coord = coord;
	}

	public TileCoordinate GetNextCoordinate() {
		TileCoordinate coord = new TileCoordinate(_coord);
		switch( _moveDir ) {
			case MapManager.MoveDirection.Left:
				coord._x -= 1;
				break;
			case MapManager.MoveDirection.Top:
				coord._y -= 1;
				break;
			case MapManager.MoveDirection.Right:
				coord._x += 1;
				break;
			case MapManager.MoveDirection.Bottom:
				coord._y += 1;
				break;
		}

		return coord;
	}
}