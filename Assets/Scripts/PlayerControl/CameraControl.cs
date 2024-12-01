using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;  // Vitesse de rotation.
        [SerializeField] private float minVerticalAngle = -40f;  // Limite inférieure de la rotation verticale.
        [SerializeField] private float maxVerticalAngle = 80f;   // Limite supérieure de la rotation verticale.
        [SerializeField] private float horizontalRotationLimit = 90f;  // Limite de rotation horizontale.

        private InputAction _lookAction;
        private float _currentVerticalRotation;
        private float _currentHorizontalRotation;

        private bool _initialized = false;

        private IEnumerator InitializeLookAction()
        {
            yield return new WaitForSeconds(0.1f); // Attendez un instant pour stabiliser les entrées.
            _initialized = true;
        }
        
        private void Start()
        {
            _lookAction = InputSystem.actions.FindAction("Look");
            var initialRotation = transform.localRotation.eulerAngles;
            _currentVerticalRotation = initialRotation.x;
            _currentHorizontalRotation = initialRotation.y;
            StartCoroutine(InitializeLookAction());
        }

        private void Update()
        {
            if (!_initialized || _lookAction == null) return;
            var lookInput = _lookAction.ReadValue<Vector2>();
            // Récupérer les entrées de la souris (mouvement).
            var horizontalMovement = lookInput.x * rotationSpeed;
            var verticalMovement = lookInput.y * rotationSpeed;

            // Appliquer la rotation horizontale (gauche/droite).
            _currentHorizontalRotation += horizontalMovement;
            _currentHorizontalRotation = Mathf.Clamp(_currentHorizontalRotation, -horizontalRotationLimit, horizontalRotationLimit);

            // Appliquer la rotation verticale (haut/bas), avec des limites.
            _currentVerticalRotation -= verticalMovement;
            _currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, minVerticalAngle, maxVerticalAngle);

            // Appliquer les rotations à la caméra.
            transform.localRotation = Quaternion.Euler(_currentVerticalRotation, _currentHorizontalRotation, 0f);

            // Faire tourner la cible (par exemple, un personnage) autour de l'axe vertical.
            transform.Rotate(0f, _currentHorizontalRotation, 0f);
        }
    }
}
