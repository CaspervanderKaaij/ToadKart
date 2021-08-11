using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadBlink : MonoBehaviour {
    public Kart myKart;
    Renderer rend;
    public int matNum = 0;
    public Texture[] idleTexture;
    public Texture boostexture;
    public Texture hitTexture;
    public enum State {
        Idle,
        Boost,
        Hit
    }
    public State curState;
    void Start () {
        rend = GetComponent<Renderer> ();
        StartCoroutine (BlinkAnim ());
    }

    int curBlinkFrame = 0;
    void Update () {
        SetCurState ();
        switch (curState) {
            case State.Idle:
                rend.materials[matNum].mainTexture = idleTexture[curBlinkFrame];
                break;
            case State.Boost:
                rend.materials[matNum].mainTexture = boostexture;
                break;
            case State.Hit:
                rend.materials[matNum].mainTexture = hitTexture;
                break;
        }
    }

    void SetCurState () {
        switch (myKart.curState) {
            case Kart.State.Normal:
            case Kart.State.Drift:
            case Kart.State.Countdown:
            case Kart.State.Destroy:
                curState = State.Idle;
                break;
            case Kart.State.Boost:
                curState = State.Boost;
                break;
            case Kart.State.Hit:
                curState = State.Hit;
                break;
        }
    }

    IEnumerator BlinkAnim () {
        curBlinkFrame = 0;
        yield return new WaitForSeconds (Random.Range (4, 7));
        curBlinkFrame = 1;
        yield return new WaitForSeconds (0.03f);
        curBlinkFrame = 2;
        yield return new WaitForSeconds (0.06f);
        curBlinkFrame = 3;
        yield return new WaitForSeconds (0.03f);
        StartCoroutine (BlinkAnim ());
    }
}