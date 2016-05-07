using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PS4CrossFade : MonoBehaviour {

    public SpriteRenderer cross;
    public float flashTime = 1.0f;
    public float minAlpha = 0.0f;

    Sequence fadeSequence;

	// Use this for initialization
	void Start ()
    {
        FadeOut();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FadeOut()
    {
        fadeSequence = DOTween.Sequence();

        Color endColor = cross.color;
        endColor.a = minAlpha;
        fadeSequence.Append(cross.DOColor(endColor, flashTime * 0.5f));

        fadeSequence.SetEase(Ease.InOutSine);
        fadeSequence.AppendCallback(FadeIn);
    }

    void FadeIn()
    {
        fadeSequence = DOTween.Sequence();

        Color endColor = cross.color;
        endColor.a = 1;
        fadeSequence.Append(cross.DOColor(endColor, flashTime * 0.5f));

        fadeSequence.SetEase(Ease.InOutSine);
        fadeSequence.AppendCallback(FadeOut);
    }
}
