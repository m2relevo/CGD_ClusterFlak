/// <summary>
/// TextLerp.cs
/// Created and implemented by Daniel Weston - 24/02/16
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Armatillery
{
    public class TextLerp : MonoBehaviour
    {
        public Color start;
        public Text m_text;
        Color end = new Color(1, 0, 0, 1);
        public float lerpTime = 2f;
        public float currentLerpTime;
        public bool change = false;

        // Use this for initialization
        void Start()
        {
            start = m_text.color;
        }

        // Update is called once per frame
        void Update()
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;

            currentLerpTime = currentLerpTime > lerpTime ? 0.0f : currentLerpTime;
            change = currentLerpTime == 0.0f ? !change : change;

            float perc = currentLerpTime / lerpTime;

            m_text.color = change ? Color.Lerp(m_text.color, start, perc) : Color.Lerp(m_text.color, end, perc);
        }
    }
}