using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Fade _Fade;

    void Awake ()
    {
        _Fade = GameObject.Find ("Root").GetComponent<Fade>();
    }

    public void GoToNextScene()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            StartCoroutine(ChangeScene("Credits"));
        }
        else if (SceneManager.GetActiveScene().name == "Credits")
        {
            StartCoroutine(ChangeScene("Main"));
        }
    }

    public void GoToScene(string name)
    {
        StartCoroutine(ChangeScene(name));
    }

    IEnumerator ChangeScene(string scene)
    {
        float fadeTime = _Fade.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}
