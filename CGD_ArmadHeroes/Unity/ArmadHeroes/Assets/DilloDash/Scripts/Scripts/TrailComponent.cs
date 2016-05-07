using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DilloDash
{
    public class TrailComponent : MonoBehaviour
    {

        public GameObject trackTrail;
        public GameObject dustTrail;
        public GameObject tarmacTrail;
        public GameObject conTrail;
        public GameObject oilTrail;

        public Vector2 trailRotatingOffset;
        public Vector2 trailStaticOffset;
        public Vector2 trailRotatingOffsetAerial;
        public Vector2 trailStaticOffsetAerial;
        [SerializeField] private Vector3 rotationAdjust;
        private List<Trails> activeTrails_L, activeTrails_R;
        private float oilTime_L = 0, oilTime_R = 0;
        private DilloDashPlayer playerScript;
        private GameObject offsetTransform;
        bool parentDead = false;

        // Use this for initialization
        void Awake()
        {
            playerScript = GetComponent<DilloDashPlayer>();
            activeTrails_L = new List<Trails>();
            activeTrails_R = new List<Trails>();
            offsetTransform = new GameObject();
            offsetTransform.name = "OffsetTransform";

        }

        // Update is called once per frame
        void Update()
        {
            if (!GameStateDD.Singleton().isGamePaused)
            {
                parentDead = playerScript.GetIsDead();
                manageTrails();
            }          
            else
            {
                PauseTrails(activeTrails_L);
                PauseTrails(activeTrails_R);
            }  
        }
        
        private void PauseTrails (List<Trails> _list)
        {
            foreach (Trails t in _list)
            {
                t.TrailRend.time += Time.deltaTime;
            }
        }
        

        public void unparentLastTrails()
        {
            if (activeTrails_L.Count > 0)            
                activeTrails_L[activeTrails_L.Count - 1].TrailObject.transform.parent = null;
            
            if (activeTrails_L.Count > 0)
                activeTrails_R[activeTrails_R.Count - 1].TrailObject.transform.parent = null;
        }

        //Create a new trail of the desired type
        void createNewTrail(TileType _type, List<Trails> _list, bool isLeft, GameObject _parent)
        {
            offsetTransform.transform.SetParent(_parent.transform);
            if (playerScript.myState == DilloDashPlayer.State.Roll || _type == TileType.CON)
            {
                switch (_type)
                {
                    case TileType.DUST:

                        _list.Add(new Trails()
                        {
                            TimeIdle = 0.0f,
                            TrailObject = Instantiate(dustTrail),
                            tileType = _type
                        });

                        break;
                    case TileType.TRACK:
                        _list.Add(new Trails()
                        {
                            TimeIdle = 0.0f,
                            TrailObject = Instantiate(trackTrail),
                            tileType = _type

                        });

                        break;
                    case TileType.TARMAC:
                        _list.Add(new Trails()
                        {
                            TimeIdle = 0.0f,
                            TrailObject = Instantiate(tarmacTrail),
                            tileType = _type

                        });

                        break;
                    case TileType.SLIP:
                        _list.Add(new Trails()
                        {
                            TimeIdle = 0.0f,
                            TrailObject = Instantiate(oilTrail),
                            tileType = _type

                        });

                        break;
                    case TileType.CON:
                        _list.Add(new Trails()
                        {
                            TimeIdle = 0.0f,
                            TrailObject = Instantiate(conTrail),
                            tileType = _type
                        });
                        break;
                }


                int i = _list.Count - 1;
                bool isAerial = _type == TileType.CON ? true : false;

                _list[i].TrailObject.transform.SetParent(offsetTransform.transform);
                _list[i].TrailRend = _list[i].TrailObject.GetComponent<TrailRenderer>();

                Vector2 offsetToUse = isAerial ? trailRotatingOffsetAerial : trailRotatingOffset;
                _list[i].TrailObject.transform.localPosition = isLeft ? new Vector3(-offsetToUse.x, -offsetToUse.y, -1) : new Vector3(offsetToUse.x, -offsetToUse.y, -1);
                _list[i].TrailObject.transform.localRotation = new Quaternion(0, 0, 0, 0);


                _list[i].TrailRend.sortingLayerName = isAerial ? ("Foreground") : ("Background");
                _list[i].TrailRend.sortingOrder = 2;
                _list[i].TrailRend.material = new Material(_list[i].TrailRend.material);
            }
            else if (_list.Count >0)                
            {
                _list[_list.Count - 1].TrailObject.transform.SetParent(null);
            }
            


        }

        void applyStaticOffset()
        {
            if (!offsetTransform)
            {
                unparentLastTrails();
                offsetTransform = new GameObject();
                offsetTransform.name = "OffsetTransform";
            }
            Transform parentTransform = transform;
            if (playerScript.GetAerialPlayer())
            {
                parentTransform = playerScript.GetAerialPlayer().gameObject.transform;
                offsetTransform.transform.position = new Vector3(parentTransform.position.x - trailStaticOffsetAerial.x, parentTransform.position.y - trailStaticOffsetAerial.y, 0);
                Quaternion temp = parentTransform.rotation;
                temp = Quaternion.Euler(temp.eulerAngles.x+rotationAdjust.x, temp.eulerAngles.y+ rotationAdjust.y, temp.eulerAngles.z + rotationAdjust.z);

                offsetTransform.transform.rotation = temp;
            } 
            else           
                offsetTransform.transform.position = new Vector3 (parentTransform.position.x - trailStaticOffset.x, parentTransform.position.y - trailStaticOffset.y, 0);
        }

        //Passes a position to the tilemanager, and converts the rutrned value to an enum
        TileType returnType(Vector2 _pos)
        {

            switch (TileManager.Singleton().whatTileEffect(_pos))
            {
                case TileEffect.DEFAULT: // default - do nothing
                    return TileType.TRACK;

                case TileEffect.TRACK: // track - do nothing
                    return TileType.TRACK;

                case TileEffect.SAND: // off track - slow player
                    return TileType.DUST;

                case TileEffect.BOOST: // boost - speed player
                    return TileType.TRACK;

                case TileEffect.SLIP: // slip tile
                    return TileType.SLIP;

                case TileEffect.POWERUP: //power up
                    return TileType.TRACK;

                case TileEffect.TARMAC: //tarmac
                    return TileType.TARMAC;
            }
          
            return TileType.TRACK;
        }

        //Updates active trail and remove faded trails
        void manageTrails()
        {
            applyStaticOffset();
            if (playerScript.GetAerialPlayer())
            {
                GameObject aP = playerScript.GetAerialPlayer().gameObject;
                if (activeTrails_L.Count > 0)
                {
                    if (activeTrails_L[activeTrails_L.Count - 1].tileType != TileType.CON || activeTrails_L[activeTrails_L.Count - 1].TrailObject.transform.root != aP)
                    {
                        createNewTrail(TileType.CON, activeTrails_L, true, aP);
                    }
                }
                else  
                {
                    createNewTrail(TileType.CON, activeTrails_L, true, aP);
                }
                if (activeTrails_R.Count > 0)
                {
                    if (activeTrails_R[activeTrails_R.Count - 1].tileType != TileType.CON || activeTrails_R[activeTrails_R.Count - 1].TrailObject.transform.root != aP)
                    {
                        createNewTrail(TileType.CON, activeTrails_R, false, aP);
                    }
                }
                else 
                {
                    createNewTrail(TileType.CON, activeTrails_R, false, aP);
                }

            }
            else if (parentDead)
            {
                unparentLastTrails();
            }
            else
            {
                if (playerScript.myState != DilloDashPlayer.State.Roll)
                {
                    unparentLastTrails();
                }
                else
                {

                    TileType currL, currR;

                    if (activeTrails_L.Count == 0 || activeTrails_L[activeTrails_L.Count - 1].TrailObject.transform.parent == null)
                    {
                        createNewTrail(returnType(transform.position), activeTrails_L, true, gameObject);
                    }
                    if (activeTrails_R.Count == 0 || activeTrails_R[activeTrails_R.Count - 1].TrailObject.transform.parent == null)
                    {
                        createNewTrail(returnType(transform.position), activeTrails_R, false, gameObject);
                    }

                    int L = activeTrails_L.Count - 1;
                    int R = activeTrails_R.Count - 1;

                    currL = returnType(transform.position) == TileType.SLIP ? TileType.SLIP : returnType(activeTrails_L[L].TrailObject.transform.position);
                    currR = returnType(transform.position) == TileType.SLIP ? TileType.SLIP : returnType(activeTrails_R[R].TrailObject.transform.position);



                    if (activeTrails_L[L].tileType != currL)
                    {
                        if (activeTrails_L[L].tileType == TileType.SLIP && oilTime_L < activeTrails_L[L].TrailRend.time)
                        {
                            oilTime_L += Time.deltaTime;
                        }
                        else
                        {
                            oilTime_L = 0;
                            createNewTrail(currL, activeTrails_L, true, gameObject);
                        }
                    }
                    else
                    {
                        activeTrails_L[L].TimeIdle = 0;
                    }
                    if (activeTrails_R[R].tileType != currR)
                    {
                        if (activeTrails_R[R].tileType == TileType.SLIP && oilTime_R < activeTrails_R[R].TrailRend.time)
                        {
                            oilTime_R += Time.deltaTime;
                        }
                        else
                        {
                            oilTime_R = 0;
                            createNewTrail(currR, activeTrails_R, false, gameObject);
                        }
                    }
                    else
                    {
                        activeTrails_R[R].TimeIdle = 0;
                    }                    
                }
            }
            List<Trails> toDestroy = new List<Trails>(); //Used for cleanup

            for (int i = 0; i < activeTrails_L.Count; i++)
            {

                //If not the latest trail in the list then make sure its unparented and increase its timer
                if (i != (activeTrails_L.Count - 1) || activeTrails_L[i].TrailObject.transform.parent == null)
                {
                    
                    activeTrails_L[i].TimeIdle += Time.deltaTime;
                    activeTrails_L[i].TrailObject.transform.parent = null;
                }
                //If Timer is beyond the trail life then it can be destroyed
                if (activeTrails_L[i].TimeIdle > activeTrails_L[i].TrailRend.time)
                {
                    toDestroy.Add(activeTrails_L[i]);
                }
            }
            //Same as the above but for the other list
            for (int i = 0; i < activeTrails_R.Count; i++)
            {
                if (i != (activeTrails_R.Count - 1) || activeTrails_R[i].TrailObject.transform.parent == null)
                {
                    activeTrails_R[i].TimeIdle += Time.deltaTime;
                    activeTrails_R[i].TrailObject.transform.parent = null;
                }

                if (activeTrails_R[i].TimeIdle > activeTrails_R[i].TrailRend.time)
                {
                    toDestroy.Add(activeTrails_R[i]);
                }
            }

            //Cleanup the objects
            for (int i = 0; i < toDestroy.Count; i++)
            {
                activeTrails_L.Remove(toDestroy[i]);
                activeTrails_R.Remove(toDestroy[i]);
                Destroy(toDestroy[i].TrailObject);
            }
            toDestroy.Clear();

        }
    }

    public enum TileType
    {
        TRACK, DUST, CON, TARMAC, SLIP
    }

    public class Trails
    {
        public TileType tileType;
        public GameObject TrailObject;
        public TrailRenderer TrailRend;
        public float TimeIdle;

    }

}