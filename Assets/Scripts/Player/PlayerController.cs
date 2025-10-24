using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5.5f;
    private float currSpeed;

    [SerializeField] private UIController ui;
    [SerializeField] private AudioSource hearbeatSound;
    private EnemyController enemy;

    [SerializeField] private Camera cam;
    [SerializeField] private Animator cameraAnim;
    private float rayLength = 15f;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private string cupName;
    [SerializeField] private string capName;
    private GameObject pickUpObj;
    private bool isFull;

    private CoffeMachineController machineController;

    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }

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
        GiveCoffee();

        InteractionHint();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = cam.transform.right * x + cam.transform.forward * z;
        movement.y = Time.deltaTime * -10;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currSpeed = runSpeed;
            cameraAnim.SetBool("Sprint", true);

            if (enemy.IsChasing())
                hearbeatSound.Play();
        }
        else
        {
            currSpeed = walkSpeed;
            cameraAnim.SetBool("Sprint", false);
        }

        controller.Move(movement.normalized * currSpeed * Time.deltaTime);
    }

    private bool CameraRaycast(out RaycastHit hit) => Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayLength);

    private void TakeObj()
    {
        if (CameraRaycast(out RaycastHit hit))
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
            if (CameraRaycast(out RaycastHit hit))
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
            if (CameraRaycast(out RaycastHit hit))
            {
                var cupController = hit.collider.gameObject.GetComponent<CoffeeCupController>();

                if (cupController != null)
                {
                    if (pickUpObj.name == capName)
                    {
                        cupController.PlaceCap(pickUpObj);

                        pickUpObj = null;

                        isFull = true;
                    }
                }
            }
        }
    }

    private void GiveCoffee()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CameraRaycast(out RaycastHit hit))
            {
                if (hit.collider.CompareTag("NPC") && pickUpObj.name == cupName && isFull)
                {
                    var npc = hit.collider.GetComponentInParent<NPCController>();

                    npc.TakeCoffee(true);
                    isFull = false;

                    Destroy(pickUpObj);
                }
            }
        }
    }

    private void InteractionHint()
    {
        if (CameraRaycast(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("PickUpObj"))
                ui.ShowHint("Press E to pick up item");
            else if (hit.collider.CompareTag("PickUpObj") && hit.collider.gameObject.name == capName && pickUpObj != null)
                ui.ShowHint("Press F to place item");
            else if (hit.collider.CompareTag("CoffeeMachine") && pickUpObj != null)
                ui.ShowHint("Press F to place item");
            else if (hit.collider.CompareTag("NPC") && pickUpObj != null)
                ui.ShowHint("Press F to give item");
            else 
                ui.HideHint();
        }
        else
            ui.HideHint();
    }
}
