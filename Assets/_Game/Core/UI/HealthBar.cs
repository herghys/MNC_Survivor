using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace HerghysStudio.Survivor
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image fill;
        [SerializeField] TMP_Text healthText;

        public void UpdateHealthBar(float sliderValue, float textValue)
        {
            fill.fillAmount = sliderValue;
            healthText.text = (Mathf.Ceil(textValue)).ToString();
        }
    }
}