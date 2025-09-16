using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public float speed = 5f;
    public GhostStates ghostState = GhostStates.InPen;
    //Tilemap stuff
    public Transform exitPosition;
    public Transform penPosition;

    public Tilemap wallTilemap;
    Vector3Int startingCell;
    [SerializeField] Vector3Int exitCell;
    [SerializeField] Vector3Int playerCell;

    //player
    public Transform pacman;
    public Vector3Int target;
    public Transform corner;
    public Transform Blinky; //Red for Inky

    //
    Animator animator;


    [SerializeField] public GhostType ghostType;
    // Enum for clean selection in the Inspector


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingCell = wallTilemap.WorldToCell(transform.position);
        exitCell = wallTilemap.WorldToCell(exitPosition.position);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerCell = GetComponent<GhostMovement>().currentCell;
        MovementAnimation();
        if (ghostState != GhostStates.Frightened)
        {
            speed = 5;
        }

        switch (ghostState)
        {
            case GhostStates.InPen:
                target = wallTilemap.WorldToCell(penPosition.position);
                //Do nothing

                break;

            case GhostStates.LeavingPen:
                // move upward until fully outside
                target = exitCell;
                // animator.SetBool("IsEaten", false);
                EatenVisualOff();
                if (playerCell.y >= exitCell.y || playerCell.y == exitCell.y)
                {
                    SetState(GhostStates.Scatter);
                    GetComponent<BoxCollider2D>().enabled = true;
                }
                break;

            case GhostStates.Chase:
                // chasing logic
                if (ghostType == GhostType.Blinky)
                {
                    target = BlinkyChase();
                }
                if (ghostType == GhostType.Pinky)
                {
                    target = PinkyChase();
                }
                if (ghostType == GhostType.Inky)
                {
                    target = InkyChase();
                }
                if (ghostType == GhostType.Clyde)
                {
                    target = ClydeChase();
                }
                break;

            case GhostStates.Scatter:
                // scatter logic
                ScatterLogic();
                break;

            case GhostStates.Frightened:
                //
                Frightened();

                break;

            case GhostStates.Eaten:
                // return to pen
                //Turn to only eyes
                Eaten();

                break;
        }
    }

    void MovementAnimation()
    {
        var direction = GetComponent<GhostMovement>().moveDir;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    void Eaten()
    {
        // animator.SetBool("IsEaten", true);
        animator.SetBool("IsFrightened", false);
        EatenVisualOn();
        

        target = wallTilemap.WorldToCell(penPosition.position);

        if (playerCell == target)
        {
            SetState(GhostStates.LeavingPen);
        }
    }
    public void EatenVisualOn()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void EatenVisualOff()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    void Frightened()
    {
        animator.SetBool("IsFrightened", true);
        //blinking
        speed = 2;

        if (playerCell == wallTilemap.WorldToCell(pacman.position))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            SetState(GhostStates.Eaten);
        }
    }

    private void ScatterLogic()
    {
        if (ghostType == GhostType.Blinky)
        {
            target = ScatterCorner();
        }
        if (ghostType == GhostType.Pinky)
        {
            target = ScatterCorner();
        }
        if (ghostType == GhostType.Inky)
        {
            target = ScatterCorner();
        }
        if (ghostType == GhostType.Clyde)
        {
            target = ScatterCorner();
        }
    }

    public Vector3Int BlinkyChase() //RED
    {
        //chase pacman position directly
        target = wallTilemap.WorldToCell(pacman.position);
        return target;
    }

    public Vector3Int PinkyChase()
    {
        Vector2Int pacmanDir = pacman.GetComponent<PlayerMovement>().moveDir;
        Vector3Int pacmanTile = wallTilemap.WorldToCell(pacman.position);
        Vector3Int targetTile = pacmanTile + new Vector3Int(pacmanDir.x * 4, pacmanDir.y * 4, 0);
        return targetTile;
    }

    public Vector3Int InkyChase()
    {
        Vector3Int pacmanTile = wallTilemap.WorldToCell(pacman.position);
        Vector3Int targetTile = pacmanTile + (pacmanTile - wallTilemap.WorldToCell(Blinky.transform.position)); //-25 -3 == -15 6
        return targetTile;
    }

    public Vector3Int ClydeChase()
    {
        if (Vector3Int.Distance(wallTilemap.WorldToCell(pacman.position), wallTilemap.WorldToCell(transform.position)) > 8)
        {
            return wallTilemap.WorldToCell(pacman.position);
        }
        SetState(GhostStates.Scatter);
        return ScatterCorner();
    }

    public Vector3Int ScatterCorner()
    {
        return wallTilemap.WorldToCell(corner.position);
    }


    public void SetState(GhostStates newState)
    { ghostState = newState; }

    public GhostStates GetState()
    { return ghostState; }



    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target);
            Gizmos.DrawSphere(target, 0.5f);
        }
    }

}
