using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoGameManager : MonoBehaviour {
    public enum State {
        IntroCutscene,
        Countdown,
        Race,
        Finish
    }

    public State curState = State.IntroCutscene;
    State lastState = State.IntroCutscene;
    public Kart[] karts;
    public int courseID = 0;
    public AllCheckpoints allCheckpoints;
    [Header ("Intro Cutscene")]
    public GameObject introObj;
    public float introTime = 5;
    [Header ("Countdown")]
    public GameObject countDownUI;
    public GameObject controlsUI;
    public float waitForCountdown;
    public AudioClip beforeCountdownSound;
    public GameObject fadeObj;
    [Header ("Race")]
    public GameObject hud;
    public GameObject song;
    public AudioClip lap2Sound;
    public AudioClip lap3Sound;
    public Text lapTxt;
    [Header ("Finish")]
    public FinishMenu finishMenu;
    public AudioClip finishSound;
    public AudioClip finishFanfare;
    public AudioClip finishMusic;
    public Timer timer;
    void Start () {
        karts = (Kart[]) FindObjectsOfType<Kart> ();
        hud.SetActive (false);

        ActivateNewState ();
        lastState = curState;

    }

    void Update () {
        if (curState != lastState) {
            ActivateNewState ();
            lastState = curState;
        }
        /*
            if (Input.GetKeyDown (KeyCode.Alpha2)) {
                DoLap2 ();
            }
            if (Input.GetKeyDown (KeyCode.Alpha3)) {
                DoLap3 ();
            }
            if (Input.GetKeyDown (KeyCode.Alpha4)) {
                Finish ();
            }
            */
    }

    public void DoLap (int lap) {
        switch (lap) {
            case 1:
                DoLap2 ();
                break;
            case 2:
                DoLap3 ();
                break;
            case 3:
                Finish ();
                break;
        }
    }

    void ActivateNewState () {
        switch (curState) {
            case State.IntroCutscene:
                DoIntroCutscene ();
                break;
            case State.Countdown:
                Invoke ("DoCountdown", waitForCountdown);
                SetkartInControl (true);
                SetkartState (Kart.State.Countdown);
                AudioSpawn.SpawnSound (beforeCountdownSound, 1, 1, 0, 0);
                controlsUI.SetActive (true);
                fadeObj.SetActive (true);
                break;
            case State.Race:
                hud.SetActive (true);
                fadeObj.SetActive (true);
                Invoke ("SetSongActive", 1);
                break;
        }
    }

    void DoLap2 () {
        AudioSpawn.SpawnSound (lap2Sound, 1, 1, 0, 0.1f);
        lapTxt.text = "2";
    }

    void DoLap3 () {
        AudioSpawn.SpawnSound (lap3Sound, 1, 1, 0, 0);
        song.GetComponent<AudioSource> ().Pause ();
        Invoke ("Lap3P2", lap3Sound.length);
        lapTxt.text = "3";
    }

    void Lap3P2 () {
        AudioSource source = song.GetComponent<AudioSource> ();
        source.pitch = 1.1f;
        source.time = 0;
        source.Play ();
    }

    void Finish () {
        StartCoroutine ("FinishEvents");
    }

    IEnumerator FinishEvents () {
        //I dont do all events in the finishmenu script because there is already a lot of data here. It's a little more messy though, I admit. I did this knowingly though.
        curState = State.Finish;
        AudioSource source = song.GetComponent<AudioSource> ();
        finishMenu.CheckState ();
        AudioSpawn.SpawnSound (finishSound, 1, 1, 0, 0);
        source.Pause ();
        SetkartInControl (false);
        DisableBackCam ();
        hud.SetActive (false);
        controlsUI.SetActive (false);

        yield return new WaitForSeconds (1.5f);
        AudioSpawn.SpawnSound (finishFanfare, 1, 1, 0, 0);
        yield return new WaitForSeconds (finishFanfare.length);
        source.pitch = 1f;
        source.time = 0;
        source.clip = finishMusic;
        source.Play ();
    }

    void DisableBackCam () {
        for (int i = 0; i < karts.Length; i++) {
            karts[i].ForceNoBackCam ();
        }
    }

    void SetSongActive () {
        song.SetActive (true);
    }

    void DoCountdown () {
        countDownUI.SetActive (true);
        Invoke ("EndCountdown", 3.4f);
    }

    void EndCountdown () {
        SetkartState (Kart.State.Normal);
        curState = State.Race;
    }

    void DoIntroCutscene () {
        introObj.SetActive (true);
        Invoke ("EndIntro", introTime);
        SetkartInControl (false);

    }
    void EndIntro () {
        curState = State.Countdown;
        introObj.SetActive (false);
        SetkartInControl (true);
        fadeObj.SetActive (true); //Doing this at multiple places for the sake of it now looking buggy while debugging. This is the important one
    }
    void SetkartInControl (bool inCon) {
        for (int i = 0; i < karts.Length; i++) {
            karts[i].inControl = inCon;
        }
    }

    void SetkartState (Kart.State newState) {
        for (int i = 0; i < karts.Length; i++) {
            karts[i].curState = newState;
        }
    }
}