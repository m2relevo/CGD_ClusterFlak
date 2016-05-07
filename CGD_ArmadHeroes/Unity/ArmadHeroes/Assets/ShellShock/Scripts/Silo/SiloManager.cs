using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShellShock
{
    public class SiloManager : MonoBehaviour
    {
        public static SiloManager Instance;
        public List<SiloScript> mSiloList;
        public float repeatTime;
        public AudioSource siloOpenSound;

        void Awake()
        {
            if (SiloManager.Instance != null)
            {
                Debug.LogError("There should only be one silo manager!");
            }
            //Debug.Log("Done!");

            Instance = this;

            mSiloList = new List<SiloScript>();
            //siloList = GameObject.FindGameObjectsWithTag("ShellShock/Silos");
            //maxSilos = siloList.Length;
            //SiloActions();
        }
        void Start()
        {
            siloOpenSound = gameObject.GetComponent<AudioSource>();
        }

        public void JoinList(SiloScript silo)
        {
            mSiloList.Add(silo);
        }

        void Update()
        {
            repeatTime -= Time.deltaTime;
            if (repeatTime > 1.0f && repeatTime < 2.0f)
            {
                CloseAll();
            }
            if (repeatTime <= 0.0f)
            {
                ShuffleSilos();
                OpenRandomPair();
            }
        }

        void ShuffleSilos()
        {
            Shuffle(mSiloList);

            for (int i = 0; i < mSiloList.Count - 1; i++)
            {
                mSiloList[i].PairedSilo = mSiloList[i + 1];
                mSiloList[i + 1].PairedSilo = mSiloList[i];
                i++;
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int rnd = UnityEngine.Random.Range(i, count);
                T tmp = list[i];
                list[i] = list[rnd];
                list[rnd] = tmp;
            }
        }

        void OpenRandomPair()
        {
            siloOpenSound.Play();
            mSiloList[0].Open();
            mSiloList[1].Open();
            repeatTime = 10f;
        }

        //    public void SiloActions()
        //    {
        //        ChooseFirst();
        //        ChooseSecond();
        //        ChangeSiloState();
        //    }

        public void CloseAll()
        {
            for (int i = 0; i < mSiloList.Count; i++)
            {
                mSiloList[i].GetComponent<ShellShock.SiloScript>().Close();
            }
        }

        //    public void ChooseFirst()
        //    {
        //        siloOne = Random.Range(0, maxSilos) + 1;
        //        if (siloOne == siloTwo)
        //        {
        //            siloOne = Random.Range(0, maxSilos) + 1;
        //        }
        //    }

        //    public void ChooseSecond()
        //    {
        //        siloTwo = Random.Range(0, maxSilos) + 1;
        //        if (siloTwo == siloOne)
        //        {
        //            siloTwo = Random.Range(0, maxSilos) + 1;
        //        }
        //    }

        //    public void ChangeSiloState()
        //    {
        //        foreach (GameObject silo in siloList)
        //        {
        //            silo.GetComponent<ShellShock.SiloScript>().SiloChange(siloOne, siloTwo);
        //        }
        //        repeatTime = 10f;
        //        close = false;
        //    }

        //    public void CloseSilos()
        //    {
        //        foreach (GameObject silo in siloList)
        //        {
        //            silo.GetComponent<ShellShock.SiloScript>().SiloChange(siloOne, siloTwo);
        //        }
        //    }

        //    public void GetOpenSilos(int siloHit)
        //    {
        //        foreach (GameObject silo in siloList)
        //        {
        //            if (siloScript.isOpen = true && siloScript.siloId != siloHit)
        //            {
        //                otherSiloPos = GetComponent<GameObject>().transform.position;
        //                playerLogic.Teleport(otherSiloPos);
        //            }
        //        }
        //    }
        //}
    }
}
