using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class Paracrate : MonoBehaviour
    {

        public GameObject paracrateObj;
        public float flybySpeed;
        public bool ammoDropped;
        Animator mAnimator;
        public int mOrientation;

        public float minTime = 8.0f;
        public float maxTime = 10.0f;
        public float minDropTime = 5.0f;
        public float maxDropTime = 15.0f;
        public bool isHeliSpawning = false;
        private float time;
        private float dropTime;

        // Use this for initialization
        void Start()
        {
            SetRandomDrop();
            time = minDropTime;
            ammoDropped = false;
            mAnimator = GetComponent<Animator>();
      
        }

        // Update is called once per frame
        void Update()
        {
            MoveHelicopter();
            time += Time.deltaTime;
            if (time >= dropTime)
            {
                AmmoDrop();
                SetRandomDrop();
            }

            if (!isHeliSpawning)
            {
                isHeliSpawning = true;  
                StartCoroutine(HeliSpawner(Random.Range(minTime, maxTime))); //the helicopter will now spawn in a random range between 5 and 9 secs
                transform.position = new Vector3(Random.Range(25f,35f), Random.Range(-5f, 8f), 1f); //randomize the spawning position of the helicopter based on the map
                //transform.Translate(-Vector2.right * flybySpeed * Time.deltaTime);
            }
         
        }

        public void AmmoDrop()
        {
            transform.Translate(-Vector2.right * flybySpeed/3f * Time.deltaTime);
            time = 0;
                Instantiate(paracrateObj, transform.position, Quaternion.identity);
                ammoDropped = false;
        }

        void MoveHelicopter()
        {
            transform.Translate(Vector2.right * flybySpeed * Time.deltaTime);
            mAnimator.SetInteger("Orientation",mOrientation);
        }

        void SetRandomDrop()
        {
            dropTime = Random.Range(minDropTime, maxDropTime);
        }

        IEnumerator HeliSpawner(float seconds)
        {
            //tempDropCoordinates = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            yield return new WaitForSeconds(5.0f);
            isHeliSpawning = false;
            
        }
    }
}