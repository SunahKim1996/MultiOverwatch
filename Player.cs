using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PhotonView pv;

    [Header("Camera")]
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Transform cam;

    [Header("Move")]
    [SerializeField] float speed = 5f;
    float moveInputX, moveInputZ = 0f;

    [Header("Head Bob")]
    float defaultPosY = 0f;

    float headBobTimer = 0f;
    float headBobSpeed = 14f;
    float headBobAmount = 0.05f;

    [Header("Look")]
    [SerializeField] GameObject upperBody;
    [SerializeField] float mouseSensitivity;
    float lookInputX, lookInputY = 0f;
    float verticalLookRotValue;

    [Header("Visible")]
    [SerializeField] List<GameObject> invisibleObj;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        defaultPosY = transform.localPosition.y;

        bool isActive = (pv.IsMine) ? true : false;
        cameraHolder.SetActive(isActive);

        OffInvisibleObj();
    }

    // 입력 감지 (프레임 기준)
    void Update()
    {
        Debug.Log($"{pv.IsMine} //////////// {upperBody.transform.localEulerAngles}");

        if (!pv.IsMine)
            return;

        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputZ = Input.GetAxisRaw("Vertical");

        lookInputX = Input.GetAxisRaw("Mouse X");
        lookInputY = Input.GetAxisRaw("Mouse Y");
    }

    // 물리 이동 (물리 프레임 기준)
    void FixedUpdate()
    {
        if (!pv.IsMine)
            return;

        transform.Translate(new Vector3(moveInputX, 0, moveInputZ) * speed * Time.deltaTime);
    }

    void LateUpdate()
    {
        if (!pv.IsMine)
            return;

        HeadBob();
        Look();
    }

    void OffInvisibleObj()
    {
        if (!pv.IsMine)
            return;

        for (int i = 0; i < invisibleObj.Count; i++)
            invisibleObj[i].SetActive(false);
    }

    void HeadBob()
    {
        Vector3 localPos = transform.localPosition;
        float curY;

        // 이동 중 
        if (Mathf.Abs(moveInputX) > 0.1f || Mathf.Abs(moveInputZ) > 0.1f)
        {
            headBobTimer += Time.deltaTime * headBobSpeed;
            curY = defaultPosY + Mathf.Sin(headBobTimer) * headBobAmount;
        }

        // 멈춤 
        else
        {
            headBobTimer = 0;
            curY = Mathf.Lerp(localPos.y, defaultPosY, Time.deltaTime * headBobSpeed);
        }

        transform.localPosition =
                new Vector3(localPos.x, curY, localPos.z);
    }
    
    void Look()
    {
        // 마우스 좌우 이동 시 -> Y축을 중심으로 Player 가 회전
        float horizontalValue = lookInputX * mouseSensitivity;
        transform.Rotate(Vector3.up * lookInputX * mouseSensitivity);

        // 마우스 상하 이동 시 -> X축을 중심으로 cameraHolder 가 회전        
        verticalLookRotValue += lookInputY * mouseSensitivity;
        verticalLookRotValue = Mathf.Clamp(verticalLookRotValue, -40f, 80f);
        float verticalValue = verticalLookRotValue * (-1);
        cameraHolder.transform.localEulerAngles = Vector3.right * verticalValue;
        
        // Player 상체도 같이 회전
        upperBody.transform.localEulerAngles = 
            new Vector3(verticalValue, horizontalValue, 0f);

        
    }
}
