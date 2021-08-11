using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartHopper : MonoBehaviour {
    public float jumpHeight = 1;
    public float minSpeedPercent = 50;
    private void OnTriggerEnter (Collider other) {
        Kart kart = other.GetComponent<Kart> ();
        if (kart != null) {
            if (kart.isGrounded == true && (kart.curState == Kart.State.Normal || kart.curState == Kart.State.Boost) && Vector3.Dot (transform.forward, kart.transform.forward) > 0 && kart.speedPerc * 100 > minSpeedPercent) {
                kart.moveV3.y = jumpHeight;
            }
        }
    }
}