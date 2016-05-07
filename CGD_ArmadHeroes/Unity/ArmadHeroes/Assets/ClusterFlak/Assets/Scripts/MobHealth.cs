using UnityEngine;
using System.Collections;
using ArmadHeroes;

public class MobHealth : MonoBehaviour
{//sets collisions and mob health for spawned enemy infantry units 
    public float MaxBosshp = 1;
    public GameObject particles;
    public GameObject location;
    public GameObject ApacheBoss;
    public UnityEngine.UI.Slider m_Slider;
    public UnityEngine.UI.Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public static int BossPhase = 1;
    public float CurrentBosshealth;
    private bool Dead;
    private bool ap1lock;
    private bool ap2lock;
    private bool ap3lock;
    void Start()
    {
        CurrentBosshealth = MaxBosshp;
        SetHealthUI();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Projectile>())
        {
            CurrentBosshealth -= 1;
            SetHealthUI();
            Destroy(col.gameObject);

            if (CurrentBosshealth == 0)
            {
                Destroy(gameObject);
                GameObject deadp = (GameObject)Instantiate(particles);
                deadp.transform.position = location.transform.position;
            }
        }
    }

    private void SetHealthUI()
    {
        m_Slider.value = CurrentBosshealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentBosshealth / MaxBosshp);
    }


}
