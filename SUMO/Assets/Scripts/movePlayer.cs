using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class movePlayer : MonoBehaviour
{
  
    private NIS inputAccion;
    private Vector2 moveInput;
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 150f;
    private Rigidbody rb;


    // para el empuje
    public float fuerzaEmpuje;
    public float radioAtaque;
    public LayerMask objetoEmpujable;
    public float tiempoEsperaEmpuje;
    public float ultimoTiempoEmpuje;


    // para el salto
    public Transform puntoPiso;      
    public float groundCheckRadius = 0.2f;
    public LayerMask piso;      // layer del piso
    private bool estaPiso;
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
        if (estaPiso)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Verifica si está tocando el suelo
        estaPiso = Physics.CheckSphere(puntoPiso.position, groundCheckRadius, piso);

        // Rotación izquierda / derecha
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
        // Movimiento hacia adelante / atrás
        Vector3 movement = transform.forward * moveInput.y;
        transform.position += movement * speed * Time.deltaTime;
    }
    /// empuje

    private void OnPush(InputValue context)
    {
        if (Time.time >= ultimoTiempoEmpuje + tiempoEsperaEmpuje)
        {
            Debug.Log("se puede hacer empuje");
            DoPush();

            ultimoTiempoEmpuje = Time.time;
        }
    }
    private void DoPush()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 0.5f, radioAtaque, objetoEmpujable);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            Debug.Log("se detecto un objeto a empujar");
            Rigidbody otherRb = hit.attachedRigidbody;
            if (otherRb != null)
            {
                Vector3 pushDir = (otherRb.transform.position - transform.position).normalized;
                pushDir.y = 0;
                otherRb.AddForce(pushDir * fuerzaEmpuje, ForceMode.Impulse);
            }
        }
    }
}