using System.Collections;
using System.Collections.Generic;
using PlayerControl;
using UnityEngine;
using UnityEngine.UI;

public class AirScript : MonoBehaviour
{
    
    [SerializeField] private Image airBar;

    [SerializeField] private AirPowerTodo airPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        airBar.enabled = airPower.enabled;
    }
}
