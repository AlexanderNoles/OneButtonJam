using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChildOnLevelLoaded : MonoBehaviour
{
    void Update()
    {
        transform.GetChild(0).gameObject.SetActive(PlayerManagment.levelProperlyStarted);
    }
}
