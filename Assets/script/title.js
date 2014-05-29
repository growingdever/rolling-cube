#pragma strict
var title_Bg:Texture;

var customSkin:GUISkin;

var soundmanager:GameObject;

var btn_w:int = 150;
var btn_h:int = 50;

var choosemenu:boolean;

function Start () {

	soundmanager = GameObject.Find("SoundManager");
	//파일 검사
	if (!title_Bg) {
		Debug.LogError("타이틀 배경이미지 없음");
		return;
	}
	if(!soundmanager){
		Debug.LogError("사운드매니저 연결바람");
		return;
	}
	
	choosemenu = false;
}

function Update () {

}

function OnGUI() {
GUI.skin = customSkin;
	//배경
	GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), title_Bg, ScaleMode.ScaleToFit);

	if(!choosemenu){
		//가로 위치: 화면 중앙, 세로 위치: 화면 3/4
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5,btn_w,btn_h), "START")){
			Debug.Log("게임 시작");
			//게임 시작 클릭
			soundmanager.SendMessage("click");
			
			//Application.LoadLevel(sceneName);
			choosemenu = true;
		}
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5 + btn_h + 10, btn_w, btn_h), "QUIT")){
			Debug.Log("게임 종료");
			//게임 종료 클릭
			Application.Quit();
		}
	} else {
	//choose difficulty
		GUI.Box(Rect(Screen.width/2 - 150,Screen.height/2 - 200,300,400),"Difficulty");
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5 - btn_h* 4.4,btn_w,btn_h), "Easy")){
			Debug.Log("이지");
			//게임 시작 클릭
			soundmanager.SendMessage("click");
			Application.LoadLevel(1);
			choosemenu = false;
		}
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5 - btn_h* 3.3,btn_w,btn_h), "Normal")){
			Debug.Log("노멀");
			//게임 시작 클릭
			soundmanager.SendMessage("click");
			//Application.LoadLevel(1);
			choosemenu = false;
		}
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5 - btn_h* 2.2,btn_w,btn_h), "Hard")){
			Debug.Log("하드");
			//게임 시작 클릭
			soundmanager.SendMessage("click");
			//Application.LoadLevel(1);
			choosemenu = false;
		}
		if(GUI.Button(Rect((Screen.width-btn_w) / 2,((Screen.height-btn_h) / 2) * 1.5,btn_w,btn_h), "Back")){
			soundmanager.SendMessage("click");
			choosemenu = false;
		}
	}
}
