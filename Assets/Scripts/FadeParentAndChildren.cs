using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeParentAndChildren : MonoBehaviour
{
    public float speed = 3f;
    public bool destroyOnFadeEnd;
    public float alphaDestroyCutoff = 0.05f;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Start()
    {
        spriteRenderers.Add(GetComponent<SpriteRenderer>());
        foreach(Transform child in transform)
        {
            spriteRenderers.Add(child.GetComponent<SpriteRenderer>());
        }
    }

    private void Update()
    {       
        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b,Mathf.Lerp(sr.color.a,0,Time.deltaTime * speed));
            if(sr.color.a < alphaDestroyCutoff)
            {
                Destroy(gameObject);
            }
        }
    }
}
