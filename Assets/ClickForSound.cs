using UnityEngine;

using UnityEngine;

/// <summary>
/// This script plays an AudioClip when the GameObject it's attached to is clicked.
/// 
/// REQUIREMENTS:
/// 1. The GameObject must have a Collider component (e.g., Box Collider, Sphere Collider).
/// 2. The GameObject must have an AudioSource component to play the sound.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnClick : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The sound that will play when the object is clicked.")]
    public AudioClip clickSound;

    // A private reference to the AudioSource component on this object.
    private AudioSource _audioSource;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// It's the best place to get references to other components.
    /// </summary>
    void Awake()
    {
        // Get the AudioSource component attached to this same GameObject.
        _audioSource = GetComponent<AudioSource>();

        // Optional: You can configure your AudioSource here if you want.
        // For example, to prevent a sound from playing when the scene starts:
        _audioSource.playOnAwake = false;
    }

    /// <summary>
    /// OnMouseDown is called by Unity whenever the user clicks on the Collider
    /// attached to this GameObject. This also works for VR controller pointers.
    /// </summary>
    private void OnMouseDown()
    {
        // Check if a sound clip has actually been assigned in the Inspector.
        if (clickSound != null)
        {
            // Play the assigned sound clip.
            // PlayOneShot is great for sound effects because it allows multiple sounds
            // to overlap, unlike .Play() which would restart the sound.
            _audioSource.PlayOneShot(clickSound);
        }
        else
        {
            // If no sound is assigned, print a helpful warning to the Unity Console.
            Debug.LogWarning("No click sound assigned on " + gameObject.name, this);
        }
    }
}
