public class EnemyData {
	public int _turn;
	public int _x, _y;
	public Enemy.EnemyType _type;
	public MapManager.MoveDirection _dir;
	public int _length;
	public int _wait;
	
	public EnemyData(int turn, int x, int y, Enemy.EnemyType type) {
		_turn = turn;
		_x = x;
		_y = y;
		_type = type;
	}
}