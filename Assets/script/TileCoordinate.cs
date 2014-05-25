using UnityEngine;
using System.Collections;

public class TileCoordinate {
	public int _x, _y;

    public TileCoordinate(int x, int y) {
        _x = x;
        _y = y;
    }

    public TileCoordinate(TileCoordinate coord) {
        _x = coord._x;
        _y = coord._y;
    }

    public bool equal(TileCoordinate coord) {
    	if( this._x == coord._x && this._y == coord._y ) 
    		return true;
    	return false;
    }
}
