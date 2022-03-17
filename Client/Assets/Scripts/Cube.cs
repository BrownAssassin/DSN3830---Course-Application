using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    public Vector3 jump = new Vector3(0f, 1f, 0f);
    public float jumpForce = 2f;

    public bool isGrounded;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        jump = new Vector3(0f, 1f, 0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidbody.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        transform.Translate(
            Input.GetAxis("Horizontal") * Time.deltaTime * 2f, 
            0f, 
            Input.GetAxis("Vertical") * Time.deltaTime * 2f
        );   
    }
}
