using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCam : MonoBehaviour {
    public float moveSpeed;
    public float rotSpeed;
    public Canvas[] canvasses;
    Camera camera;
    public Controller controller;
    public Transform parentTrans;
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = GetComponent<Camera> ();
    }

    void Update () {
        ToggleMe ();
        if (camera.enabled == true) {
            Movement ();
            ToggleUI ();
            SetTimeScale ();
            ToggleController ();
            ToggleParented ();
        }
    }

    bool movementActive = true;
    void Movement () {
        if (Input.GetKeyDown (KeyCode.BackQuote) == true)
            movementActive = !movementActive;
        if (movementActive == true) {

            transform.position += transform.TransformDirection (new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("UpDown"), Input.GetAxisRaw ("Vertical")) * moveSpeed * Time.unscaledDeltaTime);
            transform.Rotate (new Vector3 (-Input.GetAxisRaw ("Mouse Y"), Input.GetAxisRaw ("Mouse X"), 0) * Time.unscaledDeltaTime * rotSpeed);
            transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }

    void ToggleUI () {
        if (Input.GetKeyDown (KeyCode.C) == true) {
            for (int i = 0; i < canvasses.Length; i++) {
                canvasses[i].enabled = !canvasses[i].enabled;
            }
        }
    }

    void ToggleMe () {
        if (Input.GetKeyDown (KeyCode.X) == true) {
            camera.enabled = !camera.enabled;
        }
    }

    void SetTimeScale () {
        if (Input.GetKeyDown (KeyCode.Alpha0) == true) {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown (KeyCode.Alpha1) == true) {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown (KeyCode.Alpha2) == true) {
            Time.timeScale = 0.5f;
        }
    }

    void ToggleController () {
        if (Input.GetKeyDown (KeyCode.Z) == true)
            controller.enabled = !controller.enabled;
    }

    void ToggleParented () {
        if (Input.GetKeyDown (KeyCode.O) == true) {
            if (transform.parent == null) {
                transform.SetParent (parentTrans, true);
            } else {
                transform.SetParent (null);
            }
        }
    }
}