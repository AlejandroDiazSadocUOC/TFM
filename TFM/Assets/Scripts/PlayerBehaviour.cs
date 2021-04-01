using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Animator playerAnimator;
    private Rigidbody m_Rigidbody;

    private float m_MovementInputValue;
    private float m_LateralMovementInputValue;
    private float m_FireVerticalInputValue;
    private float m_FireLateralInputValue;
    private float m_Speed = 2.0f;

    private int m_VerticalInvert = 1, m_LateralInvert = 1;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_LateralMovementInputValue = Input.GetAxis("Horizontal");

        m_FireVerticalInputValue = Input.GetAxis("FireVertical");
        m_FireLateralInputValue = Input.GetAxis("FireLateral");

        RefreshAnims();
        RefreshFire();
    }

    private void RefreshAnims()
    {
        if (m_MovementInputValue > 0 && !playerAnimator.GetBool("isWalking"))
        {
            ResetBoolsAnim();
            playerAnimator.SetBool("isWalking", true);
        }else if (m_MovementInputValue < 0 && !playerAnimator.GetBool("isWalkingBw"))
        {
            ResetBoolsAnim();
            playerAnimator.SetBool("isWalkingBw", true);
        }
        
        if(m_LateralMovementInputValue != 0 && !playerAnimator.GetBool("isWalkingLateral"))
        {
            ResetBoolsAnim();
            playerAnimator.SetBool("isWalkingLateral", true);
        }

        if (m_LateralMovementInputValue == 0 && m_MovementInputValue == 0)
            ResetBoolsAnim();
    }

    private void RefreshFire()
    {
        if (m_FireVerticalInputValue > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0.0f, transform.rotation.z);
            m_VerticalInvert = 1;
        }
        else if (m_FireVerticalInputValue < 0)
        {
            //transform.localRotation = new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f, transform.rotation.z);
            m_VerticalInvert = -1;

        }

        if (m_FireLateralInputValue > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 90.0f, transform.rotation.z);
            m_LateralInvert = 1;
            m_VerticalInvert = 1;
        }
        else if (m_FireLateralInputValue < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 270.0f, transform.rotation.z);
            m_LateralInvert = -1;
            m_VerticalInvert = -1;
        }
    }

    private void ResetBoolsAnim()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isWalkingBw", false);
        playerAnimator.SetBool("isWalkingLateral", false);
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames
        Vector3 movement = Vector3.forward * m_MovementInputValue * m_Speed * Time.deltaTime ;
        Vector3 lateralMovement = Vector3.right * m_LateralMovementInputValue * m_Speed * Time.deltaTime ;
        // Apply this movement to the rigidbody's position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement + lateralMovement);
    }
}
