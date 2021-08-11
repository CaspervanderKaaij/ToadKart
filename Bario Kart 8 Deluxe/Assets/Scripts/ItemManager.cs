using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    Kart myKart;
    public enum Item {
        Mushroom = 0,
        DoubleMushroom = 1,
        TripleMushroom = 2,
        Banana = 3,
        DoubleBanana = 4,
        TripleBanana = 5,
        GreenShell = 6,
        DoubleGreenShell = 7,
        TripleGreenShell = 8,
        RedShell = 9,
        DoubleRedShell = 10,
        TripleRedShell = 11,
        BulletBill = 12,
        SpinyShell = 13,
        BobOmb = 14,
        GoldenMushroom = 15,
        Star = 16,
        Empty = -1

    }
    public Item curItem;
    public Item curVisualItem;
    public enum State {
        NoItem,
        RNG,
        HasItem
    }
    public State curState = State.NoItem;
    [HideInInspector] public State lastState = State.NoItem;
    [Header ("Misc")]
    public AutoScale headScale;

    void Start () {
        myKart = GetComponent<Kart> ();
    }

    void Update () {
        SetVisualItem ();
        lastState = curState;
    }

    void SetVisualItem () {
        switch (curState) {
            case State.NoItem:
                curVisualItem = Item.Empty;
                break;
            case State.RNG:
                curVisualItem = (Item) Random.Range (0, 16);
                break;
        }
    }

    public void GetItem () {
        print ("get item");
    }

    public void UseItem () {
        switch (curItem) {
            case Item.Empty:
                AudioSpawn.SpawnSound (myKart.kartStats.sfx_Horn, 1, 1, 0, 0);
                Instantiate (myKart.hornParticles, transform.position, Quaternion.identity, transform);
                headScale.transform.localScale = headScale.goalScale * 1.1f;
                break;
            case Item.TripleMushroom:
                curItem = Item.DoubleMushroom;
                curVisualItem = Item.DoubleMushroom;
                myKart.Boost (0.6f, 2.76f);
                myKart.animator.Play ("UseItem_Forward");
                break;
            case Item.DoubleMushroom:
                curItem = Item.Mushroom;
                curVisualItem = Item.Mushroom;
                myKart.Boost (0.6f, 2.76f);
                myKart.animator.Play ("UseItem_Forward");
                break;
            case Item.Mushroom:
                curItem = Item.Empty;
                curVisualItem = Item.Empty;
                myKart.Boost (0.6f, 2.76f);
                myKart.animator.Play ("UseItem_Forward");
                break;
        }
    }
}