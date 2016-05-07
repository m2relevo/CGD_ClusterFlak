using UnityEngine;
using System.Collections.Generic;
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
    public List<GameObject> listPlayers;
    int Thismany;
    private bool pcbool;
    void Start()
    {
        CurrentBosshealth = MaxBosshp;
        SetHealthUI();
        
        
    }

    void Update()
    {
        if (pcbool == false)
        {
            listPlayers.AddRange(GameObject.FindGameObjectsWithTag("ClusterFlak/Player"));
            pcbool = true;
        }
        int Thismany = listPlayers.Count;
        //code that spawns an amount of Apaches into the game based on boss health and current player count 

        if (CurrentBosshealth <= (MaxBosshp * 0.75f) && CurrentBosshealth > (MaxBosshp * 0.5f)) //Phase 2 Trigger
        {
            if (ap1lock == false)
            {
                for (int x = 0; x < Thismany; x++)
                { 
                GameObject.Instantiate(ApacheBoss, ap1, apq1);
                }
                ap1lock = true;


            }
        }

        if (CurrentBosshealth <= (MaxBosshp * 0.50f) && CurrentBosshealth > (MaxBosshp * 0.25f)) //Phase 3 Trigger
        {
            if (ap2lock == false)
            {
                for (int y = 0; y < Thismany; y++)
                {
                    GameObject.Instantiate(ApacheBoss, ap2, apq1);
                    
                }
                ap2lock = true;
            }
        }
        if (CurrentBosshealth <= (MaxBosshp * 0.25f)) //Phase 4 Trigger
        {
            if (ap3lock == false)
            {
                for (int z = 0; z < Thismany; z++)
                {
                    GameObject.Instantiate(ApacheBoss, ap3, apq1);
                    
                }
                ap3lock = true;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //collision code for player projectiles
        if (col.gameObject.GetComponent<Projectile>())
        {
            CurrentBosshealth -= 1;
            SetHealthUI();

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


     void DeathExplosion()//function that triggers endgame fade and explosion effects 
    { 
        GameObject apcex = (GameObject)Instantiate(APCDeath);
        GameObject.Find("fade").GetComponent<cf_fade>().winbool = true;
        xdeath = Random.Range(-2, 2);
         ydeath = Random.Range(3, 0);
        dnode = new Vector3(xdeath, ydeath, 0);
        apcex.transform.position = dnode; 
    }


}
