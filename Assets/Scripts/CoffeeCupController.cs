using UnityEngine;

public class CoffeeCupController : MonoBehaviour
{
    [SerializeField] Transform placePoint;

    public void PlaceCap(GameObject cap)
    {
        if (cap == null) return;

        var rb = cap.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        cap.transform.SetParent(placePoint);
        cap.transform.localPosition = Vector3.zero;
        cap.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }
}