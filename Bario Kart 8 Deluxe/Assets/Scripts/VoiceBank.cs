using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Character", menuName = "Voice")]
public class VoiceBank : ScriptableObject {
    public AudioClip[] getHit;
    public AudioClip fall;
     public AudioClip[] smallBoost;
      public AudioClip[] boost;

}