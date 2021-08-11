using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorFlash : MonoBehaviour {
    Image image;
    public float onTime = 1;
    public float offTime = 1;
    void Start () {
        image = GetComponent<Image> ();
        On ();
    }

    void On () {
        image.enabled = true;
        Invoke ("Off", onTime);
    }

    void Off () {
        image.enabled = false;
        Invoke ("On", offTime);
    }
}