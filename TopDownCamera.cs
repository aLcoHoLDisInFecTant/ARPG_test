using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;              // 角色
    public Vector3 offset = new Vector3(0, 3, -5); // 默认偏移
    public float followSpeed = 5f;        // 跟随平滑速度
    public float rotateSpeed = 90f;       // 视角旋转速度（角度/秒）
    public float horizontalAngleStep = 30f; // 每次按Q/E旋转的角度
    public float resetDelay = 2f;         // 多久后自动归位

    private float currentYawOffset = 0f;  // 当前横向角度偏移
    private float targetYawOffset = 0f;   // 目标角度偏移
    private float lastRotationTime = -999f;

    void Update()
    {
        HandleInput();
    }

    void LateUpdate()
    {
        if (!target) return;

        // 自动回正逻辑
        if (Time.time - lastRotationTime > resetDelay)
        {
            targetYawOffset = 0f;
        }

        // 平滑过渡旋转偏移
        currentYawOffset = Mathf.MoveTowards(currentYawOffset, targetYawOffset, rotateSpeed * Time.deltaTime);

        // 计算最终偏移向量
        Quaternion yawRotation = Quaternion.Euler(0, currentYawOffset, 0);
        Vector3 desiredOffset = yawRotation * offset;

        // 设置摄像机位置与朝向
        Vector3 desiredPosition = target.position + target.rotation * desiredOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 始终朝向角色
        Vector3 lookPos = target.position + Vector3.up * 1.5f; // 视角略高
        transform.LookAt(lookPos);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetYawOffset -= horizontalAngleStep;
            lastRotationTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            targetYawOffset += horizontalAngleStep;
            lastRotationTime = Time.time;
        }
    }
}
