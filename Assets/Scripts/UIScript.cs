using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    // Start is called before the first frame update
    private void Start()
    {
     menu.SetActive(false);   
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.M)) return;
        menu.SetActive(!menu.activeSelf);
    }
}
