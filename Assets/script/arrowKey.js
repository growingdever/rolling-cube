#pragma strict
//일시정지
var paused:boolean = false;

var isgameover:boolean = false;

var isclear:boolean = false;

var customSkin:GUISkin;

//화살표 이미지
var btn_NE:Texture;
var btn_NW:Texture;
var btn_SE:Texture;
var btn_SW:Texture;
var btn_nomove:Texture;

//메뉴 아이콘
var btn_option:Texture;

//시간
var img_time:Texture;
var img_time_back:Texture;
var maxTime:float = 5;
var amountTime:float;

var btn_w:int;
var btn_h:int;

var mainCamera:GameObject;
var mapManagerScript;

var soundmanager:GameObject;
var musicmanager:GameObject;
private var muted:boolean = false;

var count:int;

function Start () {

	soundmanager = GameObject.Find("SoundManager");
	musicmanager = GameObject.Find("MusicManager");
	mapManagerScript = mainCamera.GetComponent("MapManager");
	btn_w = Screen.height / 7;
	btn_h = btn_w;
	
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
	if (!btn_nomove) {
		Debug.LogError("턴 넘김 아이콘 없음");
		return;
	}
	if (!btn_option) {
		Debug.LogError("옵션 아이콘 없음");
		return;
	}
	if(!img_time){
		Debug.LogError("시간막대 없음");
		return;
	}
	if(!img_time_back){
		Debug.LogError("시간막대 없음");
		return;
	}
	if(!soundmanager){
		Debug.LogError("사운드매니저 연결바람");
		return;
	}
	
}

function Update () {
	if(paused){
		Time.timeScale = 0;
	}
}

function setTimeBar(length:float){
	amountTime = length;
}


function OnGUI() {
GUI.skin = customSkin;
	//시간막대
	GUI.DrawTexture(new Rect(Screen.width - btn_w*1.2 - maxTime*50, btn_h*0.1, maxTime*50, 16), img_time_back, ScaleMode.StretchToFill);
	GUI.DrawTexture(new Rect(Screen.width - btn_w*1.2 - amountTime*50, btn_h*0.1, amountTime*50, 16), img_time, ScaleMode.StretchToFill);
	
	if(!paused){
		if(GUI.Button(Rect(Screen.width - btn_w*2.2, Screen.height - btn_h*2.2,btn_w,btn_h),btn_NE)){
			mainCamera.SendMessage("MovePlayer", 2);
		}
		if(GUI.Button(Rect(Screen.width - btn_w*3.3, Screen.height - btn_h*2.2,btn_w,btn_h),btn_NW)){
			mainCamera.SendMessage("MovePlayer", 3);
		}
		if(GUI.Button(Rect(Screen.width - btn_w*2.2, Screen.height - btn_h*1.1,btn_w,btn_h),btn_SE)){
			mainCamera.SendMessage("MovePlayer", 1);
		}
		if(GUI.Button(Rect(Screen.width - btn_w*3.3, Screen.height - btn_h*1.1,btn_w,btn_h),btn_SW)){
			mainCamera.SendMessage("MovePlayer", 0);
		}
		if(GUI.Button(Rect(Screen.width - btn_w*1.1, Screen.height - btn_h*2.2,btn_w,btn_h*2.1),btn_nomove)){
			//턴 넘김
			//mainCamera.SendMessage("MovePlayer", 4);
		}
		
		if(GUI.Button(Rect(Screen.width - btn_w*1.1, btn_h*0.1, btn_w,btn_h),btn_option)){
			//optionMenu();
			
			soundmanager.SendMessage("click");
			paused = true;
		}
	
	} else {
	//메뉴
		if(!isgameover){
			if(!isclear){
			GUI.Box(Rect(Screen.width/2 - 150,Screen.height/2 - 150,300,320),"MENU");

			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 - 80,150,50),"Resume")) {
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
			
			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 -20,150,50),"Retry")) {
				Application.LoadLevel(Application.loadedLevel);
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
			
			var ismuted:MusicManager = GetComponent("MusicManager");
		
		 	if (ismuted.muted == false){
				if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +40,150,50),"Mute")) {
					soundmanager.SendMessage("soundoff");
					musicmanager.SendMessage("soundoff");
					muted = true;
				}
			}else{
				if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +40,150,50),"Unmute")) {
					soundmanager.SendMessage("soundon");
					musicmanager.SendMessage("soundon");
					soundmanager.SendMessage("click");
					muted = false;
				}
			}
			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +100,150,50),"Title")) {
				Application.LoadLevel(0);
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
		} else {
		//스테이지 클리어
			GUI.Box(Rect(Screen.width/2 - 150,Screen.height/2 - 150,300,300),"Stage Clear!");
			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +70,150,50),"Next Stage")) {
				isclear = false;
				Application.LoadLevel(Application.loadedLevel+1);
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +10,150,50),"Retry")) {
				isclear = false;
				Application.LoadLevel(Application.loadedLevel);
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
			if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2+70,150,50),"Title")) {
				isclear = false;
				Application.LoadLevel(0);
				paused = false;
				Time.timeScale = 1;
				soundmanager.SendMessage("click");
			}
		}
	} else {
	//game over
		GUI.Box(Rect(Screen.width/2 - 150,Screen.height/2 - 150,300,300),"GAME OVER");
		GUI.Label(Rect(Screen.width/2 - 75,Screen.height/2 - 70,150,50),"Score: " + count.ToString());
		if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2 +10,150,50),"Retry")) {
			isgameover = false;
			Application.LoadLevel(1);
			paused = false;
			Time.timeScale = 1;
			soundmanager.SendMessage("click");
		}
		
		if(GUI.Button(Rect(Screen.width/2 - 75,Screen.height/2+70,150,50),"Title")) {
			isgameover = false;
			Application.LoadLevel(0);
			paused = false;
			Time.timeScale = 1;
			soundmanager.SendMessage("click");
		}
	}
	}
}

function gameover(_count){
	count = _count;
	paused = true;
	isgameover = true;
}

function gameclear(_count){
	count = _count;
	paused = true;
	isclear = true;
}