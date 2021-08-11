using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waluigi_Stage_Cam : MonoBehaviour {
    public Vector2 timeRange = Vector2.one;
    public Transform[] posses;
    void Start () {
        Invoke ("UpdatePos", Random.Range (timeRange.x, timeRange.y));
    }

    void UpdatePos () {
        Invoke ("UpdatePos", Random.Range (timeRange.x, timeRange.y));
        int rng = (int) Random.Range (0, posses.Length);
        transform.position = posses[rng].position;
        transform.eulerAngles = posses[rng].eulerAngles;
    }
}