using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;  // Para las corrutinas

public class MonsterAI : MonoBehaviour
{
    [Header("Referencias Obligatorias")]
    public Transform playerTarget;  // Asignar desde Inspector
    public Animator monsterAnimator; // Asignar desde Inspector

    [Header("Configuraci�n")]
    public float attackRange = 3f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private bool isAttacking = false;
    public delegate void PlayerKilledHandler();
    public event PlayerKilledHandler onKillPlayer;

    void Start()
    {
        // 1. Obtener referencias autom�ticas
        agent = GetComponent<NavMeshAgent>();

        // 2. Validaciones cr�ticas
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

            // 4. Control de animaci�n de caminar
            bool isMoving = agent.velocity.magnitude > 0.1f;
            monsterAnimator.SetBool("IsWalking", isMoving);
        }

        // 5. L�gica de ataque
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

        // 8. Disparar animaci�n de ataque
        monsterAnimator.SetTrigger("Attack");

        // 9. Esperar a que termine el ataque (ajustar seg�n duraci�n de animaci�n)
        yield return new WaitForSeconds(1.2f);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndMenu");
        // 10. Reanudar movimiento
        //agent.isStopped = false;
        //isAttacking = false;
    }

    // 11. Llamado desde evento de animaci�n (en el frame de impacto)
    public void OnAttackHitEvent()
    {
        if (playerTarget != null &&
            Vector3.Distance(transform.position, playerTarget.position) <= attackRange * 1.2f)
        {
            Debug.Log("�Jugador eliminado!");

            // Llamar al m�todo Die en lugar de desactivar
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