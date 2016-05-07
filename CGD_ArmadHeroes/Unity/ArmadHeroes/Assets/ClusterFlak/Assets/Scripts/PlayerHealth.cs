using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public float MaxPlayerhealth = 10f;
    public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.


    private float CurrentPlayerhealth;
    private bool Dead;

    void Start()
    {
        CurrentPlayerhealth = MaxPlayerhealth;
        SetHealthUI();
    }

    void Update()
    { }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "ClusterFlak/EnemyBullet")   
        {
            CurrentPlayerhealth -= 1;
            SetHealthUI();
            if (CurrentPlayerhealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SetHealthUI()
    {
        m_Slider.value = CurrentPlayerhealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, CurrentPlayerhealth / MaxPlayerhealth);
    }
}
