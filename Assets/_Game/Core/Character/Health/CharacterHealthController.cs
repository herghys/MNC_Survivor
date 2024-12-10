using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class CharacterHealthController : MonoBehaviour
    {
        [SerializeField] HealthBar healthBar;

        private void Awake()
        {
            if (healthBar == null)
                healthBar = GetComponentInChildren<HealthBar>();
        }

        public void UpdateHealth(float sliderValue, float textValue)
        {
            healthBar.UpdateHealthBar(sliderValue, textValue);
        }
    }
}
