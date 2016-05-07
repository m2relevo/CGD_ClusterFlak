using UnityEngine;
using System.Collections;

public class sumo_explosionKiller : MonoBehaviour {

	AnimatorStateInfo animStateInfo;
	Animator animator;

	public AudioSource explosionSoundSource;

	// Use this for initialization
	void Start () {
		animator = transform.GetComponent<Animator> ();
		
		transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("explosionAnim");
		for(int i = 0 ; i < ArmadHeroes.SumoManager.instance.sumoSoundSourceList.Count; i++)
		{
			ArmadHeroes.SoundManager.instance.FadeAndKillAudio (explosionSoundSource, 0.01f);
		}
		StartCoroutine (destroyExplosion());

	}
	
	// Update is called once per frame
	void Update () {


	
	}


	IEnumerator destroyExplosion()
	{
		//WaitForSeconds(GetComponent<Animation>()
		yield return new WaitForSeconds(0.39f);
		Destroy (this.gameObject);
	}
}
