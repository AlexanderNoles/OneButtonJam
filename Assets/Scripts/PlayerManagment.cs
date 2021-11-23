using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{
    public static GameObject _instance;

    private void Awake()
    {
        _instance = gameObject;
    }
}
