using UnityEngine;
using System.Collections;

public class Meteorite : MonoBehaviour 
{
	public Vector3 _speed;
	public Vector3 _rotateSpeed;
	public bool _isAlien;

    private float _stop_point;

	// Use this for initialization
	void Start () 
	{
		//while(true)
		//{
		if(!_isAlien)
		{
			
			this.transform.localScale = Vector3.one * Random.Range(5f,10f);
			_rotateSpeed = new Vector3(Random.Range(-40f,40f),Random.Range(-40f,40f),Random.Range(-40f,40f));
		}
		if (_isAlien)
		{
			_rotateSpeed = Vector3.zero;
			this.transform.eulerAngles = new Vector3(0,180f,0);
		}

		_speed = new Vector3(Random.Range(-2f,2f),Random.Range(-5f,-1f),Random.Range(-30f,-50f));

        _stop_point = Random.Range(-10f, 10f);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_isAlien)
        {
            if (this.transform.position.z < _stop_point)
            {
                return;
            }
        }

        this.transform.position += _speed * Time.deltaTime;
		this.transform.localEulerAngles += _rotateSpeed * Time.deltaTime;

		if(this.transform.position.z < -100f)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("SpaceShip"))
		{
			//TODO::Shake the spaceship
			Destroy(this.gameObject);
		}
		
	}

	public void SlowDown(float _mul)
	{
		_speed *= _mul;
	}
}
