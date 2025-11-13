using UnityEngine;

public class TreeEdgeSpawner : MonoBehaviour
{
    public GameObject treePrefab;     // Assign your tree tile prefab here
    public string tileTag = "Tile";   // Tag used for all placed tile GameObjects
    public float tileSize = 1f;       // Size of each tile (assumes square grid)

    void Start()
    {
        PlaceTreesAroundMap();
    }

    void PlaceTreesAroundMap()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(tileTag);
        if (tiles.Length == 0)
        {
            Debug.LogWarning("No tiles found with tag '" + tileTag + "'.");
            return;
        }

        // Calculate map bounds
        Vector2 min = tiles[0].transform.position;
        Vector2 max = min;

        foreach (GameObject tile in tiles)
        {
            Vector2 pos = tile.transform.position;
            min = Vector2.Min(min, pos);
            max = Vector2.Max(max, pos);
        }

        // Expand bounds slightly to place trees just outside
        min -= Vector2.one * tileSize;
        max += Vector2.one * tileSize;

        // Place trees along top and bottom edges
        for (float x = min.x; x <= max.x; x += tileSize)
        {
            TryPlaceTree(new Vector2(x, min.y)); // Bottom
            TryPlaceTree(new Vector2(x, max.y)); // Top
        }

        // Place trees along left and right edges
        for (float y = min.y + tileSize; y < max.y; y += tileSize)
        {
            TryPlaceTree(new Vector2(min.x, y)); // Left
            TryPlaceTree(new Vector2(max.x, y)); // Right
        }
    }

    void TryPlaceTree(Vector2 position)
    {
        // Check if a tile already exists here
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit == null)
        {
            Instantiate(treePrefab, position, Quaternion.identity);
        }
    }
}