using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public enum CheckpointType {
        KeyCheckpoint,
        Finish,
        Normal
    }
    public CheckpointType checkpointType = CheckpointType.Normal;
    Collider collider;
    [HideInInspector] public Vector3 realPos;
    [HideInInspector] public Vector3 dirToNext;
    public int myCheckpointRegion;
    [HideInInspector] public AllCheckpoints allCheckpoints;

    void OnTriggerEnter (Collider other) {
        CheckpointHolder holder = other.GetComponent<CheckpointHolder> ();
        if (holder != null) {
            //if you are either on the same checkpoint region, or the one in front of it, and if the previous checkpoint has been reached
            if (holder.lastKeyCheckpoint == allCheckpoints.allKeyCheckpoints[myCheckpointRegion] ||
                holder.lastKeyCheckpoint == allCheckpoints.allKeyCheckpoints[(int) Mathf.Repeat (myCheckpointRegion - 1, allCheckpoints.allKeyCheckpoints.Count)]) {
                if (myCheckpointRegion == 0 || holder.reachedCheckpoints[myCheckpointRegion - 1] == true) {

                    holder.lastCheckpoint = this;
                    if (checkpointType != CheckpointType.Normal) {
                        holder.lastKeyCheckpoint = this;
                        holder.reachedCheckpoints[myCheckpointRegion] = true;
                    }
                    if (checkpointType == CheckpointType.Finish) {
                        holder.TryNewLap ();
                    }
                }
            }
        }
    }

    public Vector3 GetPos () {
        collider = GetComponent<Collider> ();
        return collider.bounds.center;
    }
}