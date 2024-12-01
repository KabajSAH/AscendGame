using System.Collections;
using System.Collections.Generic;
using PlayerControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public static IEnumerator OnDeath()
    {
        var p = FindAnyObjectByType<Player>();
        var a = p.Animator.GetCurrentAnimatorStateInfo(2).length;
        var es = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        p.enabled = false;
        foreach (var e in es)
        {
            e.enabled = false;
        }
        yield return new WaitForSeconds(a + 0.1f);
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
