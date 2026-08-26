using UnityEngine;

public class PowerUpFuerza : MonoBehaviour
{
    public float duracion = 10f;
    private audiomanager audiomanager;

    private void Start()
    {
        audiomanager = FindAnyObjectByType<audiomanager>();
        Destroy(gameObject, 10f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        movePlayer jugador = collision.gameObject.GetComponent<movePlayer>();

        if (jugador != null)
        {
            audiomanager.seleccionAudio(4);
            jugador.ActivarPowerUp(duracion);

            Destroy(gameObject);
        }
    }
}
