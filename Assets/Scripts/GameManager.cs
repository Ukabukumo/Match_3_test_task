using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Field field;

    private bool isFirstTile = true;
    private Tile firstTile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void FigureChoosed(Tile choosedTile)
    {
        if (field.HasAnimatedTiles()) return;

        if (isFirstTile)
        {
            firstTile = choosedTile;
            isFirstTile = false;
            return;
        }

        SwapFigures(firstTile, choosedTile);

        if (field.IsMatchedTile(firstTile) || field.IsMatchedTile(choosedTile))
        {
            field.RemoveMatches();
        }

        else
        {
            SwapFigures(firstTile, choosedTile);
        }

        isFirstTile = true;
    }

    private void SwapFigures(Tile tile1, Tile tile2)
    {
        if (IsNear(tile1, tile2))
        {
            int type = tile1.figure.Type;
            tile1.figure.SetType(tile2.figure.Type);
            tile2.figure.SetType(type);
        }
    }

    private bool IsNear(Tile tile1, Tile tile2)
    {
        if ((Mathf.Abs(tile1.X - tile2.X) == 1) && ((tile1.Y - tile2.Y) == 0) ||
            (Mathf.Abs(tile1.Y - tile2.Y) == 1) && ((tile1.X - tile2.X) == 0))
        {
            return true;
        }

        return false;
    }
}
