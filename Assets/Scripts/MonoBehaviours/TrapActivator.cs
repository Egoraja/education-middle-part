using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TrapActivator : MonoBehaviour
{
    [SerializeField] private GameObject trapObject;
    [SerializeField] private float timer = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<TrapInteraction>(out TrapInteraction trapInteraction))
        {           
            trapInteraction.StartFirstPartTrapInteraction(trapObject.transform, timer);
        }
    }
}
