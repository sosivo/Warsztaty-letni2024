using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeriodicTimer : MonoBehaviour
{
    public enum InteractionMode
    {
        SetOn,
        SetOff,
        Toggle
    }
    
    [Serializable]
    public class TimerConnection
    {
        [field: SerializeField] public ObjectInteractable Interactable { get; set; }
        [field: SerializeField] public InteractionMode InteractionMode { get; set; }
        [field: SerializeField, Range(0, 1)] public float PhaseShift { get; set; }

        public bool Handled { get; private set; }
        
        private bool lastState;

        public void Trigger()
        {
            switch (InteractionMode)
            {
                case InteractionMode.SetOn:
                    Interactable.OnInteraction();
                    lastState = true;
                    break;
                case InteractionMode.SetOff:
                    Interactable.OffInteraction();
                    lastState = false;
                    break;
                case InteractionMode.Toggle:
                    if (lastState)
                        Interactable.OffInteraction();
                    else
                        Interactable.OnInteraction();
                    lastState ^= true;
                    break;
            }

            Handled = true;
        }

        public void Reset()
        {
            Handled = false;
        }
    }
    
    [field: SerializeField] public List<TimerConnection> Connections { get; private set; }
    [field: SerializeField] public float CycleTime { get; private set; } = 2f;

    private Image timerIcon;

    private float lastCycleTime;

    private void Start()
    {
        ResetTimer();
        timerIcon = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        var timeInCycle = Time.time - lastCycleTime;

        timerIcon.fillAmount = Mathf.Clamp01(timeInCycle / CycleTime);

        foreach (var connection in Connections)
        {
            var triggerTime = CycleTime * connection.PhaseShift;
            if (timeInCycle > triggerTime && !connection.Handled)
                connection.Trigger();
        }
        
        if (timeInCycle > CycleTime) ResetTimer();
    }

    public void ResetTimer()
    {
        lastCycleTime = Time.time;
        foreach (var connection in Connections)
            connection.Reset();
    }

    private void OnDrawGizmosSelected()
    {
        const float progressBarWidth = 0.8f;
        
        foreach (var connection in Connections)
        {
            if (connection.Interactable is null)
                continue;
            
            var interactablePos = connection.Interactable.transform.position;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, interactablePos);
            Gizmos.DrawWireSphere(interactablePos, 0.3f);
            
            Gizmos.DrawWireCube(interactablePos + Vector3.up * 0.5f, new Vector3(progressBarWidth, 0.15f));

            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(interactablePos + Vector3.up * 0.5f - Vector3.right * (progressBarWidth - 0.05f) / 2 + Vector3.right * (connection.PhaseShift * (progressBarWidth - 0.05f)) / 2, new Vector3(connection.PhaseShift * (progressBarWidth - 0.05f), 0.1f));
        }
    }
}
