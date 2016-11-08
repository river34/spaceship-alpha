using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

	RootController _root;
	Animator anim;
	Vector3 origin;
	private float _attackTimer = -1.0f;
	private bool _isDead = false;
	private AlienAttackManager _alienAttackManager;
	private AudioSource _scream;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
		_root = GameObject.FindGameObjectWithTag ("Root").GetComponent<RootController> ();
		_scream = GetComponent<AudioSource> ();
	}

	void Start ()
	{
		anim.SetBool ("IsKnocking", true);
		_attackTimer = -2.0f;
		_alienAttackManager = this.transform.parent.parent.GetComponent<AlienAttackManager>();

	}

	void Update ()
	{
		_attackTimer += Time.deltaTime;
		if (_attackTimer > 6.0f && !_isDead)
		{
			anim.SetBool("Attack",true);
		}

		/*
		if (Input.GetKey (KeyCode.Q))
		{
			anim.SetBool ("IsAway", true);
			anim.SetBool ("IsKnocking", false);
			anim.Play ("FlyAway");
		}
		*/
		/*
		if (anim.GetBool ("IsKnocking") && Distance (origin, transform.position) >= 55f)
		{
			transform.position += Vector3.Normalize(origin - transform.position) * 20f * Time.deltaTime;
		}

		if (anim.GetBool ("IsAway"))
		{
			transform.position += Vector3.up * 30f * Time.deltaTime;
		}

		if (anim.GetBool ("IsAway") && Distance (origin, transform.position) <= 65f)
		{
			transform.position += Vector3.Normalize(transform.position - origin) * 30f * Time.deltaTime;
		}
		*/
	}

	IEnumerator DelayAttack()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject go = GameObject.FindGameObjectWithTag("Signal");
		if (go)
		{
			go.GetComponent<Signal>().Disorder();
			_root.PlayDecodeFailedSound ();
		}
	}

	float Distance (Vector3 one, Vector3 two)
	{
		one.y = two.y;
		return Vector3.Distance (one, two);
	}

	public void Die ()
	{
		//	anim.SetBool ("IsAway", true);
		//	anim.SetBool ("IsKnocking", false);
		//	anim.SetBool("Dead",true);
		anim.Play ("FlyAway");
		_isDead = true;
		Destroy (this.gameObject, 10.0f);
	}

	public void HitWindow ()
	{
		_root.PlayDecodeFailedSound ();

		if (_alienAttackManager._left) {
			_root.AttackSoundLeft ();
			_root.ForceShake (1, 2, .1f);
		}

		if (_alienAttackManager._right) {
			_root.AttackSoundRight ();
			_root.ForceShake (-1, 2, .1f);
		}

		GameObject go = GameObject.FindGameObjectWithTag("Signal");

		if (go)
		{
			go.GetComponent<Signal>().Disorder();

		}
	}

	public void HitWindowMinor ()
	{
		_root.PlayTapSound ();

		if (_alienAttackManager._left) {
			_root.AttackSoundMinorLeft ();
		}

		if (_alienAttackManager._right) {
			_root.AttackSoundMinorRight ();
		}
	}

	public void ResetTimer ()
	{
		_attackTimer = 0.0f;
		anim.SetBool ("Attack", false);
	}

	public void Scream ()
	{
		_scream.Play ();
	}
}
