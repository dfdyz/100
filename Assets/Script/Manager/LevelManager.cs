using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class LevelManager : MonoBehaviour, IModuleSelection
{
    public GameObject currentSelectedModule; // �洢��ǰѡ�е�ģ��

    public static LevelManager Instance;
    public Transform startPoint;            // ���λ��
    public Transform endPoint;              // �յ�λ��

    public TileBase[] registries;         // �ؿ�ע���
    public List<TileBase> tiles;            // ��Ƭ



    float grid_space = 1f;                  // ������

    [SerializeField]
    private GameObject tool_move;           // �ƶ�����

    [SerializeField]
    private OptMode mode = OptMode.Select;  // ��ǰ����ģʽ
    Vector3 m_last;                         // ��һ֡������������


    enum OptMode    // ����ģʽ��ö��
    {
        Select, Put
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    enum Dir        // ����ö��
    {
        None, XY, X, Y
    }



    Vector3 drug_start_pos = Vector3.zero;
     Dir m_dir = Dir.None;
    // Update is called once per frame
    void Update()
    {
        Vector3 m_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // �����λ��ת��Ϊ��������ϵ
        Vector3 mouse_delta = m_world - m_last;                                 // ����ƶ�����������
        if (Input.GetMouseButtonDown(0))
        {
            //����Ƿ������˱༭������
            RaycastHit2D cast_tool = Physics2D.Raycast(m_world, Vector2.zero, 0f, LayerMask.GetMask("Editor"));

            if (cast_tool.collider)                                             // �������˱༭������
            {
                drug_start_pos = m_world;
                switch (cast_tool.collider.name)
                {
                    case "right":
                        m_dir = Dir.X;
                        break;
                    case "up":
                        m_dir = Dir.Y;
                        break;
                    case "center":
                        m_dir = Dir.XY;
                        break;
                    default:
                        break;
                }
            }
            else                                                                // ���û��
            {
                m_dir = Dir.None;
                //����Ƿ������˹ؿ�����
                RaycastHit2D cast_level = Physics2D.Raycast(m_world, Vector2.zero, 0f, LayerMask.GetMask("Level"));

                switch (mode)
                {
                    case OptMode.Select:
                        SelectObject(cast_level);                               // ѡ������
                        break;
                    case OptMode.Put:

                        break;
                    default:
                        break;
                }
            }

            if (mode == OptMode.Put)// ����tile
            {
                print("put");
                float _x = m_world.x % grid_space;
                float _y = m_world.y % grid_space;
                float hs = grid_space / 2;

                if (_x > 0)
                {
                    if (_x > hs) _x = mouse_delta.x + 1 - _x;
                    else _x = mouse_delta.x - _x;
                }
                else
                {
                    if (_x < -hs) _x = mouse_delta.x + 1 - _x;
                    else _x = mouse_delta.x - _x;
                }

                
                if (_y > 0)
                {
                    if (_y > hs) _y = mouse_delta.y + 1 - _y;
                    else _y = mouse_delta.y - _y;
                }
                else
                {
                    if (_y < -hs) _y = mouse_delta.y + 1 - _y;
                    else _y = mouse_delta.y - _y;
                }

                GameObject obj = GetCurrentSelectedModule();
                TileBase t = obj.GetComponent<TileBase>();
                if(t)
                {
                    print("put2");
                    obj = GameObject.Instantiate(obj);
                    t = obj.GetComponent<TileBase>();
                    tiles.Add(t);
                }
            }

            }
        if (Input.GetMouseButtonUp(0))
        {
            m_dir = Dir.None;
            select_dirty = true;                                                //ѡ��״̬
        }

        if (select_dirty)
        {
            select_dirty = false;                                               // ����ѡ��״̬
            calSelectedCenter();                                                // ����ѡ����������ĵ�
        }


        

        if (Input.GetMouseButton(0))                                            // ������һֱ����ʱ
        {
            mouse_delta = m_world - drug_start_pos;

            float _x = mouse_delta.x % grid_space;
            float _y = mouse_delta.y % grid_space;
            float hs = grid_space / 2;

            if (_x > 0)
            {
                if (_x > hs) mouse_delta.x += 1 - _x;
                else mouse_delta.x -= _x;
            }
            else
            {
                if (_x < -hs) mouse_delta.x += 1 - _x;
                else mouse_delta.x -= _x;
            }


            switch (m_dir)
            {
                case Dir.X:
                    mouse_delta.y = 0;
                    break;
                case Dir.Y:
                    mouse_delta.x = 0;
                    break;
                case Dir.XY:
                    break;
                default:
                    mouse_delta = Vector2.zero;
                    break;
            }


            
            select_center += mouse_delta;                                       // ����ѡ����������ĵ�λ��

            foreach (TileBase t in selected)
            {
                t.transform.position = t.transform.position + mouse_delta;
            }


        }


        tool_move.SetActive(selected.Count > 0);                                //�����ƶ�����

        tool_move.transform.position = select_center;
        m_last = m_world;
    }

    Vector3 select_center = Vector3.zero;



    void calSelectedCenter()                                        // ����ѡ����������ĵ�λ��
    {
        select_center = Vector3.zero;
        foreach (TileBase t in selected)
        {
            select_center += t.transform.localPosition / selected.Count;
        }
    }

    private bool select_dirty = false;                              // �Ƿ�ѡ��

    public HashSet<TileBase> selected = new HashSet<TileBase>();    // ѡ������弯��
    void SelectObject(RaycastHit2D cast)                            // ѡ������
    {
        TileBase tile = null;
        if (cast.collider)                                          // ������������
        {
            tile = cast.collider.GetComponent<TileBase>();          // ��ȡ�����TileBase���
        }

        if (Input.GetKey(KeyCode.LeftShift))                        //��ѡ
        {
            if (tile)
            {
                if (selected.Contains(tile))
                {
                    tile.SetHighLight(false);
                    selected.Remove(tile);
                }
                else
                {
                    tile.SetHighLight(true);
                    selected.Add(tile);
                }
            }
        }
        else                                                        //��ѡ
        {
            unHighLightAllSelect();
            selected.Clear();
            if (tile)
            {
                tile.SetHighLight(true);
                selected.Add(tile);
            }
        }

        select_dirty = true;                                        //ѡ��״̬
    }

    void unHighLightAllSelect()                                     // ȡ����������ĸ�����ʾ
    {
        foreach (TileBase t in selected)
        {
            t.SetHighLight(false);
        }
    }

    bool start = false;

    public bool IsPlaying { get { return start; } }
    public void Start_End()
    {
        UIManager.Instance.CloseGrid();
        UIManager.Instance.panel.SetActive(false);
        start = !start;
        if (start)
        {
            ScriptManager.Instance.CreatePlayer();
        }
        else
        {
            ScriptManager.Instance.DestroyPlayer();
            CameraContorller.Instance.LerpCam2Zero();
        }

    }



    public void SetSelectMode()
    {
        mode = OptMode.Select;
    }

    public void SetPutMode()
    {
        mode = OptMode.Put;
    }

    public void PutObject()
    {

    }

    public Vector3 GetGridPosition()
    {
        // ���ص�ͼ��λ����Ϣ
        return new Vector3(0, 0, 0); // �����ͼ��λ��Ϊ(0, 0, 0)
    }

    public Vector2Int GetGridSize()
    {
        // ���ص�ͼ�Ĵ�С��Ϣ
        return new Vector2Int(15, 10); // �����ͼ�Ĵ�СΪ15x10
    }

    public float GetGridCellSize()
    {
        // ��������Ԫ�Ĵ�С
        return 1f; // ����ÿ������Ԫ�Ĵ�СΪ1
    }

    public GameObject GetCurrentSelectedModule()
    {
        return currentSelectedModule;
    }

}
