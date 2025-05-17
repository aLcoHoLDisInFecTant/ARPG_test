using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.5f;

    [Header("点击反馈特效")]
    public GameObject effectPrefab;
    private GameObject currentEffect;
    public CombatHandler ch;

    [Header("地面检测")]
    public LayerMask groundLayer;
    public float groundCheckDistance;

    [Header("锁定目标")]
    public Transform lockTarget;

    private CharacterController controller;
    private Vector3 targetPosition;
    private bool hasTarget = false;
    public Animator ani;

    private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    private bool isGrounded;
    private bool isAtk = false;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        ch = GetComponent<CombatHandler>();
    }

    void Update()
    {

        checkGrounded();
        ResetMove();
        HandleMouseInput();
        ApplyGravity();
        FaceTarget();
        HandleMovement();
    }

    void ResetMove() 
    {
        isAtk = ch.isAtk;
        Vector3 move = Vector3.zero;
        if (isAtk ) { controller.Move(move); }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1.5f);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
            {
                targetPosition = hit.point;
                hasTarget = true;

                // 特效
                if (effectPrefab != null)
                {
                    if (currentEffect != null)
                        Destroy(currentEffect);

                    currentEffect = Instantiate(effectPrefab, targetPosition, Quaternion.identity);
                }
            }
        }
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        if (hasTarget)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            ani.SetBool("Running", true);


            if (direction.magnitude < stoppingDistance)
            {
                hasTarget = false;
                ani.SetBool("Running", false);
            }
            else
            {
                move = direction.normalized * moveSpeed;
            }
        }

        // 重力始终有效
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void ApplyGravity()
    {
        Debug.DrawRay(transform.position, Vector3.down * (controller.height / 2 + 0.1f), isGrounded ? Color.green : Color.red);

        //Debug.Log(isGrounded);
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 贴地
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void checkGrounded () 
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
        //Debug.Log(isGrounded);
    }

    void FaceTarget()
    {
        if (lockTarget != null)
        {
            Vector3 lookPos = lockTarget.position - transform.position;
            lookPos.y = 0;
            if (lookPos.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}
