using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShellExplosion : MonoBehaviour {
    public float speed = 1;
    public Renderer explosionRenderer;
    Color startColor;
    public Light explosionLight;
    float startLightIntensity;
    void Start () {
        startColor = explosionRenderer.material.color;
        startLightIntensity = explosionLight.intensity;
    }

    float curTime = 0;
    void Update () {
        curTime += Time.deltaTime * speed;
        explosionRenderer.material.color = Color.Lerp (startColor, Color.clear, curTime);
        explosionLight.intensity = (1 - curTime) * startLightIntensity;

        if (curTime > 1) {
            Destroy (gameObject);
        }
    }
}