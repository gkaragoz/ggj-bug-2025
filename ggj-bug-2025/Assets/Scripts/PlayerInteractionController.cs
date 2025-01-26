using System;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerInteractionController : MonoBehaviour
{
    public static Action<Collider, int> OnEnterZone;
    public static Action<Collider, int> OnExitZone;
    
    
    

    private void OnTriggerEnter(Collider other)
    {
        OnEnterZone?.Invoke(other, 0);
    }
    
    private void OnTriggerExit(Collider other)
    {
        OnExitZone?.Invoke(other, 0);
    }
}