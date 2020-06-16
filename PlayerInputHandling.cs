using UnityEngine;

/// <summary>
/// Handles input that should affect the player.
/// </summary>

public class PlayerInputHandling : MonoBehaviour
{
    /// <summary>
    /// Saves the horizontal direction given by the input.
    /// </summary>
    public float direction { get; private set; }

    /// <summary>
    /// Checks if the jump button was pressed.
    /// </summary>
    public bool jump { get; private set; }

    /// <summary>
    /// Checks if the jump button was released.
    /// </summary>
    public bool releaseJump { get; private set; }


    private void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        jump = Input.GetButtonDown("Jump");
        releaseJump = Input.GetButtonUp("Jump");
    }
}