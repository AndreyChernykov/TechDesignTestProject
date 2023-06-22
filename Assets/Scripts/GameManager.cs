using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPoints;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject[] monsters;
    [SerializeField] float spawnDelay;
    [SerializeField] Transform spawnPoint;

    public static int points;
    GameObject characters;

    private void Start()
    {
        characters = GameObject.Find("Characters");

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
}
