using System.Collections;
using UnityEngine;

/// <summary>
/// This script moves a GameObject when it is clicked or triggered by a VR controller.
/// It's designed to toggle between an initial position and an offset position.
/// Make sure the GameObject has a Collider component attached.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ClickToMove : MonoBehaviour
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

    /// <summary>
    /// Called when the script instance is being loaded.
    /// We use Awake to ensure the original position is stored before anything else happens.
    /// </summary>
    void Awake()
    {
        // Store the starting position of the object.
        _originalPosition = transform.position;
    }

    /// <summary>
    /// This method is called by Unity when the Collider on this GameObject is clicked by the mouse,
    /// or targeted and triggered by many common VR interaction systems.
    /// </summary>
    private void OnMouseDown()
    {
        // Toggle the state between moved and original position.
        _isMoved = !_isMoved;

        // Determine the target position based on the new state.
        if (_isMoved)
        {
            // Calculate the target position by adding the offset to the original position.
            _targetPosition = _originalPosition + moveOffset;
        }
        else
        {
            // If not in the "moved" state, the target is the original position.
            _targetPosition = _originalPosition;
        }

        // --- Start the movement ---
        // If a movement coroutine is already running, stop it first.
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        // Start a new coroutine to handle the smooth movement.
        _moveCoroutine = StartCoroutine(MoveObject(_targetPosition));
    }

    /// <summary>
    /// A coroutine that smoothly moves the object from its current position to a target position.
    /// </summary>
    /// <param name="target">The position to move to.</param>
    private IEnumerator MoveObject(Vector3 target)
    {
        // Continue moving as long as the object is not very close to the target.
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            // Use Vector3.Lerp for smooth (linearly interpolated) movement.
            // Time.deltaTime makes the movement frame-rate independent.
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);

            // Wait until the next frame before continuing the loop.
            yield return null;
        }

        // To ensure the object reaches the exact target position, snap it to the target at the end.
        transform.position = target;
        _moveCoroutine = null; // Clear the coroutine reference
    }

    /// <summary>
    /// This is for visualization in the Unity Editor only. It draws a line
    /// showing where the object will move to.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Ensure this only runs in the editor and not in the build.
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + moveOffset;
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawSphere(endPos, 0.1f);
        }
        else // If the game is playing, draw based on the calculated original position
        {
            Gizmos.color = Color.cyan;
            Vector3 endPos = _originalPosition + moveOffset;
            Gizmos.DrawLine(_originalPosition, endPos);
            Gizmos.DrawSphere(endPos, 0.1f);
        }
    }
}
