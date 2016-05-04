using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FlashText : MonoBehaviour {

	public Text text;
	public Outline outline;
	public float flashTime = 1.0f;
	public float minAlpha = 0;

	Sequence fadeSequence;

	// Use this for initialization
	void Start () {
		FadeOut ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FadeOut(){
		fadeSequence = DOTween.Sequence ();

		Color endColor = text.color;
		endColor.a = minAlpha;
		fadeSequence.Append (text.DOColor (endColor, flashTime*0.5f));

		if (outline) {
			endColor = outline.effectColor;
			endColor.a = minAlpha;
			fadeSequence.Insert (0, (outline.DOColor (endColor, flashTime*0.5f)));
		}

		fadeSequence.SetEase (Ease.InOutSine);
		fadeSequence.AppendCallback (FadeIn);
	}

	void FadeIn(){

		fadeSequence = DOTween.Sequence ();

		Color endColor = text.color;
		endColor.a = 1;
		fadeSequence.Append (text.DOColor (endColor, flashTime*0.5f));

		if (outline) {
			endColor = outline.effectColor;
			endColor.a = 1;
			fadeSequence.Insert (0, (outline.DOColor (endColor, flashTime*0.5f)));
		}
		fadeSequence.SetEase (Ease.InOutSine);
		fadeSequence.AppendCallback (FadeOut);
	}
}
