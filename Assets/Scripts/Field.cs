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

    private void Update()
    {
        RemoveMatches();
        FallTiles();
        
        if (!HasPossibleMatches())
        {
            StartCoroutine(UIManager.instance.NoMatchesPanel(1.0f));
            ClearField();
        }
    }

    public void CreateField()
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

        List<Tile> matchedTiles = GetMatchedTiles();
        while (matchedTiles.Count > 0)
        {
            RemoveTiles(matchedTiles);
            FillTiles();
            matchedTiles = GetMatchedTiles();
        }
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

    public bool HasPossibleMatches()
    {
        if (HasAnimatedTiles() || (GetEmptyTiles().Count > 0)) return true;

        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                Tile curTile = tiles[y, x];
                int originType = curTile.figure.Type;
                List<Tile> possibleTiles = new List<Tile>();

                if (x > 0) possibleTiles.Add(tiles[y, x - 1]);
                if (x < fieldSize - 1) possibleTiles.Add(tiles[y, x + 1]);
                if (y > 0) possibleTiles.Add(tiles[y - 1, x]);
                if (y < fieldSize - 1) possibleTiles.Add(tiles[y + 1, x]);

                foreach (Tile tile in possibleTiles)
                {
                    int type = tile.figure.Type;
                    curTile.figure.SetType(type);
                    tile.figure.SetEmpty();

                    if (IsMatchedTile(curTile))
                    {
                        curTile.figure.SetType(originType);
                        tile.figure.SetType(type);
                        return true;
                    }

                    tile.figure.SetType(type);
                }

                curTile.figure.SetType(originType);
            }
        }

        return false;
    }

    private void RemoveTiles(List<Tile> matchedTiles)
    {
        CalculateScore(matchedTiles);

        foreach (Tile tile in matchedTiles)
        {
            tile.figure.SetEmpty();
        }
    }

    private void ClearField()
    {
        for (int y = 0; y < fieldSize; ++y)
        {
            for (int x = 0; x < fieldSize; ++x)
            {
                tiles[y, x].figure.SetEmpty();
            }
        }
    }

    private void CalculateScore(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            // Value of one tile
            GameManager.instance.AddScore(GetMatch(tile));
        }
    }

    private void FillTiles()
    {
        List<Tile> emptyTiles = GetEmptyTiles();

        foreach (Tile tile in emptyTiles)
        {
            tile.figure.SetType();
        }
    }

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
                        tile.figure.SpawnFigure();
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
        while ((matchedTiles.Count > 0) && (GetEmptyTiles().Count == 0) && !HasAnimatedTiles())
        {
            RemoveTiles(matchedTiles);
            matchedTiles = GetMatchedTiles();
        }
    }

    public bool IsMatchedTile(Tile tile)
    {
        // Empty tile
        if (tile.figure.IsEmpty()) return false;

        return GetMatch(tile) > 0;
    }

    public int GetMatch(Tile tile)
    {
        int match = 0;

        // Empty tile
        if (tile.figure.IsEmpty()) return match;

        int horizontalCnt = 0;
        int verticalCnt = 0;

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

            verticalCnt++;
        }

        // Count number of bottom figures
        for (int i = tile.Y + 1; i < fieldSize; ++i)
        {
            if (tiles[i, tile.X].figure != tile.figure) break;

            verticalCnt++;
        }

        if (horizontalCnt >= 2) match += horizontalCnt;
        if (verticalCnt >= 2) match += verticalCnt;
        match++;

        return match > 1 ? match : 0;
    }
    
    public float GetGap()
    {
        return gapSize;
    }
}
