using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField, Range(1f, 5f)] private float rotationSpeed;
    private Vector3 currentPosition;

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void Movement()
    {
        transform.Rotate(Vector3.up * 45 * rotationSpeed * Time.deltaTime);
    }   

    private void FixedUpdate()
    {
        Movement();
    }
}
