using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float fieldOfView = 180f; // Campo de visi�n del zombi (en grados)
    public float detectionRange = 10f; // Rango m�ximo de detecci�n del jugador
    public float chaseSpeed = 3.5f; // Velocidad cuando persigue al jugador
    public float patrolSpeed = 2f; // Velocidad cuando no persigue al jugador

    public float patrolRadius = 15f; // Radio dentro del cual se generan puntos aleatorios
    private Vector3 randomPatrolPoint; // Punto de patrullaje generado aleatoriamente

    private NavMeshAgent navMeshAgent;
    private bool isChasing = false;
    private float stoppingDistance = 2f;

    private void Start()
    {
        // Obtener el NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;

        // Generar el primer punto aleatorio de patrullaje
        GenerateRandomPatrolPoint();
    }

    private void Update()
    {
        // Verificar si el jugador est� dentro del rango de detecci�n
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange && IsPlayerInFOV())
        {
            // Persigue al jugador
            StartChase();
        }
        else
        {
            // Deja de perseguir al jugador
            StopChase();
        }

        // Patrullaje si no est� persiguiendo al jugador
        if (!isChasing)
        {
            Patrol();
        }
    }

    private bool IsPlayerInFOV()
    {
        // Vector desde el zombi al jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Vector de direcci�n del zombi
        Vector3 zombieForward = transform.forward;

        // Producto escalar para calcular el coseno del �ngulo
        float dotProduct = Vector3.Dot(zombieForward, directionToPlayer);

        // Calcular el coseno del FOV dividido por 2
        float cosFOV = Mathf.Cos(fieldOfView * 0.5f * Mathf.Deg2Rad);

        // Verificar si el jugador est� dentro del FOV
        return dotProduct > cosFOV;
    }

    private void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            navMeshAgent.speed = chaseSpeed;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stoppingDistance)
        {
            navMeshAgent.SetDestination(player.position); //Persigue al jugador

            // Reducir la velocidad si est� muy cerca del jugador
            navMeshAgent.speed = Mathf.Lerp(0.5f, 3f, distanceToPlayer / stoppingDistance);
        }
        else
        {
            navMeshAgent.ResetPath();
        }
    }

    private void StopChase()
    {
        if (isChasing)
        {
            isChasing = false;
            navMeshAgent.speed = patrolSpeed;
            GenerateRandomPatrolPoint(); // Generar un nuevo punto al detener la persecuci�n
        }
    }

    private void Patrol()
    {
        // Si el zombi lleg� al punto actual
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Generar un nuevo punto aleatorio de patrullaje
            GenerateRandomPatrolPoint();
        }

        // Mover al zombi hacia el punto de patrullaje
        navMeshAgent.SetDestination(randomPatrolPoint);
    }

    private void GenerateRandomPatrolPoint()
    {
        // Generar un punto aleatorio dentro del radio especificado
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position; // Desplazar el punto relativo a la posici�n actual

        // Asegurarse de que el punto est� sobre el NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            randomPatrolPoint = hit.position;
        }
    }
}