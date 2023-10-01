using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContorller : MonoBehaviour
{
    public static CameraContorller Instance;
    [SerializeField]
    private Camera view_cam;        //�������
    [SerializeField]
    private float cam_lerp_rate = 0.2f;

    public float s = 0.001f;        // ����ƶ��ٶȵ���������

    //[SerializeField]
    //private GameObject tile_list;

    private float target_orthographicSize = 5f;

    //Vector3 _local_pos_tl;
    //Vector3 _local_scale_tl;


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        //_local_pos_tl = tile_list.transform.localPosition;
        //_local_scale_tl = tile_list.transform.localScale;
    }

    Vector3 last = Vector3.zero;    // ��һ֡���λ��
    float mouseSpeed = 10;          // ����ƶ��ٶ�


    Vector3 pos_tmp = new Vector3(1, 1, 0);
    void Update()
    {
        // ʹ�ù��ֿ������������
       

        if (LevelManager.Instance.IsPlaying) // playģʽ
        {

        }
        else                                 // �༭ģʽ
        {
            
            target_orthographicSize = Mathf.Clamp(target_orthographicSize - Input.mouseScrollDelta.y * 0.5f, 3, 7);
            view_cam.orthographicSize = Mathf.Lerp(view_cam.orthographicSize, target_orthographicSize, 0.12f);
            DragCamera();
        }

        //HandleUI();
    }

    IEnumerator _LerpCam2Zero()
    {
        while(true)
        {
            pos_tmp = Vector3.Lerp(view_cam.transform.position, Vector3.zero, cam_lerp_rate * 2);
            pos_tmp.z = -10;
            view_cam.transform.position = pos_tmp;
            if ((view_cam.transform.position - Vector3.zero).magnitude <= 0.2f) break;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    public void LerpCam2Zero()
    {
        StartCoroutine(_LerpCam2Zero());
    }

    private void FixedUpdate()
    {
        if (LevelManager.Instance.IsPlaying) // playģʽ
        {
            view_cam.orthographicSize = Mathf.Lerp(view_cam.orthographicSize, 4.3f, cam_lerp_rate*2);
            if (ScriptManager.Instance.player_instance)
            {
                pos_tmp = Vector3.Lerp(view_cam.transform.position, ScriptManager.Instance.player_instance.transform.position, cam_lerp_rate);
                pos_tmp.z = -10;
                view_cam.transform.position = pos_tmp;
            }
        }
    }

    void DragCamera()
    {
        if (Input.GetMouseButtonDown(1)) last = Input.mousePosition;

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

    float _s = 1;
    void HandleUI() //����ʱ����UI
    {
        _s = (1 + (view_cam.orthographicSize - 5)/5f);

        //tile_list.transform.localPosition = _local_pos_tl * _s;
        //tile_list.transform.localScale = _local_scale_tl * _s;
    }



}
