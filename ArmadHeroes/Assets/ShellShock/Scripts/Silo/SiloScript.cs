using UnityEngine;
using System.Collections;

namespace ShellShock
{
    public class SiloScript : MonoBehaviour
    {
        //public SiloManager siloManager;
        private Animator siloAnimator;
        //public int siloId;
        public bool isOpen;
        //public Vector3 siloPos = new Vector3();

        private SiloScript mPairedSilo;
        public SiloScript PairedSilo
        {
            get
            {
                return mPairedSilo;
            }

            set
            {
                mPairedSilo = value;
            }
        }

        void Start()
        {
            if (SiloManager.Instance == null)
            {
                Debug.Log("Poop");
            }
            SiloManager.Instance.JoinList(this);

            siloAnimator = GetComponent<Animator>();
            siloAnimator.SetBool("isOpen", false);
        }

        void Update()
        {
            if (mPairedSilo != null)
            {
                Debug.DrawLine(transform.position, PairedSilo.transform.position);
            }
        }

        //public void SiloChange(int siloOne, int siloTwo)
        //{
        //    if (siloId == siloOne || siloId == siloTwo)
        //    {
        //        if (siloAnimator.GetBool("isOpen") == true) //Walls are down
        //        {
        //            SilosClose();
        //        }
        //        else if (siloAnimator.GetBool("isOpen") == false)
        //        {
        //            SilosOpen();
        //        }
        //    }
        //}

        public Vector2 GetPairedSiloPos()
        {
            Close();
            mPairedSilo.Close();
            return mPairedSilo.transform.position;
        }

        public void Open()
        {
            siloAnimator.SetBool("isOpen", true); //Silos Open
            isOpen = true;
            //GetOpenSiloPos();
        }

        public void Close()
        {
            siloAnimator.SetBool("isOpen", false); //Silos Close
            isOpen = false;
        }

        //public void GetOpenSiloPos()
        //{
        //    siloPos = this.transform.position;
        //}
    }
}


