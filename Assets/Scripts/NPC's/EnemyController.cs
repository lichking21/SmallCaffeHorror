using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AudioSource chaseMusic;
    [SerializeField] private AudioSource lofiMusic;
    private NavMeshAgent agent;
    private Animator anim;

    public bool isChasing;

    private NPCController npc;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        npc = GameObject.FindGameObjectWithTag("NPC").GetComponent<NPCController>();
    }

    void Update()
    {
        if (npc == null) return;

        if (npc.HasLeft())
        {
            if (!isChasing)
            {
                chaseMusic.Play();
                lofiMusic.Stop();
                isChasing = true;
            }

            agent.destination = player.position;
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    public bool IsChasing() => isChasing;
}