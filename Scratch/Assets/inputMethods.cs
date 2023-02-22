using UnityEngine;
using UnityEngine.InputSystem;

public class inputMethods : MonoBehaviour
{
    [SerializeField] float speed = 1500f;
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] Transform cam;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform playerBody;
    [SerializeField] SpringJoint joint;

    Vector2 rawInput;
    Vector3 inputDir;
    Vector2 horVelocity;
    bool attached = false;
    Vector3 hook;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        horVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
        if (horVelocity.magnitude <= maxSpeed)
            rb.AddForce(inputDir * speed * Time.deltaTime);

        if (attached)
        {
            Debug.DrawLine(playerBody.position, hook, Color.red);
        }
    }

    public void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
        inputDir = new Vector3(rawInput.x, 0, rawInput.y);
        inputDir = Quaternion.Euler(0, cam.eulerAngles.y, 0) * inputDir;
    }

    public void OnJump(InputValue value) => rb.AddForce(Vector2.up * jumpForce);

    public void OnGrapple(InputValue value)
    {
        RaycastHit camTarget;
        if (Physics.Raycast(cam.position, cam.forward, out camTarget))
            hook = camTarget.point;
            joint.maxDistance = (hook - playerBody.position).magnitude;
            joint.connectedAnchor = hook;
            attached = true;
    }

    public void OnRelease(InputValue value)
    {

    }
}
