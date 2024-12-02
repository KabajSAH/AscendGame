using PlayerControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiMenu
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Player player;

        [SerializeField] private Image hpBar;

        [SerializeField] private TextMeshProUGUI healthText;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            hpBar.fillAmount = player.health / Player.MaxHealth;
            healthText.text = player.health.ToString("F1") + " / " + Player.MaxHealth;
        }
    }
}
