using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndWhenEnemiesDead : MonoBehaviour
{
    private void Update()
    {
        if (transform.childCount == 0)
        {
            PlayerManagment.GameWon();
        }
    }
}
