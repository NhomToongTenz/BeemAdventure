using System.Collections;
using Cinemachine;
using DuAn1._Project.Scripts.SampleTest;
using KBCore.Refs;
using UnityEngine;

namespace DuAn1._Project.Scripts.Input_System
{
    public class CameraManager : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Anywhere] private InputReader input;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLoockCam;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] private float speedMultiplier = 1f;

        private bool isRMBPressed;
        private bool isDeviceMouse;
        private bool cameraMovementLock;

        private void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if(cameraMovementLock) return;

            if(isDeviceMouse && !isRMBPressed) return;

            // If the device is mouse use fixedDeltaTime, otherwise use delta Time
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            // Set the camera axis value
            freeLoockCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLoockCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;

        }

        private void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;

            //Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Resset the camera axis to prevent jumping when re-enabling mouse control
            freeLoockCam.m_XAxis.m_InputAxisValue = 0f;
            freeLoockCam.m_YAxis.m_InputAxisValue = 0f;
        }

        private void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;

            // Lock the cusor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());

        }

        private IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }
    }

}