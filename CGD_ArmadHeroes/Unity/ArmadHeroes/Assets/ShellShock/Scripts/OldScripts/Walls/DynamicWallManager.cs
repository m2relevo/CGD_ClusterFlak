using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class DynamicWallManager : MonoBehaviour
    {

        public GameObject[] dynamicWallsList;
        public float repeatTime = 2.5f;
        public float actualTime;
        public int wallGroup;
        public int maxWallGroups = 5;
        public AudioSource movingCoverSound;

        [SerializeField] bool isDilloDash = false;

        // Use this for initialization
        void Start()
        {
             dynamicWallsList = GameObject.FindGameObjectsWithTag("Dynamic");
           
            if(!isDilloDash)
                 movingCoverSound = gameObject.GetComponent<AudioSource>();

            actualTime = repeatTime;    
        }

        // Update is called once per frame
        void Update()
        {
            actualTime -= Time.deltaTime;
            if (actualTime <= 0.0f)
            {
                RandomWallDrop();
            }


        }

        public void RaiseAllWalls()
        {
            // finding every wall, every time you want to do something with them is stupid and inefficient.
            // dynamicWallsList = GameObject.FindGameObjectsWithTag("Dynamic");
            foreach (GameObject dynamicWall in dynamicWallsList)
            {
                dynamicWall.GetComponent<ShellShock.DynamicWallScript>().WallsUp();
            }
            if (!isDilloDash)
                movingCoverSound.Play();
        }

        public void DropAllWalls()
        {
           // dynamicWallsList = GameObject.FindGameObjectsWithTag("Dynamic");
            foreach (GameObject dynamicWall in dynamicWallsList)
            {
                dynamicWall.GetComponent<ShellShock.DynamicWallScript>().WallsDown();
            }
            if (!isDilloDash)
                movingCoverSound.Play();
        }

        public void RandomWallDrop()
        {
            
            //dynamicWallsList = GameObject.FindGameObjectsWithTag("Dynamic");
            wallGroup = Random.Range(0, maxWallGroups);
            foreach (GameObject dynamicWall in dynamicWallsList)
            {
                dynamicWall.GetComponent<ShellShock.DynamicWallScript>().WallChange(wallGroup);
            }
            if(!isDilloDash)
                movingCoverSound.Play();
            actualTime = repeatTime;
        }
        public void CheckDynamicWalls()
        {
            dynamicWallsList = GameObject.FindGameObjectsWithTag("Dynamic");
        }
    }
}
