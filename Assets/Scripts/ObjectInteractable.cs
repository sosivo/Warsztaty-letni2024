using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteractable : MonoBehaviour
{
    public abstract void OnInteraction();

    public abstract void OffInteraction();
}
