using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private BoxCollider2D bc2D;
    private Camera mainCamera;
    public bool mouseHeldDown
    {
        get { return Input.GetMouseButton(0); }
    }
    private bool firstFrameMouseHeldDown
    {
        get { return Input.GetMouseButtonDown(0); }
    }

    public bool firstFrameMouseNotHeld
    {
        get { return Input.GetMouseButtonUp(0); }
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
        get { return new Vector2(transform.position.x, transform.position.y) + (Vector2.down * 0.57f) + (Vector2.left * 0.05f); }
    }
    public Vector2 groundCheckExtents = new Vector2(0.2f, 0.5f);

    private bool grounded
    {
        get { return Physics2D.OverlapBox(groundCheckPos, groundCheckExtents, 0f, ground); }
    }

    private Vector2 targetDirection = Vector2.zero;
    private Vector2 savedVel;
    private float normalFixedDeltaTime;
    [HideInInspector]
    public bool dashing;
    private Vector2 defaultColliderSize;
    public Vector2 dashingColliderSize;
    public PostProcessVolume dashEffects;
    public float effectsSpeed;
    public Transform enemyEmpty;
    public Transform enemyLocatorEmpty;
    public GameObject enemyLocator;
    private bool enemyLocatorsInstantited;

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
        bc2D = GetComponent<BoxCollider2D>();
        defaultColliderSize = bc2D.size;
        mainCamera = Camera.main;
        normalFixedDeltaTime = Time.fixedDeltaTime; //0.02
    }

    void Update()
    {      
        if (mouseHeldDown && !dashing)
        {
            dashEffects.weight = Mathf.Lerp(dashEffects.weight, 1, Time.deltaTime * effectsSpeed);
            //Enemy Locator Creation
            if (!enemyLocatorsInstantited)
            {
                int numOfTargets = 0;
                List<int> chosenInts = new List<int>();
                while (numOfTargets < 3 && numOfTargets < enemyEmpty.childCount)
                {
                    int chosenInt = Random.Range(0, enemyEmpty.childCount);
                    if (!chosenInts.Contains(chosenInt))
                    {
                        Transform currentTarget = enemyEmpty.GetChild(chosenInt);
                        GameObject currentObject = Instantiate(enemyLocator, enemyLocatorEmpty);
                        currentObject.transform.localPosition = Vector3.zero;
                        currentObject.GetComponent<TrackPos>().position = currentTarget.position;
                        chosenInts.Add(chosenInt);
                        numOfTargets++;
                    }
                }
                enemyLocatorsInstantited = true;
            }
            Time.timeScale = slowedDownTimeScale;
            Time.fixedDeltaTime = normalFixedDeltaTime / (1/slowedDownTimeScale);
            //Find vector between storedMousePos and the current mouse position
            targetDirection = Vector2.ClampMagnitude(mousePosInScreenSpace - new Vector2(transform.position.x,transform.position.y), 5f);
        }
        else
        {
            if (enemyLocatorsInstantited)
            {
                foreach (Transform child in enemyLocatorEmpty)
                {
                    Destroy(child.gameObject);
                }
                enemyLocatorsInstantited = false;
            }
            dashEffects.weight = 0;
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
            gameObject.layer = 12;
            bc2D.size = defaultColliderSize;
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
        else
        {
            bc2D.size = dashingColliderSize;
            gameObject.layer = 9;
            //EnemyKillCheck
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position,new Vector2(1,1),0);
            foreach(Collider2D hit in hits)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    hit.SendMessage("Die");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red - (Color.black * 0.5f);
        Gizmos.DrawCube(groundCheckPos, groundCheckExtents);
    }
}
