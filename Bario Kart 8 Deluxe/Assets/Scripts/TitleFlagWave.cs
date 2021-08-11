using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFlagWave : MonoBehaviour {
    SkinnedMeshRenderer renderer;
    public float speed = 1;
    void Start () {
        renderer = GetComponent<SkinnedMeshRenderer> ();
    }

    void Update () {
        renderer.SetBlendShapeWeight (0, Mathf.PingPong (Time.time * speed, 100));
        renderer.SetBlendShapeWeight (1, Mathf.PingPong (100 + (Time.time * speed), 100));
    }
}