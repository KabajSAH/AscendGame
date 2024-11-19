using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameObject _win;
    // Start is called before the first frame update
    void Start()
    {
     _win.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out _))
        {
            StartCoroutine(Winning());
        }
    }

    private IEnumerator Winning()
    {
        _win.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
