using UnityEngine;
using UnityEngine.Tilemaps;

public class Wrapping : MonoBehaviour
{
    public Tilemap wallTilemap;

    void Awake()
    {
        wallTilemap.CompressBounds();
    }

    public void WrappingMovement(Vector2Int nextDir, Transform objectToMove)
    {
        if (GetNextCell(objectToMove) == true)
        {
            Vector3Int playerCell = wallTilemap.WorldToCell(objectToMove.position);

            if (nextDir == Vector2Int.right && playerCell.x > wallTilemap.cellBounds.xMax - 1)
            {
                playerCell.x = wallTilemap.cellBounds.xMin;
                objectToMove.position = wallTilemap.GetCellCenterWorld(playerCell);
            }
            else if (nextDir == Vector2Int.left && playerCell.x < wallTilemap.cellBounds.xMin)
            {
                playerCell.x = wallTilemap.cellBounds.xMax - 1;
                objectToMove.position = wallTilemap.GetCellCenterWorld(playerCell);
            }

        }
    }

    public bool GetNextCell(Transform objectToMove)
    {
        Vector3Int nextCell = wallTilemap.WorldToCell(objectToMove.position);
        var nextTile = wallTilemap.GetTile(nextCell);
        if (nextTile == null)
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3Int startCell = wallTilemap.cellBounds.min;
        Vector3Int endCell = wallTilemap.cellBounds.max;

        Vector3 start = wallTilemap.GetCellCenterWorld(startCell);
        Vector3 end = wallTilemap.GetCellCenterWorld(endCell);

        Gizmos.DrawLine(start, new Vector3(start.x, end.y, start.z));
        Gizmos.DrawLine(start, new Vector3(end.x, start.y, start.z));
        Gizmos.DrawLine(end, new Vector3(start.x, end.y, start.z));
        Gizmos.DrawLine(end, new Vector3(end.x, start.y, start.z));
    }
}
