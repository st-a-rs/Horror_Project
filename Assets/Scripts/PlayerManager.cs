using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    InputManager inputManager;
    Animator animator;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerEquipmentManager playerEquipmentManager;

    [Header("Player Actions")]
    public bool disableRootMotion;
    public bool isPerformingAction;
    public bool isPerformingQuickTurn;
    public bool isAiming;
    
    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        disableRootMotion = animator.GetBool("disableRootMotion");
        isAiming = animator.GetBool("isAiming");
        isPerformingAction = animator.GetBool("isPerformingAction");
        isPerformingQuickTurn = animator.GetBool("isPerformingQuickTurn");
    }

    private void FixedUpdate()
    {
        playerLocomotionManager.HandleAllLocomotion();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }

    public void UseCurrentWeapon()
    {
        //use knives in future
        playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera);
    }
}
