using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goo : MonoBehaviour
{
    [field: SerializeField] public List<string> AffectedTags { get; private set; }
    [field: SerializeField] public float SafetyMargin { get; private set; }

    private BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();

        // Reduce collider size by the safety margin regardless of transform scale
        var localScale = transform.localScale;
        var colSize = col.size * localScale;
        colSize -= Vector2.one * SafetyMargin;

        // Prevent colSize from being negative and keep the minimum size very small
        colSize = new Vector2(Mathf.Max(colSize.x, 0.01f), Mathf.Max(colSize.y, 0.01f)); 
        
        col.size = colSize / localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AffectedTags.Contains(other.gameObject.tag))
        {
            other.GetComponent<IKillable>().Kill();
        }
    }
}
