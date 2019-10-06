using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    public Transform follow;
    float smoothing = .25f;
    float origz;

    // Use this for initialization
    void Start()
    {
        origz = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targpos = (Vector2)follow.position;
        Vector2 currpos = (Vector2)transform.position;
        Vector2 move = (targpos - currpos) / smoothing * Time.deltaTime;

        transform.position = new Vector3(transform.position.x + move.x, transform.position.y + move.y, origz);
    }
}
