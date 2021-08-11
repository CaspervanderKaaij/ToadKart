using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLookAtIK : MonoBehaviour {
    public Transform goal;
    public Vector3 rotAdd;
    public float followSpeed = 1;
    public Kart myKart;
    [Header ("CheckForLookAtPoints")]
    public float lookRadius = 1;
    void Start () {
        lastRot = transform.rotation;
    }

    void LateUpdate () {
        CheckForLookAtPoints ();
        SmoothRot ();
    }

    Quaternion lastRot;
    float noGoalSmooth = 0;
    void SmoothRot () {
        if (myKart.curState == Kart.State.Normal || myKart.curState == Kart.State.Drift || myKart.animator.tag == "IgnoreHeadIK") { } else {
            goal = null;
        }
        if (goal == null) {
            if(noGoalSmooth < 1)
            noGoalSmooth += Time.deltaTime / 2;
            transform.rotation = Quaternion.Lerp (lastRot, transform.rotation, noGoalSmooth);
            lastRot = transform.rotation;
            return;
            // I did the smoothing this way because I want it to smooth out, but not fight with the head's animations
        }
        noGoalSmooth = 0;

        Quaternion rotGoal = Quaternion.LookRotation (goal.position - transform.position, transform.root.TransformDirection (Vector3.up)) * Quaternion.Euler (rotAdd);
        //limit the Y angle, so he doesn't look backwards, as if he's breaking his neck lmao
        if (Vector3.Dot (transform.root.forward, goal.position - transform.position) < 0) {
            if (Vector3.Dot (transform.root.right, goal.position - transform.position) < 0) {
                //L
                rotGoal = Quaternion.Euler (rotGoal.eulerAngles.x, transform.root.eulerAngles.y + 180, rotGoal.eulerAngles.z);
            } else {
                //R
                rotGoal = Quaternion.Euler (rotGoal.eulerAngles.x, transform.root.eulerAngles.y, rotGoal.eulerAngles.z);
            }
        }
        //limit his up or down looking
        float upDown = Vector3.Dot (transform.root.up, goal.position - transform.position);
        if (upDown > 0.5f) {
            rotGoal = Quaternion.Euler (rotGoal.eulerAngles.x, rotGoal.eulerAngles.y, transform.root.eulerAngles.z - 65);
        }
        if (upDown < -0.3f) {
            rotGoal = Quaternion.Euler (rotGoal.eulerAngles.x, rotGoal.eulerAngles.y, transform.root.eulerAngles.z - 115);
        }
        transform.rotation = lastRot;
        transform.rotation = Quaternion.Lerp (transform.rotation, rotGoal, Time.deltaTime * followSpeed);
        lastRot = transform.rotation;
    }

    void CheckForLookAtPoints () {
        goal = null;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll (transform.position, lookRadius, -Vector3.up, 0, LayerMask.GetMask ("LookAtTargets"), QueryTriggerInteraction.Collide);
        if (hits.Length > 0) {
            Transform closest = hits[0].transform;
            for (int i = 0; i < hits.Length; i++) {
                if (Vector3.Distance (transform.position, hits[i].transform.position) < Vector3.Distance (transform.position, closest.position)) {
                    if (hits[i].transform.root != transform.root)
                        closest = hits[i].transform;
                } else if (closest.root == transform.root) {
                    closest = hits[i].transform;
                }
            }
            if (closest.root != transform.root)
                goal = closest;
        }
    }

    void OnDrawGizmosSelected () {
        Gizmos.DrawWireSphere (transform.position, lookRadius);
    }
}