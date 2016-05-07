using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RandomSceneLoaderHelper : MonoBehaviour {

    public string[] AvailableScenes;
    
	void Awake () {
        SceneManager.LoadScene(AvailableScenes[Random.Range(0, AvailableScenes.Length)]);
	}
}