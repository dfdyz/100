using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContorller : MonoBehaviour
{
    [SerializeField]
    private Camera view_cam;        //�������

    public float s = 0.001f;        // ����ƶ��ٶȵ���������


    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 last = Vector3.zero;    // ��һ֡���λ��
    float mouseSpeed = 10;          // ����ƶ��ٶ�
    // Update is called once per frame
    void Update()
    {
        // ʹ�ù��ֿ������������
        view_cam.orthographicSize = Mathf.Clamp(view_cam.orthographicSize + Input.mouseScrollDelta.y, 3, 7);

        if(Input.GetMouseButtonDown(1)) last = Input.mousePosition;

        // ����ס����Ҽ�ʱ����������ƶ��ľ������ƶ����λ��
        if (Input.GetMouseButton(1))
        {
            Vector3 deltaMousePosition = Input.mousePosition - last;
            deltaMousePosition.Scale(view_cam.cameraToWorldMatrix.lossyScale * (view_cam.orthographicSize * Mathf.Sqrt(s)));

            deltaMousePosition.Scale(new Vector3(1, -1, 1));
            view_cam.transform.position += deltaMousePosition;
        }

        last = Input.mousePosition; // �������λ��
    }
}
