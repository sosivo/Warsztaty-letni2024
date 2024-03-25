using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PlayerInteractable
{
    [System.Serializable]
    struct ObjectInteraction
    {
        public ObjectInteractable KeyActivatedObject;
        public bool OnInteraction;
        public bool OffInteraction;
    }

    [SerializeField]
    List<ObjectInteraction> _interactions;
    public override void OnInteraction()
    {
        Debug.Log("On Interaction!");
    }
    public override void OffInteraction()
    {
        Debug.Log("Off Interaction!");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out ObjectInteractable foundInteractable))
        {
            foreach(ObjectInteraction interaction in _interactions)
            {
                if (interaction.KeyActivatedObject != foundInteractable)
                    continue;
                if (interaction.OnInteraction)
                {
                    foundInteractable.OnInteraction();
                }
                if(interaction.OffInteraction)
                { 
                    foundInteractable.OffInteraction();
                }            
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        foreach (ObjectInteraction interaction in _interactions)
        {
            Gizmos.color = Color.red;
            if (interaction.KeyActivatedObject != null)
                Gizmos.DrawLine(transform.position, interaction.KeyActivatedObject.transform.position);
        }
    }
}
