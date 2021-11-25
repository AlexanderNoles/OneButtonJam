using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Vector3 velVector;
    public bool original;
    public float maxTime = 4f;

    public Vector2 bulletCheckSize;
    public LayerMask obstacles;
    public LayerMask player;

    private bool intersectingObstacle
    {
        get { return Physics2D.OverlapBox(transform.position, bulletCheckSize, 0, obstacles); }
    }

    private bool intersectingPlayer
    {
        get { return Physics2D.OverlapBox(transform.position, bulletCheckSize, 0, player); }
    }

    private void Update()
    {
        transform.position += velVector * Time.deltaTime;
        if (intersectingObstacle)
        {
            Destroy(gameObject);
        }
        else if (intersectingPlayer)
        {
            PlayerManagment.currentHealth -= 1;
        }
        if(maxTime < 0)
        {
            Destroy(gameObject);
        }
        else if (!original)
        {
            maxTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green - (Color.black * 0.5f);
        Gizmos.DrawCube(transform.position,bulletCheckSize);
    }
}
