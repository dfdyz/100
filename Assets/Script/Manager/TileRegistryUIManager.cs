using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TileRegistryUIManager : MonoBehaviour
{
    public GameObject scrollViewContent; // Scroll View�����ķ�Χ
    public GameObject tileButtonPrefab;  // ��ť

    private TileBase[] registries; // �ؿ�ע���

    public LevelManager levelManager;

    private void Start()
    {
        registries = levelManager.registries;

        float currentButtonY = 0f; // ����׷�ٰ�ť�ĵ�ǰ��ֱλ��

        for (int i = 0; i < registries.Length; i++)
        {
            GameObject tileButton = Instantiate(tileButtonPrefab, scrollViewContent.transform);

            // �ֶ�Ųλ�ã����������ƺ��ڹ��ַ���������
            RectTransform buttonRectTransform = tileButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(buttonRectTransform.anchoredPosition.x, 80-currentButtonY);

            // ��ȡ��ť�ϵ�Text���������
            TextMeshProUGUI buttonText = tileButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = registries[i].tileName;
            }

            // ��ȡ��ť�ϵ�TileOnGUI���
            TileOnGUI tileOnGUIComponent = tileButton.GetComponent<TileOnGUI>();
            if (tileOnGUIComponent != null)
            {
                tileOnGUIComponent.SetId(i);
            }

            // ��ť��ļ��
            currentButtonY += buttonRectTransform.rect.height + 35f;

        }
        tileButtonPrefab.SetActive(false);
    }


}
