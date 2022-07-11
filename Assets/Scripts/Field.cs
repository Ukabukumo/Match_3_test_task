using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private int   fieldSize = 8;
    [SerializeField] private float gapSize   = 0.1f;
    [SerializeField] private Tile  tilePref;

    private Transform tr;
    private Tile[,]   tiles;

    private void Start()
    {
        tr = GetComponent<Transform>();
        CreateField();
    }

    private void CreateField()
    {
        tiles = new Tile[fieldSize, fieldSize];

        float tileSize = (tr.localScale.x - (fieldSize + 1) * gapSize) / fieldSize;
        tilePref.transform.localScale = new Vector2(tileSize, tileSize);

        // Position of left-up tile
        float x0 = -tr.localScale.x / 2.0f + tileSize / 2.0f + gapSize;
        float y0 = tr.localScale.x / 2.0f - tileSize / 2.0f - gapSize;

        Transform tilesStorage = new GameObject().transform;
        tilesStorage.SetParent(tr);

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                Vector2 pos = new Vector2(x0 + x * (tileSize + gapSize), y0 - y * (tileSize + gapSize));

                Tile tile = Instantiate(tilePref, pos, Quaternion.identity, tilesStorage);
                tiles[y, x] = tile;
            }
        }
    }
}
