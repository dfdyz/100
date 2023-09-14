using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Transform startPoint;            // ���λ��
    public Transform endPoint;              // �յ�λ��

    public GameObject[] registries;         // ע������
    public List<TileBase> tiles;            // ��Ƭ

    float grid_space = 1f;                  // ������

    [SerializeField]
    private GameObject tool_move;           // �ƶ�����

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
                //����Ƿ������˹ؿ�
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
            now_grid_pos = GetGridPos(select_center + mouse_delta, grid_space); // ���㵱ǰ����λ��
        }

        if (Input.GetMouseButton(0))                                            // ������һֱ����ʱ
        {
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
            if (Grid)                                                           // �༭ʱ�������
            {
                Vector3 v1 = GetGridPos(select_center, grid_space);             // �������������λ��
                mouse_delta = v1 - now_grid_pos;                                // �����ƶ���������
                now_grid_pos = v1;                                              // ���µ�ǰ����λ��
            }
           

            foreach (TileBase t in selected)
            {
                t.transform.position = t.transform.position + mouse_delta;
            }

            
        }


        tool_move.SetActive(selected.Count > 0);                                //�����ƶ�����

        tool_move.transform.position = select_center;
        m_last = m_world;
    }

    Vector3 now_grid_pos = Vector3.zero;
    Vector3 select_center = Vector3.zero;

    Vector3 GetGridPos(Vector3 pos, float g)                        // ��ȡ���������λ��
    {
        Vector3 v1 = pos;
        Vector3 v2 = new Vector3(v1.x % g, v1.y % g, 0);
        v1 -= v2;
        if (Mathf.Abs(v2.x) > g * 0.5f)
        {
            v1.x += Mathf.Sign(v2.x) * g;
        }
        if (Mathf.Abs(v2.y) > g * 0.5f)
        {
            v1.y += Mathf.Sign(v2.y) * g;
        }
        return v1;
    }

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

    bool Grid = false;                                              //�Ƿ���ʾ����
    public void EnbaleGrid(bool e)                                  //��������ɲ��ɼ�
    {
        Grid = e;
    }

}
