using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterManagment : MonoBehaviour
{
    public static GameObject targetParent;
    public static Canvas _canvas;

    void Start()
    {
        targetParent = gameObject;
        _canvas = transform.GetComponentInParent<Canvas>();
    }
}
