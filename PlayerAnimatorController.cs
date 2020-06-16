using UnityEngine;

/// <summary>
/// Handles the player's animator.
/// </summary>

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputHandling input;
    private PlayerCharacterController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponentInParent<PlayerInputHandling>();
        controller = GetComponentInParent<PlayerCharacterController>();
    }

    private void Update()
    {
        //If a horizontal movement key was pressed, play the running animation.
        animator.SetBool("Running", input.direction != 0f);

        //If the player is jumping up, play the jumping animation.
        animator.SetBool("Jumping", controller.IsJumping());

        //If the player is falling, play the falling animation.
        animator.SetBool("Falling", controller.IsFalling());

        if (controller.landed)
            animator.SetTrigger("Land");
    }
}
