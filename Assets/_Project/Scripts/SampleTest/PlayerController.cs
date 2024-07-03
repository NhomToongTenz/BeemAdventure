using System;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace DuAn1._Project.Scripts.SampleTest
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] private CharacterController controller;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookCam;
        [SerializeField, Anywhere] private InputReader input;

        [Header("Setting")] [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;

        private const float ZeroF = 0f;

        private Transform mainCam;
        private float currentSpeed;
        private float velocity;

        private void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            freeLookCam.OnTargetObjectWarped(transform,
                transform.position - freeLookCam.transform.position - Vector3.forward);
        }

        private void Start() => input.EnablePlayerActions();

        private void Update()
        {
            HandelMovement();
        }

        void HandelMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;

            // Rotate movemnt direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;

            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
                SmoothSpeed(ZeroF);
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            //Move the player
            var adjustMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedDirection);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            // adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.LookRotation(adjustedDirection);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float value) =>currentSpeed =  Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }
}