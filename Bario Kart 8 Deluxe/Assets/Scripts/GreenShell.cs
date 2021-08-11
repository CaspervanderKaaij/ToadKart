using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShell : MonoBehaviour {
    CharacterController cc;
    Vector3 moveV3;
    public float speed = 1;
    public int hitsBeforeDestroy = 10;
    void Start () {
        cc = GetComponent<CharacterController> ();
    }

    void Update () {
        CheckHit ();
        CheckWall ();
        MoveForward ();
        Gravity ();
        FinalMove ();
    }

    void MoveForward () {
        Vector3 hlp = transform.forward * speed;
        moveV3.x = hlp.x;
        moveV3.z = hlp.z;
    }

    void Gravity () {
        moveV3.y = -Vector2.SqrMagnitude (new Vector2 (moveV3.x, moveV3.z));
    }

    void CheckWall () {

        RaycastHit[] hit;
        hit = Physics.SphereCastAll (transform.position, cc.radius * 1.1f * transform.localScale.x, transform.forward, cc.radius * 1.1f * transform.localScale.x, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hit.Length; i++) {

            float ang = Mathf.Acos (Vector3.Dot (transform.up, hit[i].normal)) * Mathf.Rad2Deg;
            if (ang > cc.slopeLimit || hit[i].transform.tag == "Wall") {
                //doing a ray version because spherecast doesn't do a perfect hit.normal
                RaycastHit hitB;
                if (Physics.Raycast (transform.position, hit[i].point - transform.position, out hitB, Vector3.Distance (transform.position, hit[i].point) + 0.1f, LayerMask.GetMask ("Default"), QueryTriggerInteraction.Ignore)) {
                    ang = Mathf.Acos (Vector3.Dot (transform.up, hitB.normal)) * Mathf.Rad2Deg;
                    if (ang > cc.slopeLimit || hit[i].transform.tag == "Wall") {
                        Vector3 hlp = new Vector3 (hitB.normal.x, 0, hitB.normal.z);
                        Vector3 reflectedAng = Vector3.Reflect (transform.forward, hlp);
                        transform.LookAt (transform.position + reflectedAng);

                        hitsBeforeDestroy--;
                        if (hitsBeforeDestroy <= 0) {
                            Destroy (gameObject);
                        }

                    }

                }
                return;
            }

        }

    }

    void CheckHit () {
        RaycastHit hit;
        if (Physics.SphereCast (transform.position, cc.radius * 1.1f * transform.localScale.x, transform.forward, out hit, cc.radius * 1.1f * transform.localScale.x, LayerMask.GetMask ("Player"), QueryTriggerInteraction.Collide)) {
            if (hit.transform.GetComponent<Kart> () != null) {
                hit.transform.GetComponent<Kart> ().GetHit ();
                Destroy (gameObject);
            }
        }
    }

    void FinalMove () {
        cc.Move (moveV3 * Time.deltaTime);
    }
}