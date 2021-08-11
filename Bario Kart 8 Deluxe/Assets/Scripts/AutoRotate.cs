using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 v3;

    void Update()
    {
        transform.Rotate(v3 * Time.deltaTime);
    }
}
