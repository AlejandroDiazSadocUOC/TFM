using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{

    public List<GameObject> m_BulletsPrefab;

    private Animator playerAnimator;
    private Rigidbody m_Rigidbody;

    private float m_MovementInputValue;
    private float m_LateralMovementInputValue;
    private float m_FireVerticalInputValue;
    private float m_FireLateralInputValue;

    [SerializeField]
    private float m_CurrentSpeed = 4.0f;
    [SerializeField]
    private float m_ShootCooldown = 0.25f;
    [SerializeField]
    private float m_CurrentHealth = 3;
    [SerializeField]
    private float m_MaxHealth = 3;

    private int m_CurrentBullet = 0;
    private float m_CurrentAmmo = 10.0f;

    private float m_MaxAmmoOfBullet = 10.0f;

    private float lastShot = 0;

    private int m_VerticalInvert = 1, m_LateralInvert = 1;

    public static PlayerBehaviour m_instance;

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        lastShot = Time.time;
        StartCoroutine(CheckAmmoCoroutine());
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
        if (m_FireVerticalInputValue > 0 && CanShot())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0.0f, transform.rotation.z);
            m_VerticalInvert = 1;
            Fire();
            lastShot = Time.time;
        }
        else if (m_FireVerticalInputValue < 0 && CanShot())
        {
            //transform.localRotation = new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180.0f, transform.rotation.z);
            m_VerticalInvert = -1;
            Fire();
            lastShot = Time.time;

        }

        if (m_FireLateralInputValue > 0 && CanShot())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 90.0f, transform.rotation.z);
            m_LateralInvert = 1;
            m_VerticalInvert = 1;
            Fire();
            lastShot = Time.time;
        }
        else if (m_FireLateralInputValue < 0 && CanShot())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 270.0f, transform.rotation.z);
            m_LateralInvert = -1;
            m_VerticalInvert = -1;
            Fire();
            lastShot = Time.time;
        }
    }

    private void Fire()
    {
        Instantiate(m_BulletsPrefab[m_CurrentBullet],new Vector3(transform.position.x + 0.2f,0.2f, transform.position.z), transform.rotation);
        m_CurrentAmmo--;
    }

    private bool CanShot()
    {
        return Time.time - lastShot >= m_ShootCooldown && m_CurrentAmmo > 0;
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
        Vector3 movement = Vector3.forward * m_MovementInputValue * m_CurrentSpeed * Time.deltaTime ;
        Vector3 lateralMovement = Vector3.right * m_LateralMovementInputValue * m_CurrentSpeed * Time.deltaTime ;
        // Apply this movement to the rigidbody's position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement + lateralMovement);
    }

    private IEnumerator CheckAmmoCoroutine()
    {
        if (m_CurrentAmmo != 0)
        {
            m_CurrentAmmo = m_CurrentAmmo < m_MaxAmmoOfBullet ? m_CurrentAmmo + 1 : m_CurrentAmmo;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(CheckAmmoCoroutine());
        }
        else
        {
            m_CurrentAmmo = m_MaxAmmoOfBullet;
            yield return new WaitForSeconds(5.0f);
            StartCoroutine(CheckAmmoCoroutine());
        }
    }

    public void ReduceLife(float lifeQuantity)
    {
        m_CurrentHealth -= lifeQuantity;
    }

    public void AddLife(float LifeQuantity)
    {
        m_CurrentHealth = m_CurrentHealth < m_MaxHealth ? m_CurrentHealth+LifeQuantity > m_MaxHealth ? m_MaxHealth : m_CurrentHealth + LifeQuantity : 0;
    }

    public void AddSpeed(float speed)
    {
        m_CurrentSpeed += speed;
    }

    public void ChangeCurrentBullet(int BulletNumber)
    {
        m_CurrentBullet = BulletNumber;
    }

    public void CheckDeath()
    {
        if (m_CurrentHealth <= 0)
            OnDeath();
    }

    private void OnDeath()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }
}
