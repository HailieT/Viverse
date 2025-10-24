using UnityEngine;
using System.Collections;

public class CreaturePopOut : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How far forward the creature will pop out from its starting position.")]
    public float popOutDistance = 2.0f;

    [Tooltip("How fast the creature lunges out and retreats.")]
    public float popOutSpeed = 5.0f;

    [Tooltip("How long the creature stays visible before retreating.")]
    public float stayOutDuration = 1.0f;

    [Header("Timing")]
    [Tooltip("The minimum time (in seconds) to wait before popping out again.")]
    public float minWaitTime = 5.0f;

    [Tooltip("The maximum time (in seconds) to wait before popping out again.")]
    public float maxWaitTime = 15.0f;

    [Header("Movement Settings")]
    [Tooltip("The direction and distance the object will move from its starting point.")]
    public Vector3 moveOffset = new Vector3(0f, 0f, 1f);

    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip popOutSound;

    // Private variables to store position data
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Coroutine popOutCoroutine;

    void Start()
    {
        // Store the initial hidden position of the creature
        startPosition = transform.position;

        // Calculate the target position based on the creature's forward direction
        targetPosition = startPosition + (transform.forward * popOutDistance);

        // Start the behavior loop
        popOutCoroutine = StartCoroutine(PopOutRoutine());
    }

    private IEnumerator PopOutRoutine()
    {
        // This loop will run forever, making the creature pop out repeatedly
        while (true)
        {
            // 1. Wait for a random amount of time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // --- POP OUT SEQUENCE ---

            // Optional: Play a sound
            if (audioSource != null && popOutSound != null)
            {
                audioSource.PlayOneShot(popOutSound);
            }

            // 2. Move from startPosition to targetPosition (Lunge)
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, popOutSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }
            // Ensure it's exactly at the target position
            transform.position = targetPosition;

            // 3. Stay out for a set duration
            yield return new WaitForSeconds(stayOutDuration);

            // --- RETREAT SEQUENCE ---

            // 4. Move from targetPosition back to startPosition (Retreat)
            while (Vector3.Distance(transform.position, startPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, popOutSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }
            // Ensure it's exactly back at the start position
            transform.position = startPosition;
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Use the current position if in edit mode, or the stored original position if in play mode.
        Vector3 startPos = Application.isPlaying ? startPosition : transform.position;
        Vector3 endPos = startPos + moveOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawSphere(endPos, 0.1f);
    }
}