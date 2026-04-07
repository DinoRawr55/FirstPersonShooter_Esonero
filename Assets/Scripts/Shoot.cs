using UnityEngine;


public class Shoot : MonoBehaviour
{

    public GameObject impactPrefabWall;    // prefab per muri
    public GameObject impactPrefabEnemy;   // prefab particellare per nemici

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PlayerShoot();
        }
    }

    void PlayerShoot()
    {
        RaycastHit hit;

        // Spariamo il raggio dalla camera
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
        {
            // 1. Cerchiamo se abbiamo colpito un nemico
            EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();

            // Posizione e rotazione per l'impatto (leggermente staccato dalla superficie)
            Vector3 spawnPos = hit.point + hit.normal * 0.02f;
            Quaternion spawnRot = Quaternion.LookRotation(hit.normal);

            if (enemy != null)
            {
                // --- CASO NEMICO: PARTICELLE ---
                if (impactPrefabEnemy != null)
                {
                    // Creiamo le particelle (es. sangue)
                    GameObject blood = Instantiate(impactPrefabEnemy, spawnPos, spawnRot);
                    // Le distruggiamo dopo 1 secondo (regola tu la durata)
                    Destroy(blood, 1f);
                }

                enemy.TakeDamage(25f);
                Debug.Log("Colpito Nemico: " + enemy.name);
            }
            else
            {
                // --- CASO MURO/ALTRO: BULLET HOLE ---
                if (impactPrefabWall != null)
                {
                    // Creiamo il buco del proiettile fisso nel mondo
                    GameObject hole = Instantiate(impactPrefabWall, spawnPos, spawnRot);
                    // Lo rendiamo figlio dell'oggetto colpito (muri, casse, ecc.)
                    hole.transform.SetParent(hit.transform);

                    Destroy(hole, 10f); // Dura 10 secondi
                }
                Debug.Log("Colpito Oggetto: " + hit.transform.name);
            }
        }
    }
}

