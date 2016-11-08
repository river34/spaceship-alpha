using UnityEngine;
using System.Collections;

public class AlienPlanet : MonoBehaviour 
{
	public bool _is_flying = false;

	public bool _is_arrived = false;

	public void EnableFly()
	{
		_is_flying = true;
		StartCoroutine(Fly());
	}

	IEnumerator Fly()
	{
		while(Vector3.Distance( this.transform.position ,Vector3.zero) > 1000.0f)
		{
			this.transform.position -= new Vector3 (0, 0, Time.deltaTime * 200.0f);
			yield return null;
		}

		_is_arrived = true;
	}
}
