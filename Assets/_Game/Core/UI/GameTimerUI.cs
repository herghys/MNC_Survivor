using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace HerghysStudio.Survivor
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] Image sliderFill;
        [SerializeField] TMP_Text sliderText;

        private void Awake()
        {
            sliderFill.fillAmount = 0;
            sliderText.text = "00:00";
        }

        /// <summary>
        /// Update Time Slider
        /// </summary>
        /// <param name="fillValue"></param>
        /// <param name="fillText"></param>
        public void UpdateTime(float fillValue, string fillText)
        {
            sliderFill.fillAmount = fillValue;
            sliderText.text = fillText;
        }
    }
}
