using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{
    public float expireInterval = .2f;
    float expireTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        expireTimer += Time.deltaTime;
        if (expireTimer > expireInterval)
        {
            Destroy(gameObject);
        }
    }
}
