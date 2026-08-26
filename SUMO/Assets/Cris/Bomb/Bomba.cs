using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour
{
    [SerializeField] GameObject modelo, particulasExplosion;

    [SerializeField] float fuerzaExplosion;
    [SerializeField] float tiempoParaExplotar;

    [SerializeField] float radioExplosion;
    [SerializeField] LayerMask capasExplosion;
    private audiomanager audiomanager;

    public void Start()
    {
        audiomanager = FindAnyObjectByType<audiomanager>();
    }

    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(tiempoParaExplotar);
        audiomanager.seleccionAudio(2);
        modelo.SetActive(false);
        particulasExplosion.SetActive(true);

        Collider[] objetosColisionados = Physics.OverlapSphere(
            transform.position,
            radioExplosion,
            capasExplosion
        );

        foreach (var item in objetosColisionados)
        {
            if (item.TryGetComponent(out Rigidbody rigidColisionado))
            {
                rigidColisionado.AddExplosionForce(
                    fuerzaExplosion,
                    transform.position,
                    radioExplosion,
                    2,
                    ForceMode.Impulse
                );
            }
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explosion());
    }
}
