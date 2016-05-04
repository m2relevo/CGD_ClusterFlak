using UnityEngine;
using System.Collections;

public class WeaponDropSpawner : MonoBehaviour {

	public static WeaponDropSpawner instance;

    //public attributes
    public float SpawnEverySecondsMin;
    public float SpawnEverySecondsMax;

    // Access this from other classes to turn off random weapon spawning
    public bool RandomSpawning;

    public GameObject WeaponDropPrefab;

    // class attributes
    private float SpawnEverySeconds;
    private float timeToSpawn;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start ()
    {
        // Set initial spawn time to something at the beggining
        SpawnEverySeconds = Random.Range(SpawnEverySecondsMin, SpawnEverySecondsMax);
    }
	
	// Update is called once per frame
	void Update ()
    {

        // If random spawning turned on (only ground)
        if (RandomSpawning)
        {
            if (timeToSpawn >= SpawnEverySeconds)
            {
				Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-6.0f, 6.0f), -1.0f);
                // Spawn the next weapon drop
				SpawnDrop(position);
                // Reset timer
                timeToSpawn = 0.0f;
                // Set next spawn time randomly from min and max set in scene
                SpawnEverySeconds = Random.Range(SpawnEverySecondsMin, SpawnEverySecondsMax);
            }
            // Increment timer
            timeToSpawn += Time.deltaTime;
        }
	}

    // Spawn drop in scene
    public void SpawnDrop(Vector3 dropPosition)
    {
        
        GameObject newDrop = Instantiate(WeaponDropPrefab);
		newDrop.transform.position = dropPosition;


		newDrop.transform.SetParent (gameObject.transform);
    }
}
