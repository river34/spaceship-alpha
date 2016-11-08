using UnityEngine;
using System.Collections;

public class RootController : MonoBehaviour {
    
	// reference to controllers
    private DMXController _DMXController;
    private FloorController _FloorController;
	private GUIController _GUIController;
    private Camera _CameraLeft;
    private Camera _CameraFront;
    private Camera _CameraRight;
	private Transform _CameraGroup;
    private Transform _Cave;
    private Light _Light_1;
    private Light _Light_2;
    private GameObject _UI;
    private GameObject _UI_Text;
	private Renderer _Text;
	private GameObject _UI_Video_0;
    private GameObject _UI_Video_1;
	private GameObject _UI_Video_2;
	private GameObject _StarTravelFront;
	private GameObject _StarTravelLeft;
	private GameObject _StarTravelRight;
	private Renderer _Video_0;
    private Renderer _Video_1;
	private Renderer _Video_2;
	private Renderer _Video_3;
	private Renderer _Video_4;
	private Renderer _Video_5;
    private GameObject _UI_Script_1;
    private GameObject _UI_Script_2;
    private Renderer _Script_1;
    private Renderer _Script_2;
    private AudioSource _Power_Down;
    private AudioSource _Power_Up;
    private AudioSource _Warning_Alarm;
    private AudioSource _Attack;
    private AudioSource _Attack_1;
    private AudioSource _Attack_2;
	private AudioSource _Attack_3;
	private AudioSource _Attack_4;
	private AudioSource _Attack_5;
    private AudioSource _Decode_Success;
	private AudioSource _Decode_Failed;
	private AudioSource _Decoding;
	private AudioSource _Shake;
	private AudioSource _Tap;
	private AudioSource _Establish;
	private AudioSource _Video_Sound_0;
    private AudioSource _Video_Sound_1;
	private AudioSource _Video_Sound_2;
	private AudioSource _Video_Sound_End;
    private Fade _Fade;
    private Texture2D[] _Texture_Script;
    private Texture2D[] _Texture_Text_Trans;
    private Texture2D[] _Texture_Text_Decoding;
    private Texture2D[] _Texture_Text_Radio;
    private Texture2D[] _Texture_Text_Connected;
    private Texture2D[] _Texture_Text_Establishing;
	private Texture2D[] _Texture_Text_Warning;
	private Texture2D[] _Texture_Ship_1;
	private Texture2D[] _Texture_Ship_2;
	private Texture2D[] _Texture_Ship_3;
    private GameObject _UI_Credits_1;
    private GameObject _UI_Credits_2;
	private GameObject _UI_Credits_3;
	private GameObject _UI_Skymap_1;
	private GameObject _UI_Skymap_2;
	private GameObject _UI_Main_Text;
	private GameObject _UI_Guest;
	private GameObject _UI_Meta;
	private Renderer _Main_Text;
	private GameObject _UI_Ship;
	private Renderer _Ship;
	private GameObject _UI_Main_Ship;
	private Renderer _Main_Ship;

    // shake floor
    private float _shake_intival = 1f;
    private int _shake_num = 6;
	private int _current_shake_num = 0;
	private float _shake_voltage = 10f;
	const float _max_voltage = 10f;
    private bool _done_shaking = true;
    private int _shake_dir = 1;

    // alarm light
    private float _alarm_intival = 1f;
    private int _alarm_num = 6;
    private int _current_alarm_num = 0;
    private bool _done_alarming = true;

    // atack (... or be attacked)
	private bool _done_attacking = true;
	private float _attack_delay = .2f;

	// game time
    private float _game_time;

    // shake camera
    private Vector3 originPosition;
    private Quaternion originRotation;
    private float shake_intensity = .3f;
    private float shake_decay = 0;

    // drive
    private float _drive_speed = 1000f;

    // auto-pilot
    private float _auto_pilot_speed = 1000f;
    private Transform _Destination;
    private bool _auto_piloting = false;

	// video
	private bool _done_playing_video_0 = false;
    private bool _done_playing_command_video_1 = false;
    private bool _done_playing_command_video_2 = false;
	private bool _done_playing_startravel = false;

    // state control
    public float _pause_time = 0;

    // first attack 
	private bool _done_first_attack = false;
	private bool _done_first_attack_a = false;
	private bool _done_first_attack_b = false;
	private bool _done_first_attack_c = false;

    // mojor attack
    private bool _done_major_attack = false;

    // UI script animation
	private float changeInterval_script = 0.1F;
	private float changeInterval_text = 0.5f;
	private float changeInterval_main = 0.5f;

    // Light
    private float originalIntensity_Light_1;
    private float originalIntensity_Light_2;

    // end game
    private bool _is_end = false;

    // text
    private bool _is_decoding = false;
    private bool _is_video = false;
    private bool _is_radio = false;

	// state
	private bool _done_state = false;

	// UI ship status
	private int _ship_status = 0;
	private bool _is_status = false;

	// UI meta text
	private string _meta;

	// shake
	private float _last_shake_time = 0;

    void Awake()
    {
        _DMXController = GameObject.Find ("DMXController").GetComponent<DMXController>();
        _FloorController = GameObject.Find ("FloorController").GetComponent<FloorController>();
		_GUIController = GetComponent<GUIController> ();
        _CameraLeft = GameObject.Find ("CameraLeft").GetComponent<Camera>();
        _CameraFront = GameObject.Find ("CameraFront").GetComponent<Camera>();
		_CameraRight = GameObject.Find ("CameraRight").GetComponent<Camera>();
		_CameraGroup = GameObject.Find ("CameraNode").transform;
        _Cave = GameObject.Find("CaveBoxSetup").transform;
        _Destination = GameObject.Find("Destination").transform;
        _Light_1 = GameObject.Find("Light_1").GetComponent<Light>();
        _Light_2 = GameObject.Find("Light_2").GetComponent<Light>();
        _UI = GameObject.Find("UI");
		_UI_Text = GameObject.Find("UI_Text_1");
		_Text = _UI_Text.GetComponent<Renderer>();
		_UI_Video_0 = GameObject.Find("UI_Video_0");
        _UI_Video_1 = GameObject.Find("UI_Video_1");
		_UI_Video_2 = GameObject.Find("UI_Video_2");
		_StarTravelFront = GameObject.Find("StarTravelFront");
		_StarTravelLeft = GameObject.Find("StarTravelLeft");
		_StarTravelRight = GameObject.Find("StarTravelRight");
		_Video_0 = _UI_Video_0.GetComponent<Renderer>();
        _Video_1 = _UI_Video_1.GetComponent<Renderer>();
		_Video_2 = _UI_Video_2.GetComponent<Renderer>();
		_Video_3 = _StarTravelFront.GetComponent<Renderer>();
		_Video_4 = _StarTravelLeft.GetComponent<Renderer>();
		_Video_5 = _StarTravelRight.GetComponent<Renderer>();
        _UI_Script_1 = GameObject.Find("UI_Script_1");
        _UI_Script_2 = GameObject.Find("UI_Script_2");
        _Script_1 = _UI_Script_1.GetComponent<Renderer>();
        _Script_2 = _UI_Script_2.GetComponent<Renderer>();
        _Power_Down = GameObject.Find("Power_Down").GetComponent<AudioSource>();
        _Power_Up = GameObject.Find("Power_Up").GetComponent<AudioSource>();
        _Warning_Alarm = GameObject.Find("Warning_Alarm").GetComponent<AudioSource>();
        _Attack = GameObject.Find("Attack").GetComponent<AudioSource>();
        _Attack_1 = GameObject.Find("Attack_1").GetComponent<AudioSource>();
        _Attack_2 = GameObject.Find("Attack_2").GetComponent<AudioSource>();
		_Attack_3 = GameObject.Find("Attack_3").GetComponent<AudioSource>();
		_Attack_4 = GameObject.Find("Attack_4").GetComponent<AudioSource>();
		_Attack_5 = GameObject.Find("Attack_5").GetComponent<AudioSource>();
        _Decoding = GameObject.Find("Decoding").GetComponent<AudioSource>();
		_Decode_Success = GameObject.Find("Decode_Success").GetComponent<AudioSource>();
		_Decode_Failed = GameObject.Find("Decode_Failed").GetComponent<AudioSource>();
		_Shake = GameObject.Find("Shake").GetComponent<AudioSource>();
		_Tap = GameObject.Find("Tap").GetComponent<AudioSource>();
		_Establish = GameObject.Find("Establish").GetComponent<AudioSource>();
		_Video_Sound_0 = GameObject.Find("Video_Sound_0").GetComponent<AudioSource>();
        _Video_Sound_1 = GameObject.Find("Video_Sound_1").GetComponent<AudioSource>();
		_Video_Sound_2 = GameObject.Find("Video_Sound_2").GetComponent<AudioSource>();
		_Video_Sound_End = GameObject.Find("Video_Sound_End").GetComponent<AudioSource>();
        _Fade = GetComponent<Fade>();
        _Texture_Script = Resources.LoadAll<Texture2D>("Textures/UI_Script");
        _Texture_Text_Trans = Resources.LoadAll<Texture2D>("Textures/UI_Trans");
        _Texture_Text_Decoding = Resources.LoadAll<Texture2D>("Textures/UI_Decoding");
        _Texture_Text_Radio = Resources.LoadAll<Texture2D>("Textures/UI_Radio");
        _Texture_Text_Connected = Resources.LoadAll<Texture2D>("Textures/UI_Connected_Big");
		_Texture_Text_Establishing = Resources.LoadAll<Texture2D>("Textures/UI_Establishing_Big");
		_Texture_Text_Warning = Resources.LoadAll<Texture2D>("Textures/UI_Warning_Big");
		_Texture_Ship_1 = Resources.LoadAll<Texture2D>("Textures/UI_Ship_1");
		_Texture_Ship_2 = Resources.LoadAll<Texture2D>("Textures/UI_Ship_2");
		_Texture_Ship_3 = Resources.LoadAll<Texture2D>("Textures/UI_Ship_4");
        _UI_Credits_1 = GameObject.Find("UI_Credits_1");
        _UI_Credits_2 = GameObject.Find("UI_Credits_2");
		_UI_Credits_3 = GameObject.Find("UI_Credits_3");
		_UI_Skymap_1 = GameObject.Find("UI_Skymap_1");
		_UI_Skymap_2 = GameObject.Find("UI_Skymap_2");
        _UI_Main_Text = GameObject.Find("UI_Main_Text");
		_UI_Guest = GameObject.Find ("UI_Guest");
		_UI_Meta = GameObject.Find ("UI_Meta");
        _Main_Text = _UI_Main_Text.GetComponent<Renderer>();
		_UI_Ship = GameObject.Find("UI_Ship_1");
		_Ship = _UI_Ship.GetComponent<Renderer>();
		_UI_Main_Ship = GameObject.Find ("UI_Main_Ship");
		_Main_Ship = _UI_Main_Ship.GetComponent<Renderer>();

		_UI_Text.SetActive(false);
		_UI_Video_0.SetActive(false);
        _UI_Video_1.SetActive(false);
		_UI_Video_2.SetActive(false);
		_StarTravelFront.SetActive(false);
		_StarTravelLeft.SetActive(false);
		_StarTravelRight.SetActive(false);
        _UI_Credits_1.SetActive(false);
        _UI_Credits_2.SetActive(false);
		_UI_Credits_3.SetActive(false);
		_UI_Skymap_1.SetActive(false);
		_UI_Skymap_2.SetActive(false);
		_UI_Main_Text.SetActive(false);
		_UI_Guest.SetActive (false);
		_UI_Ship.SetActive (false);
		_UI_Main_Ship.SetActive (false);

        originalIntensity_Light_1 = _Light_1.intensity;
        originalIntensity_Light_2 = _Light_2.intensity;

		_meta = _UI_Meta.GetComponent<GUIText> ().text;
    }

    void Start ()
    {
        // FirstAttack();

        // MajorAttack();

        ShowScript ();
    }

    void Update()
    {
		_UI_Meta.GetComponent<GUIText> ().text = _meta + " "
			+ System.DateTime.Now.Month + "/" + System.DateTime.Now.Day + "/" + "2040 "
			+ System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second;

        _game_time += Time.deltaTime;

        if (_pause_time > 0)
        {
            _pause_time -= Time.deltaTime;
        }

        if (_pause_time <= 0)
        {
            _pause_time = 0;
        }

        /*
        // attack
        if (Input.GetKeyDown (KeyCode.Tab))
        {
            Debug.Log ("key Tab is down");
            if (!_done_first_attack)
            {
                Invoke ("FirstAttack", 1);   // need to give the networking time to connect
            }
            else if (!_done_major_attack)
            {
                Invoke ("MajorAttack", 1);   // need to give the networking time to connect
            }
        }
        */

        // drive
		/*
		if (Input.GetKey (KeyCode.RightArrow))
        {
            if (!_auto_piloting)
            {
                Debug.Log("key RightArrow is down");
                _Cave.position += Vector3.right * _drive_speed * Time.deltaTime;
            }
        }
        if (Input.GetKey (KeyCode.LeftArrow))
        {
            if (!_auto_piloting)
            {
                Debug.Log("key LeftArrow is down");
                _Cave.position += Vector3.left * _drive_speed * Time.deltaTime;
            }
        }
        */

        /*
        if (Input.GetKey (KeyCode.UpArrow))
        {
            if (!_auto_piloting)
            {
                Debug.Log("key UpArrow is down");
                _Cave.position += Vector3.up * _drive_speed * Time.deltaTime;
            }
        }
        if (Input.GetKey (KeyCode.DownArrow))
        {
            if (!_auto_piloting)
            {
                Debug.Log("key DownArrow is down");
                _Cave.position += Vector3.down * _drive_speed * Time.deltaTime;
            }
        }
        */

        /*
        // auto pilot
        if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            Debug.Log("key LeftShift is down");
            if (!_auto_piloting)
            {
                _auto_piloting = true;
                float step = _auto_pilot_speed * Time.deltaTime;
                StartCoroutine (AutoPilot (step));
            }
            else
            {
                _auto_piloting = false;
            }
        }
        */

        /*
        // play command video 1
        if (Input.GetKeyDown (KeyCode.LeftAlt))
        {
            Debug.Log ("key LeftAlt is down");
			PlayVideo (1);
        }

        if (_pause_time <= 0)
        {
            //_UI_Text.SetActive(false);
            _UI_Video_1.SetActive(false);
            _UI_Video_2.SetActive(false);
        }
        */
    }

    public void ShowScript ()
    {
        StartCoroutine(ShowScriptRoutine());
    }

    IEnumerator ShowScriptRoutine ()
    {
        while (true)
        {
			int index = Mathf.FloorToInt(Time.time / changeInterval_script);
            index = index % _Texture_Script.Length;
            _Script_1.material.mainTexture = _Texture_Script[index];
            index += 10;
            index = index % _Texture_Script.Length;
            _Script_2.material.mainTexture = _Texture_Script[index];
            yield return null;
        }
    }

    public void ShowTextTransmitting()
	{
		_UI_Text.SetActive (true);
        StartCoroutine(ShowTextTransmittingRoutine());
	}

	public void StopTextTransmitting()
	{
		_UI_Text.SetActive (false);
	}

    IEnumerator ShowTextTransmittingRoutine()
    {
        while (_UI_Text.activeSelf)
        {
			int index = Mathf.FloorToInt(Time.time / changeInterval_text);
            index = index % _Texture_Text_Trans.Length;
            _Text.material.mainTexture = _Texture_Text_Trans[index];
            yield return null;
        }
    }

    public void ShowTextDecoding()
    {
        _is_video = false;
        _is_decoding = true;
        _UI_Text.SetActive(true);
        StartCoroutine(ShowTextDecodingRoutine());
    }

    public void StopTextDecodingVideo()
    {
        _is_decoding = false;
        _UI_Text.SetActive(false);
    }

    IEnumerator ShowTextDecodingRoutine()
    {
        while (_is_decoding && _UI_Text.activeSelf)
        {
            int index = Mathf.FloorToInt(Time.time / changeInterval_text);
            index = index % _Texture_Text_Decoding.Length;
            _Text.material.mainTexture = _Texture_Text_Decoding[index];
            yield return null;
        }
    }

    public void ShowTextRadio()
    {
        _is_video = false;
        _is_radio = true;
        _UI_Text.SetActive(true);
        StartCoroutine(ShowTexRadioRoutine());
    }

    public void StopTextRadio()
    {
        _is_radio = false;
        _UI_Text.SetActive(false);
    }

    IEnumerator ShowTexRadioRoutine()
    {
        while (_is_radio && _UI_Text.activeSelf)
        {
            int index = Mathf.FloorToInt(Time.time / changeInterval_text);
            index = index % _Texture_Text_Radio.Length;
            _Text.material.mainTexture = _Texture_Text_Radio[index];
            yield return null;
        }
    }

    public void ShowMainTextConnected(float time)
    {
        _pause_time += time;
        _UI_Main_Text.SetActive(true);
        StartCoroutine(ShowMainTextConnectedRoutine());
    }

    public void StopMainTextConnected()
    {
        _UI_Main_Text.SetActive(false);
    }

    IEnumerator ShowMainTextConnectedRoutine()
    {
        while (_UI_Main_Text.activeSelf)
        {
            int index = Mathf.FloorToInt(Time.time / changeInterval_text);
            index = index % _Texture_Text_Connected.Length;
            _Main_Text.material.mainTexture = _Texture_Text_Connected[index];
            yield return null;
        }
    }

    public void ShowMainTextEstablishing(float time)
    {
        _pause_time += time;
        _UI_Main_Text.SetActive(true);
        StartCoroutine(ShowMainTextEstablishingRoutine());
    }

    public void StopMainTextEstablishing()
    {
        _UI_Main_Text.SetActive(false);
    }

    IEnumerator ShowMainTextEstablishingRoutine()
    {
        while (_UI_Main_Text.activeSelf)
        {
            int index = Mathf.FloorToInt(Time.time / changeInterval_text);
            index = index % _Texture_Text_Establishing.Length;
            _Main_Text.material.mainTexture = _Texture_Text_Establishing[index];
            yield return null;
        }
	}

	public void ShowMainTextWarning(float time)
	{
		_pause_time += time;
		_UI_Main_Text.SetActive(true);
		StartCoroutine(ShowMainTextWarningRoutine());
	}

	public void StopMainTextWarning()
	{
		_UI_Main_Text.SetActive(false);
	}

	IEnumerator ShowMainTextWarningRoutine()
	{
		while (_UI_Main_Text.activeSelf)
		{
			int index = Mathf.FloorToInt(Time.time / changeInterval_main);
			index = index % _Texture_Text_Warning.Length;
			_Main_Text.material.mainTexture = _Texture_Text_Warning[index];
			yield return null;
		}
	}

	public void ShowShipStatusMain ()
	{
		_UI_Main_Ship.SetActive (true);
		if (!_is_status)
		{
			_is_status = true;
			StartCoroutine(ShowShipStatusRoutine());
		}
	}

	public void HideShipStatusMain ()
	{
		_UI_Main_Ship.SetActive (false);
	}

	public void ShowShipStatus()
	{
		_UI_Ship.SetActive (true);
		if (!_is_status)
		{
			_is_status = true;
			StartCoroutine(ShowShipStatusRoutine());
		}
	}

	public void UpdateShipStatus ()
	{
		_ship_status ++;
	}

	IEnumerator ShowShipStatusRoutine ()
	{
		while (_is_status)
		{
			if (_ship_status == 1)
			{
				int index = Mathf.FloorToInt(Time.time / changeInterval_main);
				index = index % _Texture_Ship_1.Length;
				_Ship.material.mainTexture = _Texture_Ship_1[index];
				_Main_Ship.material.mainTexture = _Texture_Ship_1[index];
				yield return null;
			}
			else if (_ship_status == 2)
			{
				int index = Mathf.FloorToInt(Time.time / changeInterval_main);
				index = index % _Texture_Ship_2.Length;
				_Ship.material.mainTexture = _Texture_Ship_2[index];
				_Main_Ship.material.mainTexture = _Texture_Ship_2[index];
				yield return null;
			}
			else if (_ship_status == 3)
			{
				int index = Mathf.FloorToInt(Time.time / changeInterval_main);
				index = index % _Texture_Ship_3.Length;
				_Ship.material.mainTexture = _Texture_Ship_3[index];
				_Main_Ship.material.mainTexture = _Texture_Ship_3[index];
				yield return null;
			}

			yield return null;
		}
	}

	public void StopVideo (int index)
	{
		if (index == 0)
		{
			MovieTexture movie = (MovieTexture)_Video_0.material.mainTexture;
			_UI_Video_0.SetActive (false);
			movie.Stop ();
			_Video_Sound_0.Stop ();
			_pause_time = 0;
		}
		else if (index == 1)
		{
			MovieTexture movie = (MovieTexture)_Video_1.material.mainTexture;
			_UI_Video_1.SetActive (false);
			movie.Stop ();
			_Video_Sound_1.Stop ();
			_pause_time = 0;
		}
		else if (index == 2)
		{
			MovieTexture movie = (MovieTexture)_Video_2.material.mainTexture;
			_UI_Video_2.SetActive (false);
			movie.Stop ();
			_Video_Sound_2.Stop ();
			_pause_time = 0;
		}
		else if (index == 3)
		{
			MovieTexture movie_1 = (MovieTexture)_Video_3.material.mainTexture;
			MovieTexture movie_2 = (MovieTexture)_Video_4.material.mainTexture;
			MovieTexture movie_3 = (MovieTexture)_Video_5.material.mainTexture;
			_StarTravelFront.SetActive(false);
			_StarTravelLeft.SetActive(false);
			_StarTravelRight.SetActive(false);
			movie_1.Stop ();
			movie_2.Stop ();
			movie_3.Stop ();
			_pause_time = 0;
		}
	}

	public void PlayVideo (int index)
    {
		if (index == 0)
		{
			MovieTexture movie = (MovieTexture)_Video_0.material.mainTexture;
			if (!_done_playing_video_0 && !movie.isPlaying && movie.isReadyToPlay)
			{
				_done_playing_video_0 = true;
				_pause_time += movie.duration;
				_UI_Video_0.SetActive(true);
				movie.Play();
				_Video_Sound_0.Play();
				StartCoroutine (WaitToSetActive(movie.duration, _UI_Video_0, false));
			}
		}
		else if (index == 1)
        {
            MovieTexture movie = (MovieTexture)_Video_1.material.mainTexture;
            if (!_done_playing_command_video_1 && !movie.isPlaying && movie.isReadyToPlay)
            {
                _done_playing_command_video_1 = true;
                _pause_time += movie.duration;
                _UI_Video_1.SetActive(true);
                movie.Play();
                _Video_Sound_1.Play();
				StartCoroutine (WaitToSetActive(movie.duration, _UI_Video_1, false));
            }
        }
        else if (index == 2)
        {
            MovieTexture movie = (MovieTexture)_Video_2.material.mainTexture;
            if (!_done_playing_command_video_2 && !movie.isPlaying && movie.isReadyToPlay)
            {
                _done_playing_command_video_2 = true;
                _pause_time += movie.duration;
                _UI_Video_2.SetActive(true);
                movie.Play();
				_Video_Sound_2.Play();
				StartCoroutine (WaitToSetActive(movie.duration, _UI_Video_2, false));
            }
        }
		else if (index == 3)
		{
			MovieTexture movie_1 = (MovieTexture)_Video_3.material.mainTexture;
			MovieTexture movie_2 = (MovieTexture)_Video_4.material.mainTexture;
			MovieTexture movie_3 = (MovieTexture)_Video_5.material.mainTexture;
			if (!_done_playing_startravel && !movie_1.isPlaying && movie_1.isReadyToPlay)
			{
				_done_playing_startravel = true;
				_pause_time += movie_1.duration;
				_StarTravelFront.SetActive(true);
				_StarTravelLeft.SetActive(true);
				_StarTravelRight.SetActive(true);
				movie_1.Play();
				movie_2.Play();
				movie_3.Play();
				_Video_Sound_End.Play ();
				StartCoroutine (WaitToSetActive(movie_1.duration, _StarTravelFront, false));
				StartCoroutine (WaitToSetActive(movie_2.duration, _StarTravelLeft, false));
				StartCoroutine (WaitToSetActive(movie_3.duration, _StarTravelRight, false));
			}
		}
    }

	IEnumerator WaitToSetActive (float seconds, GameObject gameObject, bool active)
	{
		yield return new WaitForSeconds(seconds);
		gameObject.SetActive (active);
	}

    IEnumerator AutoPilot (float step)
    {
        while (_auto_piloting)
        {
            _Cave.position = Vector3.MoveTowards(_Cave.position, _Destination.position, step);
            yield return null;
        }
    }

	public void ForceShake (int dir, int num, float intival, float voltage = _max_voltage)
	{
		ShakeCamera ();
		_current_shake_num = 0;
		_shake_num = num;
		_shake_dir = dir;
		_shake_intival = intival;
		if (voltage >= 0 && voltage <= 10f) {
			_shake_voltage = voltage;
		} else {
			_shake_voltage = _max_voltage;
		}

		InvokeRepeating ("ShakeFloor", 0, _shake_intival);
		StartCoroutine (CancelInvokeForSeconds (_shake_num*_shake_intival));

		// play shake sound
		// PlayShakeSound ();

		if (_game_time - _last_shake_time >= _Shake.clip.length) {
			PlayShakeSound ();
			_last_shake_time = _game_time;
		}
	}

	IEnumerator CancelInvokeForSeconds (float time)
	{
		yield return new WaitForSeconds (time);
		CancelInvoke ();
		_shake_voltage = _max_voltage;
	}

	public void FirstAttackA ()
	{
		StartCoroutine(FirstAttackARoutine());
		_done_state = false;
	}

	IEnumerator FirstAttackARoutine ()
	{
		while (!_done_first_attack_a)
		{
			// Play attack sound
			_Attack_1.Play ();
			yield return new WaitForSeconds (_attack_delay);

			// camera shakes (when floor shakes)
			Debug.Log ("Camera shakes");
			ShakeCamera ();

			// floor shakes for one time
			Debug.Log ("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 1;
			InvokeRepeating ("ShakeFloor", 0, _shake_intival);
			yield return new WaitForSeconds (1f);

			// Play attack sound
			_Attack_2.Play ();
			yield return new WaitForSeconds (_attack_delay);

			// floor shakes for one time
			Debug.Log ("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = -1;
			InvokeRepeating ("ShakeFloor", 0, _shake_intival);
			yield return new WaitForSeconds (1f);// Play attack sound
			_Attack_1.Play ();
			yield return new WaitForSeconds (_attack_delay);

			// Play attack sound
			_Attack_3.Play ();
			yield return new WaitForSeconds (_attack_delay);

			// floor shakes for one time
			Debug.Log ("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 2;
			InvokeRepeating ("ShakeFloor", 0, _shake_intival);
			yield return new WaitForSeconds (1f);

			// UI system goes out
			Debug.Log ("UI down");
			_UI.SetActive (false);

			// play "system down" sound, nothing vistually happens for 2 seconds
			Debug.Log ("System down");
			_Power_Down.Play ();
			// yield return new WaitForSeconds(_Power_Down.clip.length);

			// light goes out
			Debug.Log ("light 1 intensity down");
			_Light_1.intensity = 0.1f;
			yield return new WaitForSeconds (1f);
			Debug.Log ("light 2 intensity down");
			_Light_2.intensity = 0.1f;
			yield return new WaitForSeconds (_Power_Down.clip.length);

			_done_first_attack_a = true;
			_done_state = true;
		}
	}

	public void FirstAttackB ()
	{
		StartCoroutine(FirstAttackBRoutine());
		_done_state = false;
	}

	IEnumerator FirstAttackBRoutine ()
	{
		while (!_done_first_attack_b)
		{
			
			// alien show up

			yield return new WaitForSeconds (3f);

			_done_first_attack_b = true;
			_done_state = true;
		}
	}

	public void FirstAttackC ()
	{
		StartCoroutine(FirstAttackCRoutine());
		_done_state = false;
	}

	IEnumerator FirstAttackCRoutine ()
	{
		while (!_done_first_attack_c)
		{
			yield return new WaitForSeconds(8f);

			// play "system up" sound, nothing vistually happens for 2 seconds
			Debug.Log("System up");
			_Power_Up.Play();

			// light goes up
			Debug.Log("light 1 intensity up");
			_Light_1.intensity = originalIntensity_Light_1;
			yield return new WaitForSeconds(2f);
			Debug.Log("light 2 intensity up");
			_Light_2.intensity = originalIntensity_Light_2;

			// UI system gose up
			Debug.Log("UI up");
			_UI.SetActive(true);

			// play "warning alarm" sound
			Debug.Log("Warning Alarm");
			PlayWarningAlarm ();

			// Alarm light flashes for three times, show warning message
			Debug.Log("alarm light flashes (3)");
			_current_alarm_num = 0;
			_alarm_num = 3;
			InvokeRepeating("AlarmLight", 0, _alarm_intival);
			ShowMainTextWarning (_alarm_num * _alarm_intival);

			// UI ship damaage status
			UpdateShipStatus ();

			yield return new WaitForSeconds(_alarm_num * _alarm_intival);

			StopMainTextWarning ();
			yield return new WaitForSeconds(.3f);
			StopWarningAlarm ();
			CancelInvoke();

			_done_first_attack_c = true;
			_done_state = true;
		}
	}

    public void FirstAttack ()
    {
        StartCoroutine(FirstAttackRoutine());
    }

    IEnumerator FirstAttackRoutine ()
    {
        while (!_done_first_attack)
        {
			// Play attack sound
			_Attack_1.Play();
			yield return new WaitForSeconds(_attack_delay);

			// camera shakes (when floor shakes)
			Debug.Log("Camera shakes");
			ShakeCamera();

            // floor shakes for one time
            Debug.Log("Floor shakes (1)");
            _current_shake_num = 0;
            _shake_num = 1;
            _shake_dir = 1;
            InvokeRepeating("ShakeFloor", 0, _shake_intival);
            yield return new WaitForSeconds(1f);

            // Play attack sound
			_Attack_2.Play();
			yield return new WaitForSeconds(_attack_delay);

            // floor shakes for one time
            Debug.Log("Floor shakes (1)");
            _current_shake_num = 0;
            _shake_num = 1;
            _shake_dir = -1;
            InvokeRepeating("ShakeFloor", 0, _shake_intival);
			yield return new WaitForSeconds(1f);

			// Play attack sound
			_Attack_3.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one time
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 2;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);
			yield return new WaitForSeconds(1f);

            // UI system goes out
            Debug.Log("UI down");
            _UI.SetActive(false);

			// play "system down" sound, nothing vistually happens for 2 seconds
			Debug.Log("System down");
			_Power_Down.Play();
			// yield return new WaitForSeconds(_Power_Down.clip.length);

            // light goes out
            Debug.Log("light 1 intensity down");
            _Light_1.intensity = 0.1f;
            yield return new WaitForSeconds(1f);
            Debug.Log("light 2 intensity down");
            _Light_2.intensity = 0.1f;
			yield return new WaitForSeconds(_Power_Down.clip.length);

			// alien show up

            // play "system up" sound, nothing vistually happens for 2 seconds
            Debug.Log("System up");
            _Power_Up.Play();

            // light goes up
            Debug.Log("light 1 intensity up");
            _Light_1.intensity = originalIntensity_Light_1;
            yield return new WaitForSeconds(2f);
            Debug.Log("light 2 intensity up");
            _Light_2.intensity = originalIntensity_Light_2;

            // UI system gose up
            Debug.Log("UI up");
            _UI.SetActive(true);
			yield return new WaitForSeconds(1f);

			// play "warning alarm" sound
			Debug.Log("Warning Alarm");
			PlayWarningAlarm ();

            // Alarm light flashes for three times, show warning message
            Debug.Log("alarm light flashes (3)");
            _current_alarm_num = 0;
			_alarm_num = 3;
			InvokeRepeating("AlarmLight", 0, _alarm_intival);
			ShowMainTextWarning (_alarm_num * _alarm_intival + .3f);
            yield return new WaitForSeconds(_alarm_num * _alarm_intival + .3f);

			StopMainTextWarning ();
			StopWarningAlarm ();
            CancelInvoke();

            _done_first_attack = true;
        }
    }

    public void MajorAttack ()
    {
        StartCoroutine(MajorAttackRoutine());
    }

	IEnumerator MajorAttackRoutine()
    {
        while (!_done_major_attack)
		{
			// Play attack sound
			_Attack_2.Play();
			yield return new WaitForSeconds(_attack_delay);

			// camera shakes (when floor shakes)
			Debug.Log("Camera shakes");
			ShakeCamera();

            // floor shakes for one times
            Debug.Log("Floor shakes (1)");
            _current_shake_num = 0;
            _shake_num = 1;
			_shake_dir = -1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// Play attack sound
			_Attack_1.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one times
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// UI ship damaage status
			UpdateShipStatus ();

			// Play attack sound
			_Attack_2.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one times
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = -1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// Play attack sound
			_Attack_1.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one times
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// Play attack sound
			_Attack_2.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one times
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = -1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// Play attack sound
			_Attack_1.Play();
			yield return new WaitForSeconds(_attack_delay);

			// floor shakes for one times
			Debug.Log("Floor shakes (1)");
			_current_shake_num = 0;
			_shake_num = 1;
			_shake_dir = 1;
			InvokeRepeating("ShakeFloor", 0, _shake_intival);

			// play "warning alarm" sound
			Debug.Log("Warning Alarm");
			PlayWarningAlarm ();

            // light flashes for six times
            Debug.Log("alarm light flashes (6)");
            _current_alarm_num = 0;
            _alarm_num = 6;
			InvokeRepeating("AlarmLight", 0, _alarm_intival);
			ShowMainTextWarning (_alarm_num * _alarm_intival);

			yield return new WaitForSeconds(_alarm_num * _alarm_intival);

			CancelInvoke();
			StopMainTextWarning ();

			// UI ship damaage status
			UpdateShipStatus ();
			ShowShipStatusMain ();

			// light flashes forever
			Debug.Log("alarm light flashes (256)");
			_current_alarm_num = 0;
			_alarm_num = 256;
			InvokeRepeating("AlarmLight", 0, _alarm_intival);

			yield return new WaitForSeconds(_alarm_num * _alarm_intival);

			StopWarningAlarm ();
            CancelInvoke();

            _done_major_attack = true;
        }
	}

	public void PlayWarningAlarm ()
	{
		_Warning_Alarm.Play ();
	}

	public void StopWarningAlarm ()
	{
		_Warning_Alarm.Stop ();
	}

    public void PlayDecodingSound ()
    {
		Debug.Log ("Play Decoding Sound");
        _Decoding.Play();
    }

    public void StopDecodingSound()
    {
        _Decoding.Stop();
    }

    public void PlayDecodeSuccessSound()
    {
        _Decode_Success.Play();
    }

	public void PlayDecodeFailedSound()
	{
		_Decode_Failed.Play();
	}

    public void ShakeFloor ()
    {
        // move floor up and down
        if (_current_shake_num >= _shake_num)
        {
			_done_attacking = true;
            Debug.Log ("Done attacking");
            _FloorController.enable();
            _FloorController.resetFloor ();
            return;
        }

        _current_shake_num++;

        if (_shake_dir == 1)
        {
            StartCoroutine(RaiseFloorLeft(_shake_voltage, _shake_intival / 2));
            StartCoroutine(RaiseFloorRight(_shake_voltage, _shake_intival / 2));
        }
		else if (_shake_dir == -1)
        {
            StartCoroutine(RaiseFloorRight(_shake_voltage, _shake_intival / 2));
            StartCoroutine(RaiseFloorLeft(_shake_voltage, _shake_intival / 2));
        }
		else if (_shake_dir == 2)
		{
			StartCoroutine(RaiseFloorBack(_shake_voltage, _shake_intival / 2));
			StartCoroutine(RaiseFloorFront(_shake_voltage, _shake_intival / 2));
		}
		else if (_shake_dir == -2)
		{
			StartCoroutine(RaiseFloorRight(_shake_voltage, _shake_intival / 2));
			StartCoroutine(RaiseFloorFront(_shake_voltage, _shake_intival / 2));
		}

		// play shake sound
		// PlayShakeSound ();
    }

    IEnumerator RaiseFloorLeft (float voltage, float time)
    {
        while (!_done_shaking)
        {
            yield return new WaitForSeconds (0.1f);
        }

        _done_shaking = false;
        Debug.Log ("Floor Left" + " " + _current_shake_num);
        _FloorController.enable();
        _FloorController.raiseLeft(voltage);
        yield return new WaitForSeconds (time);
        _done_shaking = true;
    }

    IEnumerator RaiseFloorRight (float voltage, float time)
    {
        while (!_done_shaking)
        {
            yield return new WaitForSeconds (0.1f);
        }

        _done_shaking = false;
        Debug.Log ("Floor Right" + " " + _current_shake_num);
        _FloorController.enable();
        _FloorController.raiseRight (voltage);
        yield return new WaitForSeconds (time);
        _done_shaking = true;
	}

	IEnumerator RaiseFloorBack (float voltage, float time)
	{
		while (!_done_shaking)
		{
			yield return new WaitForSeconds (0.1f);
		}

		_done_shaking = false;
		Debug.Log ("Floor Back" + " " + _current_shake_num);
		_FloorController.enable();
		_FloorController.raiseBack(voltage);
		yield return new WaitForSeconds (time);
		_done_shaking = true;
	}

	IEnumerator RaiseFloorFront (float voltage, float time)
	{
		while (!_done_shaking)
		{
			yield return new WaitForSeconds (0.1f);
		}

		_done_shaking = false;
		Debug.Log ("Floor Front" + " " + _current_shake_num);
		_FloorController.enable();
		_FloorController.raiseFront (voltage);
		yield return new WaitForSeconds (time);
		_done_shaking = true;
	}

    public void AlarmLight ()
    {
        // turn lights on and off
		if (_current_alarm_num >= _alarm_num)
		{
            return;
        }

        _current_alarm_num++;
        StartCoroutine (AlarmLightOn (_alarm_intival / 2));
        StartCoroutine (AlarmLightOff (_alarm_intival / 2));
    }

    IEnumerator AlarmLightOn (float time)
    {
        while (!_done_alarming)
		{
            yield return new WaitForSeconds (0.1f);
        }

        _done_alarming = false;
        // Debug.Log ("Light On" + " " + _current_alarm_num);
        _DMXController.TurnOn ("Master", 255, 0, 0, 0, 255);
        yield return new WaitForSeconds (time);
        _done_alarming = true;
    }

    IEnumerator AlarmLightOff (float time)
    {
        while (!_done_alarming)
        {
            yield return new WaitForSeconds (0.1f);
        }

        _done_alarming = false;
        // Debug.Log ("Light Off" + " " + _current_alarm_num);
        _DMXController.TurnOn ("Master", 0, 0, 0, 0, 0);
        yield return new WaitForSeconds (time);
        _done_alarming = true;
    }

	public void ShakeCamera ()
	{
		StartCoroutine (ShakeCameraRoutine ());
    }

    IEnumerator ShakeCameraRoutine ()
    {
        while (_done_shaking)
        {
            yield return new WaitForSeconds (0.1f);
        }

        Debug.Log ("Shake Camera");

        originPosition = _CameraGroup.position;
        originRotation = _CameraGroup.rotation;
        shake_intensity = .01f;

        while (true)
        {
			if (!_done_shaking) {
				_CameraGroup.position = originPosition + Random.insideUnitSphere * shake_intensity;
				_CameraGroup.rotation = new Quaternion (
					originRotation.x + Random.Range (-shake_intensity, shake_intensity) * .2f,
					originRotation.y + Random.Range (-shake_intensity, shake_intensity) * .2f,
					originRotation.z + Random.Range (-shake_intensity, shake_intensity) * .2f,
					originRotation.w + Random.Range (-shake_intensity, shake_intensity) * .2f
				);
			} else {

				_CameraGroup.transform.position = originPosition;
				_CameraGroup.transform.rotation = originRotation;
			}
            yield return null;
        }
    }

    public void StartGame ()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        float fadeTime = _Fade.BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
		ShowShipStatusMain ();
		ShowShipStatus ();
    }

	public void ShowSkymap ()
	{
		_UI_Skymap_1.SetActive (true);
	}

	public void UpdateSkymap ()
	{
		_UI_Skymap_1.SetActive (false);
		_UI_Skymap_2.SetActive (true);
	}

	public void HideSkymap ()
	{
		_UI_Skymap_2.SetActive (false);
	}

	public void FlyToDestination()
	{
		_UI.SetActive (false);
		HideShipStatusMain ();
		StopMainTextWarning ();
		StopWarningAlarm ();
		CancelInvoke();
	}

    public void EndGame()
    {
		_pause_time = 0;
        StartCoroutine(EndGameRoutine());
    }

    IEnumerator EndGameRoutine()
	{
        float fadeTime = _Fade.BeginFade(1);
        yield return new WaitForSeconds(fadeTime + 2f);

        fadeTime = _Fade.BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);

		_UI.SetActive (true);
		_UI_Guest.SetActive (true);
		_UI_Guest.GetComponent<GUIText>().text += _GUIController.GetGuest1() + " & " + _GUIController.GetGuest2();
		yield return new WaitForSeconds(3f);

		fadeTime = _Fade.BeginFade(1);
		yield return new WaitForSeconds(fadeTime + 2f);

		fadeTime = _Fade.BeginFade(-1);
		yield return new WaitForSeconds(fadeTime);

		_UI_Guest.SetActive (false);
		_UI.SetActive (false);

        _is_end = true;
    }

    public void ShowCredits ()
    {
		_pause_time += 6f;
        StartCoroutine(ShowCreditsRotine());
    }

    IEnumerator ShowCreditsRotine ()
    {	
		_UI.SetActive (true);
		_UI_Credits_1.SetActive(true);
		yield return new WaitForSeconds(2f);
		_UI_Credits_1.SetActive(false);

		_UI_Credits_2.SetActive(true);
		yield return new WaitForSeconds(2f);
		_UI_Credits_2.SetActive(false);

		_UI_Credits_3.SetActive(true);
		yield return new WaitForSeconds(2f);
		_UI_Credits_3.SetActive(false);

		_UI.SetActive (false);

		float fadeTime = _Fade.BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
    }

    public bool GameIsEnd ()
    {
        return _is_end;
	}

	public bool StateIsDone ()
	{
		return _done_state;
	}

	public void AttackSoundLeft ()
	{
		_Attack_1.Play ();
	}

	public void AttackSoundRight ()
	{
		_Attack_2.Play ();
	}

	public void AttackSoundMinorLeft ()
	{
		_Attack_4.Play ();
	}

	public void AttackSoundMinorRight ()
	{
		_Attack_5.Play ();
	}

	public void PlayShakeSound ()
	{
		_Shake.Play ();
	}

	public void PlayTapSound ()
	{
		_Tap.Play ();
	}

	public void PlayEstablishSound ()
	{
		_Establish.Play ();
	}
}
