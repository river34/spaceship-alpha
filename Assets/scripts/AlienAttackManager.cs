using UnityEngine;
using System.Collections;

public class AlienAttackManager : MonoBehaviour 
{
	public GameObject [] _alien;

	public RootController _root;

	public bool _enable = false;
	public bool _left = false;
	public bool _right = false;
	public float _generateTimer = 0.0f;
	public float _generateInterval = 5.0f;
	//public float _attackTimer = -2.0f;
	public GameObject _alienEntity;
	public int _moveTimer = 0;

	private GameObject _alienCurrent;

	public bool _firstWin = false;

	void Start () 
	{
		_generateTimer = 4.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputKey();
		if(_enable)
		{
			if(!_left && !_right)
				GenerateAlien();
		}
	}

	void InputKey()
	{
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			_root.ForceShake (1, 1, .1f, 2f);
			if(_left)
			{
				_moveTimer++;
				//TODO::Play the animation
				if(_moveTimer >= 5)
				{
					_left = false;
					Dead ();
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_root.ForceShake (-1, 1, .1f, 2f);
			if(_right)
			{
				_moveTimer++;
				//TODO::Play the animation
				if(_moveTimer >= 5)
				{
					_right = false;
					Dead ();
				}

			}
		}
	}

	void Dead()
	{
		if(!_firstWin)
		{
			_firstWin = true;
			_enable = false;
		}
		_alienCurrent.GetComponent<AlienController> ().Die ();
		_moveTimer = 0;
	}
		

	void GenerateAlien()
	{
		_generateTimer += Time.deltaTime;
		if(_generateTimer > _generateInterval)
		{
			int tmp = Random.Range(0,2);
			if(tmp < _alien.Length)
			{
				if(tmp == 0)
				{
					GameObject go = (GameObject)Instantiate (_alienEntity,Vector3.zero,Quaternion.identity);
					go.transform.parent = _alien [0].transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localEulerAngles = Vector3.zero;
					_alienCurrent = go;
					_left = true;
				}
				else if(tmp == 1)
				{
					GameObject go = (GameObject)Instantiate (_alienEntity,Vector3.zero,Quaternion.identity);
					go.transform.parent = _alien [1].transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localEulerAngles = Vector3.zero;
					_alienCurrent = go;
					_right = true;
				}
			}
			_generateInterval = Random.Range (3f, 7f);
			_generateTimer = 0.0f;
		//	_attackTimer = -2.0f;

		}
	}


	public void SetEnable(bool e)
	{
		_enable = e;
	}

}
