using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPoints;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] GameObject exitPanel;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject[] monsters;
    [SerializeField] GameObject[] helpMessages;
    [SerializeField] GameObject windowPause;
    [SerializeField] float spawnDelay;
    [SerializeField] Transform spawnPoint;

    public static int points;
    bool isPause;

    GameObject characters;
    AsyncOperation asyncOperation;

    private void Start()
    {
        characters = GameObject.Find("Characters");

        StartCoroutine(Help());
        StartCoroutine(SpawnMonsters());
    }

    private void Update()
    {
        DataDisplayed();
    }

    void DataDisplayed()
    {
        textPoints.text = "Points " + points;
        textHealth.text = "Health " + playerManager.Health;
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameObject monster = Instantiate(monsters[Random.Range(0, monsters.Length)], spawnPoint.position, Quaternion.identity);
            monster.transform.parent = characters.transform;
            
        }
    }

    public void ExitActivate()
    {
        exitPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync("scene_menu");
    }

    IEnumerator Help()
    {
        for(int i = 0; i < helpMessages.Length; i++)
        {
            yield return new WaitForSeconds(2);
            helpMessages[i].SetActive(true);
            yield return new WaitForSeconds(3);
            helpMessages[i].SetActive(false);
        }
    }

    public void Pause()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1f;
            windowPause.SetActive(false);
        }
        else
        {
            isPause = true;
            Time.timeScale = 0;
            windowPause.SetActive(true);
        }
    }
}
