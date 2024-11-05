using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private EarthPower _earth;

    [SerializeField] private Image shieldBar;

    [SerializeField] private TextMeshProUGUI shieldText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        shieldBar.fillAmount = _earth.passiveShield / EarthPower.MaxPassive;
        shieldText.enabled = _earth.enabled;
        shieldText.text = _earth.passiveShield.ToString("F1") + "/" + EarthPower.MaxPassive;
    }
}
