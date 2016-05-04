/// <summary>
/// Oversees the construction and storage of each player's barricade.
/// Implemented by Sam Endean 28/03/2016
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Armatillery
{
	public class Barricade : MonoBehaviour
	{
		public BarricadeBag LDL, RDL, LUL, RUL, LL, RL, UL, DL, LDU, RDU, LUU, RUU, LU, RU, UU, DU;

        public ParticleSystem buildPS;

		public float timePerBag = 0.5f,
		currentBagTime;

		public ArmaPlayer player;

		public bool complete;

		[SerializeField]
		private float bagHealth;

		private BarricadeBag[] bags;

		private BarricadeBag currentBag;

		private void Start ()
		{
			bags = new BarricadeBag[16] {RUL, RUU, RDL, RDU, LDL, LDU, LUL, LUU, RL, RU, DL, DU, LL, LU, UL, UU};

			currentBagTime = 0f;
			currentBag = null;

			foreach (BarricadeBag _bag in bags)
			{
				_bag.SetHealth(bagHealth);
				_bag.SetMaxHealth(bagHealth);
				_bag.SetBarricade(this);
			}
		}

		private void LateUpdate()
		{
			//flip the barricade to compensate
			if (player.shootDir.x < 0)
			{
				transform.localScale = new Vector3(-0.5f, transform.localScale.y, transform.localScale.z);
			}
			else
			{
				transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
			}

		}

		/// <summary>
		/// Constructs the barricade by lerping the alpha of the current bag, once done, move on.
		/// </summary>
		/// <returns><c>true</c>, if barricade has completed construction, <c>false</c> otherwise.</returns>
		public void ConstructBarricade ()
		{
            var fu = buildPS.emission;
            fu.enabled = true;

            //if all of the bags are complete, tell the calling object not to progress
            if (BarricadeComplete())
			{
				complete = true;
				return;
			}

			//find the next bag if not currently forming one
			if (currentBag == null)
			{
				currentBag = ThisBag ();
				if (currentBag == null)
				{
					return;
				}

				currentBag.gameObject.SetActive(true);
			}
				
			//now alter the alpha value
			currentBagTime += timePerBag/10;

			Color currentColour = currentBag.GetSpriteRenderer().color;

			float tempAlpha = Mathf.Lerp(currentColour.a, 1.0f, currentBagTime/timePerBag);

			currentBag.SetSpriteRendererColour(SetColor(tempAlpha, currentColour));

			//if the current bag is complete, allow it to move along to the next
			if (currentBag.GetSpriteRenderer().color.a == 1.0f)
			{
				//if the completed barricade was a top one, disable the lower one's collider volume
				TestCompleted();

				currentBag.SetHealth(currentBag.GetMaxHealth());

				currentBagTime = 0f;
				currentBag = null;
			}
		}

		public void DestoryBag (BarricadeBag _bag)
		{
			//test to see if top bag, if so reactivate bottom bag's collider
			if (_bag.GetUpperBag())
			{
				_bag.GetLowerBag().SetPolyGonColliderEnabled(true);
			}

			_bag.gameObject.SetActive(false);
		}

		/// <summary>
		/// Returns build progress as a percentage, should only be called while being built.
		/// </summary>
		/// <returns>The percentage built.</returns>
		public float BuildPercentage()
		{
			float total = 0;

			//add the build progress of all bags
			foreach (BarricadeBag _bag in bags)
			{
				total += _bag.GetSpriteRenderer().color.a;
			}

			//divide by the total number of bags and multiply by 100 to get a percentage
			total /= 16;

			return (total *= 100);
		}

		/// <summary>
		/// Returns the health as a percentage, should be called when damaged to update slider2
		/// </summary>
		/// <returns>The health as a percentage.</returns>
		public float HealthPercentage()
		{
			float total = 0;

			foreach(BarricadeBag _bag in bags)
			{
				total += (_bag.GetHealth() / _bag.GetMaxHealth());
			}

			total /= 16;

			return (total *= 100);
		}

		private void TestCompleted()
		{
			if (currentBag == LDU)
			{
				LDL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == RDU)
			{
				RDL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == LUU)
			{
				LUL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == RUU)
			{
				RUL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == RU)
			{
				RL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == LU)
			{
				LL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == UU)
			{
				UL.SetPolyGonColliderEnabled(false);
			}
			else if (currentBag == DU)
			{
				DL.SetPolyGonColliderEnabled(false);
			}
		}

		private Color SetColor (float _alpha, Color _colour)
		{
			return new Color(_colour.r, _colour.g, _colour.b, _alpha);
		}

		private bool BarricadeComplete()
        {
            for (int i = 0; i < 16; i++)
			{
				//if that bag is not active, the full barricade is not complete
				if (!bags [i].gameObject.activeSelf)
				{
					return false;
				}
				else if (bags[i].GetSpriteRenderer().color.a != 0.0f)
				{
					return false;
				}
			}
			//if this is reached, all of the barricade is compelete
			return true;
		}

		private bool BarricadeBegun()
		{
			for (int i = 0; i < 16; i++)
			{
				if (bags[i].gameObject.activeSelf)
				{
					return true;
				}
			}
			//if this is reached, then the barricade has not been started
			return false;
		}

		public void CancelBuilding()
        {
            //Debug.Log("false");
            var fu = buildPS.emission;
            fu.enabled = false;
            
            if (currentBag != null)
			{
				Color currentColour = currentBag.GetSpriteRenderer().color;
				currentBag.SetSpriteRendererColour(SetColor(0f, currentColour));

				currentBag.gameObject.SetActive(false);
				currentBag = null;
			}
		}

		public void RemoveBarricade()
		{
			for (int i = 0; i < 16; i++)
			{
				if (bags[i].gameObject.activeSelf)
				{
					Color currentColour = bags[i].GetSpriteRenderer().color;
					bags[i].SetSpriteRendererColour(SetColor(0f, currentColour));

					bags[i].SetPolyGonColliderEnabled(true);
					bags[i].SetHealth(0.0f);
					bags[i].gameObject.SetActive(false);
				}
			}
							
			currentBagTime = 0f;
			currentBag = null;
			complete = false;
		}

		/// <summary>
		/// If building in progress the next bag is found, else the one closest to the facing is done
		/// </summary>
		/// <returns>The bag.</returns>
		private BarricadeBag ThisBag()
		{
			//if it hasnt started, get the closest to facing
			if (!BarricadeBegun())
			{
				foreach (BarricadeBag _bag in bags)
				{
					_bag.SetHealth(0.0f);
				}

				return bags[0];
			}

			for (int i = 0; i < 16; i++)
			{
				if (!bags [i].gameObject.activeSelf)
				{
					return bags[i];
				}
			}

			return null;
		}
	}
}