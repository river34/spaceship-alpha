using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour 
{
	public GameObject _cube;
	public GameObject _cubeGoal;
	public Material _select;
	public Material _notSelect;

	public float []_signalIntensity;
	public float []_signalGoalIntensity;
	private GameObject [] _signal;
	public int _col = 121; 
	public int _interval = 10;

	public int _currentNum = 0;
	public float _decodeSpeed = 10.0f;
	public float [] _goalSignal;
	public float _winRange = 0.5f;

	private float _timer = 0.0f;
	public  float _cubeSize = 1.0f;
	public bool _isWin = false;

	private GameObject _line;
	// Use this for initialization
	void Start () 
	{
		_signalIntensity = new float[_col];
		_signalGoalIntensity = new float[_col];
		_signal = new GameObject[_col];
		Generate();
		_currentNum = _interval;


		//_signalIntensity[0] = 3.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_timer += Time.deltaTime;
		InputNum();
		if(_timer > 3.0f)
			CheckWin();



	}


	void InputNum()
	{
		if(Input.GetKey(KeyCode.S))
		{
			_signalIntensity[_currentNum] -= _decodeSpeed * Time.deltaTime;
			NodeChangeUpdate(_currentNum);
		}
		if(Input.GetKey(KeyCode.W))
		{
			_signalIntensity[_currentNum] += _decodeSpeed * Time.deltaTime;
			NodeChangeUpdate(_currentNum);
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			if(_currentNum > _interval * 3 / 2)
			{
				//_signal[_currentNum].transform.GetComponent<Renderer>().material = _notSelect;
				_currentNum -= _interval;
				_line.transform.localPosition = new Vector3(_signal[_currentNum].transform.position.x,0,2.8f);
				//_signal[_currentNum].transform.GetComponent<Renderer>().material = _select;
			}
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			if(_currentNum < _col - _interval * 3/2)
			{
				//_signal[_currentNum].transform.GetComponent<Renderer>().material = _notSelect;
				_currentNum += _interval;
				_line.transform.localPosition = new Vector3(_signal[_currentNum].transform.position.x,0,2.8f);
				//_signal[_currentNum].transform.GetComponent<Renderer>().material = _select;
			}
		}

	}

	void NodeChangeUpdate(int num, float intensity = 0f, bool force = false)
	{
		if(num >= _interval && num <= _col - _interval)
		{
			NodeChangeUpdateOneSide(num,intensity,force);
			NodeChangeUpdateOneSide(num + _interval,intensity,force);
			UpdateSignal(num);
		}
		else
			Debug.Log(num + "invalid");
	}

	void NodeChangeUpdateOneSide(int num, float intensity = 0f, bool force = false)
	{
		if(force)
			_signalIntensity[num] = intensity;
		float a = _signalIntensity[num - _interval];
		float b = _signalIntensity[num];
//		float c = _signalIntensity[num + _interval];
		for(int i = num - _interval + 1, j = 1; i < num ;i++,j++)
		{
			float value = (a - b) / 2f * Mathf.Cos(Mathf.PI / _interval * j) + (a + b) / 2f;
			_signalIntensity[i] = value;
		}

	}

	void UpdateSignalAll()
	{
		for(int i = 0 ; i < _col ; i++)
		{
			StartCoroutine(UpdateOneSignal(i));
		}
	}

	void UpdateSignal(int num)
	{
		for(int i = num - _interval ; i < num + _interval ; i++)
		{
			if(i > 0 && i < _col)
				StartCoroutine(UpdateOneSignal(i));
		}
	}

	IEnumerator UpdateOneSignal(int num)
	{
		GameObject i = _signal[num];
		while(Mathf.Abs(i.transform.position.y - _signalIntensity[num]) > 0.05f)
		{
			float tmpPosi = i.transform.position.y;
			i.transform.position = new Vector3(i.transform.position.x, Mathf.Lerp(tmpPosi,_signalIntensity[num],0.5f) ,i.transform.position.z);
			yield return null;
		}
		//return;
	}

	void Generate()
	{
		for(int i = 0 ; i < _col ; i++)
		{
			GameObject go = (GameObject)Instantiate(_cubeGoal,new Vector3(-0.1f,0,i * _cubeSize),Quaternion.identity);
			go.transform.localScale = Vector3.one * _cubeSize;
			go.name = i.ToString();
			go.transform.parent = this.transform.FindChild("Goal");
			_signal[i] = go;
			go.SetActive(false);
			_signalIntensity[i] = 0f;
		}

		SetGoal(_goalSignal);
		for(int i = 0; i < _col ; i ++)
			_signalGoalIntensity[i] = _signalIntensity[i];

		StartCoroutine(DelayGenerate(1.0f));

	}


	void SetGoal(float []goal)
	{
		for(int i = 0 ; i < goal.Length ; i++)
		{
			int numTemp = (i + 1) * _interval;
			if(numTemp < _col)
			{
				NodeChangeUpdate(numTemp,goal[i],true);
				Debug.Log(i + " " + goal[i]);
			}
		}
	}

	IEnumerator DelayGenerate(float t)
	{
		yield return new WaitForSeconds(t);
		for(int i = 0 ; i < _col ; i++)
		{
			_signal[i].SetActive(true);
			GameObject go = (GameObject)Instantiate(_cube,new Vector3(0,0,i * _cubeSize),Quaternion.identity);
			go.transform.localScale = Vector3.one * _cubeSize;
			go.name = i.ToString();
			go.transform.parent = this.transform.FindChild("Signal");
			_signal[i] = go;
			_signalIntensity[i] = 0f;
		}

	//	_signal[_currentNum].transform.GetComponent<Renderer>().material = _select;
		_line = (GameObject)Instantiate(_cube,Vector3.zero,Quaternion.identity);
		//_line.transform.parent = this.transform;
		_line.transform.localScale = new Vector3(0.3f * _cubeSize,12f,0.3f * _cubeSize) ;

		this.transform.eulerAngles = new Vector3(0,90f,0);
		this.transform.position -=new Vector3( _col * _cubeSize / 2f,0, -3f);
		Debug.Log(_currentNum);
		_line.transform.position = new Vector3(_signal[_currentNum].transform.position.x,0,2.8f);


		//_signal[_signal.Length - 1].SetActive(false);

	}

	public bool CheckWin()
	{
		for(int i = _interval ; i < _col ; i += _interval)
		{
			if(Mathf.Abs(_signalGoalIntensity[i] - _signalIntensity[i]) > _winRange)
				return false;
		}
		Debug.Log("Win!!!!!!");
		_line.SetActive(false);
		_isWin = true;
		return true;
	}




	public void Disorder()
	{
		Debug.Log("Call Disorder");
		float []goal = new float[_goalSignal.Length - 2];
		for(int i = 0 ; i < goal.Length ; i++)
		{
			goal[i] = Random.Range(-4f,4f);
		}

		SetGoal(goal);
		_signalIntensity[_signal.Length - _interval - 1] -= 0.01f;
		NodeChangeUpdate(_signal.Length - _interval - 1);
		//UpdateSignalAll();
	}

}
