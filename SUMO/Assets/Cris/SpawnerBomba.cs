using UnityEngine;

public class SpawnerBomba : MonoBehaviour
{
    [SerializeField] GameObject prefabBomba;
    [SerializeField] float tiempoEntreBombas = 7f;
    [SerializeField] float radioRespawn = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(CrearBomba), 0f, tiempoEntreBombas);
    }

    private void CrearBomba()
    {
        Vector2 posicionAleatoria = Random.insideUnitCircle * radioRespawn;

        Vector3 posicion = new Vector3(
            transform.position.x + posicionAleatoria.x,
            transform.position.y,
            transform.position.z + posicionAleatoria.y
        );

        Instantiate(prefabBomba, posicion, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radioRespawn);
    }
}
