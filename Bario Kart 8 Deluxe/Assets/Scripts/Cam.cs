using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    Vector3 offset;
    Transform goal;
    public float rotFollowSpeed = 10;
    public float followSpeed;
    Kart kart;
    float startXRot;
    DemoGameManager manager;
    public Vector3 finishOffset;
    void Start () {
        manager = FindObjectOfType<DemoGameManager> ();
        goal = transform.parent;
        kart = goal.GetComponent<Kart> ();

        if (kart.isPlayer == false) {
            gameObject.SetActive (false);
            return;
        }

        offset = transform.localPosition;
        startXRot = transform.localEulerAngles.x;
        transform.SetParent (null);
        followPos = goal.position;

        kart.myCam = this;

    }

    void ResetFollowSpeed () {
        followSpeed /= 10;
    }

    Vector3 followPos = Vector3.zero;
    Vector3 vel = Vector3.zero;
    void Update () {
        if (kart.curState != Kart.State.Destroy && manager.curState != DemoGameManager.State.Finish) {
            transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (goal.eulerAngles.x + startXRot, goal.eulerAngles.y, goal.eulerAngles.z + ((Mathf.DeltaAngle (0, kart.kartPivot.localEulerAngles.z) / 2))), Time.deltaTime * rotFollowSpeed);
            Vector3 v = transform.TransformDirection (offset.x, 0, offset.z);
            v.y = 0;
            followPos = Vector3.SmoothDamp (followPos, goal.position, ref vel, followSpeed);
            transform.position = followPos + (Vector3.up * offset.y) + v;
            RayCheck ();
            CountdownCam ();
        } else if (manager.curState == DemoGameManager.State.Finish) {
            if (kart.curState != Kart.State.Destroy) {
                transform.position = goal.position + goal.TransformDirection (finishOffset);
                transform.LookAt (goal.position);
            }
        }

    }

    bool didCountdownCam = false;
    void CountdownCam () {
        if (kart.curState == Kart.State.Countdown && didCountdownCam == false) {
            followPos = goal.position + goal.up + (goal.forward * 2);
            followSpeed *= 10;
            Invoke ("ResetFollowSpeed", 2);
            didCountdownCam = true;
        }
    }

    void RayCheck () {
        RaycastHit hit;
        if (Physics.Raycast (transform.position + (transform.up * 2), -transform.up, out hit, 2.5f, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
            transform.position = hit.point + (transform.up * 0.5f);
        }
    }

    public void ResetCam () {
        transform.rotation = Quaternion.Euler (goal.eulerAngles.x + startXRot, goal.eulerAngles.y, goal.eulerAngles.z + ((Mathf.DeltaAngle (0, kart.kartPivot.localEulerAngles.z) / 2)));
        Vector3 v = transform.TransformDirection (offset.x, 0, offset.z);
        v.y = 0;
        followPos = goal.position;
        transform.position = followPos + (Vector3.up * offset.y) + v;
        vel = Vector3.zero;
    }
}