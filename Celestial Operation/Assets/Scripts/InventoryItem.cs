using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    /// <summary>
    /// Show this item, while it is currently equipped.
    /// </summary>
    abstract public void Activate();

    /// <summary>
    /// Hide this item, while it is not currently equipped.
    /// </summary>
    abstract public void Deactivate();
}
