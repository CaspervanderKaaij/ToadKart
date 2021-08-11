using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour {
    public ItemManager myItemManager;
    public Image myImage;
    public Sprite[] itemSprites;
    public GoToColor itemFlash;

    void Update () {
        if (myItemManager.curVisualItem != ItemManager.Item.Empty) {
            myImage.sprite = itemSprites[(int) myItemManager.curVisualItem];
            myImage.color = Color.white;
        } else {
            myImage.sprite = null;
            myImage.color = Color.clear;
        }
        if (myItemManager.curState != myItemManager.lastState && myItemManager.curState == ItemManager.State.HasItem) {
            itemFlash.SetToNewColor (itemFlash.colorGoal, itemFlash.lerpSpeed);
            itemFlash.img.color = Color.white;
        }
    }
}