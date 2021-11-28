using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManagment : MonoBehaviour
{
    public static GameObject _instance;
    public static PlayerManagment managmentInstance;
    public static int maxHealth = 1;
    public static int currentHealth;
    public static bool gameOver = false;
    public static bool levelProperlyStarted = false;
    public static float finalTime;
    public float timeToLevelStart = 3f;
    public GameObject gameOverEffect;
    public GameObject gameWonEffect;
    public TextMeshProUGUI timeText;

    private void Awake()
    {
        gameOver = false;
        levelProperlyStarted = false;
        currentHealth = maxHealth;
        _instance = gameObject;
        managmentInstance = this;
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
            managmentInstance.gameOverEffect.SetActive(true);
        }     
    }

    public static void GameWon()
    {
        if(!gameOver)
        {
            GameEnded();
            managmentInstance.gameWonEffect.SetActive(true);
            managmentInstance.timeText.text = $"Final Time: {finalTime.ToString().Substring(0,finalTime.ToString().IndexOf(".")+3)}";
        }      
    }

    private static void GameEnded()
    {
        gameOver = true;
        finalTime = TimerControl.currentTime;
        _instance.GetComponent<SpriteRenderer>().enabled = false;
    }
}
