using UnityEngine;

/// <summary>
/// An example script for an item that can be collected.
/// In a VR project, you would call 'TriggerCollection()' from a
/// UnityEvent like "Select Entered" on an XRGrabInteractable.
/// </summary>
public class CollectibleItem : MonoBehaviour
{
    [Tooltip("Optional: Effect to spawn when collected (e.g., particles, sound)")]
    public GameObject collectionEffectPrefab;

    // --- How to call this in VR ---
    // 1. Add an "XR Grab Interactable" component to this object.
    // 2. Find the "Interactable Events" -> "Select Entered" event.
    // 3. Click the '+'.
    // 4. Drag this GameObject (with this script) into the "Object" slot.
    // 5. From the dropdown, select "CollectibleItem" -> "TriggerCollection()".
    //
    // Now, when you grab the object in VR, it will call this function.

    /// <summary>
    /// This is the public function you call to register the collection.
    /// </summary>
    public void TriggerCollection()
    {
        // 1. Tell the manager a collection happened
        if (ProgressionManager.Instance != null)
        {
            ProgressionManager.Instance.RegisterCollection();
        }
        else
        {
            Debug.LogError("ProgressionManager is missing from the scene!");
            return; // Stop if there's no manager
        }

        // 2. (Optional) Spawn a cool effect
        if (collectionEffectPrefab != null)
        {
            Instantiate(collectionEffectPrefab, transform.position, transform.rotation);
        }

        // 3. Destroy this collectible item
        Destroy(gameObject);
    }

    // --- Alternative for VR: Trigger Collider ---
    // You could also attach a Collider set to "Is Trigger"
    // and call this when the player's hand enters it.
    private void OnTriggerEnter(Collider other)
    {
        // Make sure your VR hand has a specific Tag, e.g., "PlayerHand"
        if (other.CompareTag("PlayerHand"))
        {
            TriggerCollection();
        }
    }
}
