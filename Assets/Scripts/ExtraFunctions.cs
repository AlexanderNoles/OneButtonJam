using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFunctions : MonoBehaviour
{
    public static Quaternion currentRotation(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 normalizedPlayerVector = (targetPos - currentPos).normalized;
        float rot_z = Mathf.Atan2(normalizedPlayerVector.y, normalizedPlayerVector.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z);
    }
}
