using UnityEngine;
using System.Collections;

public class BossMachineGunTurret : MonoBehaviour
{
    private GameObject Target;
    public GameObject bullet;
    public GameObject location;

    // Use this for initialization
    void Start()
    {
        GameObject Target = GameObject.FindGameObjectWithTag("ClusterFlak/Player");
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
            GameObject enemybullet = (GameObject)Instantiate(bullet);
            enemybullet.transform.parent = location.transform;
            enemybullet.transform.position = location.transform.position;
            enemybullet.transform.localPosition = new Vector2(0 + i, 0);
            enemybullet.transform.localRotation = Quaternion.Euler(0 + i, 0, 0);
            enemybullet.transform.parent = null;
        }
        Destroy(gameObject);
    }
}