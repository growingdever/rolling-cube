#pragma strict

var bgm : AudioClip;


private static var instance:MusicManager;
public static function GetInstance() : MusicManager {
return instance;
}

public static var muted:boolean = false;

function Start () {
	gameObject.AddComponent(AudioSource);
	playmusic();
}

function Awake() {
    if (instance != null && instance != this) {
        Destroy(this.gameObject);
        return;
    } else {
        instance = this;
    }
    DontDestroyOnLoad(this.gameObject);
}

function playmusic(){
	audio.clip = bgm;
	audio.volume = 50;
	audio.loop = true;
	audio.Play();
}

function soundoff(){
	audio.enabled = false;
	muted = true;
}

function soundon(){
	audio.enabled = true;
	muted = false;
}
