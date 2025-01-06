using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [SerializeField] private Animation sceneTransitionAnimtion;

    private void Awake() => Instance = this;

    public void SwitchToScene(string sceneName)
    {
        StartCoroutine(SceneSwitchCoroutine(sceneName));
    }

    private IEnumerator SceneSwitchCoroutine(string sceneName)
    {
        sceneTransitionAnimtion.Play("TransitionStart");
        yield return new WaitForSeconds(sceneTransitionAnimtion.GetClip("TransitionStart").length);

        if(SceneManager.GetActiveScene().name == "Level24")
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
       
    }
}
