/// <summary>
/// Created and implemented by Sam Endean on 02/03/2016
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using DG.Tweening;
using ArmadHeroes;

namespace Armatillery
{
	public class ArmaDropShip : MonoBehaviour
	{
		#region Public Members
		[System.NonSerialized]
		public Vector3 m_spawnPos = Vector3.zero;
		public GameObject m_dropShipSprite;
		public float maxForce = 5f;
		#endregion

		#region Private Memebers
		private enum ArmaDropShipStates {deliverTroop, returnTrip};
		private ArmaDropShipStates m_state;
		private Vector3 targetPos,
		acceleration, velocity;
		#endregion

		#region Unity callbacks
		private void Start()
		{
			Init ();
		}

		protected virtual void Update()
		{
            switch (GameManager.instance.state)
            {
                case ArmadHeroes.GameStates.game:
                    Tick();
                    break;
                case ArmadHeroes.GameStates.pause:
                    break;
                case ArmadHeroes.GameStates.gameover:
                    break;
                default:
                    break;
            }
		}
		#endregion

		#region 'Custom' callbacks
		public void Init()
		{
			WorldTile spawnTile;
			//set self to a random world tile out of view for the pos
			do
			{
				//select a random drop point
				spawnTile = WorldGenerator.instance.m_world [Random.Range(0, WorldGenerator.instance.m_mapWidth - 1), Random.Range(0, WorldGenerator.instance.m_mapHeight - 1)];

			} while (spawnTile.m_worldTileObject.GetComponent<SpriteRenderer>().isVisible);

			transform.position = spawnTile.m_worldTileObject.transform.position;


			//set initial player state
			m_state = ArmaDropShipStates.deliverTroop;

			WorldTile dropPoint;

			//generate a new drop point until one is generated which is not on a path or otherwise blocked
			bool tileBlocked;
			do
			{
				tileBlocked = false;

				//select a random drop point
				dropPoint = WorldGenerator.instance.m_world [Random.Range(0, WorldGenerator.instance.m_mapWidth - 1), Random.Range(0, WorldGenerator.instance.m_mapHeight - 1)];

				foreach (List<WorldTile> _path in WorldGenerator.instance.allPaths)
				{
					if (_path.Contains(dropPoint))
					{
						tileBlocked = true;
					}
				}

			} while (tileBlocked && dropPoint.m_worldTileObject.GetComponent<SpriteRenderer>().isVisible);

			Vector3 offset = new Vector3 (0, 0.15f, 0);

			targetPos = dropPoint.m_worldTileObject.transform.position + offset;

			//m_armaAnimator = this.GetComponent<Animator> ();
		}
		/// <summary>
		/// Controlled update for objects, 
		/// Tick is called in the update 
		/// pending on the current game state
		/// </summary>
		protected void Tick()
		{
			switch (m_state)
			{
			case ArmaDropShipStates.deliverTroop:
				MoveToDrop ();
				break;
			case ArmaDropShipStates.returnTrip:
				LeaveArea ();
				break;
			default:
				break;
			}
		}
		#endregion

		#region Enemy behaviours
		/// <summary>
		/// Moves to the drop zone (pulled by force)
		/// </summary>
		private void MoveToDrop ()
		{
			Move ();

			if (Vector3.Distance (transform.position, targetPos) < 0.625f)
			{
				Vector3 dropSpawnPos = targetPos;
				dropSpawnPos.y += m_dropShipSprite.transform.localPosition.y - transform.position.y;

				Vector3 offset = new Vector3 (0, 0.15f, 0);

				//request the manager spawns a paratrooper above this pos to drop down
				EnemyManager.instance.RequestSpawn(dropSpawnPos, targetPos + offset, true);

				m_state = ArmaDropShipStates.returnTrip;
			}
		}

		private void LeaveArea ()
		{
			Move ();

			if (!GetComponent<SpriteRenderer> ().isVisible)
			{
				gameObject.SetActive (false);
			}
		}

		private void Move()
		{
			acceleration += 1f * (targetPos - transform.position).normalized * Time.deltaTime;

			velocity += acceleration * Time.deltaTime;

			transform.position += velocity * Time.deltaTime;
		}

		#endregion
	}
}