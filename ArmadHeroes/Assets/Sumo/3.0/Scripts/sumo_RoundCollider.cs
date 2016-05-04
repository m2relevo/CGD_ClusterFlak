using UnityEngine;
using System.Collections;

public class sumo_RoundCollider : MonoBehaviour
{
    public GameObject ringSprite;
	public bool run;

    void Awake()
    {
        ringSprite = GameObject.Find("ringSprite");
        CircleCollider2D arenaCollider = (CircleCollider2D)ringSprite.gameObject.AddComponent(typeof(CircleCollider2D));
        arenaCollider.radius = 0.420f;
        arenaCollider.isTrigger = true;
		if (run) 
		{
			// StartCoroutine (ScaleOverTime (1));
		}
    }

    void Update()
    {

    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = ringSprite.transform.localScale;
        Vector3 destinationScale = new Vector3(2.5f, 2.5f, 0f);

        float currentTime = 0.0f;

        while (currentTime < 60f)
        {
            ringSprite.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime += 0.00030f);        
            yield return null;
        }    
        
        Destroy(gameObject);
    }
}
