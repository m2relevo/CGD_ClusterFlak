using UnityEngine;
using System.Collections;
using ArmadHeroes;

public class TestControllerMovement : MonoBehaviour
{

    public float speed;

    //Bullet stuff
    public GameObject StandardBullet;
    public GameObject ProjectileSpawnLoc;



	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Movement by left stick and aim by right stick
        float Movex = ControllerManager.instance.GetController(0).moveX.GetValue();
        float Movey = ControllerManager.instance.GetController(0).moveY.GetValue();

        float Rotx = ControllerManager.instance.GetController(0).aimX.GetValue();
        float Roty = ControllerManager.instance.GetController(0).aimY.GetValue();

        Vector2 direction = new Vector2(Movex, Movey).normalized;
        Quaternion aim = new Quaternion(Rotx, Roty, 0f, 0f);

        Move(direction);
        Rotate(aim);

        if (ControllerManager.instance.GetController(0).shootButton.JustPressed())
        {
            Firestandard();
        }
    }

    void Move(Vector2 direction)
    {

        Vector2 pos = transform.position;

        pos += direction * speed * Time.deltaTime;

        transform.position = pos;
    }

    void Rotate(Quaternion aim)
    {

        Quaternion Rotation = transform.rotation;

        Rotation = aim;

        transform.rotation = Rotation;
    }

    void Firestandard()
    {
        GameObject enemybullet = (GameObject)Instantiate(StandardBullet);
        enemybullet.transform.parent = ProjectileSpawnLoc.transform;
        enemybullet.transform.position = ProjectileSpawnLoc.transform.position;
        enemybullet.transform.localPosition = new Vector2(0, 0);
        enemybullet.transform.localRotation = Quaternion.Euler(0, 0, 0);
        enemybullet.transform.parent = null;

    }
}
