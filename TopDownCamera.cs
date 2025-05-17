using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform target;              // ��ɫ
    public Vector3 offset = new Vector3(0, 3, -5); // Ĭ��ƫ��
    public float followSpeed = 5f;        // ����ƽ���ٶ�
    public float rotateSpeed = 90f;       // �ӽ���ת�ٶȣ��Ƕ�/�룩
    public float horizontalAngleStep = 30f; // ÿ�ΰ�Q/E��ת�ĽǶ�
    public float resetDelay = 2f;         // ��ú��Զ���λ

    private float currentYawOffset = 0f;  // ��ǰ����Ƕ�ƫ��
    private float targetYawOffset = 0f;   // Ŀ��Ƕ�ƫ��
    private float lastRotationTime = -999f;

    void Update()
    {
        HandleInput();
    }

    void LateUpdate()
    {
        if (!target) return;

        // �Զ������߼�
        if (Time.time - lastRotationTime > resetDelay)
        {
            targetYawOffset = 0f;
        }

        // ƽ��������תƫ��
        currentYawOffset = Mathf.MoveTowards(currentYawOffset, targetYawOffset, rotateSpeed * Time.deltaTime);

        // ��������ƫ������
        Quaternion yawRotation = Quaternion.Euler(0, currentYawOffset, 0);
        Vector3 desiredOffset = yawRotation * offset;

        // ���������λ���볯��
        Vector3 desiredPosition = target.position + target.rotation * desiredOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // ʼ�ճ����ɫ
        Vector3 lookPos = target.position + Vector3.up * 1.5f; // �ӽ��Ը�
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
