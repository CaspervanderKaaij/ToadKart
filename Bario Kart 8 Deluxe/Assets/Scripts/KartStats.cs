using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kart", menuName = "KartType")]
public class KartStats : ScriptableObject
{
    public float speed;
    public float steeringSpeed;//top speed when steering/ going left or right
    public float backwardsSpeed;
    public float acceleration1;
    public float acceleration2;
    public float acceleration3;
    public float acceleration4;
    public float accelerationBase;
    public float handling;//turn acceleration speed
    public float handlingRange;//the max turn speed
    public float drift;//how fast to change direction while drifting
    public float driftRange;//for inputting left and right while drifting
    public float miniTurboDuration;
    public float offRoadSpeed;
    public float offRoadHandling;//turn speed off road
    public float traction;//slippyness
    public float weight;//how much you move when someone bumps into you
    [Header("SFX")]
    public AudioClip sfx_Idle;
    public AudioClip sfx_EngineLoop;
    public AudioClip sfx_Horn;
    public AudioClip sfx_DashMini;
    public AudioClip sfx_Dash;

}