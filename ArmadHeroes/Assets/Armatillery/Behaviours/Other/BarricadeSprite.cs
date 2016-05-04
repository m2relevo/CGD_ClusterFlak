
using UnityEngine;
using System.Collections;

namespace Armatillery
{
	[ExecuteInEditMode]
	public class BarricadeSprite : MonoBehaviour
	{
		PolygonCollider2D hitBox, spriteBox;
		public float Base, extendXY, height;
		public Vector2 colliderOffset;
		public Vector2 origin=Vector2.one;
		public ArmaPlayer player;
		public Barricade barricade;

		private BarricadeBag barriBag; 

		float newOpacity = 1;
		SpriteRenderer sr;

		void Start()
		{
			barriBag = GetComponent<BarricadeBag>();
			sr = barriBag.GetSpriteRenderer();
		}

		void Update()
		{
			if (sr != null)
			{
				if (sr.color.a != newOpacity)
				{
					sr.color = new Color(1, 1, 1, Mathf.Lerp(sr.color.a, newOpacity, Time.deltaTime * 10f));
				}
			}

			if (origin == Vector2.one)
			{
				if (!sr)
				{
					origin.y = -GetComponentInParent<SpriteRenderer>().sprite.bounds.extents.y;
					origin.x = 0;// -GetComponentInParent<SpriteRenderer>().sprite.bounds.extents.x/2;
				}
				else
				{
					origin.y = -GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
					origin.x = 0;//-GetComponent<SpriteRenderer>().sprite.bounds.extents.x/2;
				}
			}

			if (!GetComponent<PolygonCollider2D>())
				gameObject.AddComponent<PolygonCollider2D>();
			else if (hitBox == null)
				hitBox = GetComponent<PolygonCollider2D>();
			else
			{
				if (height <= 0)
				{
					Vector2[] hitBoxPoints =
					{
						new Vector2(origin.x,origin.y),
						new Vector2(origin.x+Base,origin.y+(Base/2)),
						new Vector2(origin.x,origin.y+Base),
						new Vector2(origin.x-Base,origin.y+(Base/2)),
					};

					//Depth
					if (extendXY > 0)
					{
						hitBoxPoints[1] += new Vector2(extendXY, extendXY / 2);
						hitBoxPoints[2] += new Vector2(extendXY, extendXY / 2);
					}
					else if (extendXY < 0)
					{
						hitBoxPoints[3] += new Vector2(extendXY, -extendXY / 2);
						hitBoxPoints[2] += new Vector2(extendXY, -extendXY / 2);
					}
					hitBox.points = hitBoxPoints;
				}
				else
				{
					Vector2[] hitBoxPoints =
					{
						new Vector2(origin.x,origin.y),
						new Vector2(origin.x+Base,origin.y+(Base/2)),
						new Vector2(origin.x+Base,origin.y+(Base/2)+height),
						new Vector2(origin.x,origin.y+Base+height),
						new Vector2(origin.x-Base,origin.y+(Base/2)+height),
						new Vector2(origin.x-Base,origin.y+(Base/2)),
					};

					//Depth
					if (extendXY > 0)
					{
						hitBoxPoints[1] += new Vector2(extendXY, extendXY / 2);
						hitBoxPoints[2] += new Vector2(extendXY, extendXY / 2);
						hitBoxPoints[3] += new Vector2(extendXY, extendXY / 2);
					}
					else if (extendXY < 0)
					{
						hitBoxPoints[3] += new Vector2(extendXY, -extendXY / 2);
						hitBoxPoints[4] += new Vector2(extendXY, -extendXY / 2);
						hitBoxPoints[5] += new Vector2(extendXY, -extendXY / 2);
					}
					hitBox.points = hitBoxPoints;
				}
					
				hitBox.offset = colliderOffset;
			}
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			//check if col is a projectile
			if (col.gameObject.GetComponent<ProjectileBase>() && player.health > 0)
			{
				ProjectileBase _projectile = col.gameObject.GetComponent<ProjectileBase>();
				//make sure owner of projectile is not the same as the ActorType
				if (_projectile.type != player.type)
				{
					barriBag.TakeDamage(_projectile.damage);

					GameObject explosion = ExplosionManager.instance.GetExplosion();
					explosion.transform.position = this.transform.position;
					explosion.SetActive(true);
					explosion.GetComponent<Explosion>().InitExplode(1f);

					col.gameObject.SetActive(false);
				}
			}


		}
	}
}