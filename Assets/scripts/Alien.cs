using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour 
{
	private AlienAttackManager _alienAttackManager;
	// Use this for initialization
	void Start () 
	{
		_alienAttackManager = this.transform.parent.GetComponent<AlienAttackManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
