#pragma strict
var title_Bg:Texture;

var sceneName:String = "main";


var btn_w:int = 150;
var btn_h:int = 50;

function Start () {
	//파일 검사
	if (!title_Bg) {
		Debug.LogError("타이틀 배경이미지 없음");
		return;
	}
}

function Update () {

}

function OnGUI() {
	//배경
	GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), title_Bg, ScaleMode.ScaleToFit);

	//가로 위치: 화면 중앙, 세로 위치: 화면 3/4
	if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5,btn_w,btn_h), "START")){
		Debug.Log("게임 시작");
		//게임 시작 클릭
		Application.LoadLevel(sceneName);
	}
	if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5 + btn_h,btn_w,btn_h), "QUIT")){
		Debug.Log("게임 종료");
		//게임 종료 클릭
		Application.Quit();
	}
}