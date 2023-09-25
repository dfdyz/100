using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    public string tileName;        // ��ͼ�������
    public int tileType;           // ��ͼ�������

    private bool isHighlighted;    // ��ͼ���Ƿ񱻸�����ʾ

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetHighLight(bool b)
    {
        this.isHighlighted = b;

        // ������Ҫ���õ�ͼ��ĸ�����ʾЧ��
        if (isHighlighted)
        {
            // ���ø�������ɫ�����ʵȵ�
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            // �ָ���������ɫ�����ʵȵ�
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
