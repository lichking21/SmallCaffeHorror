using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform barDestinationPoint;
    [SerializeField] private Transform streetDestinationPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    private bool isTaken = false;

    void Update()
    {
        if (!isTaken)
        {
            agent.destination = barDestinationPoint.position;
        }
        else
        {
            agent.destination = streetDestinationPoint.position;    
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void TakeCoffee(bool take)
    {
        isTaken = take;
    }
}