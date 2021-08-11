using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinishMenu : MonoBehaviour {
    public enum State {
        StartText,
        NewRecordText,
        Menu,
        End
    }
    public State curState = State.StartText;
    DemoGameManager manager;
    [Header ("Start Text")]
    public GameObject startText;
    public Timer timer;
    [Header ("NewRecordTxt")]
    public Text recordTxt;
    public Animator recordAnim;
    public AudioClip recordClip;
    [Header ("Menu")]
    public Controller con;
    public GameObject MenuObject;
    public Transform cursorObj;
    public GameObject newRecordMenuIndicator;
    public UICharSelectNeighbour curSelected;
    public Text myTime;
    public Text bestTime;
    public GoToColor fade;

    void Start () {
        manager = FindObjectOfType<DemoGameManager> ();
        cursorObj.gameObject.SetActive (false);
    }
    public void CheckState () {
        switch (curState) {
            case State.StartText:
                DoStartText ();
                break;
            case State.NewRecordText:
                DoNewRecordText ();
                break;
            case State.Menu:
                DoMenu ();
                break;
        }
    }

    void DoStartText () {
        startText.SetActive (true);
        timer.enabled = false;
        //check if new record
        SaveData data = SaveSystem.LoadData ();
        if (data.bestTime[manager.courseID] == 0 || data.bestTime[manager.courseID] > timer.curTim) {
            // if (true) {
            //new record
            curState = State.NewRecordText;
            //save the record
            data.bestTime[manager.courseID] = timer.curTim;
            SaveSystem.Save (data);
            Invoke ("CheckState", 3.5f);
        } else {
            //no record
            curState = State.Menu;
            Invoke ("CheckState", 2);
        }

    }

    void DoNewRecordText () {
        recordTxt.text = "NEW RECORD!";
        recordAnim.Play ("FinishAnim", 0, 0);
        AudioSpawn.SpawnSound (recordClip, 1, 1, 0, 0);
        curState = State.Menu;
        Invoke ("CheckState", 2);
        newRecordMenuIndicator.SetActive (true);
    }
    void DoMenu () {
        myTime.text = timer.ConvertToTime (timer.curTim);
        bestTime.text = timer.ConvertToTime (SaveSystem.LoadData ().bestTime[manager.courseID]);
        MenuObject.SetActive (true);
        Invoke ("NoControl", 2);
    }

    void NoControl () {
        cursorObj.gameObject.SetActive (true);
    }

    void Update () {
        if (curState == State.Menu && IsInvoking ("CheckState") == false && IsInvoking ("NoControl") == false) {
            SelectOption ();
        }
    }

    void SelectOption () {
        // vertical movement
        if (con.vertInput == 0) {
            CancelInvoke ("NoVert");
        }
        if (IsInvoking ("NoVert") == false) {

            if (con.vertInput > 0 && curSelected.up != null) {
                curSelected = curSelected.up.GetComponent<UICharSelectNeighbour> ();
                Invoke ("NoVert", 0.2f);
            }
            if (con.vertInput < 0 && curSelected.down != null) {
                curSelected = curSelected.down.GetComponent<UICharSelectNeighbour> ();
                Invoke ("NoVert", 0.2f);
            }
            cursorObj.position = curSelected.transform.position;

            //confirming

            if (con.accelerate == true) {
                Confirm ();
            }
        }

    }

    void Confirm () {
        curState = State.End;
        cursorObj.gameObject.SetActive (false);
        switch (curSelected.name) {
            case "Retry":
                fade.SetToNewColor (Color.black, fade.lerpSpeed);
                Invoke ("Retry", 2);
                break;
            case "Menu":
                fade.SetToNewColor (Color.white, fade.lerpSpeed);
                Invoke ("Menu", 2);
                break;
            case "Quit":
                fade.SetToNewColor (Color.black, fade.lerpSpeed);
                Invoke ("Quit", 2);
                break;
        }
    }

    void NoVert () {

    }

    void Retry () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    void Menu () {
        SceneManager.LoadScene ("MenuScene");
    }

    void Quit () {
        Application.Quit ();
    }
}