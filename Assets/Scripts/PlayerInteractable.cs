using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteractable : MonoBehaviour
{
    public abstract void OnInteraction();

    public abstract void OffInteraction();
}
