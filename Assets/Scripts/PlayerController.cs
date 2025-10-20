using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;    

    [SerializeField] private float speed = 1f;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = cam.transform.right * x + cam.transform.forward * z;
        movement.y = 0;

        controller.Move(movement.normalized * speed * Time.deltaTime);
    }
}
