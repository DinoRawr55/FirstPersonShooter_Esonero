using UnityEngine;
using System.Collections;

public class AdvancedShoot : MonoBehaviour
{
    [Header("Statistiche Arma")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f;

    [Header("Effetti Impatto")]
    public GameObject impactPrefabWall;    // Trascina qui lo Sprite del foro
    public GameObject impactPrefabEnemy;   // Trascina qui il Particle System (Sangue)

    [Header("Riferimenti")]
    public Camera fpsCam;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // 1. Cerchiamo il componente EnemyController
            EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();

            Vector3 offsetSpawnPos = hit.point + (hit.normal * 0.02f);
            Quaternion lookRotation = Quaternion.LookRotation(hit.normal);

            if (enemy != null)
            {
                // --- CASO NEMICO: SOLO PARTICELLE ---
                if (impactPrefabEnemy != null)
                {
                    GameObject blood = Instantiate(impactPrefabEnemy, offsetSpawnPos, lookRotation);
                    Destroy(blood, 1f); // Le particelle spariscono subito
                }

                // Applica il danno al nemico
                enemy.TakeDamage(damage);
                Debug.Log("Colpito Nemico: " + enemy.name);
            }
            else
            {
                // --- CASO MURO/ALTRO: BULLET HOLE FISSO ---
                if (impactPrefabWall != null)
                {
                    GameObject hole = Instantiate(impactPrefabWall, offsetSpawnPos, lookRotation);

                    // Lo incolliamo all'oggetto colpito (muro, cassa, ecc.)
                    hole.transform.SetParent(hit.transform);

                    Destroy(hole, 10f); // Dura 10 secondi
                }
                Debug.Log("Colpito Oggetto: " + hit.transform.name);
            }
        }
    }
}
