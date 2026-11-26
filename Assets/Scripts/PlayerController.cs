using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public CharacterController controller;
    public float walkSpeed = 12f;
    public float sprintSpeed = 20f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Agacharse")]
    public float standingHeight = 1.8f;
    public float crouchHeight = 0.9f;

    private float currentSpeed;
    private Vector2 moveInput;
    private Vector3 velocity;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        // Bloquea el cursor en el centro de la pantalla para jugar en FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Resetear gravedad si toca el suelo
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Solución "Investigar": si la cabeza choca con un techo, anular impulso vertical
        if ((controller.collisionFlags & CollisionFlags.Above) != 0 && velocity.y > 0)
        {
            velocity.y = 0f;
        }

        // 2. Lectura del Nuevo Input System (Teclado)
        if (Keyboard.current != null)
        {
            float x = 0f;
            float z = 0f;

            if (Keyboard.current.wKey.isPressed) z = 1f;
            if (Keyboard.current.sKey.isPressed) z = -1f;
            if (Keyboard.current.aKey.isPressed) x = -1f;
            if (Keyboard.current.dKey.isPressed) x = 1f;

            // Correr con Left Shift
            currentSpeed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : walkSpeed;

            // Agacharse con Q
            controller.height = Keyboard.current.qKey.isPressed ? crouchHeight : standingHeight;

            moveInput = new Vector2(x, z).normalized;

            // Salto único con Space
            if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // 3. Cálculo de dirección y gravedad
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = (move * currentSpeed) + (Vector3.up * velocity.y);
        controller.Move(finalMovement * Time.deltaTime);
    }
}