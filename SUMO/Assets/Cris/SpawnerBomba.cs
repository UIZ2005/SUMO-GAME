using UnityEngine;

public class SpawnerBomba : MonoBehaviour
{
    [SerializeField] GameObject prefabBomba;
    [SerializeField] GameObject prefabPowerUp;

    [SerializeField] float tiempoEntreBombas = 7f;
    [SerializeField] float radioRespawn = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(CrearObjeto), 0f, tiempoEntreBombas);
    }

    private void CrearObjeto()
    {
        // Posición aleatoria dentro del radio
        Vector2 posicionAleatoria = Random.insideUnitCircle * radioRespawn;

        Vector3 posicion = new Vector3(
            transform.position.x + posicionAleatoria.x,
            transform.position.y,
            transform.position.z + posicionAleatoria.y
        );

        // Número aleatorio entre 0 y 1
        float probabilidad = Random.value;

        // 65% bomba, 35% Power Up
        if (probabilidad <= 0.65f)
        {
            Instantiate(prefabBomba, posicion, Quaternion.identity);
        }
        else
        {
            Instantiate(prefabPowerUp, posicion, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radioRespawn);
    }
}