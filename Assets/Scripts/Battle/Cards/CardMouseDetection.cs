using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMouseDetection : MonoBehaviour
{
    [SerializeField] SpriteRenderer _glowingBorder;
    public float Duration;
    public float YOffset;
    void OnMouseEnter()
    {
        GlowBorder();
    }

    void OnMouseDown()
    {
        transform.localScale = Vector3.one * 0.7f;
        transform.rotation = Quaternion.identity;
        if (DOTween.IsTweening(transform)) // ���� ���� �� ���� ������Ʈ�� ���� Dotween�� ���� ���̶��
        {
            DOTween.Kill(transform); // �ش� Dotween�� ���
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // ���콺 ��ġ
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // ���� ��ǥ�� ��ȯ
        transform.position = objPosition; // ī���� transform�� ���콺�� ���󰡰� ��


        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư�� ������ �ִ� ����
        {
            if(transform.position.y > YOffset)
            {
                BezierCurveDrawer.Inst.DrawCurveFromScreenBottom();
            }
            else
            {
                BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //���� ����ϴ�.
            }
        }
    }

    void OnMouseUpAsButton()
    {
        BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //���� ����ϴ�.
        HandManager.Inst.ArrangeCards();
    }

    void HideCard()
    {
        DOTween.Kill(transform);
        SpriteRenderer[] CardSRs = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer Sr in CardSRs)
        {
            //���İ� 0����
        }
        TMPro.TMP_Text[] Texts = GetComponentsInChildren<TMPro.TMP_Text>();
        //�ؽ�Ʈ���� ���İ��� ��� 1��
    }

    // ���콺�� �ݶ��̴��� ������ �� ����� �Լ�
    void OnMouseExit()
    {
        HideBorder();
    }


    void GlowBorder()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // �׵θ� �ܻ��� ������
        _glowingBorder.DOColor(targetColor, Duration);
    }

    void HideBorder()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // �׵θ� �ܻ��� �����ϰ� �Ⱥ��̰�
        _glowingBorder.DOColor(targetColor, Duration);
    }
    
}
