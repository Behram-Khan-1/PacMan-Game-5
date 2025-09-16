using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostMovement : MonoBehaviour
{
    float speed;
    public Tilemap wallTilemap; // Assign your wall Tilemap in Inspector
    public Tilemap intersectionTilemap; // the tilemap where we marked areas where ghosts can turn.
    [SerializeField] public Vector3Int currentCell;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] public Vector2Int moveDir;  // Current moving dir
    Ghost ghost;
    public Wrapping wrapping;

    void Start()
    {

        currentCell = wallTilemap.WorldToCell(transform.position);
        targetPos = wallTilemap.GetCellCenterWorld(currentCell);
        transform.position = targetPos;

        ghost = GetComponent<Ghost>();

    }

    void Update()
    {
        speed = GetComponent<Ghost>().speed;
        Movement();
    }

    void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            currentCell = wallTilemap.WorldToCell(targetPos);
            targetPos = wallTilemap.GetCellCenterWorld(currentCell);

            if (GhostManager.Instance.isFrightened == true)
            {
                GetRandomDirections();
            }

            if (!CanMove(moveDir) || GetValidDirections().Count > 1
            && GhostManager.Instance.isFrightened == false)
            {
                PickOptimalDirection(ghost.target);
            }

            var nextCell = wallTilemap.WorldToCell(transform.position + new Vector3(moveDir.x, moveDir.y, 0));
            targetPos = wallTilemap.GetCellCenterWorld(nextCell);

        }
    }
    void PickOptimalDirection(Vector3Int target)
    {
        List<Vector2Int> validDirections = GetValidDirections();
        if (validDirections.Count == 0) return;

        float minimalDistance = 1000;
        Vector2Int minDistance = moveDir;

        foreach (Vector2Int dir in validDirections)
        {
            Vector3Int testCell = currentCell + new Vector3Int(dir.x, dir.y, 0);
            float distance = Vector3Int.Distance(target, testCell);

            if (distance < minimalDistance)
            {
                minimalDistance = distance;
                minDistance = dir;
            }
        }

        moveDir = minDistance;
        // Debug.Log(moveDir);

    }

    List<Vector2Int> GetValidDirections()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> validDirections = new List<Vector2Int>();

        for (int i = 0; i < directions.Length; i++)
        {
            if (CanMove(directions[i]) && directions[i] != -moveDir)
            {
                validDirections.Add(directions[i]);
                // Debug.Log(directions[i]);
            }
        }
        return validDirections;
    }

    void GetRandomDirections()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<Vector2Int> validDirections = new List<Vector2Int>();

        for (int i = 0; i < directions.Length; i++)
        {
            if (CanMove(directions[i]) && directions[i] != -moveDir)
            {
                validDirections.Add(directions[i]);
                // Debug.Log(directions[i]);
            }
        }

        var randomIndex = Random.Range(0, validDirections.Count);
        moveDir = validDirections[randomIndex];
    }

    bool CanMove(Vector2 dir)
    {
        // Check if the next cell is a wall
        // Get 1 tile ahead of players direction and check if its walkable or not.
        Vector3Int nextCell = wallTilemap.WorldToCell(transform.position + new Vector3(dir.x, dir.y, 0));
        var tile = wallTilemap.GetTile(nextCell);

        if (tile == null) // if player goes out of bounds, then we get null as no tile is there and we have to warp
        {
            wrapping.WrappingMovement(moveDir, transform);
            return true; // so we continue moving and dont stop
        }
        // || 
        if (tile.name.Contains("Wall_00")) //Wall_00 is walkable, all others are non walkable
        {
            return true;
        }

        if (tile.name.Contains("Wall_37") && ghost.GetState() == GhostStates.LeavingPen
        || tile.name.Contains("Wall_37") && ghost.GetState() == GhostStates.Eaten)
        {
            return true;
        }
        return false;
    }

    // Visual debugging
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.DrawSphere(targetPos, 0.5f);
        }
    }




}
