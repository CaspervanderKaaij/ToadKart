using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetCountdownText : MonoBehaviour {
    int cur = 0;
    public Text text;
    public void SetMyCountDownText () {
        switch (cur) {
            case 0:
                text.text = "2";
                break;
            case 1:
                text.text = "1";
                break;
            case 2:
                text.text = "GO!";
                break;
        }
        cur++;
    }
}