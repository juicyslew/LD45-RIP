using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    float targzoom;
    float origzoom;
    float currzoom;
    float smoothing = .25f;

    void Start()
    {
        origzoom = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        targzoom = origzoom * ((Time.timeScale + 1) / 2f);
        currzoom = Camera.main.orthographicSize;
        float movezoom = (targzoom - currzoom) / smoothing * Time.deltaTime;
        Camera.main.orthographicSize += movezoom;
    }
}
