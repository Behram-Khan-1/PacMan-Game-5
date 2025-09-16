using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Tilemap wallTilemap; // Assign your wall Tilemap in Inspector

    [SerializeField] private Vector3Int currentCell;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] public Vector2Int moveDir = Vector2Int.zero;   // Current moving dir
    [SerializeField] public Vector2Int nextDir = Vector2Int.zero;   // Queued direction

    InputHandler input;
    public Wrapping wrapping;

    void Start()
    {
        input = GetComponent<InputHandler>();
        // Set initial position
        //Get players currentPos to Cell and set targetPos to Cell too.
        currentCell = wallTilemap.WorldToCell(transform.position);
        targetPos = wallTilemap.GetCellCenterWorld(currentCell);
        transform.position = targetPos;
    }

    void Update()
    {
        nextDir = input.nextDir;
        Movement();
    }
    void Movement()
    {
        //Move player towards targetPos.
        //In start of game, it will set targetPos to currentCell (meaning players own position).
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f) // if reached center of targetPos cell
        {
            currentCell = wallTilemap.WorldToCell(targetPos); // Move currentCell to targetPos as player moved from Current to Target

            if (CanMove(nextDir)) // If player can move to nextDir, the queued direction, then set moveDir to nextDir
            {
                //at every cell, it will check if player can move for example up. If it can move, it will set moveDir to nextDir (up)    
                moveDir = nextDir;
            }

            if (CanMove(moveDir)) //Make player move in moveDir. it will move straight until nextDir becomes true and changes MoveDir
            {
                var nextCell = wallTilemap.WorldToCell(transform.position + new Vector3(moveDir.x, moveDir.y, 0));
                targetPos = wallTilemap.GetCellCenterWorld(nextCell);
            }
        }
        TurnFace(moveDir);
    }
    private void TurnFace(Vector2Int moveDir)
    {
        if (moveDir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveDir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (moveDir.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (moveDir.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    } 
    bool CanMove(Vector2 dir)
    {
        // Check if the next cell is a wall
        // Get 1 tile ahead of players direction and check if its walkable or not.
        Vector3Int nextCell = wallTilemap.WorldToCell(transform.position + new Vector3(dir.x, dir.y, 0));
        var tile = wallTilemap.GetTile(nextCell);

        if (tile == null) // if player goes out of bounds, then we get null as no tile is there and we have to warp
        {
            wrapping.WrappingMovement(moveDir, this.transform);
            return true; // so we continue moving and dont stop
        }

        if (tile.name.Contains("Wall_00")) //Wall_00 is walkable, all others are non walkable
        {
            return true;
        }
        return false;
    }

}

