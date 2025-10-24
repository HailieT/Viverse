using UnityEngine;
using UnityEngine.Events;

// This class defines what a "step" in your progression looks like.
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