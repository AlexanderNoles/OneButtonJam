using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoScroll : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    private float imageLength;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        imageLength = GetComponent<SpriteRenderer>().sprite.bounds.extents.x * 2;
    }

    private void Update()
    {
        transform.position += new Vector3(scrollSpeed * Time.deltaTime,0);
        if(transform.position.x > startPos.x + imageLength)
        {
            transform.position -= new Vector3(imageLength,0);
        }
    }
}
