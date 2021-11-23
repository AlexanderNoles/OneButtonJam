using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunner : MonoBehaviour
{
    public float range = 3.5f;
    public Vector3 currentTargetPos;
    public float turningSpeed = 3f;

    void Update()
    {
        Vector3 playerVector = PlayerManagment._instance.transform.position - transform.position;
        if (playerVector.sqrMagnitude < range * range)
        {
            //Player within range
            currentTargetPos = Vector3.Lerp(currentTargetPos, PlayerManagment._instance.transform.position, Time.deltaTime * turningSpeed);
            transform.rotation = currentRotation(currentTargetPos,transform.position);
        }
    }

    private Quaternion currentRotation(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 normalizedPlayerVector = (targetPos - currentPos).normalized;
        float rot_z = Mathf.Atan2(normalizedPlayerVector.y,normalizedPlayerVector.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f,0f,rot_z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
