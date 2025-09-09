using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2Int nextDir;
    void Update()
    {
        HandleInput();
    }
    void HandleInput() //Taking Queued direction nextDir
    {
        // Record nextDir when player presses a key
        if (Input.GetKeyDown(KeyCode.W)) nextDir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) nextDir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) nextDir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) nextDir = Vector2Int.right;
    }

}
