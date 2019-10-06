using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DestructibleTileMap : MonoBehaviour
{
    #region Properties
    private Tilemap tileMap;
    private GridLayout grid;
    private Vector3Int tilePosition;
    public GameObject ParticleObject;
    public AudioSource DestroySound;
    #endregion
    #region Unity callbacks
    public void Start()
    {
        tileMap = GetComponent<Tilemap>();
        grid = GetComponentInParent<GridLayout>();
    }
    #endregion
    public void DestroyTilesExplosion(float radius, Vector3 point)
    {
        
        Vector3Int pPos = grid.WorldToCell(point);
        Debug.Log(pPos);
        bool onehit = false;
        for (int i = Mathf.CeilToInt(-radius); i < radius; i++)
        {
            for (int j = Mathf.CeilToInt(-radius); j < radius; j++)
            {
                if (new Vector2(i, j).magnitude < radius)
                {
                    Vector3Int checkPos = pPos + new Vector3Int(i, j, 0);
                    if (!Equals(tileMap.GetTile(checkPos), null))
                    {
                        onehit = true;
                        tileMap.SetTile(checkPos, null);
                        Instantiate(ParticleObject, grid.CellToWorld(checkPos), transform.rotation);
                    }
                }
            }
        }
        if (onehit) { Instantiate(DestroySound.gameObject, point, transform.rotation); }
        /*TileBase tileToDamage = tileMap.GetTile(grid.WorldToCell(point));
        if (!Equals(tileToDamage, null))
        {
            if (tileToDamage is ScriptableTile)
            {
                ((ScriptableTile)tileToDamage).DestroyTile();
                tileMap.RefreshTile(((ScriptableTile)tileToDamage).tilePosition);
            } 
        }*/
    } 
}