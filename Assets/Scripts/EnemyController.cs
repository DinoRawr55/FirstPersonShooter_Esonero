using System;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{

    public Transform Player;

    [Header("Movement")]
    public float speed = 3.5f;
    public float attackDistance = 2f;       // distanza per fare danno
    public float detectionDistance = 10f;
    public float fleeDistance = 15f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float fleeHealthThreshold = 30f;
    public float damage = 10f;
    public float attackCooldown = 1.5f;

    private float currentHealth;
    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    public GameManager gameManager;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        currentHealth = maxHealth;

        // CERCA IL PLAYER AUTOMATICAMENTE
        if (Player == null)
        {
            // Cerca l'oggetto che ha il Tag "Player"
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
            else
            {
                Debug.LogError("ERRORE: Non hai messo il Tag 'Player' al tuo giocatore!");
            }
        }

        // CERCA IL GAMEMANAGER AUTOMATICAMENTE
        if (gameManager == null)
        {
            gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();
        }
    }


    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (currentHealth <= fleeHealthThreshold)
        {
            Flee();
        }
        else if (distanceToPlayer <= detectionDistance)
        {
            ChasePlayer();

            // Attacco se vicino e cooldown scaduto
            if (distanceToPlayer <= attackDistance && Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(Player.position);
    }

    void Flee()
    {
        Vector3 dirToPlayer = transform.position - Player.position;
        Vector3 newPos = transform.position + dirToPlayer.normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPos, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void AttackPlayer()
    {
        PlayerHealth ph = Player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
            Debug.Log("Player colpito! Danno: " + damage);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Funzione di morte del nemico
    void Die()
    {
        if (gameManager != null)
        {
            gameManager.EnemyKilled(); // Chiama il metodo EnemyKilled dal GameManager
        }

        Destroy(gameObject); // Distrugge il nemico
    }
}
