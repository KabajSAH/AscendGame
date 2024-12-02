using PlayerControl;
using UnityEngine;
using UnityEngine.UI;

namespace UiMenu
{
    public class WaterBar : MonoBehaviour
    {
    
        [SerializeField] private Image waterBar;

        [SerializeField] private WaterPowerTODO waterPower;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            waterBar.enabled = waterPower.enabled;
        }
    }
}
