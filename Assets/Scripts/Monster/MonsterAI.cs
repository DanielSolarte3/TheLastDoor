using UnityEngine;
using UnityEngine.AI;
using System.Collections;  // Para las corrutinas

public class MonsterAI : MonoBehaviour
{
    [Header("Referencias Obligatorias")]
    public Transform playerTarget;  // Asignar desde Inspector
    public Animator monsterAnimator; // Asignar desde Inspector

    [Header("Configuración")]
    public float attackRange = 3f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private bool isAttacking = false;
    public delegate void PlayerKilledHandler();
    public event PlayerKilledHandler onKillPlayer;

    void Start()
    {
        // 1. Obtener referencias automáticas
        agent = GetComponent<NavMeshAgent>();

        // 2. Validaciones críticas
        if (monsterAnimator == null)
            monsterAnimator = GetComponent<Animator>();

        if (playerTarget == null)
            playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTarget == null) return;

        // 3. Movimiento hacia el jugador
        if (!isAttacking)
        {
            agent.SetDestination(playerTarget.position);

            // 4. Control de animación de caminar
            bool isMoving = agent.velocity.magnitude > 0.1f;
            monsterAnimator.SetBool("IsWalking", isMoving);
        }

        // 5. Lógica de ataque
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    // 6. Corrutina para manejar el ataque
    private System.Collections.IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        // 7. Rotar hacia el jugador suavemente
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotationProgress = 0;
        while (rotationProgress < 1)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationProgress
            );
            rotationProgress += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        // 8. Disparar animación de ataque
        monsterAnimator.SetTrigger("Attack");

        // 9. Esperar a que termine el ataque (ajustar según duración de animación)
        yield return new WaitForSeconds(1.2f);

        // 10. Reanudar movimiento
        agent.isStopped = false;
        isAttacking = false;
    }

    // 11. Llamado desde evento de animación (en el frame de impacto)
    public void OnAttackHitEvent()
    {
        if (playerTarget != null &&
            Vector3.Distance(transform.position, playerTarget.position) <= attackRange * 1.2f)
        {
            Debug.Log("¡Jugador eliminado!");

            // Llamar al método Die en lugar de desactivar
            PlayerController playerController = playerTarget.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die();
            }

            if (onKillPlayer != null)
                onKillPlayer();
        }
    }
}