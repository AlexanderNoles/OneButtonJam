using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.flipY = transform.parent.rotation.eulerAngles.z > 90 && transform.parent.rotation.eulerAngles.z < 270;
    }
}
