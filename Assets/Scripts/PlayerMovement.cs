﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    public bool mouseHeldDown
    {
        get { return Input.GetMouseButton(0); }
    }
    private bool firstFrameMouseHeldDown
    {
        get { return Input.GetMouseButtonDown(0); }
    }
    private Vector2 mousePosInScreenSpace
    {
        get { return mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraCanvas.planeDistance)); }
    }
    private int horizontalDirection
    {
        get
        {
            if (rb.velocity.x == 0)
            {
                return 0;
            }
            return (int)(rb.velocity.x / rb.velocity.x);
        }
    }

    //Ground check
    private Vector2 groundCheckPos
    {
        get { return new Vector2(transform.position.x, transform.position.y) + (Vector2.down * 0.45f); }
    }
    public Vector2 groundCheckExtents = new Vector2(0.2f, 0.5f);

    private bool grounded
    {
        get { return Physics2D.OverlapBox(groundCheckPos, groundCheckExtents, 0f, ground); }
    }

    private Vector2 storedMousePos;
    private Vector2 targetDirection = Vector2.zero;
    private Vector2 savedVel;
    private float normalFixedDeltaTime;
    private bool dashing;

    const float horizontalDrag = 0.02f;
    const float gravity = 0.2f;


    public Canvas cameraCanvas;
    public float slowedDownTimeScale = 0.01f;
    public float dashMultiplier = 4f;
    public float dashTime = 0.5f;
    private float timeLeft;
    public LayerMask ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        normalFixedDeltaTime = Time.fixedDeltaTime; //0.02
    }

    void Update()
    {
        if (mouseHeldDown)
        {
            Time.timeScale = slowedDownTimeScale;
            Time.fixedDeltaTime = normalFixedDeltaTime / (1/slowedDownTimeScale);
            if (firstFrameMouseHeldDown)
            {
                storedMousePos = mousePosInScreenSpace;
            }
            //Find vector between storedMousePos and the current mouse position
            targetDirection = Vector2.ClampMagnitude(mousePosInScreenSpace - new Vector2(transform.position.x,transform.position.y), 5f);
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = normalFixedDeltaTime;
            if (targetDirection != Vector2.zero)
            {
                dashing = true;
                savedVel = Vector2.ClampMagnitude(targetDirection, 2);
                rb.velocity = targetDirection * dashMultiplier;           
                timeLeft = dashTime;
                targetDirection = Vector2.zero;
            }

            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else if (dashing)
            {
                dashing = false;
                if(rb.velocity.y < savedVel.y)
                {
                    rb.velocity = new Vector2(savedVel.x,rb.velocity.y);
                }
                else
                {
                    rb.velocity = savedVel;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dashing)
        {
            if (!grounded)
            {
                rb.velocity += (Vector2.down * gravity);
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, horizontalDirection * horizontalDrag), rb.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red - (Color.black * 0.5f);
        Gizmos.DrawCube(groundCheckPos, groundCheckExtents);
    }
}