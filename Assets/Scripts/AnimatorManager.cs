using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    [Header("Hand IK Constraints")]
    public TwoBoneIKConstraint rightHandIK; // Enable character to hold weapon properly
    public TwoBoneIKConstraint leftHandIK;

    [Header("Aiming Constraints")]
    public MultiAimConstraint spine01; //Constraints turn the characer toward Aiming Target
    public MultiAimConstraint spine02;
    public MultiAimConstraint head;

    RigBuilder rigBuilder;
    PlayerManager playerManager;
    PlayerLocomotionManager playerLocomotionManager;


    float snappedHorizontal;
    float snappedVertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    public void PlayAnimationWithOutRootMotion(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if (horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }


        if  (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isRunning && snappedVertical > 0) //No running backwards.
        {
            snappedVertical = 2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }
    //While aiming character will turn toward center of screen
    
    public void UpdateAimConstraints()
    {
        if (playerManager.isAiming)
        {
            spine01.weight = 0.3f;
            spine02.weight = 0.3f;
            head.weight = 0.7f;
        }
        else
        {
            spine01.weight = 0f;
            spine02.weight = 0f;
            head.weight = 0f;
        }
    }
    private void OnAnimatorMove()
    {
        if (playerManager.disableRootMotion)
            return;

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;

        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidBody.drag = 0;
        playerLocomotionManager.playerRigidBody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }

}
