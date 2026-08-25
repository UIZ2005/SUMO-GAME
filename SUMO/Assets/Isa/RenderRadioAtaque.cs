using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RenderRadioAtaque : MonoBehaviour
{
    private movePlayer jugador;

    [Header("Configuración visual")]
    public int segmentos = 50;
    public float offsetAltura = 0.05f;

    public enum ColorJugador { Rojo, Rosa, Verde, Azul }

   
    public ColorJugador colorSeleccionado = ColorJugador.Rojo;

   
    public Color colorRojo = new Color(1f, 0.2f, 0.2f, 0.5f);
    public Color colorRosa = new Color(1f, 0.4f, 0.7f, 0.5f);
    public Color colorVerde = new Color(0.3f, 1f, 0.3f, 0.5f);
    public Color colorAzul = new Color(0.2f, 0.5f, 1f, 0.5f);

    
    public Color colorEnCooldown = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private float radioAnterior = -1f;

    void Awake()
    {
        jugador = GetComponent<movePlayer>();
        if (jugador == null)
            jugador = GetComponentInParent<movePlayer>();
        if (jugador == null)
        {
            Debug.LogError($"No se encontró movePlayer en '{gameObject.name}' ni en sus padres.");
        }
        mesh = new Mesh();
        mesh.name = "CirculoRelleno";
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material = mat;
    }

    void Update()
    {
        if (jugador == null) return;

        bool debeVerse = jugador.estaPiso;
        meshRenderer.enabled = debeVerse;

        if (!debeVerse) return;

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
        Vector3[] vertices = new Vector3[segmentos + 1];
        int[] triangles = new int[segmentos * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < segmentos; i++)
        {
            float angulo = ((float)i / segmentos) * 2f * Mathf.PI;
            float x = Mathf.Cos(angulo) * radio;
            float z = Mathf.Sin(angulo) * radio;
            vertices[i + 1] = new Vector3(x, 0f, z);
        }
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
        Vector3 centro = jugador.transform.position;
        centro.y = jugador.puntoPiso.position.y + offsetAltura;
        transform.position = centro;
    }

    private void ActualizarColor()
    {
        bool puedeEmpujar = Time.time >= jugador.ultimoTiempoEmpuje + jugador.tiempoEsperaEmpuje;
        meshRenderer.material.color = puedeEmpujar ? ObtenerColorDisponible() : colorEnCooldown;
    }

    private Color ObtenerColorDisponible()
    {
        switch (colorSeleccionado)
        {
            case ColorJugador.Rojo: return colorRojo;
            case ColorJugador.Rosa: return colorRosa;
            case ColorJugador.Verde: return colorVerde;
            case ColorJugador.Azul: return colorAzul;
            default: return colorRojo;
        }
    }
}