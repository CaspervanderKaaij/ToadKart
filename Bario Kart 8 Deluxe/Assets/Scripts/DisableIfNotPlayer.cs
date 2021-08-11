using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfNotPlayer : MonoBehaviour {
    public Kart myKart;
    public AudioSource[] audioSources;

    void Start () {
        if (myKart.isPlayer == false) {
            for (int i = 0; i < audioSources.Length; i++) {
                Destroy (audioSources[i]);
            }
        }
        Destroy (this);
    }
}