using UnityEngine;

/// <summary>
/// Translates the input from <c>PlayerInputHandling</c> to <c>PlayerCharacterController</c>.
/// </summary>

[RequireComponent(typeof(PlayerInputHandling))]
[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandling input;
    private PlayerCharacterController controller;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandling>();
        controller = GetComponent<PlayerCharacterController>();
    }

    private void Update()
    {
        //Ask the controller to stop the jump if the jump key is released.
        controller.StopJumping(input.releaseJump);
    }

    private void FixedUpdate()
    {
        //Moves depending on the input and reset the jump input.
        controller.Move(input.direction, input.jump);
    }
}
