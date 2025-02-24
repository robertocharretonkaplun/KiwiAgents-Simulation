using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.GraphicsBuffer;

public class NeckMovement : MonoBehaviour
{
    // Reference to the neck bone and jaw bone transforms
    public Transform neckBone; 
    public Transform jawBone;


    // Speed at which the neck rotates and the maximum angle it can rotate
    public float rotationSpeed = 15f;   
    public float rotationAngle = 30f;

    // Current angle of rotation and movement direction
    private float currentAngle = 0f;
    private bool movingRight = true;

    // LateUpdate is used to ensure the rotation is applied after all other updates
    void LateUpdate()
    {
        // If the neck or jaw bones are not assigned, exit the function
        if (neckBone == null || jawBone == null) return;

        // Update the neck and jaw rotations
        UpdateNeckRotation();
        UpdateJawRotation();
    }

    /// <summary>
    /// Handles the oscillation of the neck, moving right and left within the specified range.
    /// </summary>
    void UpdateNeckRotation()
    {
        // Oscillate the neck's rotation from right to left
        currentAngle = movingRight ? currentAngle + rotationSpeed * Time.deltaTime : currentAngle - rotationSpeed * Time.deltaTime;

        // Invert the direction when the maximum angle is reached
        if (currentAngle > rotationAngle) movingRight = false;
        else if (currentAngle < -rotationAngle) movingRight = true;

        // Apply the rotation to the neck on the Y-axis
        neckBone.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }

    /// <summary>
    /// Updates the jaw rotation to follow the neck's movement.
    /// </summary>
    void UpdateJawRotation()
    {
        // The jaw follows the neck's movement with the same rotation
        jawBone.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }
}