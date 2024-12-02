using PlayerControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiMenu
{
    public class EarthBar : MonoBehaviour
    {
        [SerializeField] private EarthPower earth;
        [SerializeField] private Image shieldBar;
    
        [SerializeField] private TextMeshProUGUI shieldText;
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        private void Update()
        {
            shieldBar.fillAmount = earth.timeTillCooldown / EarthPower.Cooldown;
            shieldText.enabled = earth.enabled;
            shieldText.text = (EarthPower.Cooldown - earth.timeTillCooldown).ToString("F1");
        }
    }
}
