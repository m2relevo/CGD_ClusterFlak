using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ArmadHeroes;

namespace Astrodillos{
	public class IcbmManager : MonoBehaviour {

		public GameObject icbmPrefab;

		List<GameObject> icbms = new List<GameObject>();
		private bool useGravity {
			set{ }
			get { return Gametype_Astrodillos.instance.UseGravity(); }
		}

		float spawnTime = 0;

		#region Unity Behaviours
		// Use this for initialization
		void Start () {
			SetRandomSpawnTime ();
			spawnTime = 1;//
		}
		
		// Update is called once per frame
		void Update () {
			if (GameManager.instance.state == GameStates.pause) {
				return;
			}
			if(spawnTime>0){
				spawnTime -= Time.deltaTime;
				if(spawnTime<=0){
					SpawnIcbm();
					SetRandomSpawnTime ();

				}
			}
		}
		#endregion

		void SpawnIcbm(){
			GameObject icbmGo = GetPooledIcbm ();
			FlyingICBM icbm = icbmGo.GetComponent<FlyingICBM> ();

			Vector2 spawnPosition = new Vector2 ();

			//Top or right of screen
			if (RandomBool () || useGravity) {
				spawnPosition = new Vector2(Random.Range(-6,6), 8);
			} else {
				spawnPosition = new Vector2(13,Random.Range(-11,11));
			}

			//Flip for bottom/left
			if (RandomBool () && !useGravity) {
				spawnPosition *= -1;
			}


			//Random speed
			float speed = Random.Range (20.0f, 50.0f);
			//Spawn
			icbm.Spawn (spawnPosition, speed, RandomBool(), useGravity);


		}

		GameObject GetPooledIcbm(){
			for (int i = 0; i<icbms.Count; i++) {
				if(!icbms[i].activeSelf){
					icbms[i].SetActive(true);
					return icbms[i];
				}
			}
			GameObject icbm = Instantiate (icbmPrefab);
			
			icbm.transform.SetParent (gameObject.transform);
			icbms.Add (icbm);
			return icbms[icbms.Count-1];
		}

		void SetRandomSpawnTime(){
			spawnTime = useGravity ? Random.Range (5.0f, 15.0f) : Random.Range(3.0f, 8.0f);
		}

		bool RandomBool(){
			return Random.value > 0.5f;
		}

		public void RemoveAll(){
			for (int i = 0; i<icbms.Count; i++) {
				if (icbms [i].activeSelf) {
					icbms [i].GetComponent<FlyingICBM> ().Reset ();
					icbms[i].SetActive (false);
				}

			}
		}
	}
}
