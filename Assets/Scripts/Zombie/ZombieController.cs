using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad del movimiento del zombie
    public float detectionRange = 10f; // Rango de detección del zombie
    public float followRange = 20f; // Rango de seguimiento del zombie
    public Transform[] waypoints; // Puntos de patrulla del zombie
    [SerializeField] private Transform view;
    [SerializeField] private CharacterAnimatorController animatorController;
    private float rotationSpeed = 10f;
    private int currentWaypointIndex = 0; // Índice del punto de patrulla actual
    [SerializeField] private Transform playerTransform; // Transform del jugador
    private bool isFollowingPlayer = false; // Booleano para determinar si el zombie está siguiendo al jugador

    void Update()
    {
        if (!isFollowingPlayer) // Si el zombie no está siguiendo al jugador
        {
            Patrol(); // Realizar patrulla
        }
        else
        {
            FollowPlayer(); // Seguir al jugador
        }
    }

    void Patrol()
    {
        // Calcular la dirección hacia el siguiente punto de patrulla
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0f;

        // Rotar al zombie hacia la dirección del siguiente punto de patrulla
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        // Mover el zombie hacia el siguiente punto de patrulla
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

        animatorController.ZombiePatroll(true);

        // Si el zombie llegó al punto de patrulla actual, seleccionar el siguiente punto de patrulla
        if (transform.position == waypoints[currentWaypointIndex].position)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Realizar un raycast de largo alcance en dirección al jugador
        RaycastHit hit;
        if (Physics.Raycast(view.position, playerTransform.position - transform.position, out hit, detectionRange))
        {
            // Si el raycast golpeó al jugador, cambiar a seguir al jugador
            if (hit.collider.CompareTag("Player"))
            {
                isFollowingPlayer = true;
            }
        }
    }

    void FollowPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;

        animatorController.ZombieChasing(true);

        // Rotar al zombie hacia la dirección del jugador
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        // Mover el zombie hacia el jugador
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        var lastPositionKnwown = playerTransform.position;

        // Realizar un raycast de largo alcance en dirección al jugador
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, followRange))
        {
            // Si el raycast no golpeó al jugador, volver a patrullar
            if (!hit.collider.CompareTag("Player"))
            {
                transform.position = Vector3.MoveTowards(transform.position, lastPositionKnwown, moveSpeed * Time.deltaTime);
                
                if (transform.position == lastPositionKnwown) 
                    isFollowingPlayer = false;

                animatorController.ZombieChasing(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(view.position, transform.forward * detectionRange);
    }
}
