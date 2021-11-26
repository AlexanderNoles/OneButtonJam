using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{
    public static GameObject _instance;
    public static int maxHealth = 1;
    public static int currentHealth;
    public static bool gameOver = false;
    public static bool levelProperlyStarted = false;
    public float timeToLevelStart = 3f;
    public GameObject gameOverEffect;
    public GameObject gameWonEffect;

    private void Awake()
    {
        currentHealth = maxHealth;
        _instance = gameObject;
    }

    private void Update()
    {
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            GameOver();
        }

        if(timeToLevelStart < 0)
        {
            levelProperlyStarted = true;
        }
        else
        {
            timeToLevelStart -= Time.deltaTime;
        }
    }

    public static void GameOver()
    {
        if (!gameOver)
        {
            GameEnded();
            _instance.GetComponent<PlayerManagment>().gameOverEffect.SetActive(true);
        }     
    }

    public static void GameWon()
    {
        if(!gameOver)
        {
            GameEnded();
            _instance.GetComponent<PlayerManagment>().gameWonEffect.SetActive(true);
        }      
    }

    private static void GameEnded()
    {
        gameOver = true;
        _instance.GetComponent<SpriteRenderer>().enabled = false;
    }
}
