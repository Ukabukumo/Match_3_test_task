using UnityEngine;
using System;

public class FigureManager : MonoBehaviour
{
    public static FigureManager instance;

    public Sprite[] figureSprites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Tuple<int, Sprite> GetRandomFigure()
    {
        int ind = UnityEngine.Random.Range(0, figureSprites.Length);
        Sprite sprite = figureSprites[ind];

        return Tuple.Create(ind, sprite);
    }

    public Sprite GetFigure(int figureInd)
    {
        return figureSprites[figureInd];
    }
}
