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

    public int GetRandomFigure()
    {
        int type = UnityEngine.Random.Range(0, figureSprites.Length);

        return type;
    }

    public Sprite GetFigure(int figureType)
    {
        return figureSprites[figureType];
    }

    public int GetFiguresNumber()
    {
        return figureSprites.Length;
    }
}
