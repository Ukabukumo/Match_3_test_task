using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color selected;
    [SerializeField] private Color unselected;
    private SpriteRenderer sr;

    public Figure figure;
    public int figureScale = 2;
    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetValues(int x, int y)
    {
        X = x;
        Y = y;

        figure = Instantiate(figure, transform.position, Quaternion.identity, transform);
        figure.transform.localScale = figure.transform.localScale * figureScale;
        figure.SetType();
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            sr.color = selected;
        }

        else
        {
            sr.color = unselected;
        }
    }

    private void OnMouseDown()
    {
        if (!figure.IsEmpty() && GameManager.instance.inGame)
        {
            GameManager.instance.FigureSelected(GetComponent<Tile>());
        }
    }
}
