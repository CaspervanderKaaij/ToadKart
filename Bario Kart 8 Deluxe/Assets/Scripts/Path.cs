using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
    public List<Transform> path = new List<Transform> ();
    [HideInInspector] public List<bool> grounded = new List<bool> ();
    public Kart[] karts;
    void Start () {
        GetPath ();
        karts = FindObjectsOfType<Kart> ();
    }

    void GetPath () {
        path.Clear ();
        for (int i = 0; i < transform.childCount; i++) {
            path.Add (transform.GetChild (i));
            grounded.Add (false);

            RaycastHit hit;
            if (Physics.Raycast (path[i].position, Vector3.down, out hit, 2, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
                path[i].position = hit.point + (Vector3.up / 2);
                grounded[i] = true;
            }

            path[i].transform.LookAt (transform.GetChild ((int) Mathf.Repeat (i + 1, transform.childCount)).transform.position);
        }
    }

    public virtual Transform GetClosestPoint (Vector3 toCheck, bool groundedOnly) {
        Transform toReturn = path[0];
        for (int i = 1; i < path.Count; i++) {
            if (Vector3.Distance (toCheck, toReturn.position) > Vector3.Distance (toCheck, path[i].position)) {
                if (groundedOnly == false) {
                    toReturn = path[i];
                } else if (grounded[i] == true) {
                    toReturn = path[i];
                }
            }
        }
        return toReturn;
    }

}