using UnityEngine;
using System.Collections;

namespace Astrodillos{
	public class ScreenWrap : MonoBehaviour {

		public bool hasWrapped { get; private set; }
		Vector2 position;
		Vector2 screenToWorldMax;
		Vector2 screenToWorldMin;
		// Use this for initialization
		void Start()
		{
			hasWrapped = false;
			screenToWorldMax = Camera.main.ViewportToWorldPoint(new Vector2(1.01f, 1.01f));
			screenToWorldMin = Camera.main.ViewportToWorldPoint(new Vector2(-0.01f, -0.01f));
		}

		// Update is called once per frame
		void Update()
		{

			position = gameObject.transform.position;
			//Vector2 scale = transform.localScale; Variable never used
			if (position.x > screenToWorldMax.x)
			{
				Wrap (new Vector2 (screenToWorldMin.x, position.y));
			}
			if (position.x < screenToWorldMin.x)
			{
				Wrap (new Vector2 (screenToWorldMax.x, position.y));
			}
			if (position.y > screenToWorldMax.y)
			{
				Wrap (new Vector2 (position.x, screenToWorldMin.y));
			}
			if (position.y < screenToWorldMin.y)
			{
				Wrap (new Vector2 (position.x, screenToWorldMax.y));
			}
		}

		void Wrap(Vector2 newPos){
			gameObject.transform.position = newPos;
			hasWrapped = true;
		}
	}
}