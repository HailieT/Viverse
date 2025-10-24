using UnityEngine;
using UnityEngine.Events;
using TMPro; // Required for TextMeshProUGUI

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

    [Header("UI")]
    [Tooltip("The TextMeshPro component to display the current collection count.")]
    public TMPro.TextMeshProUGUI valueText; // Correctly uses TMPro type

    [Tooltip("The total number of collectibles in the scene.")]
    public int maxCollectionCount = 9;

    [Header("Progression Events")]
    [Tooltip("Define all the steps of your scene's progression here.")]
    [SerializeField]
    private ProgressionStep[] progressionSteps;

    private void Start()
    {
        // Initial setup of the UI when the scene starts.
        UpdateText();
    }

    /// <summary>
    /// Call this method from other scripts (like your collectible) to
    /// increment the counter and check for new events.
    /// Example usage: ProgressionManager.Instance.RegisterCollection();
    /// </summary>
    public void RegisterCollection()
    {
        // 1. Increment the count
        collectionCount++;
        Debug.Log($"Progression count is now: {collectionCount}");

        // 2. Update the UI text immediately
        UpdateText();

        // 3. Check if this new count matches any progression step
        foreach (ProgressionStep step in progressionSteps)
        {
            if (step.countToTrigger == collectionCount)
            {
                // 4. If it matches, fire the event(s)
                Debug.Log($"Triggering progression step for count {collectionCount}: {step.description}");
                step.onTrigger.Invoke();

                // We break here assuming only one event fires per count.
                break;
            }
        }
    }

    /// <summary>
    /// Updates the UI Text component to show the current progress.
    /// </summary>
    private void UpdateText()
    {
        if (valueText != null) // Check if the UI component is assigned
        {
            valueText.text = $"{collectionCount}/{maxCollectionCount}";
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