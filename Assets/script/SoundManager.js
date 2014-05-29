#pragma strict

var click_sound : AudioClip;
var roll_sound : AudioClip;
var dead_sound : AudioClip;

private static var instance:SoundManager;
public static function GetInstance() : SoundManager {
return instance;
}

public var muted:boolean = false;

function Start () {
	gameObject.AddComponent(AudioSource);
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

public function click(){
	audio.PlayOneShot(click_sound);
}

public function roll(){
	audio.PlayOneShot(roll_sound);
}

public function dead(){
	audio.PlayOneShot(dead_sound);
}

function soundoff(){
	audio.enabled = false;
	muted = true;
}

function soundon(){
	audio.enabled = true;
	muted = false;
}
