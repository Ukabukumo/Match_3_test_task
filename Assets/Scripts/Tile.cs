using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameObject figure;

    private void Awake()
    {
        figure = new GameObject("Figure");  
        figure.AddComponent<SpriteRenderer>();
        figure.GetComponent<SpriteRenderer>().sortingLayerName = "Figure";
        figure.transform.position = transform.position;
        figure.transform.SetParent(transform);
    }

    private void Start()
    {
        figure.GetComponent<SpriteRenderer>().sprite = FigureManager.instance.GetRandomFigure();
    }
}
