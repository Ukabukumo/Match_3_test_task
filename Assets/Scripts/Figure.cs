using UnityEngine;
using System.Collections;

public class Figure : MonoBehaviour
{
    private SpriteRenderer sr;
    private Tile tile;
    private Animation anim;
    private float scale;

    public float Duration { get; private set; }
    public bool IsAnimated { get; private set; }

    public int Type { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animation>();
        tile = transform.parent.GetComponent<Tile>();

        Duration = 0.3f;

        sr.sortingLayerName = "Figure";

        CreateAnimations();
    }

    private void CreateAnimations()
    {
        float gap = transform.parent.parent.parent.GetComponent<Field>().GetGap();
        float offset = 1.0f + gap / transform.parent.localScale.x;

        // Animation of moving figure down
        AnimationCurve moveDownCurve = AnimationCurve.Linear(0.0f, offset, Duration, 0);
        AnimationClip moveDownClip = new AnimationClip();
        moveDownClip.legacy = true;
        moveDownClip.SetCurve("", typeof(Transform), "localPosition.y", moveDownCurve);

        // Animation of moving figure up
        AnimationCurve moveUpCurve = AnimationCurve.Linear(0.0f, -offset, Duration, 0);
        AnimationClip moveUpClip = new AnimationClip();
        moveUpClip.legacy = true;
        moveUpClip.SetCurve("", typeof(Transform), "localPosition.y", moveUpCurve);

        // Animation of moving figure left
        AnimationCurve moveLeftCurve = AnimationCurve.Linear(0.0f, offset, Duration, 0);
        AnimationClip moveLeftClip = new AnimationClip();
        moveLeftClip.legacy = true;
        moveLeftClip.SetCurve("", typeof(Transform), "localPosition.x", moveLeftCurve);

        // Animation of moving figure right
        AnimationCurve moveRightCurve = AnimationCurve.Linear(0.0f, -offset, Duration, 0);
        AnimationClip moveRightClip = new AnimationClip();
        moveRightClip.legacy = true;
        moveRightClip.SetCurve("", typeof(Transform), "localPosition.x", moveRightCurve);

        // Animation of spawn figure
        AnimationCurve spawnFigureCurve = AnimationCurve.Linear(0.0f, 0, Duration, tile.figureScale);
        AnimationClip spawnFigureClip = new AnimationClip();
        spawnFigureClip.legacy = true;
        spawnFigureClip.SetCurve("", typeof(Transform), "localScale.x", spawnFigureCurve);
        spawnFigureClip.SetCurve("", typeof(Transform), "localScale.y", spawnFigureCurve);

        // End of animation event
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.functionName = "OnAnimationEnd";
        animEvent.time = Duration;

        moveDownClip.AddEvent(animEvent);
        moveUpClip.AddEvent(animEvent);
        moveLeftClip.AddEvent(animEvent);
        moveRightClip.AddEvent(animEvent);
        spawnFigureClip.AddEvent(animEvent);
        
        anim.AddClip(moveDownClip, "MoveDown");
        anim.AddClip(moveUpClip, "MoveUp");
        anim.AddClip(moveLeftClip, "MoveLeft");
        anim.AddClip(moveRightClip, "MoveRight");
        anim.AddClip(spawnFigureClip, "SpawnFigure");
    }

    public void MoveDown()
    {
        anim.PlayQueued("MoveDown");
        IsAnimated = true;
    }

    public void MoveUp()
    {
        anim.PlayQueued("MoveUp");
        IsAnimated = true;
    }

    public void MoveLeft()
    {
        anim.PlayQueued("MoveLeft");
        IsAnimated = true;
    }

    public void MoveRight()
    {
        anim.PlayQueued("MoveRight");
        IsAnimated = true;
    }

    public void SpawnFigure()
    {
        anim.PlayQueued("SpawnFigure");
        IsAnimated = true;
    }

    private void OnAnimationEnd()
    {
        IsAnimated = false;
    }

    public void SetType()
    {
        int type = FigureManager.instance.GetRandomFigure();
        SetType(type);
    }

    public void SetType(int type)
    {
        Type = type;
        sr.sprite = FigureManager.instance.GetFigure(type);
    }

    public void SetEmpty()
    {
        Type = -1;
        sr.sprite = null;
    }

    public bool IsEmpty()
    {
        return Type == -1;
    } 

    #region Operators

    public static bool operator ==(Figure fig1, Figure fig2)
    {
        return fig1.Equals(fig2);
    }

    public static bool operator !=(Figure fig1, Figure fig2)
    {
        return !fig1.Equals(fig2);
    }

    public override bool Equals(object obj)
    {
        return Type == ((Figure)obj).Type;
    }
    public override int GetHashCode()
    {
        return 0;
    }

    #endregion
}
