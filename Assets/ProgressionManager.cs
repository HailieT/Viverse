using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent

// This class defines what a "step" in your progression looks like.
// [System.Serializable] makes it show up in the Unity Inspector.
[System.Serializable]
public class ProgressionStep
{
    [Tooltip("A description of what this step does, for your reference.")]
    public string description;

    [Tooltip("The collection count that will trigger this event.")]
    public int countToTrigger;

    [Tooltip("The events that will fire when this count is reached.")]
    public UnityEvent onTrigger;
}

/// <summary>
/// A singleton manager that tracks a progression count and fires
/// UnityEvents at specific count milestones.
/// </summary>
public class ProgressionManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    // This makes the manager accessible from anywhere using "ProgressionManager.Instance"
    public static ProgressionManager Instance { get; private set; }

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set this as the one and only instance
        Instance = this;

        // Optional: Uncomment this if your manager needs to persist between scene loads
        // DontDestroyOnLoad(gameObject);
    }
    // --- End Singleton Pattern ---

    [Header("Progression State")]
    [Tooltip("The current number of items collected.")]
    [SerializeField]
    private int collectionCount = 0;

    [Header("Progression Events")]
    [Tooltip("Define all the steps of your scene's progression here.")]
    [SerializeField]
    private ProgressionStep[] progressionSteps;

    /// <summary>
    /// Call this method from other scripts (like your collectible) to
    /// increment the counter and check for new events.
    /// </summary>
    public void RegisterCollection()
    {
        // 1. Increment the count
        collectionCount++;
        Debug.Log($"Progression count is now: {collectionCount}");

        // 2. Check if this new count matches any progression step
        foreach (ProgressionStep step in progressionSteps)
        {
            if (step.countToTrigger == collectionCount)
            {
                // 3. If it matches, fire the event(s)
                Debug.Log($"Triggering progression step for count {collectionCount}: {step.description}");
                step.onTrigger.Invoke();

                // We break here assuming only one event fires per count.
                // Remove 'break;' if you want multiple events with the same count to fire.
                break;
            }
        }
    }

    /// <summary>
    /// Public getter to allow other scripts to check the current count if needed.
    /// </summary>
    public int GetCurrentCount()
    {
        return collectionCount;
    }
}