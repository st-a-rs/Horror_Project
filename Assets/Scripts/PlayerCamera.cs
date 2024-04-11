using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public InputManager inputManager;
    PlayerManager playerManager;

    public Transform cameraPivot;
    public Camera cameraObject;

    [Header("Camera Follow Targets")]
    public GameObject player; //Follows player while not aiming
    public Transform aimedCameraPosition; //Follows this position while aiming

    Vector3 cameraFollowVelocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 cameraRotation;
    Quaternion targetRotation;

    [Header("Camera Speeds")]
    public float cameraSmoothTime = 0.2f;
    public float aimedCameraSmoothTime = 3f;

    float lookAmountVertical;
    float lookAmountHorizontal;
    float maximumPivotAngle = 15;
    float minimumPivotAngle = -15;

    private void Awake()
    {
        inputManager = player.GetComponent<InputManager>();
        playerManager = player.GetComponent<PlayerManager>();
    }

    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
    }
    
    private void FollowPlayer()
    {
        if (playerManager.isAiming)
        {
            targetPosition = Vector3.SmoothDamp(transform.position, aimedCameraPosition.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
        else
        {
            targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
    }
    
    private void RotateCamera()
    {
        if (playerManager.isAiming)
        {
            cameraPivot.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);


            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, aimedCameraSmoothTime);
            transform.rotation = targetRotation;


            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, aimedCameraSmoothTime);
            cameraObject.transform.localRotation = targetRotation;
        }
        else
        {
            cameraObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);


            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
            transform.rotation = targetRotation;

            if (inputManager.quickTurnInput)
            {
                inputManager.quickTurnInput = false;
                lookAmountVertical = lookAmountVertical + 180;
                cameraRotation.y = cameraRotation.y + 180;
                transform.rotation = targetRotation;
                // SMOOTHER TRANSITION

            }

            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
            cameraPivot.localRotation = targetRotation;
        }
    }
}
