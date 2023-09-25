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
    public Button Back;
    public Button Obstacle1;
    public Button Obstacle2;
    public Button Obstacle3;
    public static GameObject selectedModule;


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
        Back.onClick.AddListener(OnBackClick);
        // Ϊÿ����ť���OnClick�¼�
        Obstacle1.onClick.AddListener(() => OnObstacleClick(Obstacle1.gameObject));
        Obstacle2.onClick.AddListener(() => OnObstacleClick(Obstacle2.gameObject));
        Obstacle3.onClick.AddListener(() => OnObstacleClick(Obstacle3.gameObject));


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
    public interface IModuleSelection
    {
        GameObject GetCurrentSelectedModule();
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

        // ���ð�ťΪ����״̬
        Obstacle1.gameObject.SetActive(true);
        Obstacle2.gameObject.SetActive(true);
        Obstacle3.gameObject.SetActive(true);
        Back.gameObject.SetActive(true);

        // Interactable ��������Ϊ false
        Put.interactable = false;

        Select.interactable = false;

        
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
         OnBackClick();
         OnSelectClick();
        //Put.interactable = false;

        
    }

    private void OnBackClick()
    {

        Obstacle1.gameObject.SetActive(false);
        Obstacle2.gameObject.SetActive(false);
        Obstacle3.gameObject.SetActive(false);
        Back.gameObject.SetActive(false);


        Put.interactable = true;

        Select.interactable = true;
    }

    private void OnObstacleClick(GameObject obstacle)
    {
        Put.interactable = true;
        // ���õ�ǰѡ�е�ģ��
        selectedModule = obstacle;
        levelManager.currentSelectedModule = selectedModule;

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
