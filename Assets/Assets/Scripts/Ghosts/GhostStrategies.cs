// // Create a new file, e.g., GhostStrategies.cs
// using UnityEngine;
// using UnityEngine.Tilemaps;

// public static class GhostStrategies
// {
//     public delegate Vector3Int ChaseStrategy(Ghost ghost);

//     // 1. Blinky (Red): Direct Aggression - targets Pac-Man's exact cell.
//     public static Vector3Int BlinkyStrategy(Ghost ghost)
//     {
//         return ghost.wallTilemap.WorldToCell(ghost.pacman.position);
//     }

    // // 2. Pinky (Pink): Ambush - targets 4 tiles ahead of Pac-Man.
    // public static Vector3Int PinkyStrategy(Ghost ghost)
    // {
    //     Vector3 pacmanWorldPos = ghost.pacman.position;
    //     Vector2Int pacmanDir = ghost.pacman.GetComponent<PlayerMovement>().moveDir; // Get Pac-Man's direction
    //     Vector3 targetWorldPos = pacmanWorldPos + new Vector3(pacmanDir.x, pacmanDir.y, 0) * 4f;
    //     return ghost.wallTilemap.WorldToCell(targetWorldPos);
    // }

    // // 3. Inky (Cyan): Complex - Uses a point relative to Blinky and Pac-Man.
    // public static Vector3Int InkyStrategy(Ghost ghost)
    // {
    //     // Find Blinky in the scene (assuming he's named "Blinky" or "red")
    //     Ghost blinky = GameObject.Find("red")?.GetComponent<Ghost>(); // Better to get this via GhostManager
    //     if (blinky == null) return ghost.scatterTargetTile; // Fallback

    //     Vector3Int pacmanTile = ghost.wallTilemap.WorldToCell(ghost.pacman.position);
    //     Vector3Int blinkyTile = ghost.wallTilemap.WorldToCell(blinky.transform.position);

    //     // Classic Inky formula: PacmanTile + (PacmanTile - BlinkyTile)
    //     Vector3Int targetTile = pacmanTile + (pacmanTile - blinkyTile);
    //     return targetTile;
    // }

    // // 4. Clyde (Orange): Procrastinator - chases if far, scatters if close.
    // public static Vector3Int ClydeStrategy(Ghost ghost)
    // {
    //     float distanceToPacman = Vector3.Distance(ghost.transform.position, ghost.pacman.position);
    //     return (distanceToPacman > 8f) ? ghost.wallTilemap.WorldToCell(ghost.pacman.position) : ghost.scatterTargetTile;
    // }
// }