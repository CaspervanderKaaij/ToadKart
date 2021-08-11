using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    Text text;
    void Start () {
        text = GetComponent<Text> ();
    }

    [HideInInspector] public float curTim = 0;
    void Update () {
        curTim += Time.deltaTime;
        text.text = ConvertToTime (curTim);
    }

    public string ConvertToTime (float tim) {

        float min = Mathf.FloorToInt (tim / 60);
        float sec = Mathf.FloorToInt (tim % 60);
        float mil = (tim % 1) * 1000;
        return string.Format ("{0:00}:{1:00}:{2:000}", min, sec, mil);
    }
}