using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	
	private string _guest_1 = "Mary";
	private string _guest_2 = "Max";

    void OnGUI ()
	{
        Debug.Log ("OnGUI");

		GUI.BeginGroup(new Rect(1280,1024,1280,1024));

		_guest_1 = GUI.TextField (new Rect(390, 300, 200, 40), _guest_1);
		_guest_2 = GUI.TextField (new Rect(690, 300, 200, 40), _guest_2);

		GUI.EndGroup ();
    }

	public string GetGuest1()
	{
		return _guest_1;
	}

	public string GetGuest2()
	{
		return _guest_2;
	}
}
