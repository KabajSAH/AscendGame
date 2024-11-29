using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public static IEnumerator OnDeath()
    {
        var p = FindAnyObjectByType<Player>();
        var es = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        p.enabled = false;
        foreach (var e in es)
        {
            e.enabled = false;
        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level 1");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
