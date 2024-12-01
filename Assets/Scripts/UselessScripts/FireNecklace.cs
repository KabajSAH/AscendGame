using System;
using PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;

namespace UselessScripts
{
    public class FireNecklace : MonoBehaviour
    {
        private GameObject _canvas;

        [SerializeField] private GameObject obstacles;
        // Start is called before the first frame update
        void Start()
        {
            bool found = false;

            // Vérifier chaque enfant direct
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Canvas>() == null) continue;
                found = true;
                _canvas = child.gameObject;
                break;  // On s'arrête dès qu'on trouve le composant
            }

            if (found)
            {
                _canvas.SetActive(false);
            }
            obstacles.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>( out _))
            {
                _canvas.SetActive(true);
            }
       
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                _canvas.transform.forward = ( _canvas.transform.position - other.transform.position).normalized;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                _canvas.SetActive(false);
            }
        
        }

        private void OnDestroy()
        {
            obstacles.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
