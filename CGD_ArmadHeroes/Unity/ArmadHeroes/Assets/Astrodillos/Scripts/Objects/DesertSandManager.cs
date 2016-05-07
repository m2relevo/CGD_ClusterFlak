using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Astrodillos
{
    public class DesertSandManager : MonoBehaviour
    {
        void Update()
        {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
	        ResetPos();
	        MoveDustCloud();
        }
        void GetPosition()
        {
            float yPos = Random.value;
            Vector2 fromScreen = Camera.main.ViewportToWorldPoint(new Vector2(0, yPos));
           	transform.position = new Vector2(-transform.position.x, fromScreen.y);

        }
        void MoveDustCloud()
        {
            {
                transform.Translate(10 * Time.deltaTime, 0, 0);
                ResetPos();
            }
        }
        void ResetPos()
        {
            if (Camera.main.WorldToViewportPoint(transform.position).x > 1f)
            {
               GetPosition();
            }
        }
    }
}