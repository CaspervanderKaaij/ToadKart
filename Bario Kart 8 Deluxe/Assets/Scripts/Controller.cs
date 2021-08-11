using System; //so I can convert a bool to an int
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public enum Type {
        Keyboard,
        PS4Controller,
        DirectInputController,
        CPU
    }
    public Type myType;
    public float horInput;
    public float vertInput;
    public bool accelerate;
    public bool backAccelerate;
    [HideInInspector] public bool justDrift; // these are to check if you just clicked/released the button
    public bool drift;
    [HideInInspector] public bool justItem;
    public bool useItem;
    public bool backCam = false;
    [HideInInspector] public bool justBackCam = false;
    public bool inControl = true;

    void Update () {
        if (inControl == true) {
            switch (myType) {
                case Type.Keyboard:
                    KeyboardUpdate ();
                    break;
            }
        } else {
            NullControl ();
        }
    }

    void KeyboardUpdate () {
        horInput = Convert.ToInt32 (Input.GetKey (KeyCode.D)) + -Convert.ToInt32 (Input.GetKey (KeyCode.A));
        vertInput = Convert.ToInt32 (Input.GetKey (KeyCode.W)) + -Convert.ToInt32 (Input.GetKey (KeyCode.S));
        accelerate = Input.GetKey (KeyCode.Space);
        backAccelerate = Input.GetKey (KeyCode.LeftControl);
        justDrift = drift;
        drift = Input.GetKey (KeyCode.LeftShift);
        justItem = useItem;
        useItem = Input.GetKey (KeyCode.E);
        justBackCam = backCam;
        backCam = Input.GetKey (KeyCode.Q);
    }

    void NullControl () {
        horInput = 0;
        vertInput = 0;
        accelerate = false;
        backAccelerate = false;
        justDrift = drift;
        drift = false;
        justItem = useItem;
        useItem = false;
        backCam = false;
        justBackCam = false;
    }
}