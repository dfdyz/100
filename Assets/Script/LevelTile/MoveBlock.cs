using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : TileBase
{
    [SerializeField]
    private GameObject highlight;

    private float currentPosition;
    private float direction = 1;          // ��ʼ����Ϊ����

    private const float maxPosition = 4;  // �ƶ����λ�ã�5��

    public float moveSpeed = 1f;          // �ƶ��ٶ�

    void Update()
    {
        if (LevelManager.Instance.GetMode() == LevelManager.OptMode.Start)
        {
            Move();
        }
    }

    private void Move()
    {
        float movement = direction * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(movement, 0f, 0f);

        currentPosition += Mathf.Abs(movement);

        if (currentPosition > maxPosition || !LevelManager.Instance.CheckInRange(transform.position))//�������λ�û򳬳��߽�
        {
            currentPosition = 0;
            direction *= -1;        // �ı䷽��
        }
       
    }

    public override void SetHighLight(bool b)
    {
        highlight.SetActive(b);
    }
}
