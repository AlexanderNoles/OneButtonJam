using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement pm;
    private SpriteRenderer sr;
    private bool firstFrameDash;
    public Sprite normalSprite;
    public Sprite dashSprite;
    public Sprite fallSprite;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (pm.dashing)
        {
            if (firstFrameDash)
            {
                sr.sprite = dashSprite;
                sr.flipY = pm.rb.velocity.x < 0;
                float targetX = pm.rb.velocity.normalized.x;
                float targetY = pm.rb.velocity.normalized.y;
                transform.right = (transform.position + new Vector3(targetX, targetY)) - transform.position;
                firstFrameDash = false;
            }
        }
        else
        {
            firstFrameDash = true;
            sr.flipX = false;
            sr.flipY = false;
            transform.rotation = Quaternion.identity;
            if(pm.rb.velocity.y == 0)
            {
                sr.sprite = normalSprite;
            }
            else
            {
                sr.sprite = fallSprite;
            }
        }

        if (pm.firstFrameMouseNotHeld)
        {
            firstFrameDash = true;
        }
    }
}
