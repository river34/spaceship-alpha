using UnityEngine;
using System.Collections;

public class StateController : MonoBehaviour 
{
	public int _state;
	public float _timer;
    
	public GameObject[] _signal;
	public AlienPlanet _alienPlanet;
	public GameObject _coordinate;

	private Renderer _video;
	private Material _ScreenOri;
	private RootController _root;

    private bool _done_state = true;
	private MeteoriteGenerator _meteoriteGenerator;
	private AlienAttackManager _alienAttackManager;

	public GameObject _alien;
	public GameObject _alienFlying;
	// Use this for initialization
	void Awake () 
	{
		_state = 0;
		_timer = 0;

		_root = GetComponent<RootController>();
		_meteoriteGenerator = GameObject.Find("MeteoriteManager").GetComponent<MeteoriteGenerator>();
		_alienPlanet = GameObject.Find("AlienPlanet").GetComponent<AlienPlanet>();
		_alienAttackManager = GameObject.Find("AlienAttackManager").GetComponent<AlienAttackManager>();

		_alienAttackManager.SetEnable (false);
		for(int i = 0 ; i < _signal.Length ; i++)
			_signal[i].SetActive(false);

		_coordinate.SetActive (false);
    }
	
	// Update is called once per frame
	void Update () 
	{
		_timer += Time.deltaTime;

		if (_state == 0)
		{
			Debug.Log ("State:" + _state + "  Start");

			if (_done_state)
			{
				_done_state = false;
				_root.StartGame();
				_meteoriteGenerator.gameObject.SetActive (true);
			}

			if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 9;
				_timer = 0f;
				_meteoriteGenerator.SlowDownAll(.5f);
				_done_state = true;
			}

			/*
			if (Input.GetKeyDown(KeyCode.E))
			{
				_state = 110;
				_timer = 0f;
				_done_state = true;
			}
			*/
		}

		if (_state == 9)
		{
			Debug.Log ("State:" + _state + "  Establishing...");

			if (_done_state)
			{
				_done_state = false;
				_root.HideShipStatusMain ();
				_root.ShowMainTextEstablishing(2f);
				_root.PlayEstablishSound();
			}

			if (_root._pause_time <= 0.1f)
			{
				_root.StopMainTextEstablishing();
				_state = 10;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 10)
		{
			Debug.Log ("State:" + _state + "  Decode Signal 1");

			if (_done_state) {
				_done_state = false;
				_root.PlayDecodingSound ();
				_root.ShowTextDecoding ();
				_root.ShowShipStatus ();
			}

			_signal[0].SetActive(true);
			bool result = _signal[0].GetComponent<Signal>()._isWin;

			if (result)
			{
				_state = 20;
				_timer = 0f;
				StartCoroutine(DelayDisactive(_signal[0]));
				//_signal[0].SetActive(false);
				_root.StopDecodingSound();
				_root.StopTextDecodingVideo();
				//_root.PlayDecodeSuccessSound();
				_done_state = true;
			}
		}

		if (_state == 20)
		{
			Debug.Log ("State:" + _state + "  Video from daughter");

			if (_done_state)
			{
				_done_state = false;
				_root.PlayVideo(0);
				_root.ShowTextTransmitting();
			}

			if (_root._pause_time <= 0.1f || Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 31;
				_timer = 0f;
				_root.StopVideo(0);
				_root.StopTextTransmitting();
				_done_state = true;
			}
		}

		if (_state == 30)
		{
			Debug.Log ("State:" + _state + "  First attack");

			if (_done_state)
			{
				_done_state = false;
				_root.FirstAttack();
			}

			if(_timer > 15f && Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 31;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 31)
		{
			Debug.Log ("State:" + _state + "  First attack A");

			if (_done_state)
			{
				_done_state = false;
				_root.FirstAttackA ();
				_root.ShowShipStatusMain ();
			}

			if(_root.StateIsDone())
			{
				_state = 32;
				_timer = 0f;
				_root.HideShipStatusMain ();
				_done_state = true;
			}
		}

		if (_state == 32)
		{
			Debug.Log ("State:" + _state + "  First attack B");

			if (_done_state)
			{
				_done_state = false;
				_root.FirstAttackB ();
				_alienAttackManager._enable = true;
			}
			//
			if(_alienAttackManager._firstWin)
			//if(_root.StateIsDone())
			{
				_state = 33;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 33)
		{
			Debug.Log ("State:" + _state + "  First attack C");

			if (_done_state)
			{
				_done_state = false;
				_root.FirstAttackC ();
			}

			if(_root.StateIsDone())
			{
				_state = 38;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 38)
		{
			Debug.Log ("State:" + _state + "  Show ship status");

			if (_done_state)
			{
				_done_state = false;
				_root.ShowShipStatusMain ();
			}

			if(Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 39;
				_timer = 0f;
				_root.HideShipStatusMain ();
				_root.ShowShipStatus ();
				_done_state = true;
			}
		}

		if (_state == 39)
		{
			Debug.Log ("State:" + _state + "  Establishing...");

			if (_done_state)
			{
				_done_state = false;
				_root.ShowMainTextEstablishing (2f);
				_root.PlayEstablishSound();
			}

			if (_root._pause_time <= 0.1f)
			{
				_root.StopMainTextEstablishing();
				_state = 40;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 40)
		{
			Debug.Log ("State:" + _state + "  Decode signal 2");

			if (_done_state) {
				_done_state = false;
				_root.PlayDecodingSound ();
				_root.ShowTextDecoding ();
				_root.ShowShipStatus ();
			}

			_signal[1].SetActive(true);
			bool result = _signal[1].GetComponent<Signal>()._isWin;

			_alienAttackManager._enable = true;

			if (result)
			{
				_alienAttackManager._enable = false;

				_state = 50;
				_timer = 0f;
				StartCoroutine(DelayDisactive(_signal[1]));
				//_signal[1].SetActive(false);
				_root.StopDecodingSound();
				_root.StopTextDecodingVideo();
				//_root.PlayDecodeSuccessSound();
				_done_state = true;
			}
		}

		if (_state == 50)
		{
			Debug.Log ("State:" + _state + "  Video from command 1");

			if (_done_state)
			{
				_done_state = false;
				_root.PlayVideo(1);
				_root.ShowTextTransmitting();
			}

			if (_root._pause_time <= 0.1f || Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 60;
				_timer = 0f;
				_root.StopVideo(1);
				_root.StopTextTransmitting();
				_done_state = true;
			}
		}

		if (_state == 60)
		{
			Debug.Log ("State:" + _state + "  Place coordinates");

			if (_done_state)
			{
				_root.ShowSkymap ();
				_coordinate.SetActive (true);
				_root.PlayDecodingSound ();
				_root.ShowShipStatus ();
				_done_state = false;
			}

			bool result = _coordinate.GetComponent<Coodinate> ()._isWin;

			if (result && Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 70;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 70)
		{
			Debug.Log ("State:" + _state + "  Update map");

			if (_done_state)
			{
				_root.UpdateSkymap ();
				_done_state = false;
			}

			if (_timer >= 1f && Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_root.HideSkymap ();
				Destroy (_coordinate);
				_state = 79;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 79)
		{
			Debug.Log ("State:" + _state + "  Establishing...");

			if (_done_state)
			{
				_done_state = false;
				_root.ShowMainTextEstablishing(2f);
				_root.PlayEstablishSound();
			}

			if (_root._pause_time <= 0.1f)
			{
				_root.StopMainTextEstablishing();
				_state = 80;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 80)
		{
			Debug.Log ("State:" + _state + "  Decode signal 3");

			_signal[2].SetActive(true);
			bool result = _signal[2].GetComponent<Signal>()._isWin;

			_alienAttackManager._enable = true;

			if (_done_state) {
				_done_state = false;
				_root.PlayDecodingSound ();
				_root.ShowTextDecoding ();
				_root.ShowShipStatus ();
			}

			if (result)
			{
				_alienAttackManager._enable = false;

				_state = 90;
				_timer = 0f;
				StartCoroutine(DelayDisactive(_signal[2]));
				//_signal[2].SetActive(false);
				_root.StopDecodingSound();
				_root.StopTextDecodingVideo();
				//_root.PlayDecodeSuccessSound();
				_done_state = true;
			}
		}

		if (_state == 90)
		{
			Debug.Log ("State:" + _state + "  Video from command 2");

			if (_done_state)
			{
				_done_state = false;
				_root.PlayVideo(2);
				_root.ShowTextTransmitting();
				_alienFlying.SetActive(true);
                _alienFlying.GetComponent<MeteoriteGenerator>()._enable = true;
            }

			if (_root._pause_time <= 0.1f || Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 100;
				_timer = 0f;
				_root.StopVideo(2);
				_root.StopTextTransmitting();
				_done_state = true;
			}
		}

		if (_state == 100)
		{
			Debug.Log ("State:" + _state + "  Major attack");

			if (_done_state)
			{
				_done_state = false;
				_root.MajorAttack();
				// aliens fly to you
				// aliens hit you
			}

			if(_timer >= 15f && Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 110;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 110)
		{
			Debug.Log ("State:" + _state + "  Fly to destination");

			if (_done_state)
			{
				_done_state = false;
				_alienFlying.GetComponent<MeteoriteGenerator>()._enable = false;
				_meteoriteGenerator.gameObject.SetActive (false);
				_root.FlyToDestination();
				_root.PlayVideo(3);
			}

			if (_root._pause_time <= 1f)
			{
				_state = 120;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 120)
		{
			Debug.Log ("State:" + _state + "  End");

			if (_done_state)
			{
				_done_state = false;
				_root.EndGame();
			}

			if (_root.GameIsEnd()) {
				_state = 130;
				_timer = 0f;
				_done_state = true;
			}
		}

		if (_state == 130)
		{
			Debug.Log("State:" + _state + "  Credits");

			if (_done_state)
			{
				_done_state = false;
				_root.ShowCredits();
			}

			if (_root._pause_time <= 0.1f && Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 0;
				_timer = 0f;
				_done_state = true;
			}
		}

		// original
		/*
		if(_state == 0)
		{
			Debug.Log("State:" + _state + "  Radio");

            if (_done_state)
            {
                _done_state = false;
                _root.StartGame();
            }

            _root.ShowTextRadio();

            if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 10;
				_timer = 0f;
				_meteoriteGenerator.SlowDownAll(.5f);
                _root.StopTextRadio();
                _done_state = true;
            }
		}


		if(_state == 10)
		{
			Debug.Log ("State:" + _state + "  First Attack");

            if (_done_state)
            {
                _done_state = false;
				_root.FirstAttack();
            }

			if(_timer > 15f)
			{
				_state = 20;
				_timer = 0f;
                _done_state = true;
            }
		}

		if(_state == 20)
		{
			Debug.Log("State:" + _state + "  Radio");

            _root.ShowTextRadio();

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _state = 30;
				_timer = 0f;
                _root.StopTextRadio();
            }
		}

		if(_state == 30)
		{
			Debug.Log("State:" + _state + "  Decode Signal");

            if (_done_state)
            {
                _done_state = false;
                _root.ShowMainTextEstablishing(2f);
            }

            if (_root._pause_time <= 0.1f)
            {
                _root.StopMainTextEstablishing();

                _signal[0].SetActive(true);
                bool result = _signal[0].GetComponent<Signal>()._isWin;
                _root.PlayDecodingSound();
                _root.ShowTextDecoding();

                if (result)
                {
                    _state = 40;
                    _timer = 0f;
                    _signal[0].SetActive(false);
                    _root.StopDecodingSound();
                    _root.StopTextDecodingVideo();
                    _root.PlayDecodeSuccessSound();
                    _done_state = true;
                }
            }
		}

		if(_state == 40)
		{
			Debug.Log("State:" + _state + "  Video (Command 1)");

            if (_done_state)
            {
                _done_state = false;
                _root.PlayVideo(1);
            }

            if (_root._pause_time <= 0.1f)
			{
				_state = 50;
				_timer = 0f;
                _done_state = true;
            }
		}

		if(_state == 50)
		{
			Debug.Log("State:" + _state + "  Radio");

            _root.ShowTextRadio();

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _state = 60;
				_timer = 0f;
                _root.StopTextRadio();
            }
		}

		if(_state == 60)
		{
			Debug.Log("State:" + _state + "  Decode Signal 2");

			if (_done_state)
			{
				_done_state = false;
				_root.ShowMainTextEstablishing (2f);
			}

			if (_root._pause_time <= 0.1f)
			{
				_root.StopMainTextEstablishing();

				_signal[1].SetActive(true);
				bool result = _signal[1].GetComponent<Signal>()._isWin;
				_root.PlayDecodingSound();
				_root.ShowTextDecoding();

				if (result)
				{
					_state = 70;
					_timer = 0f;
					_signal[1].SetActive(false);
					_root.StopDecodingSound();
					_root.StopTextDecodingVideo();
					_root.PlayDecodeSuccessSound();
					_done_state = true;
				}
			}
		}

		if(_state == 70)
		{
			Debug.Log("State:" + _state + "  Video (Command 2)");

            if (_done_state)
            {
                _done_state = false;
                _root.PlayVideo(2);
            }

            if (_root._pause_time <= 0.1f)
            {
				_state = 75;
				_timer = 0f;
                _done_state = true;
            }
		}

		if(_state == 75)
		{
			Debug.Log("State:" + _state + "  Major Attack");

            if (_done_state)
            {
                _done_state = false;
                _root.MajorAttack();
            }

			if(_timer > 6f)
			{
				_state = 80;
				_timer = 0f;
                _done_state = true;
            }
		}

		if(_state == 80)
		{
			Debug.Log("State:" + _state + "  Radio");
            _root.ShowTextRadio();

            if (Input.GetKeyDown(KeyCode.LeftAlt))
			{
				_state = 90;
				_timer = 0f;
                _root.StopTextRadio();
            }
		}

		if (_state == 90)
		{
			Debug.Log("State:" + _state + "  End?");

			if (Input.GetKeyDown(KeyCode.F))
			{
				_alienPlanet.EnableFly();
				_meteoriteGenerator.SlowDownAll(2.0f);
			}

            if (_alienPlanet._is_arrived && _done_state)
            {
                _done_state = false;
                // fade into black
                _root.EndGame();
            }

			if (_root.GameIsEnd()) {
				_state = 100;
				_timer = 0f;
                CancelInvoke();
                _done_state = true;
            }
		}

        if (_state == 100)
        {
            Debug.Log("State:" + _state + "  Credits");

            if (_done_state)
            {
                _done_state = false;
				_meteoriteGenerator.gameObject.SetActive (false);
                _root.ShowCredits();
            }

			if (Input.GetKeyDown(KeyCode.LeftAlt) && _root._pause_time <= 0.1f)
            {
                _state = 0;
                _timer = 0f;
                _done_state = true;
            }
        }
        */
    }

	IEnumerator DelayDisactive(GameObject g)
	{
		yield return new WaitForSeconds(0.1f);
		g.SetActive(false);
	}
}
