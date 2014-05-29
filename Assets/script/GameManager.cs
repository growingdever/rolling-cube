using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	int _level;
	public int GetLevel() {
		return _level;
	}
	public void SetLevel(int l) {
		_level = l;
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
