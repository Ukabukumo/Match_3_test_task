using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public const int SCORE_VALUE = 10;
    public bool inGame = false;
    
    [SerializeField] private Field field;

    private bool isFirstTile = true;
    private Tile firstTile;
    private int score = 0;
    private float gameDuration = 60.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        field.CreateField();
        StartCoroutine(GameOver(gameDuration));
        inGame = true;
    }

    public void AddScore(int points)
    {
        if (inGame)
        {
            score += (points - 2) * SCORE_VALUE;
        }
        
        UIManager.instance.SetScore(score);
    }

    public void FigureSelected(Tile selectedTile)
    {
        if (field.HasAnimatedTiles()) return;

        selectedTile.SetSelected(true);

        if (!(isFirstTile) && (firstTile.transform == selectedTile.transform))
        {
            isFirstTile = true;
            firstTile.SetSelected(false);
            return;
        }

        if (isFirstTile)
        {
            firstTile = selectedTile;
            isFirstTile = false;
            return;
        }

        if (!IsNear(firstTile, selectedTile))
        {
            firstTile.SetSelected(false);
            firstTile = selectedTile;
            return;
        }

        SwapFigures(firstTile, selectedTile);
        float duration = firstTile.figure.Duration;

        if (!field.IsMatchedTile(firstTile) && !field.IsMatchedTile(selectedTile))
        {
            SwapFigures(firstTile, selectedTile);
            duration += firstTile.figure.Duration;
        }

        isFirstTile = true;
        StartCoroutine(EndSelection(firstTile, selectedTile, duration));
    }

    private IEnumerator EndSelection(Tile tile1, Tile tile2, float time)
    {
        yield return new WaitForSeconds(time);

        tile1.SetSelected(false);
        tile2.SetSelected(false);
    }

    private IEnumerator GameOver(float time)
    {
        float timeLeft = time;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UIManager.instance.SetTime((int)Mathf.Ceil(timeLeft));
            yield return null;
        }

        inGame = false;
        UIManager.instance.GameOverPanel(true);
    }

    private void SwapFigures(Tile tile1, Tile tile2)
    {
        int type = tile1.figure.Type;
        tile1.figure.SetType(tile2.figure.Type);
        tile2.figure.SetType(type);

        MoveFigures(tile1, tile2);
    }

    private void MoveFigures(Tile tile1, Tile tile2)
    {
        Vector3 dist = tile1.transform.position - tile2.transform.position;

        // tile1 to the left of the tile2
        if (dist.x < 0)
        {
            tile1.figure.MoveLeft();
            tile2.figure.MoveRight();
        }

        // tile1 to the right of the tile2
        else if (dist.x > 0)
        {
            tile1.figure.MoveRight();
            tile2.figure.MoveLeft();
        }

        // tile1 below tile2
        else if (dist.y < 0)
        {
            tile1.figure.MoveDown();
            tile2.figure.MoveUp();
        }

        // tile1 above tile2
        else if (dist.y > 0)
        {
            tile1.figure.MoveUp();
            tile2.figure.MoveDown();
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
