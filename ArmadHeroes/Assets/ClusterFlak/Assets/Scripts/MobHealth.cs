using UnityEngine;
using System.Collections;

public class MobHealth : MonoBehaviour
{
    public float MaxBosshp = 3;
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
        Debug.Log(MaxBosshp);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ClusterFlak/PlayerBullet")
        {
            Debug.Log("MOB hit");
            CurrentBosshealth -= 1;
            SetHealthUI();
            Debug.Log(MaxBosshp);
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
