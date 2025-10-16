using System.Collections;
using UnityEngine;


/// <summary>
/// This script moves a GameObject when it is GRABBED by a VR controller.
/// It's designed to toggle between an initial position and an offset position.
/// This is ideal for objects you "grab" to interact with, like chairs or drawers.
///
/// --- THIS IS THE VR-READY VERSION FOR GRABBING ---
///
/// HOW TO USE:
/// 1. If you have the old script on your object, remove it.
/// 2. Add THIS script to your object (e.g., the chair).
/// 3. Add a Rigidbody component: Go to Add Component -> Physics -> Rigidbody.
///    In the Rigidbody settings, CHECK the "Is Kinematic" box. This is crucial.
/// 4. IMPORTANT: Add an "XR GRAB Interactable" component to the same object.
///    (If you have an "XR Simple Interactable", remove it).
/// 5. In the "XR Grab Interactable" component, find the "Select Entered" event. This event
///    fires when you successfully grab the object.
/// 6. Click the '+' to add an event.
/// 7. Drag the object itself into the 'Object' field below "Runtime Only".
/// 8. From the function dropdown, select "ClickToMoveVR" -> "PerformGrabAction()".
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))] // Ensures the correct interactable is attached
public class ClickToMoveVR : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The direction and distance the object will move from its starting point.")]
    public Vector3 moveOffset = new Vector3(0f, 0f, 1f);

    [Tooltip("How fast the object moves to the target position.")]
    public float moveSpeed = 2f;

    // Private variables to track the state
    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private bool _isMoved = false;
    private Coroutine _moveCoroutine;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Store the starting position of the object.
        _originalPosition = transform.position;
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // This prevents the object from being physically moved by the controller.
        _grabInteractable.trackPosition = false;
        _grabInteractable.trackRotation = false;
    }

    /// <summary>
    /// This public method is called by the XR Grab Interactable's "Select Entered" event.
    /// </summary>
    public void PerformGrabAction()
    {
        // Toggle the state between moved and original position.
        _isMoved = !_isMoved;

        // Determine the target position based on the new state.
        _targetPosition = _isMoved ? _originalPosition + moveOffset : _originalPosition;

        // --- Start the movement ---
        // Stop any existing movement to avoid conflicts if the object is grabbed again quickly.
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(MoveObject(_targetPosition));
    }

    /// <summary>
    /// A coroutine that smoothly moves the object from its current position to a target position.
    /// </summary>
    private IEnumerator MoveObject(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Snap to the final position to ensure accuracy.
        transform.position = target;
        _moveCoroutine = null;
    }

    /// <summary>
    /// Draws a line in the editor to visualize the movement path.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Use the current position if in edit mode, or the stored original position if in play mode.
        Vector3 startPos = Application.isPlaying ? _originalPosition : transform.position;
        Vector3 endPos = startPos + moveOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawSphere(endPos, 0.1f);
    }
}

