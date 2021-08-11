using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISelectChar : MonoBehaviour {
    public float startWait = 1;
    bool started = false;
    bool startedB = false;
    public GameObject startChar;
    public Controller controller;
    GameObject selectedChar;
    UICharSelectNeighbour neigh;
    public GameObject charRender;
    bool hasSelected = false;
    public AutoRotate charRotate;
    public AudioClip selectedClip;
    public GameObject nextObj;
    public float nextObjWaitTime = 2;
    public GameObject finalBlack;
    public float waitForLoad = 1;
    public AudioSource song;

    void Start () {
        started = false;
        Invoke ("DoStart", startWait);
        transform.position += Vector3.right * 10000;
        charRender.SetActive (false);
    }

    void DoStart () {
        started = true;
        UpdateChar (startChar);
    }

    void Update () {
        if (started == false)
            return;
        if (hasSelected == false) {
            CheckInput ();
            CheckSelectedInput ();
        } else {
            charRotate.transform.rotation = Quaternion.Lerp (charRotate.transform.rotation, Quaternion.Euler (0, 205, 0), Time.deltaTime * 10);

            if (startedB == true && controller.accelerate == true && finalBlack.activeSelf == false) {
                finalBlack.SetActive (true);
                Invoke("LoadLevel",waitForLoad);
            }
            if(finalBlack.activeSelf == true){
                song.volume = Mathf.MoveTowards(song.volume,0,Time.deltaTime);
            }
        }
    }

    void LoadLevel(){
        SceneManager.LoadScene(1);
    }

    void CheckInput () {

        if (controller.vertInput > 0 && neigh.up != null && IsInvoking ("NoVertInput") == false) {
            UpdateChar (neigh.up);
            Invoke ("NoVertInput", 0.4f);
        }

        if (controller.vertInput < 0 && neigh.down != null && IsInvoking ("NoVertInput") == false) {
            UpdateChar (neigh.down);
            Invoke ("NoVertInput", 0.4f);
        }

        if (controller.horInput > 0 && neigh.right != null && IsInvoking ("NoHorInput") == false) {
            UpdateChar (neigh.right);
            Invoke ("NoHorInput", 0.4f);
        }

        if (controller.horInput < 0 && neigh.left != null && IsInvoking ("NoHorInput") == false) {
            UpdateChar (neigh.left);
            Invoke ("NoHorInput", 0.4f);
        }

        if (controller.vertInput == 0) {
            CancelInvoke ("NoVertInput");
        }
        if (controller.horInput == 0) {
            CancelInvoke ("NoHorInput");
        }
    }

    void CheckSelectedInput () {
        if (controller.accelerate == true) {
            hasSelected = true;
            transform.position += Vector3.right * 10000;
            AudioSpawn.SpawnSound (selectedClip, 1, 1, 0, 0);
            charRotate.enabled = false;
            Invoke ("DoNext", nextObjWaitTime);
        }
    }

    void DoNext () {
        nextObj.SetActive (true);
        Invoke ("DoNextB", 0.4f);
    }

    void DoNextB () {
        startedB = true;
    }

    void NoHorInput () {
        //invoke function
    }
    void NoVertInput () {
        //invoke function
    }

    void UpdateChar (GameObject newChar) {
        selectedChar = newChar;
        transform.position = selectedChar.transform.position;
        neigh = newChar.GetComponent<UICharSelectNeighbour> ();
        charRender.SetActive (false);
        CancelInvoke ("SetCharVisible");
        Invoke ("SetCharVisible", Random.Range (0.3f, 1.3f)); //this is an intentional feature. It's a joke, that the character has to load, despite Toad being the only character
        charRotate.transform.Rotate (0, Random.Range (0, 360f), 0);
    }

    void SetCharVisible () {
        charRender.SetActive (true);
    }
}