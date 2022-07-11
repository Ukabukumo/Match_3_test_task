using UnityEngine;
using System;
using System.Collections.Generic;

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

        List<Tile> matchedTiles = GetMatches();
        while (matchedTiles.Count != 0)
        {
            RemoveMatches(matchedTiles);
            matchedTiles = GetMatches();
        }
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
                tile.SetValues(x, y);
                tiles[y, x] = tile;
            }
        }
    }

    private List<Tile> GetMatches()
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

    private void RemoveMatches(List<Tile> matchedTiles)
    {
        foreach (Tile tile in matchedTiles)
        {
            for (int i = tile.Y; i > 0; --i)
            {
                tiles[i, tile.X].SetFigure(tiles[i - 1, tile.X].FigureInd);
            }

            tiles[0, tile.X].SetFigure();
        }
    }

    private bool IsMatchedTile(Tile tile)
    {
        int horizontalCnt = 1;
        int vertiaclCnt = 1;

        // Count number of right figures
        for (int i = tile.X + 1; i < fieldSize; ++i)
        {
            if (tiles[tile.Y, i] != tile) break;

            horizontalCnt++;
        }

        // Count number of left figures
        for (int i = tile.X - 1; i >= 0; --i)
        {
            if (tiles[tile.Y, i] != tile) break;

            horizontalCnt++;
        }

        // Count number of top figures
        for (int i = tile.Y - 1; i >= 0; --i)
        {
            if (tiles[i, tile.X] != tile) break;

            vertiaclCnt++;
        }

        // Count number of bottom figures
        for (int i = tile.Y + 1; i < fieldSize; ++i)
        {
            if (tiles[i, tile.X] != tile) break;

            vertiaclCnt++;
        }

        return horizontalCnt >= 3 || vertiaclCnt >= 3;
    }    
}
