using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartWheel : MonoBehaviour {
    public Kart kart;
    public Vector3 turnDir;
    public float turnWithHor = 0;
    Vector3 baseAng = Vector3.zero;

    void Start () {
        baseAng = transform.localEulerAngles;
    }

    void Update () {
        //  transform.Rotate ((turnDir * kart.speedPerc) * Time.deltaTime);
        baseAng += (turnDir * kart.speedPerc) * Time.deltaTime;
        transform.localEulerAngles = new Vector3 (baseAng.x, baseAng.y + (kart.con.horInput * turnWithHor), baseAng.z);
    }
}