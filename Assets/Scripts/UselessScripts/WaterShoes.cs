using PlayerControl;
using UnityEngine;

namespace UselessScripts
{
    public class WaterShoes : MonoBehaviour
    {
        private GameObject _canvas;

        // Start is called before the first frame update
        private void Start()
        {
            var found = false;

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
    }
}
