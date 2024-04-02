using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float Duration;
    // 마우스가 콜라이더 안으로 들어갔을 때 실행될 함수
    void OnMouseEnter()
    {
        PointEnter();
    }

    // 마우스가 콜라이더를 떠났을 때 실행될 함수
    void OnMouseExit()
    {
        PointerExit();
    }


    void PointEnter()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // 알파값을 1로 설정
        spriteRenderer.DOColor(targetColor, Duration);
    }

    void PointerExit()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = spriteRenderer.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 알파값을 1로 설정
        spriteRenderer.DOColor(targetColor, Duration);
    }
    
}
