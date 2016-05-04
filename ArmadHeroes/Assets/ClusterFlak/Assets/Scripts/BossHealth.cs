using UnityEngine;
using System.Collections;

using ArmadHeroes;

public class BossHealth : MonoBehaviour
{
    public float MaxBosshp = 1;
    public GameObject particles;
    public GameObject location;
    public GameObject ApacheBoss;
    public UnityEngine.UI.Slider m_Slider;                         
    public UnityEngine.UI.Image m_FillImage;                        
    public Color m_FullHealthColor = Color.green;       
    public Color m_ZeroHealthColor = Color.red;        
    public static int BossPhase = 1;
    private Vector3 ap1 = new Vector3(-15, 15, 0);
    private Vector3 ap2 = new Vector3(0, 20, 0);
    private Vector3 ap3 = new Vector3(15, 15, 0);
    private Quaternion apq1 = Quaternion.Euler(0, 0, 0);
    public static float CurrentBosshealth;
    private bool Dead;
    private bool ap1lock;
    private bool ap2lock;
    private bool ap3lock;
    private Vector3 dnode;
    private int xdeath;
    private int ydeath;
    public GameObject APCDeath;

    void Start()
    {
        CurrentBosshealth = MaxBosshp;
        SetHealthUI();
        Debug.Log(MaxBosshp);
    }

    void Update()
    {
        if (CurrentBosshealth <= (MaxBosshp * 0.75f) && CurrentBosshealth > (MaxBosshp * 0.5f)) //Phase 2 Trigger
        {
            if (ap1lock == false)
            {
                GameObject.Instantiate(ApacheBoss, ap1, apq1);
                ap1lock = true;
            }
        }

        if (CurrentBosshealth <= (MaxBosshp * 0.50f) && CurrentBosshealth > (MaxBosshp * 0.25f)) //Phase 3 Trigger
        {
            if (ap2lock == false)
            {
                GameObject.Instantiate(ApacheBoss, ap2, apq1);
                ap2lock = true;
            }
        }
        if (CurrentBosshealth <= (MaxBosshp * 0.25f)) //Phase 4 Trigger
        {
            if (ap3lock == false)
            {
                GameObject.Instantiate(ApacheBoss, ap3, apq1);
                ap3lock = true;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {



        if (col.gameObject.GetComponent<Projectile>())
        {
            Debug.Log("boss hit");
            CurrentBosshealth -= 1;
            SetHealthUI();
            Debug.Log(MaxBosshp);

            if (CurrentBosshealth <= 0)
            {
                Destroy(gameObject);
                DeathExplosion();
            }

            Destroy(col.gameObject);
        }
            if (CurrentBosshealth == 0)
            {
                Destroy(gameObject);
                GameObject deadp = (GameObject)Instantiate(particles);
                deadp.transform.position = location.transform.position;
            }
        

    }

    private void SetHealthUI()
    {
        m_Slider.value = CurrentBosshealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentBosshealth / MaxBosshp);
    }


     void DeathExplosion()
    {
        GameObject apcex = (GameObject)Instantiate(APCDeath);
        GameObject.Find("fade").GetComponent<cf_fade>().winbool = true;
        xdeath = Random.Range(-2, 2);
         ydeath = Random.Range(3, 0);
        dnode = new Vector3(xdeath, ydeath, 0);
        apcex.transform.position = dnode; 
    }


}
