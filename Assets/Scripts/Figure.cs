using UnityEngine;
using System.Collections;

public class Figure : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animation anim;

    public bool IsAnimated { get; private set; }

    public int Type { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animation>();

        sr.sortingLayerName = "Figure";

        CreateAnimations();
    }

    private void CreateAnimations()
    {
        float gap = transform.parent.parent.parent.GetComponent<Field>().GetGap();
        float offset = 1.0f + gap / transform.parent.localScale.x;
        float duration = 1.0f;

        // Animation of moving figure down
        AnimationCurve moveDownCurve = AnimationCurve.Linear(0.0f, offset, duration, 0);
        AnimationClip moveDownClip = new AnimationClip();
        moveDownClip.legacy = true;
        moveDownClip.SetCurve("", typeof(Transform), "localPosition.y", moveDownCurve);

        // End of animation event
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.functionName = "OnAnimationEnd";
        animEvent.time = moveDownClip.length;
        moveDownClip.AddEvent(animEvent);
        
        anim.AddClip(moveDownClip, "MoveDown");
    }

    /*public void MoveDown()
    {
        anim.Play("MoveDown");
    }*/

    public void MoveDown()
    {
        anim.Play("MoveDown");
        IsAnimated = true;
        //StartCoroutine(OnAnimationEnd(anim.GetClip("MoveDown").length));
    }

    private void OnAnimationEnd()
    {
        IsAnimated = false;
    }

    /*private IEnumerator OnAnimationEnd(float time)
    {
        yield return new WaitForSeconds(time);

        IsAnimated = false;
    }*/

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
