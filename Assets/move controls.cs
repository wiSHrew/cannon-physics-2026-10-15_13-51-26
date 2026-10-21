
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class moveControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        movement = Vector3.ClampMagnitude(movement, 1f);

        if (characterController.isGrounded)
        {
            verticalVelocity.y = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(
            (movement * moveSpeed + verticalVelocity) * Time.deltaTime
        );
    }
}
