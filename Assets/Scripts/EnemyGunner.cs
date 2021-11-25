using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
{
    private SpriteRenderer sr;
    public float range = 3.5f;
    public float turningTime = 3f;
    private float turningTimeLeft;
    public LayerMask obstacles;

    [Header("Firing")]
    public float timeBetweenShots = 0.3f;
    private float timeLeft;
    public GameObject bulletEmpty;
    public GameObject bullet;
    public float bulletSpeed = 4f;
    private Vector3 savedPos;

    [Header("Blood Effect")]
    public Vector3 offset;

    private void Start()
    {
        savedPos = bulletEmpty.transform.localPosition;
        sr = GetComponent<SpriteRenderer>();
        turningTimeLeft = turningTime;
    }

    void Update()
    {
        Vector3 playerVector = PlayerManagment._instance.transform.position - transform.position;
        if (playerVector.sqrMagnitude < range * range)
        {
            //Player within range
            if (!GroundInBetween(playerVector)) 
            {
                //No obstacles in the way
                bool intendedFlip = playerVector.x < 0;
                if(intendedFlip != sr.flipX)
                {
                    if(turningTimeLeft < 0)
                    {
                        sr.flipX = intendedFlip;
                    }
                    else
                    {
                        turningTimeLeft -= Time.deltaTime;
                    }
                }
                else
                {
                    turningTimeLeft = turningTime;
                    setBulletEmptyPos(sr.flipX);
                    if (timeLeft < 0)
                    {
                        //Fire
                        GameObject currentBullet = Instantiate(bullet, bulletEmpty.transform);
                        currentBullet.transform.position = bulletEmpty.transform.position;
                        currentBullet.transform.rotation = currentRotation(bulletEmpty.transform.position, transform.position);
                        currentBullet.GetComponent<BulletControl>().velVector = -(transform.position - bulletEmpty.transform.position).normalized * bulletSpeed;
                        currentBullet.GetComponent<BulletControl>().original = false;
                        timeLeft = timeBetweenShots;
                    }
                    else
                    {
                        timeLeft -= Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            timeLeft = timeBetweenShots;
        }
    }

    private Quaternion currentRotation(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 normalizedPlayerVector = (targetPos - currentPos).normalized;
        float rot_z = Mathf.Atan2(normalizedPlayerVector.y,normalizedPlayerVector.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f,0f,rot_z);
    }

    private void setBulletEmptyPos(bool flipped)
    {
        if (!flipped)
        {
            bulletEmpty.transform.localPosition = savedPos;
            return;
        }
        bulletEmpty.transform.localPosition = -savedPos;
    }

    private bool GroundInBetween(Vector3 direction)
    {
        //Debug.DrawLine(transform.position, transform.position + (direction * range),Color.red,1f);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, Mathf.Clamp(direction.magnitude, 0, range), obstacles);
        return hit.Length > 0;
    }

    private void Die()
    {
        //Temporary
        Instantiate(Resources.Load("DeadEnemy"), transform.position + (Vector3.down * 0.5f), Quaternion.identity);
        GameObject bloodSplatter = (GameObject)Instantiate(Resources.Load("BloodSplatter"), transform.position + offset, Quaternion.identity);
        bloodSplatter.GetComponent<RectTransform>().SetParent(BloodSplatterManagment.targetParent.transform);
        bloodSplatter.GetComponent<RectTransform>().localScale = Vector3.one;
        bloodSplatter.GetComponent<RectTransform>().Rotate(Vector3.forward * (90 * Random.Range(0,4)));
        bulletEmpty.transform.parent = bloodSplatter.transform;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
