using System.Collections;
using System.Collections.Generic;
using PlayerControl;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [SerializeField] private Player player;
    // Start is called before the first frame update
    private void Start()
    {
     menu.SetActive(false);   
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        player.enabled = !player.enabled;
        menu.SetActive(!menu.activeSelf);
    }
}
