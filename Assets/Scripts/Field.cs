using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Field : MonoBehaviour
{
    [SerializeField] private int   fieldSize = 8;
    [SerializeField] private float gapSize   = 0.1f;
    [SerializeField] private Tile  tilePref;

    private Transform tr;
    private Tile[,]   tiles;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void Start()
    {
        CreateField();
    }

    private void Update()
    {
        RemoveMatches();
        FallTiles();
    }

    private void CreateField()
    {
        tiles = new Tile[fieldSize, fieldSize];

        float tileSize = (tr.localScale.x - (fieldSize + 1) * gapSize) / fieldSize;
        tilePref.transform.localScale = new Vector2(tileSize, tileSize);

        // Position of left-up tile
        float x0 = -tr.localScale.x / 2.0f + tileSize / 2.0f + gapSize;
        float y0 = tr.localScale.x / 2.0f - tileSize / 2.0f - gapSize;

        Transform tilesStorage = new GameObject("Tiles").transform;
        tilesStorage.SetParent(tr);

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                Vector2 pos = new Vector2(x0 + x * (tileSize + gapSize), y0 - y * (tileSize + gapSize));

                Tile tile = Instantiate(tilePref, pos, Quaternion.identity, tilesStorage);
                tile.SetValues(x, y);
                tiles[y, x] = tile;
            }
        }

        RemoveMatches();
    }

    public List<Tile> GetMatchedTiles()
    {
        List<Tile> matchedTiles = new List<Tile>();

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                if (IsMatchedTile(tiles[y, x]))
                {
                    matchedTiles.Add(tiles[y, x]);
                }
            }
        }

        return matchedTiles;
    }

    public List<Tile> GetEmptyTiles()
    {
        List<Tile> emptyTiles = new List<Tile>();

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                if (tiles[y, x].figure.IsEmpty())
                {
                    emptyTiles.Add(tiles[y, x]);
                }
            }
        }

        return emptyTiles;
    }

    public bool HasAnimatedTiles()
    {
        List<Tile> animatedTiles = new List<Tile>();

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                if (tiles[y, x].figure.IsAnimated)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void RemoveTiles(List<Tile> matchedTiles)
    {
        foreach (Tile tile in matchedTiles)
        {
            tile.figure.SetEmpty();
        }
    }

    /*private IEnumerator FallTiles()
    {
        List<Tile> emptyTiles = GetEmptyTiles();

        bool hasAnimated = false;

        while (emptyTiles.Count > 0 || hasAnimated)
        {
            hasAnimated = false;

            foreach (Tile tile in emptyTiles)
            {
                // Top line
                if (tile.Y == 0)
                {
                    Tile tileBelow = tiles[tile.Y + 1, tile.X];

                    if (!tileBelow.figure.IsAnimated)
                    {
                        tile.figure.SetType();
                    }
                }
                else
                {
                    Tile tileAbove = tiles[tile.Y - 1, tile.X];
                    if (tileAbove.figure.IsAnimated)
                    {
                        hasAnimated = true;
                        continue;
                    }

                    if (tileAbove.figure.IsEmpty())
                    {
                        continue;
                    }

                    tile.figure.SetType(tileAbove.figure.Type);
                    tileAbove.figure.SetEmpty();
                    tile.figure.MoveDown();
                }
            }

            emptyTiles = GetEmptyTiles();
            yield return null;
        }
    }*/

    private void FallTiles()
    {
        List<Tile> emptyTiles = GetEmptyTiles();

        if (emptyTiles.Count > 0)
        {
            foreach (Tile tile in emptyTiles)
            {
                // Top line
                if (tile.Y == 0)
                {
                    Tile tileBelow = tiles[tile.Y + 1, tile.X];

                    if (!tileBelow.figure.IsAnimated)
                    {
                        tile.figure.SetType();
                    }
                }
                else
                {
                    Tile tileAbove = tiles[tile.Y - 1, tile.X];
                    if (tileAbove.figure.IsAnimated ||
                        tileAbove.figure.IsEmpty())
                    {
                        continue;
                    }

                    tile.figure.SetType(tileAbove.figure.Type);
                    tileAbove.figure.SetEmpty();
                    tile.figure.MoveDown();
                }
            }
        }
    }

    public void RemoveMatches()
    {
        List<Tile> matchedTiles = GetMatchedTiles();
        while (matchedTiles.Count > 0 && !HasAnimatedTiles())
        {
            RemoveTiles(matchedTiles);
            //StartCoroutine(FallTiles());
            matchedTiles = GetMatchedTiles();
        }
    }

    public bool IsMatchedTile(Tile tile)
    {
        // Empty tile
        if (tile.figure.IsEmpty()) return false;

        int horizontalCnt = 1;
        int vertiaclCnt = 1;

        // Count number of right figures
        for (int i = tile.X + 1; i < fieldSize; ++i)
        {
            if (tiles[tile.Y, i].figure != tile.figure) break;

            horizontalCnt++;
        }

        // Count number of left figures
        for (int i = tile.X - 1; i >= 0; --i)
        {
            if (tiles[tile.Y, i].figure != tile.figure) break;

            horizontalCnt++;
        }

        // Count number of top figures
        for (int i = tile.Y - 1; i >= 0; --i)
        {
            if (tiles[i, tile.X].figure != tile.figure) break;

            vertiaclCnt++;
        }

        // Count number of bottom figures
        for (int i = tile.Y + 1; i < fieldSize; ++i)
        {
            if (tiles[i, tile.X].figure != tile.figure) break;

            vertiaclCnt++;
        }

        return horizontalCnt >= 3 || vertiaclCnt >= 3;
    }  
    
    public float GetGap()
    {
        return gapSize;
    }
}
