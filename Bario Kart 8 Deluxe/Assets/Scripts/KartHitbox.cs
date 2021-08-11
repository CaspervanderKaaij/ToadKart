using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartHitbox : MonoBehaviour {
    public Kart myKart;

    private void OnTriggerEnter (Collider other) {
        if (other.GetComponent<KartHitbox> () != null) {
            BumpKart (other);
        }
        if (other.tag == "Hitbox") {
            myKart.GetHit ();
        }
    }

    void BumpKart (Collider other) {
        Kart kart = other.GetComponent<KartHitbox> ().myKart;
        float curSpeed = 0;
        curSpeed += Mathf.Abs (kart.moveV3.x) * Mathf.Abs (kart.moveV3.x);
        curSpeed += Mathf.Abs (kart.moveV3.z) * Mathf.Abs (kart.moveV3.z);
        curSpeed = Mathf.Sqrt (curSpeed);
        curSpeed /= 20;
        curSpeed = Mathf.Max (0.5f, curSpeed);
        myKart.Bonk ((transform.position - other.transform.position) * other.GetComponent<KartHitbox> ().myKart.kartStats.weight * curSpeed);
    }
}