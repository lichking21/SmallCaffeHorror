using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Transform barDestinationPoint;
    [SerializeField] private Transform streetDestinationPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    private bool isTaken = false;
    public bool hasLeft = false;

    void Start()
    {
        agent.destination = barDestinationPoint.position;
    }

    void Update()
    {
        if (isTaken)
            agent.destination = streetDestinationPoint.position;

        if (ReachedPoint(streetDestinationPoint))
            hasLeft = true;
        

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void TakeCoffee(bool take)
    {
        isTaken = take;
    }

    private bool ReachedPoint(Transform point)
    {
        if (!agent.pathPending && Vector3.Distance(agent.transform.position, point.position) <= 0.5f)
        {
            if (!agent.hasPath || agent.velocity.magnitude <= 0)
                return true;
        }
        return false;
    }

    public bool HasLeft() => hasLeft;
}