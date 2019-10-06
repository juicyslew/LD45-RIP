using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplosionProperties : MonoBehaviour
{
    float explodeStrength = 18f;
    DestructibleTileMap destroyable;
    // Start is called before the first frame update
    void Start()
    {
        destroyable = GameObject.FindGameObjectWithTag("Destructibles").GetComponent<DestructibleTileMap>();
        destroyable.DestroyTilesExplosion(GetComponent<CircleCollider2D>().radius, transform.position);
        Camera.main.GetComponent<CameraShaker>().CamShake(.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float getForce(float dist)
    {
        return Mathf.Lerp(explodeStrength,explodeStrength/2, dist / GetComponent<CircleCollider2D>().radius);
    }

    public void setStrength(float newstrength)
    {
        explodeStrength = newstrength;
    }
}
