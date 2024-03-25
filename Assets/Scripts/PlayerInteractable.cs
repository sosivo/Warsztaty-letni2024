using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteractable : MonoBehaviour
{
    public abstract void OnInteraction(Player player);

    public abstract void OffInteraction(Player player);
}
