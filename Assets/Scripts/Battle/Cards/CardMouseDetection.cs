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
        if (DOTween.IsTweening(transform)) // 만약 현재 이 게임 오브젝트에 대해 Dotween이 실행 중이라면
        {
            DOTween.Kill(transform); // 해당 Dotween을 취소
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // 마우스 위치
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // 월드 좌표로 변환
        transform.position = objPosition; // 카드의 transform을 마우스를 따라가게 함
    }

    void OnMouseUpAsButton()
    {
        HandManager.Inst.ArrangeCards();
    }

    // 마우스가 콜라이더를 떠났을 때 실행될 함수
    void OnMouseExit()
    {
        PointerExit();
    }


    void PointEnter()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // 테두리 잔상을 빛나게
        _glowingBorder.DOColor(targetColor, Duration);
    }

    void PointerExit()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 테두리 잔상을 투명하게 안보이게
        _glowingBorder.DOColor(targetColor, Duration);
    }
    
}
