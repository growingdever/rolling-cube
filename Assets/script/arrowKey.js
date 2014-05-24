#pragma strict
//일시정지
var paused:boolean = false;

//화살표 이미지
var btn_NE:Texture;
var btn_NW:Texture;
var btn_SE:Texture;
var btn_SW:Texture;

//배경
var bg_black:Texture;

//메뉴 아이콘
var btn_option:Texture;

var btn_w:int = 50;
var btn_h:int = 50;

var mainCamera:GameObject;
var mapManagerScript;

function Start () {
	//파일 검사
	if (!btn_NE) {
		Debug.LogError("북동 화살표 없음");
		return;
	}
	if (!btn_NW) {
		Debug.LogError("북서 화살표 없음");
		return;
	}
	if (!btn_SE) {
		Debug.LogError("남동 화살표 없음");
		return;
	}
	if (!btn_SW) {
		Debug.LogError("남서 화살표 없음");
		return;
	}
	if (!bg_black) {
		Debug.LogError("검정배경 없음");
		return;
	}
	if (!btn_option) {
		Debug.LogError("옵션 아이콘 없음");
		return;
	}

	mapManagerScript = mainCamera.GetComponent("MapManager");
}

function Update () {
	if(paused){
		Time.timeScale = 0;
	}
}

function OnGUI() {
	if(!paused){
		if(GUI.Button(Rect(Screen.width - 60,Screen.height - 110,btn_w,btn_h),btn_NE)){
			mainCamera.SendMessage("MovePlayer", 2);
		}
		if(GUI.Button(Rect(Screen.width - 110,Screen.height - 110,btn_w,btn_h),btn_NW)){
			mainCamera.SendMessage("MovePlayer", 3);
		}
		if(GUI.Button(Rect(Screen.width - 60,Screen.height - 60,btn_w,btn_h),btn_SE)){
			mainCamera.SendMessage("MovePlayer", 1);
		}
		if(GUI.Button(Rect(Screen.width - 110,Screen.height - 60,btn_w,btn_h),btn_SW)){
			mainCamera.SendMessage("MovePlayer", 0);
		}
		if(GUI.Button(Rect(Screen.width - 60,10,50,50),btn_option)){
			optionMenu();
			paused = true;
		}
	} else {
		GUI.Box(Rect(250,50,300,400),"option / menu");

		if(GUI.Button(Rect(360,100,80,20),"Resume")) {
			paused = false;
		}
	 
		if(GUI.Button(Rect(360,130,80,20),"title")) {
			Application.LoadLevel(0);
		}
	}
}

function optionMenu(){
	var darken = new GameObject();
	darken.AddComponent("GUITexture");
	darken.guiTexture.texture = bg_black;
	darken.guiTexture.color.a = 0.5f;
	darken.transform.position = new Vector3(0.5f ,0.5f, 1.0f);
}