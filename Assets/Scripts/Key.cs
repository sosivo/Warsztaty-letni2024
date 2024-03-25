using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PlayerInteractable
{
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    public override void OnInteract()
    {
        Debug.Log("On Interaction!");
    }
    public override void OffInteract()
    {
        Debug.Log("Off Interaction!");
    }
}
