using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script allows a GameObject to act as an interactor that can trigger actions on other GameObjects.
/// It can change the material of a target renderer and toggle a light component on and off when this object is clicked.
/// </summary>
public class VRInteractor : MonoBehaviour
{
    [Header("Target Object Settings")]
    [Tooltip("The GameObject that will be affected when this object is clicked. Assign the TV, Light Fixture, etc. here.")]
    public GameObject targetObject;

    [Header("Material Swap Settings")]
    [Tooltip("Check this if you want to change the material of the target object.")]
    public bool changeMaterial = false;

    [Tooltip("The new material to apply to the target object's renderer. For example, a screen-on material for a TV.")]
    public Material newMaterial;

    [Tooltip("The original material of the target object. This will be stored automatically if left empty, but can be assigned.")]
    public Material originalMaterial;

    private Renderer targetRenderer;
    private bool isMaterialSwapped = false;

    [Header("Light Toggle Settings")]
    [Tooltip("Check this if you want to toggle a light on and off.")]
    public bool toggleLight = false;

    [Tooltip("The Light component to toggle on and off. This can be on the target object or any other object.")]
    public Light targetLight;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// We use this to get the Renderer component from our target object and store its original material.
    /// </summary>
    void Start()
    {
        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null && originalMaterial == null)
            {
                // Store the original material if it hasn't been set.
                originalMaterial = targetRenderer.material;
            }
        }
        else
        {
            Debug.LogError("Target Object not assigned in the VRInteractor script on " + gameObject.name);
        }

        if (toggleLight && targetLight == null)
        {
            Debug.LogError("Target Light not assigned in the VRInteractor script on " + gameObject.name + ", but Toggle Light is enabled.");
        }
    }

    /// <summary>
    /// This method is called in Unity when the mouse is clicked over the Collider on this GameObject.
    /// In a VR context, your VR interaction system (like VRTK, Oculus Integration, etc.) will need to call this method on the object you "point and click" at.
    /// </summary>
    public void PerformAction()
    {
        // --- Material Changing Logic ---
        if (changeMaterial && targetRenderer != null && newMaterial != null && originalMaterial != null)
        {
            if (isMaterialSwapped)
            {
                targetRenderer.material = originalMaterial;
            }
            else
            {
                targetRenderer.material = newMaterial;
            }
            isMaterialSwapped = !isMaterialSwapped; // Toggle the state
        }

        // --- Light Toggling Logic ---
        if (toggleLight && targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }
    }

    /// <summary>
    /// OnMouseDown is a built-in Unity function that is called when the user has pressed the mouse button while over the GUIElement or Collider.
    /// This is great for testing in the editor without VR.
    /// </summary>
    private void OnMouseDown()
    {
        // This allows for easy testing in the Unity Editor by just clicking the object.
        PerformAction();
    }
}
