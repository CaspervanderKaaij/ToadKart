using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShell : MonoBehaviour {

    ItemPath path;
    public float rotSpeed = 1;
    CharacterController cc;
    Vector3 moveV3 = Vector3.zero;
    public float speed = 50;
    public float distanceForNext = 10;
    void Start () {
        cc = GetComponent<CharacterController> ();
        SetUpPath ();
        InvokeRepeating ("CheckNextPoint", 0.2f, 0.2f);
    }

    int curPoint = 0;
    void SetUpPath () {
        path = FindObjectOfType<ItemPath> ();
        curPoint = 0;
        GetClosestPoint ();
    }

    void GetClosestPoint () {
        for (int i = 1; i < path.path.Count; i++) {
            if (Vector3.Distance (transform.position, path.path[curPoint].position) > Vector3.Distance (transform.position, path.path[i].position)) {
                if (Physics.Raycast (transform.position, transform.position - path.path[i].position, Vector3.Distance (transform.position, path.path[i].position), LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore) == false) {
                    curPoint = i;
                }
            }
        }
        //  curPoint++;
        //   curPoint = (int) Mathf.Repeat (curPoint, path.path.Count);
    }

    void CheckNextPoint () {
        Vector3 pos = path.path[curPoint].position;
        pos.y = transform.position.y;
        if (Vector3.Distance (transform.position, pos) < distanceForNext) {
            curPoint++;
            curPoint = (int) Mathf.Repeat (curPoint, path.path.Count);
        }//
    }

    void Update () {
        LookAtPathPoint ();
        MoveForward ();
        Gravity ();
        FinalMove ();
    }

    void Gravity () {
        if (path.grounded[curPoint] == true) {
            moveV3.y = -speed;
        } else {
            moveV3.y = -(transform.position.y - path.path[curPoint].position.y);
        }
    }

    void LookAtPathPoint () {
        Vector3 pos = path.path[curPoint].position;
        pos.y = transform.position.y;
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (-(transform.position - pos), Vector3.up), rotSpeed * Time.deltaTime);
    }

    void MoveForward () {
        Vector3 hlp = transform.forward * speed;
        moveV3.x = hlp.x;
        moveV3.z = hlp.z;
    }

    void FinalMove () {
        cc.Move (moveV3 * Time.deltaTime);
    }
}