using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScriptableTile : Tile
{
    /*#region Properties
    [Space(20)]
    [Header("ScriptableTile")]
    /// <summary>
    /// Life remaining before destroy tile
    /// </summary>
    /// <summary>
    /// Sprite to display when life is below 50%
    /// </summary>
    //public Sprite brokenSprite;
    public ITilemap tileMap;
    public Vector3Int tilePosition;
    #endregion
    #region Tile Overriding
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //Store some data
        this.tileMap = tilemap;
        this.tilePosition = position;
        return base.StartUp(position, tilemap, go);
    }
    #endregion
    #region Implementation
    /// <summary>
    /// Apply damage to the tile
    /// </summary>
    /// <param name="pDamage"></param>
    public void DestroyTile()
    {
        base.sprite = null;
        //Spawn Particle System
    }
    #endregion

    public BoxCollider2D col;
    #region Asset DataBase
    [MenuItem("Assets/ScriptableTile")]
    public static void CreateDestructibleTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Scriptable Tile", "ScriptableTile_", "Asset", "Save Scriptable Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ScriptableTile>(), path);
    }
    #endregion*/
}