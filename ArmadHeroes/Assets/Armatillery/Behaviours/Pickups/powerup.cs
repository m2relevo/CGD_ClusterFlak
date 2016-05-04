using UnityEngine;
using System.Collections;

namespace Armatillery
{
    public class powerup : MonoBehaviour
    {
        public ArmadHeroes.BulletModifier m_powerupType;

        public void Init()
        {
            StartCoroutine(destoryMe());
        }
        IEnumerator destoryMe()
        {
            yield return new WaitForSeconds(6);
            GetComponentInParent<Animator>().Play("powerup_flash");
            yield return new WaitForSeconds(4);
            powerUpManager.instance.disablePowerup(transform.parent.gameObject);
        }
    }
}