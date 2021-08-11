using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {
    public GameObject breakEffect;
    public AudioClip breakClip;
    public float respawnTime = 5;
    void OnTriggerEnter (Collider other) {
        if (other.GetComponent<ItemManager> () != null) {
            other.GetComponent<ItemManager> ().GetItem ();
            gameObject.SetActive (false);
            Instantiate (breakEffect, transform.position, Quaternion.identity);
            AudioSpawn.SpawnSound (breakClip, 1, Random.Range (0.95f, 1.05f), 0, 0);
            Invoke ("Respawn", respawnTime);
        }
    }

    void Respawn () {
        transform.localScale = Vector3.zero;
        gameObject.SetActive (true);
    }
}