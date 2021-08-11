using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMoveToPos : MonoBehaviour {
    RectTransform rect;
    public Vector3 goalPos;
    public float lerpSpeed;
    void Start () {
        rect = GetComponent<RectTransform> ();
    }

    void Update () {
        rect.localPosition = Vector3.Lerp (rect.localPosition, goalPos, Time.deltaTime * lerpSpeed);
    }
}