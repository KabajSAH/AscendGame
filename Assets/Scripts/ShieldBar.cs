using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
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
        shieldBar.fillAmount = earth.passiveShield / EarthPower.MaxPassive;
        shieldText.enabled = earth.enabled;
        shieldText.text = earth.passiveShield.ToString("F1") + "/" + EarthPower.MaxPassive;
    }
}
