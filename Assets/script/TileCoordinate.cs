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

    public override bool Equals(System.Object obj) {
    	if (obj == null)
			return false;

		TileCoordinate coord = obj as TileCoordinate;
		return this.Equals (coord);
    }

	public bool Equals(TileCoordinate coord) {
		if (coord == null)
			return false;
		return this._x == coord._x && this._y == coord._y;
	}

	public override int GetHashCode()
	{
		return _x ^ _y;
	}
}
