using UnityEngine;

public class CoffeMachineController : MonoBehaviour
{
    [SerializeField] private Transform PlacePoint;
    private bool isON;
    private bool isFinished;

    public void PlaceCup(GameObject cup)
    {
        if (!isON || cup == null) return;

        cup.transform.SetParent(PlacePoint);
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.Euler(0, 0, 0);

        var rb = cup.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void TrunOn()
    {
        if (!isON)
        {
            isON = true;
            
            // TurnOn sounds
        }
    }
}