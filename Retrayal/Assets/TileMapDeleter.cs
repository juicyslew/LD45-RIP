using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapDeleter : MonoBehaviour
{
    Tilemap tilemap;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("Got HERE!");
        //Detecting the Grid Position of Player
        if (col.gameObject.name == "Explosion")
        {
            
        }
    }
}
