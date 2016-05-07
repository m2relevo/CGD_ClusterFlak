using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMachineGunTurretControl : MonoBehaviour
{//DEPRECATED
    private GameObject Target;
    public List<GameObject> ListTarget;
    public GameObject bullet;
    public GameObject location;

    // Use this for initialization
    void Start()
    {
        ListTarget.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
        int RNG = Random.Range(0, ListTarget.Count);
        

        GameObject Target = (GameObject)ListTarget[RNG];
        transform.right = Target.transform.position - transform.position;
        firepro();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }


    void firepro()
    {
        for (int i = 0; i < 5; i++)
        {
            int RNG = Random.Range(-1, 1);

            GameObject enemybullet = (GameObject)Instantiate(bullet);
            enemybullet.transform.parent = location.transform;
            enemybullet.transform.position = location.transform.position;
            enemybullet.transform.localPosition = new Vector2(0 + i, 0);
            enemybullet.transform.localRotation = Quaternion.Euler(0 + RNG, 0, 0);
            enemybullet.transform.parent = null;
        }
        Destroy(gameObject);
    }
}