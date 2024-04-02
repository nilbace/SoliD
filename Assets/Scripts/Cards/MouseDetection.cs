using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float Duration;
    // ���콺�� �ݶ��̴� ������ ���� �� ����� �Լ�
    void OnMouseEnter()
    {
        PointEnter();
    }

    // ���콺�� �ݶ��̴��� ������ �� ����� �Լ�
    void OnMouseExit()
    {
        PointerExit();
    }


    void PointEnter()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // ���İ��� 1�� ����
        spriteRenderer.DOColor(targetColor, Duration);
    }

    void PointerExit()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // ���İ��� 1�� ����
        spriteRenderer.DOColor(targetColor, Duration);
    }
    
}
