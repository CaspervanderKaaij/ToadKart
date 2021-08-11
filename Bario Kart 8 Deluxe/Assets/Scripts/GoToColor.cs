using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoToColor : MonoBehaviour {
    public Color colorGoal;
    public float lerpSpeed = 1;
    [HideInInspector] public Image img;
    Color startColor;
    void Start () {
        img = GetComponent<Image> ();
        startColor = img.color;
    }

    float tim = 0;
    void Update () {
        if (tim <= 1)
            tim += Time.deltaTime * lerpSpeed;
        img.color = Color.Lerp (startColor, colorGoal, tim);
    }

    public void SetToNewColor (Color newClr, float sped) {
        lerpSpeed = sped;
        startColor = img.color;
        tim = 0;
        colorGoal = newClr;
    }
}