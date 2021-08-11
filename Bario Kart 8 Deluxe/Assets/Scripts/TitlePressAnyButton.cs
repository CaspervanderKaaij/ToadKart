using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePressAnyButton : MonoBehaviour {

    public AudioClip startSound;
    public float startWait = 2;
    bool started = false;
    public float waitForObjBTime = 0.5f;
    public GameObject nextObj;
    public GameObject nextObjB;
    void Start () {
        Invoke ("DoStart", startWait);
        transform.GetChild (0).gameObject.SetActive (false);
        started = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DoStart () {
        StartCoroutine (Flash ());
        started = true;
    }

    IEnumerator Flash () {
        transform.GetChild (0).gameObject.SetActive (true);
        yield return new WaitForSeconds (1.5f);
        transform.GetChild (0).gameObject.SetActive (false);
        yield return new WaitForSeconds (0.5f);
        StartCoroutine (Flash ());
    }

    void Update () {
        if (Input.anyKeyDown == true && started == true) {
            StopCoroutine (Flash ());
            gameObject.SetActive (false);
            AudioSpawn.SpawnSound (startSound, 1, 1.15f, 0, 0.6f);
            Invoke ("DoNext", waitForObjBTime);
            nextObj.SetActive (true);
        }
    }

    void DoNext () {
        nextObjB.SetActive (true);
    }
}