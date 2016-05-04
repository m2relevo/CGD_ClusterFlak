using UnityEngine;
using System.Collections;

public class ApacheHealth : MonoBehaviour
{
    private float MaxBosshp = 20;
    public GameObject particles;
    public GameObject location;

    public UnityEngine.UI.Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public UnityEngine.UI.Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
  

    private float CurrentBosshealth;
    private bool Dead;
    
    void Start()
    {
        CurrentBosshealth = 20;
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
            Debug.Log("boss hit");
            CurrentBosshealth -= 1;
            SetHealthUI();
            Debug.Log(MaxBosshp);
            if (CurrentBosshealth <= 0)
            {
                GameObject deadp = (GameObject)Instantiate(particles);
                deadp.transform.position = location.transform.position;
                Destroy(this.gameObject);
                
            }
        }
    }

    private void SetHealthUI()
    {
        m_Slider.value = CurrentBosshealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentBosshealth / MaxBosshp);
    }


}
