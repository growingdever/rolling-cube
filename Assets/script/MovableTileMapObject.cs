using UnityEngine;
using System.Collections;

public class MovableTileMapObject : MonoBehaviour {
	private int _type;
    protected TileCoordinate _coord;
    protected int _moveDir;
	protected GameObject _model;

    public MovableTileMapObject(int type, GameObject model, TileCoordinate coord)
		: base() {
        _type = type;
        _model = model;
        _coord = coord;

        _model.transform.position = new Vector3(_coord._x, 1, _coord._y);
    }

    public int GetType() {
        return _type;
    }

	public GameObject GetModel() {
		return _model;
	}

    public virtual void Move() {
        switch( _moveDir ) {
            case MapManager.DIR_LEFT:
                _coord._x -= 1;
                break;
            case MapManager.DIR_TOP:
                _coord._y -= 1;
                break;
            case MapManager.DIR_RIGHT:
                _coord._x += 1;
                break;
            case MapManager.DIR_BOTTOM:
                _coord._y += 1;
                break;
        }
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
            case MapManager.DIR_LEFT:
                coord._x -= 1;
                break;
            case MapManager.DIR_TOP:
                coord._y -= 1;
                break;
            case MapManager.DIR_RIGHT:
                coord._x += 1;
                break;
            case MapManager.DIR_BOTTOM:
                coord._y += 1;
                break;
        }

        return coord;
    }
}
