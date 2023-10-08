using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclicBlock : TileBase
{
    [SerializeField]
    private GameObject highlight;

    private float direction = -1;          // ��ʼ����Ϊ����
    private Vector3 initialPosition;       // ��ʼλ��

    [SerializeField]
    private float moveSpeed = 5f;          // �ƶ��ٶ�

    [SerializeField]
    private float intervalTime = 2f;      // ���ʱ��

    void Start()
    {
        StartCoroutine(MoveCoroutine());    // ����Э��
    }

    void Update()
    {
        if (LevelManager.Instance.GetMode() != LevelManager.OptMode.Start)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, 0f);
            initialPosition = transform.position;
        }   
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (LevelManager.Instance.GetMode() == LevelManager.OptMode.Start)
            {
                float movement = direction * moveSpeed * Time.deltaTime;
                transform.position += new Vector3(0f, movement, 0f);

                if (!LevelManager.Instance.CheckInRange(transform.position))//�����߽�
                {
                    transform.position = initialPosition;
                    // �ȴ�һ��ʱ��
                    yield return new WaitForSeconds(intervalTime);
                }
            }
            yield return null;
        }
    }


    public override void SetHighLight(bool b)
    {
        highlight.SetActive(b);
    }
}
