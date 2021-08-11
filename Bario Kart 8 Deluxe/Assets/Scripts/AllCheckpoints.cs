using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCheckpoints : MonoBehaviour {
    public List<Checkpoint> all;
    public List<Checkpoint> allKeyCheckpoints;
    public Checkpoint finish;

    public void Setup () {
        all.Clear ();
        allKeyCheckpoints.Clear ();
        int lastRegion = -1; // not 0 because I want the first region to be 0, and when it detected a new region, it add 1 to lastRegion.
        for (int i = 0; i < transform.childCount; i++) {
            //set up my variables
            all.Add (transform.GetChild (i).GetComponent<Checkpoint> ());
            if (all[i].checkpointType != Checkpoint.CheckpointType.Normal) {
                allKeyCheckpoints.Add (all[i]);
                lastRegion++;
                if (all[i].checkpointType == Checkpoint.CheckpointType.Finish) {
                    finish = all[i];
                }
            }

            //set up some checkpoint variables
            all[i].realPos = all[i].GetPos ();
            Checkpoint frontCheckpoint = transform.GetChild ((int) Mathf.Repeat (i + 1, transform.childCount)).GetComponent<Checkpoint> ();
            all[i].dirToNext = -(all[i].realPos - frontCheckpoint.realPos).normalized;
            all[i].myCheckpointRegion = lastRegion;
            all[i].allCheckpoints = this;
        }
    }

}