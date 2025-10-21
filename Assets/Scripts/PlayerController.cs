using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 1f;

    [SerializeField] private Transform holdPoint;
    private GameObject pickUpObj;
    private float rayLength = 3f;


    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickUpObj == null) TakeObj();
            else DropObj();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = cam.transform.right * x + cam.transform.forward * z;
        movement.y = 0;

        controller.Move(movement.normalized * speed * Time.deltaTime);
    }

    private void TakeObj()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayLength))
        {
            if (hit.collider.CompareTag("PickUpObj"))
            {
                pickUpObj = hit.collider.gameObject;

                pickUpObj.transform.SetParent(holdPoint);
                pickUpObj.transform.localPosition = Vector3.zero;
                pickUpObj.transform.localRotation = Quaternion.Euler(0, 0, 0);

                var rb = pickUpObj.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }
    private void DropObj()
    {
        pickUpObj.transform.SetParent(null);

        var rb = pickUpObj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        pickUpObj = null;
    }
}
