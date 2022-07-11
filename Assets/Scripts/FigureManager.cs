using UnityEngine;

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

    public Sprite GetRandomFigure()
    {
        return figureSprites[Random.Range(0, figureSprites.Length)];
    }
}
