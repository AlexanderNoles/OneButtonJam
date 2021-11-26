using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{
    public static GameObject _instance;
    public static int maxHealth = 1;
    public static int currentHealth;

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
    }

    public static void GameOver()
    {
        
    }
}
