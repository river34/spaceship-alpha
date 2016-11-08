using UnityEngine;
using System.Collections;

public class Coodinate : MonoBehaviour 
{
	public Vector3 _place1;
	public Vector3 _place2;
	public GameObject _current;
	public GameObject _correctDesti;
	public float _moveSpeed = 10.0f;
	public float _range = 0.8f;

	public bool _isWin = false;
	public int _targetPlace = 0;
	// Use this for initialization
	void Start () 
	{
		_place1 += this.transform.position;
		_place2 += this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputKey();
		CheckWin();
	}

	void InputKey()
	{
		if(Input.GetKey(KeyCode.A))
		{
			_current.transform.position -= new Vector3( _moveSpeed * Time.deltaTime , 0 , 0 );
		}
		if(Input.GetKey(KeyCode.D))
		{
			_current.transform.position += new Vector3( _moveSpeed * Time.deltaTime , 0 , 0 );
		}

		if(Input.GetKey(KeyCode.W))
		{
			_current.transform.position += new Vector3( 0 ,  _moveSpeed * Time.deltaTime , 0 );
		}

		if(Input.GetKey(KeyCode.S))
		{
			_current.transform.position -= new Vector3( 0 ,  _moveSpeed * Time.deltaTime , 0 );
		}
	}

	void CheckWin()
	{
		if(_targetPlace == 0)
		{
			if(Vector3.Distance(_place1,_current.transform.position) < _range)
			{
				GameObject go = (GameObject) Instantiate(_correctDesti,_place1,Quaternion.identity);
				go.transform.parent = this.transform;
				_targetPlace = 1;
			}
		}
		else if(_targetPlace == 1)
		{
			if(Vector3.Distance(_place2,_current.transform.position) < _range)
			{
				GameObject go = (GameObject)Instantiate(_correctDesti,_place2,Quaternion.identity);
				go.transform.parent = this.transform;
				_targetPlace = 2;
				_isWin = true;
				_current.GetComponent<Renderer> ().enabled = false;
			}
		}
	}

}
