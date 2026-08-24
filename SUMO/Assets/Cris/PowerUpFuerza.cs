using UnityEngine;

public class PowerUpFuerza : MonoBehaviour
{
    public float duracion = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        movePlayer jugador = collision.gameObject.GetComponent<movePlayer>();

        if (jugador != null)
        {
            jugador.ActivarPowerUp(duracion);

            Destroy(gameObject);
        }
    }
}
