using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHolder : MonoBehaviour {
    public Checkpoint lastCheckpoint;
    public Checkpoint lastKeyCheckpoint;
    DemoGameManager manager;
    public bool[] reachedCheckpoints;
    public int curLap = 0;

    void Start () {
        manager = FindObjectOfType<DemoGameManager> ();
        //set up first checkpoint at start
        lastCheckpoint = manager.allCheckpoints.finish;
        lastKeyCheckpoint = manager.allCheckpoints.finish;
        reachedCheckpoints = new bool[manager.allCheckpoints.allKeyCheckpoints.Count];
    }

    public void TryNewLap () {
        bool didAll = true;
        for (int i = 0; i < reachedCheckpoints.Length; i++) {
            if (reachedCheckpoints[i] == false) {
                didAll = false;
            }
        }

        if (didAll == true) {
            LapComplete ();
        }
    }

    void LapComplete () {
        for (int i = 1; i < reachedCheckpoints.Length; i++) { //skipping checkpoint 0 cause you're on it when going over a finish line
            reachedCheckpoints[i] = false;
        }
        curLap++;
        manager.DoLap (curLap);
    }
}