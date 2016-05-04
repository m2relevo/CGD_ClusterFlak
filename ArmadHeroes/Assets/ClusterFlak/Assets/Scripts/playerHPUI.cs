using UnityEngine;
using System.Collections;

public class playerHPUI : MonoBehaviour
{
    public float Playerhealth;

    void Start()
    {
    }

    void Update()
    { }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "ClusterFlak/EnemyBullet")
        {
            Playerhealth -= 1;
            transform.localScale -= new Vector3(0.05f, 0F, 0F);
            Debug.Log("Health UI decreased");

            if (Playerhealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
