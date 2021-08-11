using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAnim : MonoBehaviour
{
    Renderer rend;
    public float rate = 0.01f;
    public Sprite[] sprites;
    void Start()
    {
        rend = GetComponent<Renderer>();
        InvokeRepeating("UpdateSprite",rate,rate);
    }

    int curSprite = 0;
    void UpdateSprite()
    {
        curSprite = (int)Mathf.Repeat(curSprite + 1,sprites.Length);

        rend.material.mainTexture = sprites[curSprite].texture;
    }
}
