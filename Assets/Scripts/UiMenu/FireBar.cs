using PlayerControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UiMenu
{
    public class FireBar : MonoBehaviour
    {
        [SerializeField] private FirePower fire;
        [SerializeField] private Image fireBar;
    
        [SerializeField] private TextMeshProUGUI fireText;
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        private void Update()
        {
            fireBar.fillAmount = fire.timeSinceShoot / FirePower.MaxBeforeAttack;
            fireText.enabled = fire.enabled;
            fireText.text = Mathf.Clamp01(FirePower.MaxBeforeAttack - fire.timeSinceShoot).ToString("F1");
        }
    }
}
