using UnityEngine;
using UnityEngine.InputSystem;

public class movePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private NIS inputAccion;

    private Vector2 moveInput;

    public float speed= 5f;

    public float jumpForce = 5f;

    public float rotationSpeed = 150f;

    private Rigidbody rb;

    public void Awake()
    {
        inputAccion = new NIS();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    private void OnMove(InputValue context)
    {
        moveInput = context.Get<Vector2>();

    }
    private void OnJump(InputValue context)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Rotación izquierda / derecha
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(0f, rotation, 0f);

        // Movimiento hacia adelante / atrás
        Vector3 movement = transform.forward * moveInput.y;

        transform.position += movement * speed * Time.deltaTime;
    }
}
