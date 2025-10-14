using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates an object back and forth between two target rotations when clicked.
/// Ideal for doors, levers, and switches.
/// Requires a Collider component on the same GameObject to detect clicks.
/// </summary>
public class InteractiveRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The rotation of the object in its 'active' or 'open' state. Uses local rotation.")]
    public Vector3 openRotationEuler;

    [Tooltip("The rotation of the object in its 'inactive' or 'closed' state. Uses local rotation.")]
    public Vector3 closedRotationEuler;

    [Tooltip("How long in seconds the rotation animation should take.")]
    public float rotationDuration = 1.0f;

    [Tooltip("An optional easing curve for a more dynamic animation. Leave empty for linear.")]
    public AnimationCurve easingCurve;

    // --- Private State Variables ---
    private bool isOpen = false;
    private bool isMoving = false;

    /// <summary>
    /// This method is called by Unity when the script first loads.
    /// We use it to set the object to its initial 'closed' state.
    /// </summary>
    void Start()
    {
        // Start in the closed position without animation.
        transform.localEulerAngles = closedRotationEuler;
    }

    /// <summary>
    /// This is a built-in Unity function that is called when the user clicks on a GameObject
    /// that has a Collider component. This works for mouse clicks and for most VR pointer/raycast systems.
    /// </summary>
    private void OnMouseDown()
    {
        // Prevent starting a new rotation if one is already in progress.
        if (isMoving)
        {
            return;
        }

        // Toggle the state. If it was open, now it's closing, and vice-versa.
        isOpen = !isOpen;

        // Start the rotation coroutine. This is how we animate over time.
        StartCoroutine(RotateObject());
    }

    /// <summary>
    /// A Coroutine that handles the smooth rotation animation over the specified duration.
    /// </summary>
    private IEnumerator RotateObject()
    {
        isMoving = true;
        float elapsedTime = 0f;

        // Determine our start and end points for this specific animation
        Quaternion startingRotation = transform.localRotation;
        Quaternion finalRotation = isOpen ? Quaternion.Euler(openRotationEuler) : Quaternion.Euler(closedRotationEuler);

        // Loop until the animation is complete
        while (elapsedTime < rotationDuration)
        {
            // Calculate our progress factor (a value from 0 to 1)
            float progress = elapsedTime / rotationDuration;

            // Apply the easing curve if one is provided
            float easedProgress = (easingCurve != null && easingCurve.length > 0) ? easingCurve.Evaluate(progress) : progress;

            // Slerp (Spherical Linear Interpolation) is used for smoothly interpolating between two rotations.
            transform.localRotation = Quaternion.Slerp(startingRotation, finalRotation, easedProgress);

            // Wait for the next frame before continuing the loop
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // After the loop, snap to the final rotation to ensure it's precise.
        transform.localRotation = finalRotation;
        isMoving = false;
    }
}