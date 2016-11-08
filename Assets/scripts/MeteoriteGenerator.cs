using UnityEngine;
using System.Collections;

public class MeteoriteGenerator : MonoBehaviour 
{
	public float _generateTime;
	public GameObject _meteorite;
	private float _timer;
	public bool _enable;
	// Use this for initialization
	void Start () 
	{
		_enable = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_enable)
		{
			_timer += Time.deltaTime;
			if(_timer > _generateTime)
			{
				_timer = 0;
				Vector3 posi = new Vector3(Random.Range(-100,100),Random.Range(-100,100),500f);
				GameObject go = (GameObject)Instantiate(_meteorite,posi,Quaternion.identity);
				go.transform.parent = this.transform;
			}
		}
	}

	public void SlowDownAll(float _mul)
	{
		_generateTime /= _mul;
		this.gameObject.BroadcastMessage("SlowDown",_mul);
	}
		
}
