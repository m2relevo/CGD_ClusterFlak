using UnityEngine;
using System.Collections.Generic;

namespace DilloDash
{

    struct vehicleStruct
    {
        public GameObject vehicle;
        public bool isGoingDown;
        public int lastNodeHit;
    }

    

    public class VehicleManager : MonoBehaviour
    {
        private static VehicleManager singleton;
        public static VehicleManager Singleton() { return singleton; }

        Animator m_animator;
        enum Directions { N, NE, E, SE, S, SW, W, NW }

        string[] downDir;
        string[] upDir;

        [SerializeField] GameObject vehiclePrefab;
        [SerializeField] GameObject[] nodesGoingDown;
        [SerializeField] GameObject[] nodesGoingUp;

        

        [SerializeField] float spawnDelay = 8f;
        [SerializeField] float vehicleSpeed = 50.0f;

        bool goingDown = true;
        public bool isVehiclesActive = false;

        GameObject vehicleParent;

        List<vehicleStruct> vehicles;

        // Use this for initialization
        void Start()
        {
            singleton = this;

            vehicles = new List<vehicleStruct>();

            InvokeRepeating("instantiateVehicle", 0.0f, spawnDelay );

            // the strings pre-declared for updating vehicle sprite
            downDir = new string[3];
            downDir[0] = "SErun";
            downDir[1] = "Srun";
            downDir[2] = "SErun";

            upDir = new string[3];
            upDir[0] = "NErun";
            upDir[1] = "Nrun";
            upDir[2] = "NErun";

        }

        public void setVehiclesActive(bool isActive)
        {
            isVehiclesActive = isActive;
        }

        // Update is called once per frame
        void Update()
        {
            if (isVehiclesActive)
            {
                if (vehicles.Count > 0)
                {
                    for (int i = 0; i < vehicles.Count; ++i)
                    {
                        vehicleStruct vStruct = vehicles[i];
                        GameObject vehicleGO = vStruct.vehicle;
                        GameObject[] nodes = vStruct.isGoingDown ? nodesGoingDown : nodesGoingUp;

                        // move the car towards the next node
                        vehicleGO.transform.position = Vector3.MoveTowards(vehicleGO.transform.position, nodes[vStruct.lastNodeHit + 1].transform.position, vehicleSpeed * Time.deltaTime);

                        // if close enough to the next node
                        if (Vector3.Distance(vehicleGO.transform.position, nodes[vStruct.lastNodeHit + 1].transform.position) < 0.1f)
                        {
                            // update the sprite (and increment the last node hit)
                            m_animator = vehicleGO.GetComponentInChildren<Animator>();
                            if (vStruct.lastNodeHit != 2)
                                m_animator.SetTrigger(vStruct.isGoingDown ? downDir[++vStruct.lastNodeHit] : upDir[++vStruct.lastNodeHit]);
                            else
                                ++vStruct.lastNodeHit;
                            if(vStruct.lastNodeHit == 2)
                            {
                                vehicleGO.GetComponentInChildren<SpriteRenderer>().flipX = true;
                            }

                            Collider2D[] vehicleColliders = vehicleGO.GetComponentsInChildren<Collider2D>();

                            //++vStruct.lastNodeHit; // REMOVE WHEN ANIMATOR IS UNCOMMENTED

                            if (vStruct.lastNodeHit != 3)
                            {
                                if (vStruct.isGoingDown)
                                {
                                    vehicleColliders[vStruct.lastNodeHit].enabled = true;
                                    vehicleColliders[vStruct.lastNodeHit - 1].enabled = false;
                                }
                                else
                                {
                                    vehicleColliders[(vehicleColliders.Length - 1) - vStruct.lastNodeHit].enabled = true;
                                    vehicleColliders[((vehicleColliders.Length - 1) - vStruct.lastNodeHit) + 1].enabled = false;
                                }
                            }

                            // pass the vehicle back to the struct
                            vehicles[i] = vStruct;

                            // delete the vehicle if it reaches the end
                            if (vStruct.lastNodeHit + 1 == nodes.Length)
                            {
                                destroyVehicle(vStruct);
                                // end the loop
                                i = vehicles.Count;
                            }
                        }


                    }
                }
            }
        }

        void instantiateVehicle()
        {
            // invert which side it spawns from
            goingDown = !goingDown;

            if (vehicleParent == null)
                vehicleParent = new GameObject("vehicleParent");

            // create the vehicle
            vehicleStruct vStruct = new vehicleStruct();
            vStruct.vehicle = Instantiate(vehiclePrefab) as GameObject;
            vStruct.isGoingDown = goingDown;
            vStruct.lastNodeHit = 0;

            // set correct location
            vStruct.vehicle.transform.position = goingDown ? nodesGoingDown[0].transform.position : nodesGoingUp[0].transform.position;
            vStruct.vehicle.GetComponentInChildren<Animator>().SetTrigger(vStruct.isGoingDown ? downDir[vStruct.lastNodeHit] : upDir[vStruct.lastNodeHit]);
            vStruct.vehicle.transform.parent = vehicleParent.transform;

            Collider2D[] vehicleColliders = vStruct.vehicle.GetComponentsInChildren<Collider2D>();

            for (int i = 0; i < vehicleColliders.Length; ++i)
            {
                vehicleColliders[i].enabled = false;
            }

            if (vStruct.isGoingDown)
                vehicleColliders[0].enabled = true;
            else
                vehicleColliders[2].enabled = true;



            // add to the vehicle list
            vehicles.Add(vStruct);
        }

        void destroyVehicle(vehicleStruct vStruct)
        {
            Destroy(vStruct.vehicle);
            vehicles.Remove(vStruct);
        }
    }
}