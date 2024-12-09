using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace HerghysStudio.Survivor
{
    public class GameCounter : MonoBehaviour
    {
        [SerializeField] TMP_Text textView;
        [SerializeField] Image icon;

        [SerializeField] Sprite iconSprite;

        private void Awake()
        {
            icon.sprite = iconSprite;
            UpdateText(0);
        }

        public void UpdateText(object value)
        {
            textView.text = value.ToString();
        }
    }
}
