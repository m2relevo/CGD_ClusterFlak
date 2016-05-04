/// <summary>
/// Barricade bag component allows the bags to die instead of just reducing damage
/// Created and implemented by Sam Endean - 28/04/16
/// </summary>

using UnityEngine;
using System.Collections;
namespace Armatillery
{
	public class BarricadeBag : MonoBehaviour
	{
		//the health and maxhealth of this bag
		private float health, maxHealth; 

		//the spriterenderer of this bag
		private SpriteRenderer sRenderer;

		//the polygon colider of this bag
		private PolygonCollider2D polyCollider;

		//the barricade this belongs to
		private Armatillery.Barricade barricade;

		//whether or not this is an upper bag
		[SerializeField]
		private bool upperBag;
		//only upper bags have lowerBags
		[SerializeField] 
		private BarricadeBag lowerBag;

		public SpriteRenderer GetSpriteRenderer ()
		{
			return sRenderer;
		}
		public void SetSpriteRendererColour(Color _newColour)
		{
			sRenderer.color = _newColour;
		}

		public void SetBarricade(Armatillery.Barricade _newBarricade)
		{
			barricade = _newBarricade;
		}

		public void SetPolyGonColliderEnabled(bool _enabled)
		{
			polyCollider.enabled = _enabled;
		}

		public float GetHealth()
		{
			return health;
		}
		public void SetHealth(float _newHealth)
		{
			health = _newHealth;
		}

		public float GetMaxHealth()
		{
			return maxHealth;
		}
		public void SetMaxHealth(float _newMaxHealth)
		{
			maxHealth = _newMaxHealth;
		}

		public bool GetUpperBag()
		{
			return upperBag;
		}

		public BarricadeBag GetLowerBag()
		{
			return lowerBag;
		}

		private void Awake()
		{
			sRenderer = GetComponent<SpriteRenderer>();
			polyCollider = GetComponent<PolygonCollider2D>();

			gameObject.SetActive(false);
		}

		/// <summary>
		/// Handles logic for taking damage.
		/// </summary>
		public void TakeDamage(float _damage)
		{
			health -= _damage;

			barricade.player.m_UI.UpdateSliderOne(barricade.HealthPercentage());

			if (health <= 0.0f)
			{
				health = 0.0f;

				barricade.DestoryBag(this);
			}
		}
	}
}