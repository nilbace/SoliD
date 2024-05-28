using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMouseDetection : MonoBehaviour
{
    [SerializeField] SpriteRenderer _glowingBorder;
    public float Duration;
    void OnMouseEnter()
    {
        PointEnter();
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
    }

    void OnMouseUpAsButton()
    {
        HandManager.Inst.ArrangeCards();
    }

    // ���콺�� �ݶ��̴��� ������ �� ����� �Լ�
    void OnMouseExit()
    {
        PointerExit();
    }


    void PointEnter()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // �׵θ� �ܻ��� ������
        _glowingBorder.DOColor(targetColor, Duration);
    }

    void PointerExit()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // �׵θ� �ܻ��� �����ϰ� �Ⱥ��̰�
        _glowingBorder.DOColor(targetColor, Duration);
    }
    
}
