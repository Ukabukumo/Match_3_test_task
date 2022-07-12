using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    private const int FIGURE_SCALE = 2;

    public Figure figure;
    public int X { get; private set; }
    public int Y { get; private set; }

    public void SetValues(int x, int y)
    {
        X = x;
        Y = y;

        figure = Instantiate(figure, transform.position, Quaternion.identity, transform);
        figure.transform.localScale = figure.transform.localScale * FIGURE_SCALE;
        figure.SetType();
    }

    private void OnMouseDown()
    {
        if (!figure.IsEmpty())
        {
            GameManager.instance.FigureChoosed(GetComponent<Tile>());
        }
    }
}
