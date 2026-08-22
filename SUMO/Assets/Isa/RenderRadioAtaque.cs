using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RenderRadioAtaque : MonoBehaviour
{
    private movePlayer jugador;

    [Header("Configuración visual")]
    public int segmentos = 50;
    public float alturaSuelo = 0.05f;

    [Header("Colores según estado")]
    public Color colorDisponible = new Color(0.2f, 0.5f, 1f, 0.5f); // azulito, con transparencia
    public Color colorEnCooldown = new Color(0.5f, 0.5f, 0.5f, 0.5f); // gris, con transparencia

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private float radioAnterior = -1f; // para no regenerar la malla si el radio no cambió

    void Awake()
    {
        jugador = GetComponent<movePlayer>();
        if (jugador == null)
            jugador = GetComponentInParent<movePlayer>();

        if (jugador == null)
        {
            Debug.LogError($"[RenderRadioAtaque] No se encontró movePlayer en '{gameObject.name}' ni en sus padres.");
        }

        // Configurar el mesh
        mesh = new Mesh();
        mesh.name = "CirculoRelleno";
        GetComponent<MeshFilter>().mesh = mesh;

        // Configurar el material (transparente, sin luces, para que el color se vea plano y limpio)
        meshRenderer = GetComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material = mat;
    }

    void Update()
    {
        if (jugador == null) return;

        // Solo regenera la geometría si el radio cambió (optimización simple)
        if (!Mathf.Approximately(radioAnterior, jugador.radioAtaque))
        {
            GenerarCirculo(jugador.radioAtaque);
            radioAnterior = jugador.radioAtaque;
        }

        ActualizarPosicion();
        ActualizarColor();
    }

    private void GenerarCirculo(float radio)
    {
        mesh.Clear();

        // +1 porque el vértice 0 es el centro del abanico de triángulos
        Vector3[] vertices = new Vector3[segmentos + 1];
        int[] triangles = new int[segmentos * 3];

        vertices[0] = Vector3.zero; // centro

        for (int i = 0; i < segmentos; i++)
        {
            float angulo = ((float)i / segmentos) * 2f * Mathf.PI;
            float x = Mathf.Cos(angulo) * radio;
            float z = Mathf.Sin(angulo) * radio;
            vertices[i + 1] = new Vector3(x, 0f, z);
        }

        // Arma los triángulos como un abanico: centro + dos puntos consecutivos del borde
        for (int i = 0; i < segmentos; i++)
        {
            int actual = i + 1;
            int siguiente = (i + 1) % segmentos + 1;

            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = actual;
            triangles[i * 3 + 2] = siguiente;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void ActualizarPosicion()
    {
        Vector3 centro = jugador.transform.position + jugador.transform.forward * 0.5f;
        centro.y = alturaSuelo;
        transform.position = centro;
    }

    private void ActualizarColor()
    {
        bool puedeEmpujar = Time.time >= jugador.ultimoTiempoEmpuje + jugador.tiempoEsperaEmpuje;
        meshRenderer.material.color = puedeEmpujar ? colorDisponible : colorEnCooldown;
    }
}