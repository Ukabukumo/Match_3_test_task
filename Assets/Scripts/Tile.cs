using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    private const int FIGURE_SCALE = 2;

    private GameObject figure;
    private SpriteRenderer sr;

    public int X { get; private set; }
    public int Y { get; private set; }
    public int FigureInd { get; private set; }

    public void SetValues(int x, int y)
    {
        X = x;
        Y = y;
        CreateFigure();
    }

    private void CreateFigure()
    {
        if (figure != null) Destroy(figure);

        figure = new GameObject("Figure");
        sr = figure.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Figure";
        figure.transform.position = transform.position;
        figure.transform.localScale = transform.localScale * FIGURE_SCALE;
        figure.transform.SetParent(transform);

        SetFigure();
    }

    public void SetFigure()
    {
        Tuple<int, Sprite> figureInfo = FigureManager.instance.GetRandomFigure();
        FigureInd = figureInfo.Item1;
        sr.sprite = figureInfo.Item2;
    }

    public void SetFigure(int figureInd)
    {
        FigureInd = figureInd;
        sr.sprite = FigureManager.instance.GetFigure(figureInd);
    }

    public static bool operator ==(Tile tile1, Tile tile2)
    {
        return tile1.Equals(tile2);
    }

    public static bool operator !=(Tile tile1, Tile tile2)
    {
        return !tile1.Equals(tile2);
    }

    public override bool Equals(object obj)
    {
        return FigureInd == ((Tile)obj).FigureInd;
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
