using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 1f;
    private float rayLength = 10f;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private string cupName;
    [SerializeField] private string capName;
    private GameObject pickUpObj;

    private CoffeMachineController machineController;

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickUpObj == null) TakeObj();
            else DropObj();
        }

        TurnOnMachine();
        PlaceCap();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = cam.transform.right * x + cam.transform.forward * z;
        movement.y = Time.deltaTime * -10;

        controller.Move(movement.normalized * speed * Time.deltaTime);
    }

    private void TakeObj()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayLength))
        {
            if (hit.collider.CompareTag("PickUpObj"))
            {
                pickUpObj = hit.collider.gameObject;

                if (pickUpObj.name == cupName)
                {
                    if (machineController != null && machineController.isPouring)
                    {
                        Debug.Log("Coffee is pouring");
                        return;
                    }
                }

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

        //pickUpObj.transform.position += Vector3.up * 0.05f;

        var rb = pickUpObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        pickUpObj = null;
    }

    private void TurnOnMachine()
    {
        if (pickUpObj == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayLength))
            {
                machineController = hit.collider.gameObject.GetComponent<CoffeMachineController>();

                if (machineController != null)
                {
                    if (pickUpObj.name == cupName)
                    {
                        machineController.TrunOn();
                        machineController.PlaceCup(pickUpObj);

                        pickUpObj = null;
                    }
                }
            }
        }
    }
    
    private void PlaceCap()
    {
        if (pickUpObj == null) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayLength))
            {
                Debug.Log("Raycast at object: " + hit.collider.gameObject.name);
                var cupController = hit.collider.gameObject.GetComponent<CoffeeCupController>();

                if (cupController != null)
                {
                    if (pickUpObj.name == capName)
                    {
                        cupController.PlaceCap(pickUpObj);

                        pickUpObj = null;
                    }
                }
            }
        }
    }
}
