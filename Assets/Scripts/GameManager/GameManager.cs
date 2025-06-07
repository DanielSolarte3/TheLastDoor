using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Configuraci�n Monstruo")]
    public GameObject monsterPrefab;
    public Transform monsterSpawnPoint;
    public float spawnTime = 10f; // 3 minutos

    [Header("Referencias")]
    public GameObject player;
    public string menuScene = "MainMenu";

    private float timer;
    private bool monsterSpawned;

    void Start()
    {
        timer = spawnTime;
        monsterSpawned = false;

        // Ocultar monstruo inicialmente
        if (monsterPrefab != null)
            monsterPrefab.SetActive(false);
    }

    void Update()
    {
        if (!monsterSpawned)
        {
            timer -= Time.deltaTime;

            // Mostrar tiempo en consola (opcional)
            Debug.Log("Tiempo restante: " + Mathf.Ceil(timer) + "s");

            if (timer <= 0)
            {
                SpawnMonster(); 
                monsterSpawned = true;
            }
        }
    }

    void SpawnMonster()
    {
        monsterPrefab.SetActive(true);

        // Posiciona correctamente al monstruo
        monsterPrefab.transform.position = monsterSpawnPoint.position;

        // Fuerza la actualizaci�n del NavMesh
        UnityEngine.AI.NavMeshAgent agent = monsterPrefab.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.Warp(monsterSpawnPoint.position);
        }

        MonsterAI monsterAI = monsterPrefab.GetComponent<MonsterAI>();
        if (monsterAI != null)
        {
            monsterAI.playerTarget = player.transform;
            monsterAI.onKillPlayer += LoadMenuScene;
        }
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene(menuScene);
    }
}