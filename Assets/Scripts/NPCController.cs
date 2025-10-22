using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    void Update()
    {
        agent.destination = destinationPoint.position;
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}