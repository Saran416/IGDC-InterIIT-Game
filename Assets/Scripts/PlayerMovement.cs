using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float gravityValue;
    [SerializeField] float GroundCheckRadius;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask groundLayer;

    private bool isGrounded;    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Vector3 move = transform.right * horizontal + transform.forward * vertical;
        
        controller.Move(direction * speed * Time.deltaTime);

        // Ground Check
        //if (Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, groundLayer))
        //{
        //    isGrounded = true;
        //    Debug.Log("Ground");
        //} else {
        //    controller.Move(-Vector3.up * gravityValue * Time.deltaTime);
        //}

        
    }
}
