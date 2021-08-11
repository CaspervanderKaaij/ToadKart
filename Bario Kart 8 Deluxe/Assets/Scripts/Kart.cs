using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
[RequireComponent (typeof (CharacterController))]

public class Kart : MonoBehaviour {
    CharacterController cc;
    [Header ("viewable stats")]
    public Vector3 moveV3 = Vector3.zero;
    public Vector3 bonkV3 = Vector3.zero;
    public enum State {
        Normal,
        Boost,
        Drift,
        Countdown,
        Hit,
        Destroy
    }
    public State curState = State.Normal;
    public bool isOffRoad = false;
    public bool isGrounded = true;
    public bool isPlayer = false;
    public bool inControl = true;
    public int place = 1;
    bool backCamming = false;
    [Header ("to set")]
    public float cc_ = 100;
    public KartStats kartStats;
    public VoiceBank voiceBank;
    public Controller con;
    public float bonkResetSpeed = 10;
    public float hopStr = 1;
    public float gravityStr;
    public float gravitySpeed;
    public Transform kartPivot;
    public Transform kartFrontPivot;
    public GameObject boostEff;
    public PostProcessVolume boostVolume;
    public GameObject[] backCamObjects;
    public ItemManager myItemManager;
    [HideInInspector] public Cam myCam;
    public Animator animator;
    public GoToColor fade;
    [Header ("Particles")]
    public ParticleSystem fireParticles;
    public GameObject drfitLParticles;
    public GameObject driftRParticles;
    public GameObject redDriftLParticles;
    public GameObject redDriftRParticles;
    public GameObject blueDriftLParticles;
    public GameObject blueDriftRParticles;
    public GameObject getHitParticles;
    public GameObject hornParticles;
    [Header ("Sound")]
    public AudioClip sfx_Hop;
    public AudioClip sfx_BonkPlayer;
    public AudioClip sfx_BonkWall;
    public AudioSource sfx_Motor;
    public AudioSource sfx_Drifting;
    public AudioClip sfx_mirrorOn;
    public AudioClip sfx_mirrorOff;
    public AudioClip sfx_boost;
    public AudioClip sfx_boostMini;
    public AudioClip sfx_GetHit;
    public AudioClip sfx_Land;
    void Start () {
        cc = GetComponent<CharacterController> ();
        lastY = transform.position.y;
        lastLastY = lastY;
        ForceNoBackCam ();
    }

    void Update () {
        CheckLastState ();
        StateHandeler ();
    }

    void StateHandeler () {
        con.inControl = inControl;
        switch (curState) {
            case State.Normal:
                FinalMove ();
                isGrounded = IsGrounded ();
                if (curState == State.Destroy)
                    return;
                Gravity ();
                Turn ();
                MoveForward ();
                Hop ();
                CheckFrontBonk ();
                ResetBonk ();
                CheckBackCam ();
                SetMotorSound ();

                ItemInput ();
                break;
            case State.Boost:
                FinalMove ();
                isGrounded = IsGrounded ();
                if (curState == State.Destroy)
                    return;
                Gravity ();
                Turn ();
                BoostForward ();
                CheckFrontBonk ();
                ItemInput ();
                CheckBackCam ();
                SetMotorSound ();
                break;
            case State.Drift:
                FinalMove ();
                isGrounded = IsGrounded ();
                if (curState == State.Destroy)
                    return;
                Gravity ();
                DriftTurn ();
                DriftForward ();
                CheckFrontBonk ();
                SetDiftFireParticles ();
                ResetBonk ();
                CheckEndDrift ();
                CheckBackCam ();
                SetMotorSound ();

                ItemInput ();

                break;
            case State.Countdown:
                FinalMove ();
                isGrounded = IsGrounded ();
                if (curState == State.Destroy)
                    return;
                Gravity ();
                Countdown ();
                SetMotorSound ();
                break;
            case State.Hit:
                FinalMove ();
                isGrounded = IsGrounded ();
                if (curState == State.Destroy)
                    return;
                CheckBackCam ();
                Gravity ();
                HitState ();
                SetMotorSound ();
                break;
        }
    }
    State lastState = State.Normal;
    void CheckLastState () {
        if (lastState != curState) {
            //State we go out of
            switch (lastState) {
                case State.Drift:
                    driftRParticles.SetActive (false);
                    drfitLParticles.SetActive (false);
                    redDriftLParticles.SetActive (false);
                    redDriftRParticles.SetActive (false);
                    blueDriftLParticles.SetActive (false);
                    blueDriftRParticles.SetActive (false);
                    sfx_Drifting.Stop ();
                    break;
                case State.Boost:
                    fireParticles.Stop ();
                    break;
                case State.Countdown:
                    speedPerc = 0;
                    if (countDownAmount > 1.95f) {
                        GetHit ();
                        if (isPlayer == true) {
                            AudioSpawn.SpawnSound (voiceBank.getHit[Random.Range (0, voiceBank.getHit.Length)], 1, 1, 0, 0);
                        }
                    } else if (countDownAmount > 1.65f) {
                        Boost (0.4f, 2.75f);
                        if (isPlayer == true) {
                            AudioSpawn.SpawnSound (voiceBank.smallBoost[Random.Range (0, voiceBank.smallBoost.Length)], 1, 1, 0, 0);
                        }
                    } else if (countDownAmount > 1f) {
                        Boost (0.2f, 2f);
                        if (isPlayer == true) {
                            AudioSpawn.SpawnSound (voiceBank.boost[Random.Range (0, voiceBank.boost.Length)], 1, 1, 0, 0);
                        }
                    }
                    break;
            }
            //The new state
            switch (curState) {
                case State.Drift:
                    if (turningRight == true)
                        driftRParticles.SetActive (true);
                    else
                        drfitLParticles.SetActive (true);
                    sfx_Drifting.Play ();
                    break;
                case State.Boost:
                    if (isPlayer == true && fireParticles.isPlaying == false) {

                        if (curBoostStr > 0.2f) {
                            AudioSpawn.SpawnSound (sfx_boost, 0.6f, 1, 0, 0);
                        } else {
                            AudioSpawn.SpawnSound (sfx_boostMini, 0.1f, 1, 0, 0);
                        }
                    }
                    fireParticles.Play ();
                    break;
            }
            driftTime = 0;
            lastState = curState;
        }
    }

    void SetMotorSound () {
        if (isPlayer == false)
            return;
        AudioClip finalClip = null;
        switch (curState) {
            case State.Normal:
                if (speedPerc > 0) {
                    finalClip = kartStats.sfx_EngineLoop;
                } else {
                    finalClip = kartStats.sfx_Idle;
                }
                break;
            case State.Boost:
                if (curBoostStr > 0.2f) {
                    finalClip = kartStats.sfx_Dash;
                } else {
                    finalClip = kartStats.sfx_DashMini;
                }
                break;
            case State.Drift:
                finalClip = kartStats.sfx_EngineLoop;
                break;
            case State.Hit:
                finalClip = kartStats.sfx_Idle;
                break;
            case State.Countdown:
                if (countDownAmount > 0) {
                    finalClip = kartStats.sfx_EngineLoop;
                } else {
                    finalClip = kartStats.sfx_Idle;
                }
                break;

        }
        if (sfx_Motor.clip != finalClip) {
            sfx_Motor.clip = finalClip;
            sfx_Motor.Play ();
        }
    }

    void ItemInput () {
        if (con.useItem == true && con.justItem == false && IsInvoking ("NoItemInp") == false) {
            myItemManager.UseItem ();
            Invoke ("NoItemInp", 0.4f);
        }
    }

    void NoItemInp () {

    }

    public void GetHit () {
        if (curState != State.Hit && IsInvoking ("Invincible") == false) {
            curState = State.Hit;
            Invoke ("StopHitState", 1);
            Invoke ("Invincible", 2);
            bonkV3 = Vector3.zero;
            moveV3 = Vector3.zero;
            curSpeed = 0;
            speedPerc = 0;
            turnAcc = 0;

            Instantiate (getHitParticles, transform.position, Quaternion.identity);
            if (isPlayer == true && Random.Range (0, 3) > 0.5f) {
                AudioSpawn.SpawnSound (voiceBank.getHit[Random.Range (0, voiceBank.getHit.Length)], 1, 1, 0, 0);
            }
            if (isPlayer == true) {
                AudioSpawn.SpawnSound (sfx_GetHit, 0.6f, 1, 0, 0);
            }

            animator.Play ("GetHit");
            animator.SetFloat ("hor", 0);

        }
    }

    void CheckBackCam () {
        if (con.backCam == true && con.justBackCam == false) {
            backCamming = !backCamming;
            for (int i = 0; i < backCamObjects.Length; i++) {
                backCamObjects[i].SetActive (backCamming);
            }
            if (backCamming == true) {
                AudioSpawn.SpawnSound (sfx_mirrorOn, 0.5f, 1, 0, 0);
            } else {
                AudioSpawn.SpawnSound (sfx_mirrorOff, 0.5f, 1, 0, 0);
            }
        }
    }

    public void ForceNoBackCam () {

        backCamming = false;
        for (int i = 0; i < backCamObjects.Length; i++) {
            backCamObjects[i].SetActive (backCamming);
        }

    }

    void Invincible () {

    }
    void HitState () {

    }

    void StopHitState () {
        curState = State.Normal;
        animator.Play ("GetHitEnd", 0, animator.GetCurrentAnimatorStateInfo (0).normalizedTime);
    }

    float countDownAmount = 0;
    void Countdown () {
        if (con.accelerate == true) {
            countDownAmount += Time.deltaTime;
            speedPerc = 1; // this makes the wheels move
        } else {
            countDownAmount = 0;
            speedPerc = 0;
        }
        animator.SetFloat ("hor", Mathf.Lerp (animator.GetFloat ("hor"), con.horInput, Time.deltaTime * 10));
    }

    void DriftForward () {
        Vector3 goal = transform.forward * (kartStats.speed / 3.6f * 0.9f) * (cc_ / 100);
        moveV3.x = Mathf.MoveTowards (moveV3.x, goal.x, Time.deltaTime * kartStats.traction);
        moveV3.z = Mathf.MoveTowards (moveV3.z, goal.z, Time.deltaTime * kartStats.traction);
    }
    bool turningRight = true;
    void DriftTurn () {
        float finalTurn = kartStats.drift;
        float inp = con.horInput;
        if (turningRight == false) {
            finalTurn = -finalTurn;
            if (inp < 0)
                driftTime += Time.deltaTime;
            inp = Mathf.Min (inp, 0.2f);
            kartPivot.localRotation = Quaternion.Lerp (kartPivot.localRotation, Quaternion.Euler (kartPivot.localEulerAngles.x, -30, kartPivot.localEulerAngles.z), Time.deltaTime * 10);
        } else {
            if (inp > 0)
                driftTime += Time.deltaTime;
            inp = Mathf.Max (inp, -0.2f);
            kartPivot.localRotation = Quaternion.Lerp (kartPivot.localRotation, Quaternion.Euler (kartPivot.localEulerAngles.x, 30, kartPivot.localEulerAngles.z), Time.deltaTime * 10);
        }
        finalTurn += inp * kartStats.driftRange;

        transform.Rotate (0, finalTurn * Time.deltaTime, 0);

        animator.SetFloat ("hor", Mathf.Lerp (animator.GetFloat ("hor"), con.horInput, Time.deltaTime * 10));
    }

    float redStartTime = 1f;
    float blueStartTime = 2.5f;
    void SetDiftFireParticles () {

        redDriftLParticles.SetActive (((driftTime < blueStartTime && driftTime > redStartTime) && turningRight == false));
        blueDriftLParticles.SetActive (((driftTime > blueStartTime) && turningRight == false));

        redDriftRParticles.SetActive (((driftTime < blueStartTime && driftTime > redStartTime && turningRight == true)));
        blueDriftRParticles.SetActive (((driftTime > blueStartTime) && turningRight == true));

    }

    float driftTime = 0;
    void CheckEndDrift () {
        if (con.drift == false) {
            curState = State.Normal;

            //check boost
            if (driftTime >= blueStartTime) {
                Boost (0.3f, 2.75f);
                if (isPlayer == true && Random.Range (0, 3) > 1) {
                    AudioSpawn.SpawnSound (voiceBank.boost[Random.Range (0, voiceBank.boost.Length)], 1, 1, 0, 0);
                }
            } else if (driftTime >= redStartTime) {
                Boost (0.1f, 2f);
                if (isPlayer == true && Random.Range (0, 3) > 1) {
                    AudioSpawn.SpawnSound (voiceBank.smallBoost[Random.Range (0, voiceBank.smallBoost.Length)], 1, 1, 0, 0);
                }
            }

            return;
        }
        if (isOffRoad == true || con.accelerate == false || isGrounded == false) {
            curState = State.Normal;
            return;

        }

        driftTime += Time.deltaTime / 2;
    }

    public void Bonk (Vector3 bonkDir) {
        bonkV3 = bonkDir * kartStats.weight;
        bonkV3.y = 0;
        if (isPlayer == true)
            AudioSpawn.SpawnSound (sfx_BonkPlayer, 1, 1, 0, 0);
        if (curState == State.Drift) {
            curState = State.Normal;
        }
    }

    void ResetBonk () {
        bonkV3 = Vector3.MoveTowards (bonkV3, Vector3.zero, Time.deltaTime * bonkResetSpeed);
    }

    void CheckFrontBonk () {
        RaycastHit[] hit;
        hit = Physics.SphereCastAll (transform.position, cc.radius * 1.1f, transform.forward, cc.radius * 1.1f, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hit.Length; i++) {

            float ang = Mathf.Acos (Vector3.Dot (transform.up, hit[i].normal)) * Mathf.Rad2Deg;
            if (hit[i].point.y < kartPivot.position.y)
                ang = -ang;
            if (ang > cc.slopeLimit || hit[i].transform.tag == "Wall") {
                //doing a ray version because spherecast doesn't do a perfect hit.normal
                RaycastHit hitB;
                if (Physics.Raycast (transform.position, hit[i].point - transform.position, out hitB, Vector3.Distance (transform.position, hit[i].point) + 0.1f, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
                    ang = Mathf.Acos (Vector3.Dot (transform.up, hitB.normal)) * Mathf.Rad2Deg;
                    if (hitB.point.y < kartPivot.position.y)
                        ang = -ang;
                    if (ang > cc.slopeLimit || hit[i].transform.tag == "Wall") {
                        if (Vector3.Dot (new Vector3 (moveV3.x, 0, moveV3.z), new Vector3 (hitB.normal.x, 0, hitB.normal.z)) < 0 && IsInvoking ("NoBonk") == false) {

                            Vector3 helper = hitB.normal * ((kartStats.speed / 3.6f));

                            moveV3.x = helper.x;
                            moveV3.z = helper.z;
                            Invoke ("NoBonk", 0.1f);
                            if (isPlayer == true)
                                AudioSpawn.SpawnSound (sfx_BonkWall, speedPerc / 3, 1, 0, 0);

                            if (curState == State.Drift) {
                                curState = State.Normal;
                            }
                        }
                    }

                }
                return;
            }

        }
    }

    void NoBonk () {

    }

    void Hop () {
        if (isGrounded == true && con.drift == true && con.justDrift == false) {
            moveV3.y = hopStr;
            if (isPlayer == true)
                AudioSpawn.SpawnSound (sfx_Hop, 0.5f, Random.Range (0.9f, 1.1f), 0, 0);
            return;
        } else if (isGrounded == true && wasGrounded == false && isOffRoad == false && Mathf.Abs (con.horInput) > 0.5f && speedPerc > 0.8f) {
            turnAcc = 0;
            curState = State.Drift;

        }

    }
    float curBoostStr = 0;
    public void Boost (float boostTime, float boostStr) {
        if (boostStr >= curBoostStr) {
            curBoostStr = boostStr;
            curState = State.Boost;
            CancelInvoke ("CancelBoost");
            Invoke ("CancelBoost", boostTime);
            boostVolume.weight = 1;
            CancelInvoke ("SetBoostEff");
            SetBoostEff ();
        }
    }

    void CancelBoost () {
        if (curState == State.Boost) {
            curState = State.Normal;
            curBoostStr = 0;
        }
    }

    void SetBoostEff () {
        boostVolume.weight = Mathf.MoveTowards (boostVolume.weight, 0, Time.deltaTime);
        if (isPlayer == false)
            boostVolume.weight = 0;

        if (boostVolume.weight > 0) {
            boostEff.SetActive (true);
            Invoke ("SetBoostEff", Time.deltaTime);
            // if (fireParticles.isPlaying == false)
            //     fireParticles.Play ();

        } else {
            boostEff.SetActive (false);
            //  fireParticles.Stop ();
        }
    }

    void BoostForward () {

        curSpeed = (kartStats.speed / 3.6f) * curBoostStr;
        speedPerc = 1;

        Vector3 goal = transform.forward * curSpeed * (cc_ / 100);
        moveV3.x = goal.x;
        moveV3.z = goal.z;
    }

    float turnAcc = 0;
    void Turn () {
        if (curSpeed > 0) {
            if (isOffRoad == false)
                turnAcc = Mathf.MoveTowards (turnAcc, con.horInput * kartStats.handlingRange, Time.deltaTime * kartStats.handling);
            else
                turnAcc = Mathf.MoveTowards (turnAcc, con.horInput * kartStats.handlingRange, Time.deltaTime * kartStats.offRoadHandling);
            if (isGrounded == false)
                turnAcc = Mathf.MoveTowards (turnAcc, con.horInput * kartStats.handlingRange / 5, Time.deltaTime * kartStats.handling * 5);
        } else {
            turnAcc = Mathf.MoveTowards (turnAcc, 0, Time.deltaTime * kartStats.handling);
        }
        turningRight = (turnAcc > 0);
        transform.Rotate (0, turnAcc * Time.deltaTime, 0);
        animator.SetFloat ("hor", Mathf.Lerp (animator.GetFloat ("hor"), con.horInput, Time.deltaTime * 10));
    }

    float lastY = 0;
    float lastLastY = 0;
    void Gravity () {
        float yMomentum = transform.position.y - lastY;
        float earlierYMomentum = lastY - lastLastY;

        float dist = Vector3.Distance (Vector3.right * yMomentum, Vector3.right * earlierYMomentum);

        if ((yMomentum < -0.01f && earlierYMomentum > 0f && isGrounded == true) || (wasGrounded == true && isGrounded == false && moveV3.y < 0)) {
            //add physics to go up when suddenly going downwards.. which I didn't end up doing
        } else {
            if (isGrounded == true) {
                moveV3.y = Mathf.Min (-1, -(Mathf.Abs (moveV3.x) + Mathf.Abs (moveV3.z))); // floorsticking
            } else {
                moveV3.y = Mathf.MoveTowards (moveV3.y, gravityStr, Time.deltaTime * gravitySpeed); //gravity
            }
        }
        lastLastY = lastY;
        lastY = transform.position.y;

        if (isGrounded == true) {
            lastGroundedPos = transform.position;
        }
    }
    bool wasGrounded = true;
    bool IsGrounded () {
        wasGrounded = isGrounded;
        if (moveV3.y > 0)
            return false;
        RaycastHit hit;
        if (Physics.SphereCast (transform.position + (Vector3.up * cc.height / 2), cc.radius, -transform.up, out hit, cc.height * 0.6f, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
            CheckOffroad (hit);
            CheckDestroyRoad (hit);
            SetKartPivot (hit);
            CheckBoostPannel (hit);
            if (wasGrounded == false && isPlayer == true) {
                // AudioSpawn.SpawnSound (sfx_Land, 0.5f, 1.2f, 0, 0); I couldn't find a good sound :( 
            }
            return true;
        }

        isOffRoad = false;
        SetKartPivot ();
        if (wasGrounded == true) {
            moveV3.y = 0;
        }

        return false;
    }

    void CheckOffroad (RaycastHit hit) {
        isOffRoad = (hit.transform.tag == "Offroad");
    }

    void CheckDestroyRoad (RaycastHit hit) {
        KillerRoad killRoad = hit.transform.GetComponent<KillerRoad> ();
        if (killRoad != null) {
            curState = State.Destroy;
            cc.enabled = false;
            Instantiate (killRoad.destroyParticle, transform.position, killRoad.destroyParticle.transform.rotation);
            if (isPlayer == true) {
                AudioSpawn.SpawnSound (killRoad.destroyClip, 1, 1, 0, 0);
                if (Random.Range (0, 3) > 1)
                    AudioSpawn.SpawnSound (voiceBank.fall, 1, 1, 0, 0);
            }
            transform.position -= Vector3.up * 100000;
            Invoke ("Respawn", 3);
            Invoke ("DestroyFade", 1.5f);
            ForceNoBackCam ();
            moveV3 = Vector3.zero;
            speedPerc = 0;
            turnAcc = 0;

        }
    }

    void DestroyFade () {
        fade.SetToNewColor (Color.black, fade.lerpSpeed);
    }

    Vector3 lastGroundedPos = Vector3.zero;
    void Respawn () {
        transform.position += Vector3.up * 100000;
        CarRespawn path = FindObjectOfType<CarRespawn> ();
        Vector3 angleHelp = transform.eulerAngles;
        transform.forward = GetComponent<CheckpointHolder> ().lastCheckpoint.dirToNext;
        transform.eulerAngles = new Vector3 (angleHelp.x, transform.eulerAngles.y, angleHelp.z);
        transform.position = GetComponent<CheckpointHolder> ().lastCheckpoint.realPos;
        transform.position += transform.up * 20;
        cc.enabled = true;
        curState = State.Normal;
        myCam.ResetCam ();
        curSpeed = 0;
        turnAcc = 0;
        speedPerc = 0;
        moveV3 = Vector3.zero;
        fade.SetToNewColor (Color.clear, fade.lerpSpeed);
    }

    void CheckBoostPannel (RaycastHit hit) {
        if (hit.transform.tag == "BoostPannel") {
            Boost (0.2f, 2f);
        }
    }

    void SetKartPivot (RaycastHit hit) {
        Vector3 lookAtPos = kartPivot.position + (transform.forward * 0.1f);
        if (Physics.Raycast (transform.position + (-transform.forward * -kartFrontPivot.localPosition.z), -transform.up, out hit, cc.height, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {

            Vector3 trueHitpoint = hit.point + (Vector3.up * -0.02f); //this 0.02 is a fix for a bug with the character controller collision code. It's slightly inaccurate

            lookAtPos = trueHitpoint;

        }

        Quaternion rotGoal = Quaternion.LookRotation (lookAtPos - kartPivot.position, transform.up);
        if (Physics.Raycast (transform.position + (-transform.right * -kartFrontPivot.localPosition.x), -transform.up, out hit, cc.height, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
            if (hit.point.y > kartFrontPivot.position.y) {
                float tan = Mathf.Rad2Deg * Mathf.Atan ((hit.point.y - kartPivot.position.y) / (Mathf.Abs (kartFrontPivot.localPosition.x)));
                rotGoal *= Quaternion.Euler (0, 0, tan);
                cc.Move (transform.TransformDirection (-(hit.point.y - kartPivot.position.y) * 2, 0, 0) * Time.deltaTime);
            }
        }
        if (Physics.Raycast (transform.position + (transform.right * -kartFrontPivot.localPosition.x), -transform.up, out hit, cc.height, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
            if (hit.point.y > kartFrontPivot.position.y) {
                float tan = Mathf.Rad2Deg * Mathf.Atan ((hit.point.y - kartPivot.position.y) / (Mathf.Abs (kartFrontPivot.localPosition.x)));
                rotGoal *= Quaternion.Euler (0, 0, -tan);
                cc.Move (transform.TransformDirection ((hit.point.y - kartPivot.position.y) * 2, 0, 0) * Time.deltaTime);
            }
        }
        kartPivot.rotation = Quaternion.Lerp (kartPivot.rotation, rotGoal, Time.deltaTime * 7.5f);
    }

    void SetKartPivot () {
        if (isGrounded == true)
            kartPivot.transform.localRotation = Quaternion.Lerp (kartPivot.localRotation, Quaternion.Euler (Vector3.zero), Time.deltaTime * 15);
        else
            kartPivot.transform.localRotation = Quaternion.Lerp (kartPivot.localRotation, Quaternion.Euler (new Vector3 (20, 0, 0)), Time.deltaTime * 1.5f);
    }

    float curSpeed = 0;
    void MoveForward () {
        if (isGrounded == true) {
            //acceleration
            float kSpeed = kartStats.speed;
            if (con.accelerate == true) {
                if (Mathf.Abs (turnAcc) > kartStats.handlingRange / 3) {
                    kSpeed *= kartStats.steeringSpeed;
                }
                if (isOffRoad == true)
                    kSpeed *= kartStats.offRoadSpeed;

                curSpeed = Mathf.MoveTowards (curSpeed, kSpeed / 3.6f, Time.deltaTime * GetAcceleration ()); //by deviding by 3.6, you convert from kmph to mps
            } else {
                curSpeed = Mathf.MoveTowards (curSpeed, 0, Time.deltaTime * GetAcceleration ());
            }
            curSpeed = Mathf.Min (curSpeed, kSpeed / 3.6f);
        }
        Vector3 goal = transform.forward * curSpeed * (cc_ / 100);
        float trac = kartStats.traction;
        if (isGrounded == false)
            trac = 0;
        moveV3.x = Mathf.MoveTowards (moveV3.x, goal.x, Time.deltaTime * trac);
        moveV3.z = Mathf.MoveTowards (moveV3.z, goal.z, Time.deltaTime * trac);

    }

    [HideInInspector] public float speedPerc = 0;
    float GetAcceleration () {
        if (con.accelerate == false) {
            speedPerc = Mathf.MoveTowards (speedPerc, 0, Time.deltaTime * 2);
            return kartStats.accelerationBase;
        }
        speedPerc = 0;
        if (isOffRoad == false)
            speedPerc = curSpeed / (kartStats.speed / 3.6f);
        else
            speedPerc = curSpeed / ((kartStats.speed * kartStats.offRoadSpeed) / 3.6f);
        if (speedPerc < 0.25f)
            return kartStats.accelerationBase * kartStats.acceleration1;
        else if (speedPerc < 0.5f)
            return kartStats.accelerationBase * kartStats.acceleration2;
        else if (speedPerc < 0.75f)
            return kartStats.accelerationBase * kartStats.acceleration3;
        else if (speedPerc < 0.95f)
            return kartStats.accelerationBase * kartStats.acceleration4;

        return 0;

    }

    void FinalMove () {
        cc.Move ((moveV3 + bonkV3) * Time.deltaTime);
    }
}