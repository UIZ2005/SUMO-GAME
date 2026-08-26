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
    public GameObject[] players;

    // para el empuje
    public float fuerzaEmpuje;
    public float radioAtaque;
    public LayerMask objetoEmpujable;
    public float tiempoEsperaEmpuje;
    public float ultimoTiempoEmpuje;

    public GameObject prefabParticulasEmpuje;


    // para el salto
    public Transform puntoPiso;      
    public float groundCheckRadius = 0.2f;
    public LayerMask piso;      // layer del piso
    public bool estaPiso;

    private Coroutine powerUpCoroutine;
    public bool isdead=false;
    public GameObject canva;
    private GameManager manager;
    public int numplayer;

    public void Awake()
    {
        inputAccion = new NIS();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
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
        if (isdead) return;
        // Verifica si est� tocando el suelo
        estaPiso = Physics.CheckSphere(puntoPiso.position, groundCheckRadius, piso);

        // Rotaci�n izquierda / derecha
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);
        // Movimiento hacia adelante / atr�s
        Vector3 movement = transform.forward * moveInput.y;
        transform.position += movement * speed * Time.deltaTime;
    }
    /// empuje
    private void OnStart(InputValue context)
    {
        manager.start();
    }
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
        Collider[] hits = Physics.OverlapSphere(transform.position, radioAtaque, objetoEmpujable);
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

        // Activar explosión de partículas desde el centro del jugador
        if (prefabParticulasEmpuje != null)
        {
            Instantiate(prefabParticulasEmpuje, transform.position, Quaternion.identity);
        }
    }

    public void ActivarPowerUp(float duracion)
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);

            fuerzaEmpuje /= 2f;
            radioAtaque /= 1.5f;
        }

        powerUpCoroutine = StartCoroutine(PowerUpCoroutine(duracion));
    }

    private System.Collections.IEnumerator PowerUpCoroutine(float duracion)
    {
        fuerzaEmpuje *= 2f;
        radioAtaque *= 1.5f;

        yield return new WaitForSeconds(duracion);

        fuerzaEmpuje /= 2f;
        radioAtaque /= 1.5f;

        powerUpCoroutine = null;
    }
    public void skinplayer(int jugador)
    {
        numplayer = jugador;
        players[jugador-1].SetActive(true);
    }

}