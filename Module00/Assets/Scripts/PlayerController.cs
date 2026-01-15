using UnityEngine;
using UnityEngine.InputSystem;
//Utilizza le librerie di unity

[RequireComponent(typeof(Rigidbody))] //Assicura che il GameObject abbia un componente Rigidbody
public class PlayerController : MonoBehaviour
{
    [Header("Movement")] //Intestazione per il movimento nell'ispector
    [SerializeField] private float moveSpeed = 6f; //[SerializeField] rende la variabile visibile e modificabile nell'Inspector di Unity

    [Header("Jump")] //Intestazione per il salto nell'ispector
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundMask = ~0;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake() //Viene chiamato prima di start e serve per inizializzare le variabili o componenti
    {
        rb = GetComponent<Rigidbody>();
    }

    // Collegato dal PlayerInput (Unity Events)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Collegato dal PlayerInput (Unity Events)
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsGrounded())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Vector3 desired = new Vector3(moveInput.x, 0f, moveInput.y).normalized * moveSpeed;

        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(desired.x, v.y, desired.z);
    }

    private void Jump()
    {
        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}