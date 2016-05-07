using UnityEngine;
using System.Collections;
namespace ShellShock
{
    public class GameType_ShellShock : MonoBehaviour
    {
        //Singleton
        public static GameType_ShellShock instance;

        public ParticlePooler explosionPool;
        public ParticlePooler sparksPool;
        public ParticlePooler dustPool;
        public AudioClip explosionSfx;

        
        // Use this for initialization

        void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Explosion(Vector3 pos, bool playSound = true, bool areaOfEffectDmg = false)
        {
            ParticleSystem ps = explosionPool.GetNext();
            ps.gameObject.transform.position = pos;
            ps.Play();

            if (playSound)
            {
                ArmadHeroes.SoundManager.instance.PlayClip(explosionSfx, pos);
            }

            //if (areaOfEffectDmg)
            //{
            //    foreach (Actor_Armad actor in PlayerManager.instance.players)
            //    {
            //        float distance = Vector3.Distance(actor.transform.position, explosionPos);
            //        if (distance < 0.6f)
            //        {
            //            actor.TakeDamage(50);
            //        }
            //    }
            //}

        }

        public void Sparks(Vector3 pos, bool playSound = true, bool areaOfEffectDmg = false)
        {
            ParticleSystem ps = sparksPool.GetNext();
            ps.gameObject.transform.position = pos;
            ps.Play();
        }

        public void Dust(Vector3 pos, bool playSound = true, bool areaOfEffectDmg = false)
        {
            ParticleSystem ps = dustPool.GetNext();
            ps.gameObject.transform.position = pos;
            ps.Play();
        }
    }
}
