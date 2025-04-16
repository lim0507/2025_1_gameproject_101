using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float turnSpeed = 10;

    [Header("���� ���� ����")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;

        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if(movement.magnitude>0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                EnableGlider();
            }
            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <= 0)
            {
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            DisableGlider();
        }

        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
       

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //�������� ������ ����ŭ ��ü�� �ش�
            isGrounded = false;                                     //������ �ϴ� ���� ������ �������⶧���� F��� ���ش�
            realGrouned = false;
            coyoteTimeCounter = 0;
        }

        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }
        }
    }

    

    void EnableGlider()
    {
        if(gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, gliderFallSpeed, vertical * gliderMoveSpeed);
        rb.velocity = gliderVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag( "Ground"))
        {
            realGrouned = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }

        if(other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("���� Ŭ����");
        }
    }
    void UpdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if(coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
}
