using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AsyncOperation asyncOperation;

    public void Play()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync("scene_game");
    }
}
