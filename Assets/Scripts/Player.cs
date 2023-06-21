using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float rotationSpeed = 5f;

    public bool IsMoving => isMoving;
    private bool isMoving = false;


    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        var input = new Vector2(horizontal, vertical).normalized;

        var direction = new Vector3(input.x, 0, input.y);

        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        isMoving = direction.magnitude > 0;
        
        if (isMoving)
        {
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
