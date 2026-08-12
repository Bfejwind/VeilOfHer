using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MementosManager : MonoBehaviour
{
    public static MementosManager Instance { get; private set; }

    private HashSet<string> collectedIDs = new HashSet<string>();

    [Header("Fires whenever a new memento is collected (passes total count)")]
    public UnityEvent<int> OnCollectionChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasCollected(string id)
    {
        return collectedIDs.Contains(id);
    }

    public void Collect(string id)
    {
        if (collectedIDs.Add(id)) // returns true only if it was newly added
        {
            OnCollectionChanged?.Invoke(collectedIDs.Count);
        }
    }

    public int GetCollectedCount() => collectedIDs.Count;
}
