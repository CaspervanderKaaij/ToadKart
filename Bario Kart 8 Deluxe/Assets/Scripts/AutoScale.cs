using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour {
    public Vector3 goalScale = Vector3.one;
    public float speed = 1;

    void Update () {
        transform.localScale = Vector3.Lerp (transform.localScale, goalScale, Time.deltaTime * speed);
    }
}