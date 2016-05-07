using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ArmadHeroes
{
    public class AccoladeDisplay : MonoBehaviour
    {
        [SerializeField] private Text accoladeName = null;
        [SerializeField] private Text description = null;
        [SerializeField] private Image plaque = null;

        private float fadeTime = 0.0f;
        private float fadeDuration = 0.2f;

        private bool fadeIn = false;
        private bool fadeOut = false;

        public void Init(string _name, string _description)
        {
            accoladeName.text = _name;
            description.text = _description;
            fadeIn = true;
        }

        void Update()
        {
            if (fadeIn)
            {
                fadeTime = fadeTime >= fadeDuration ? fadeDuration : fadeTime += Time.deltaTime;
                accoladeName.color = new Color(0, 0, 0, fadeTime / fadeDuration);
                description.color = new Color(0, 0, 0, fadeTime / fadeDuration);
                plaque.color = new Color(1, 1, 1, fadeTime / fadeDuration);
                fadeIn = fadeTime == fadeDuration ? false : true;
            }
            else if (fadeOut)
            {
                fadeTime = fadeTime <= 0.0f ? 0.0f : fadeTime -= Time.deltaTime;
                accoladeName.color = new Color(0, 0, 0, fadeTime / fadeDuration);
                description.color = new Color(0, 0, 0, fadeTime / fadeDuration);
                plaque.color = new Color(1, 1, 1, fadeTime / fadeDuration);
                fadeOut = fadeTime == 0.0f ? false : true;
            }
        }
        
        public void FadeOut()
        {
            fadeOut = true;
        }
    }
}
