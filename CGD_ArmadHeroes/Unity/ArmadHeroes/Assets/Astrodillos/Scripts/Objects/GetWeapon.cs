using UnityEngine;
using System.Collections;
using DG.Tweening;
using ArmadHeroes;

namespace Astrodillos
{
    public class GetWeapon : MonoBehaviour
    {
    
        public Sprite[] weaponSprites;
        public AudioClip weaponClip;
		public ParticleSystem pulse;
        public bool desertSpawn;
        public int levelSpawn;
        SpriteRenderer spRend;
        WeaponType type;
		Renderer pulseRenderer;
       
        void Start()
        {
			pulseRenderer = pulse.GetComponent<Renderer> ();
            spRend = GetComponent<SpriteRenderer>();
            SetWeaponType();
			spRend.sortingOrder = SpriteOrdering.GetOrder (transform.position.y);
			pulseRenderer.sortingOrder = spRend.sortingOrder - 1;
            gameObject.SetActive(true);
        }
      
        void Update()
        {
            //if (levelSpawn != Gametype_Astrodillos.instance.GetCurrentLevel()||desertSpawn != Gametype_Astrodillos.instance.UseGravity())
            //{
            //  Destroy(gameObject);
            //}
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<Actor_Armad>())
            {
                col.gameObject.GetComponent<Actor_Armad>().WeaponSwitch(type);
                SoundManager.instance.PlayClip(weaponClip);
                Destroy(gameObject);     
            }
          
        }
        void SetWeaponType()
        {
          WeaponSwitch(Random.Range(0, 4));
        }

        void WeaponSwitch(int whatever)
        {
            switch (whatever)
            {
                case 0:
                    {
                        spRend.sprite = weaponSprites[0];
                        type = WeaponType.FLAMETHROWER;
                        break;
                    }
                case 1:
                    {
                        spRend.sprite = weaponSprites[1];
                        type = WeaponType.LASER;
                        break;
                    }
                case 2:
                    {
                        spRend.sprite = weaponSprites[2];
                        type = WeaponType.MACHINEGUN;
                        break;
                    }
                case 3:
                    {
                        spRend.sprite = weaponSprites[3];
                        type = WeaponType.SHOTGUN;
                        break;
                    }
                case 4:
                    {
                        spRend.sprite = weaponSprites[3];
                        type = WeaponType.SHOTGUN;
                        break;
                    }
            }
        }
      
       
    }
}