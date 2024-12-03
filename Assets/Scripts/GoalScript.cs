using System;
using System.Collections;
using System.Collections.Generic;
using PlayerControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameObject win;
    // Start is called before the first frame update
    private void Start()
    {
     win.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Player>(out var p)) return;
        StopAllAnimations(p.Animator);
        p.enabled = false;
        StartCoroutine(Winning());
    }

    public void StopAllAnimations(Animator animator)
    {
        // Réinitialiser tous les paramètres
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.name, false);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.ResetTrigger(parameter.name);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(parameter.name, 0f);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(parameter.name, 0);
                    break;
            }
        }

        // Passer à un état vide (si défini)
        animator.Play("Idle");
        
    }
    
    private IEnumerator Winning()
    {
        win.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
