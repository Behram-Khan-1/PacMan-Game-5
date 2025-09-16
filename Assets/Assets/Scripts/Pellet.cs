using UnityEngine;
using UnityEngine.Tilemaps;

public class Pellet : MonoBehaviour
{
    public Tilemap pelletTilemap;

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerPos = pelletTilemap.WorldToCell(transform.position);
        TileBase pelletPos = pelletTilemap.GetTile(playerPos); // get the tile at player pos

        if (pelletPos != null)
        {
            if (pelletPos.name.Contains("Pellet_Small"))
            {
                pelletTilemap.SetTile(playerPos, null);
                UIManager.Instance.IncreaseScore(1);
                //Score increase
            }
            if (pelletPos.name.Contains("Pellet_Large"))
            {
                UIManager.Instance.IncreaseScore(2);
                pelletTilemap.SetTile(playerPos, null);
                //Ghost Flee Call
                GhostManager.Instance.Frightened();
            }
        }
    }
}
