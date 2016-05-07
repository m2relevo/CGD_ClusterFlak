using UnityEngine;
using System.Collections;

public class spawnMultiEnemy : MonoBehaviour {
    //creates multiple enemies spawning out of the apc
    public GameObject enemy1, enemy2, enemy3;
    public GameObject loc1, loc2, loc3;
    GameObject spawn1, spawn2, spawn3;
    Vector2 loc1V2, loc2V2, loc3V2;
    bool hasSpawned = false;
    public int spawnCount = 0;
    float timer = 2f;

    // Use this for initialization
    void Start() {
        hasSpawned = false;
    }

    // Update is called once per frame
    void Update() {//spawns enemies, and makes them move towards a node

        timer = timer -= Time.deltaTime;

        if (!hasSpawned)
        {
            spawnEnemies();
        }

        if (hasSpawned)
        {
            moveTowards();
        }
    }

    void spawnEnemies()
    {
        spawn1 = (GameObject)Instantiate(enemy1, transform.position, Quaternion.identity);
        spawn2 = (GameObject)Instantiate(enemy2, transform.position, Quaternion.identity);
        spawn3 = (GameObject)Instantiate(enemy3, transform.position, Quaternion.identity);
        spawnCount = 3;
        hasSpawned = true;
    }

    void moveTowards()
    {//makes each enemy move towards a specific node 
        if (timer > 0)
            {
        loc1V2 = loc1.transform.position;
        spawn1.transform.position = Vector2.MoveTowards(new Vector2(spawn1.transform.position.x, spawn1.transform.position.y), loc1V2, 5 * Time.deltaTime);

        loc2V2 = loc2.transform.position;
        spawn2.transform.position = Vector2.MoveTowards(new Vector2(spawn2.transform.position.x, spawn2.transform.position.y), loc2V2, 4 * Time.deltaTime);

        loc3V2 = loc3.transform.position;
        spawn3.transform.position = Vector2.MoveTowards(new Vector2(spawn3.transform.position.x, spawn3.transform.position.y), loc3V2, 5 * Time.deltaTime);
             }
    }
}
