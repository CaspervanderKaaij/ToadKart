using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpawn : MonoBehaviour {
    public static void SpawnSound (AudioClip clip, float volume, float pitch, float delay, float startPoint) {
        GameObject g = new GameObject ();
        g.name = "sfx " + clip.name;
        AudioSource sauce = g.AddComponent<AudioSource> ();
        sauce.clip = clip;
        sauce.volume = volume;
        sauce.pitch = pitch;
        sauce.time = startPoint;
        sauce.PlayDelayed (delay);
        Destroy (g, (clip.length * pitch) + delay - startPoint);
    }
}