using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Button TilemapEditor;
    public Button Select;
    public Button Put;

    public GameObject panel;        //ѡ���ͼ�����
    public LevelManager levelManager;

    private bool showGrid = false;

    [SerializeField]
    private LineRenderer lineRenderer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
        TilemapEditor.onClick.AddListener(OnTilemapEditorClick);
        Select.onClick.AddListener(OnSelectClick);
        Put.onClick.AddListener(OnPutClick);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
    }

    private void Update()
    {
        if (showGrid)
        {
            DrawGrid();
        }
    }

    private void OnTilemapEditorClick()
    {
        panel.SetActive(true);

    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    private void OnSelectClick()
    {
        levelManager.SetSelectMode();
    }

    private void OnPutClick()
    {
        levelManager.SetPutMode();
        showGrid = true;
        if (showGrid)
        {
            DrawGrid();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
        ClosePanel();
    }

    public void CloseGrid()
    {
        showGrid = false;
        lineRenderer.positionCount = 0;
    }

    private void DrawGrid()
    {
        // ��ȡ����Ĵ�С��λ����Ϣ
        Vector3 gridPosition = levelManager.GetGridPosition();
        Vector2Int gridSize = levelManager.GetGridSize();
        float gridCellSize = levelManager.GetGridCellSize();

        // ���岻�ɱ༭������
        int editableRows = 3;

        //���������ƫ������ʹ���ĵ�λ��0,0
        float xOffset = -gridSize.x * gridCellSize / 2;
        float yOffset = -gridSize.y * gridCellSize / 2;

        // ��������
        List<Vector3> gridLines = new List<Vector3>();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                
                if (y >= editableRows)
                {
                    Vector3 cellPosition = gridPosition + new Vector3(x * gridCellSize + xOffset, y * gridCellSize + yOffset, 0);
                    // ���ÿ������Ԫ��������
                    gridLines.Add(cellPosition);
                    gridLines.Add(cellPosition + new Vector3(gridCellSize, 0, 0));

                    gridLines.Add(cellPosition + new Vector3(gridCellSize, 0, 0));
                    gridLines.Add(cellPosition + new Vector3(gridCellSize, gridCellSize, 0));

                    gridLines.Add(cellPosition + new Vector3(gridCellSize, gridCellSize, 0));
                    gridLines.Add(cellPosition + new Vector3(0, gridCellSize, 0));

                    gridLines.Add(cellPosition + new Vector3(0, gridCellSize, 0));
                    gridLines.Add(cellPosition);

                    // ����������һ�У�������Ҳ������
                    if (x < gridSize.x - 1)
                    {
                        gridLines.Add(cellPosition + new Vector3(gridCellSize, 0, 0));
                        gridLines.Add(cellPosition + new Vector3(gridCellSize, gridCellSize, 0));
                    }
                }
            }
        }
        lineRenderer.positionCount = gridLines.Count;
        lineRenderer.SetPositions(gridLines.ToArray());
    }
}
