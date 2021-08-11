using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaluigiStagePirhanaPlant : MonoBehaviour {
    public SkinnedMeshRenderer renderer;
    public float biteAnimSpeed = 1;
    public Transform inPos;
    public Transform outPos;
    public float inOutRate;
    public float moveSpeed = 5;
    public bool goingIn = true;
    void Start () {
        InvokeRepeating ("SwitchInOut", inOutRate, inOutRate);
    }

    void Update () {
        BiteAnim ();
        Move ();
    }

    void Move () {
        if (goingIn == true) {
            transform.position = Vector3.MoveTowards (transform.position, inPos.position, Time.deltaTime * moveSpeed);
        } else {
            transform.position = Vector3.MoveTowards (transform.position, outPos.position, Time.deltaTime * moveSpeed);
        }
    }

    void SwitchInOut () {
        goingIn = !goingIn;
    }
    void BiteAnim () {
        renderer.SetBlendShapeWeight (0, Mathf.PingPong (Time.time * 100 * biteAnimSpeed, 100));
    }
}