using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    Animator animator;
    PlayerManager playerManager;
    PlayerUIManager playerUIManager;

    [Header("playerMovement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;

    [Header("CameraRotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool runInput;
    public bool quickTurnInput;
    public bool aimingInput;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
            playerControls.PlayerMovement.QuickTurn.performed += i => quickTurnInput = true;
            playerControls.PlayerActions.Aim.performed += i => aimingInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimingInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleQuickTurnInput();
        HandleAimingInput();
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);

        //TEMP MOVE FIX
        if (verticalMovementInput != 0 || horizontalMovementInput != 0) 
        {
            animatorManager.rightHandIK.weight = 0;
            animatorManager.leftHandIK.weight = 0;
        }
        else
        {
            animatorManager.rightHandIK.weight = 1;
            animatorManager.leftHandIK.weight = 1;
        }
    }

    private void HandleCameraInput()
        {
        horizontalCameraInput = cameraInput.x;
        verticalCameraInput = cameraInput.y;
        }

    private void HandleQuickTurnInput()
    {
        if (playerManager.isPerformingAction)
            return;

        if (quickTurnInput)
        {
            animator.SetBool("isPerformingQuickTurn", true);
            animatorManager.PlayAnimationWithOutRootMotion("Quick Turn", true);
        }
    }

    private void HandleAimingInput()
    {
        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            aimingInput = false;
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);
            return;
        }
        if (aimingInput)
        {
            animator.SetBool("isAiming", true);
            playerUIManager.crossHair.SetActive(true);
        }
        else
        {
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);
        }

        animatorManager.UpdateAimConstraints();
    }
}
