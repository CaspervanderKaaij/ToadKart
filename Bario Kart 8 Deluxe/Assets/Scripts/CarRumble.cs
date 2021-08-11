using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRumble : MonoBehaviour {
    float startY;
    public float intensity = 0.1f;
    void Start () {
        startY = transform.localPosition.y;
    }

    void Update () {
        transform.localPosition = new Vector3 (transform.localPosition.x, startY + Random.Range (-intensity / 2, intensity / 2), transform.localPosition.z);
    }
}