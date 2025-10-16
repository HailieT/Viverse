using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script rotates a GameObject when it is interacted with by a VR controller.
/// It's designed to toggle between an initial rotation and an offset rotation,
/// perfect for objects like doors, lids, or levers.
/// 
/// HOW TO USE IN VR:
/// 1. Add this script to your object (e.g., a door).
/// 2. Add an "XR Simple Interactable" component to the same object.
/// 3. In the "XR Simple Interactable" component, find the "Select Entered" event.
/// 4. Click the '+' to add an event.
/// 5. Drag the object itself into the 'Object' field.
/// 6. From the function dropdown, select "RotateOnSelectVR" -> "ToggleRotation()".
/// </summary>
[RequireComponent(typeof(Collider))]
public class RotateOnSelectVR : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The rotation to apply when selected. Use the Y-axis for a standard door.")]
    public Vector3 rotationOffset = new Vector3(0f, 90f, 0f);

    [Tooltip("How fast the object rotates to the target angle.")]
    public float rotationSpeed = 2f;

    // Private variables to track the state
    private Quaternion _originalRotation;
    private Quaternion _targetRotation;
    private bool _isRotated = false;
    private Coroutine _rotateCoroutine;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Store the starting rotation of the object.
        _originalRotation = transform.rotation;
    }

    /// <summary>
    /// This is a public method that can be called by other scripts or Unity Events.
    /// In VR, we trigger this using the "Select Entered" event on an XR Interactable component.
    /// </summary>
    public void ToggleRotation()
    {
        // Toggle the state between rotated and original rotation.
        _isRotated = !_isRotated;

        // Determine the target rotation based on the new state.
        // We multiply Quaternions to combine the rotations.
        _targetRotation = _isRotated ? _originalRotation * Quaternion.Euler(rotationOffset) : _originalRotation;

        // --- Start the rotation ---
        if (_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
        }
        _rotateCoroutine = StartCoroutine(RotateObject(_targetRotation));
    }

    /// <summary>
    /// A coroutine that smoothly rotates the object to a target rotation.
    /// </summary>
    private IEnumerator RotateObject(Quaternion target)
    {
        // Loop until the angle between the current and target rotation is very small.
        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            // Slerp (Spherical Linear Interpolation) is used for smooth rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the final rotation to ensure accuracy.
        transform.rotation = target;
        _rotateCoroutine = null;
    }

    /// <summary>
    /// Draws a helper line in the editor to visualize the open direction.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Determine the start rotation based on whether we are in Play Mode or Edit Mode.
        Quaternion startRot = Application.isPlaying ? _originalRotation : transform.rotation;

        // Calculate the "open" rotation and the direction vector.
        Quaternion endRot = startRot * Quaternion.Euler(rotationOffset);
        Vector3 forwardDir = endRot * Vector3.forward;

        // Draw a line from the object's position showing where it will face when open.
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, forwardDir * 1.5f);
    }
}