using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMouseDetection : MonoBehaviour
{
    [SerializeField] SpriteRenderer _glowingBorder;
    public float Duration;
    public float YOffset;
    public static bool IsUsing;
    private bool _needTarget;
    private CardGO thisCardGO;
    [SerializeField] private bool IsCanceled; //우클릭으로 취소했는지

    private void Start()
    {
        thisCardGO = GetComponent<CardGO>();
        _needTarget = thisCardGO.thisCardData.NeedTarget;
    }
    void Update()
    {
        if ( !IsCanceled &&IsUsing && Input.GetMouseButtonUp(1))
        {
            CancelUse();
        }
    }
    void OnMouseEnter()
    {
        if (IsUsing) return;
        GlowBorder();
    }

    void OnMouseDown()
    {
        IsCanceled = false;
        transform.localScale = Vector3.one * 0.7f;
        transform.rotation = Quaternion.identity;
        if (DOTween.IsTweening(transform)) // 만약 현재 이 게임 오브젝트에 대해 Dotween이 실행 중이라면
        {
            DOTween.Kill(transform); // 해당 Dotween을 취소
        }
        MoveCardFront();
    }

    void OnMouseDrag()
    {
        //취소됐다면 작동하지않음
        if (IsCanceled) return;

        if(!IsUsing)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // 마우스 위치
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // 월드 좌표로 변환
            transform.position = objPosition; // 카드의 transform을 마우스를 따라가게 함
        }

        //어느정도 들어 올려야 사용중 판정
        if (!IsUsing && transform.position.y > YOffset)
        {
            Debug.Log("사용중판정");
            IsUsing = true;
            if(_needTarget) transform.DOMove(new Vector3(-0, -2.3f, 0), 0.15f);

        }

        if (IsUsing)
        {
            if(_needTarget)
            {
                BezierCurveDrawer.Inst.DrawCurveFromScreenBottom();
            }
            else
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // 마우스 위치
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // 월드 좌표로 변환
                transform.position = objPosition; // 카드의 transform을 마우스를 따라가게 함
            }
        }
    }

    private void OnMouseUp()
    {
        if (IsCanceled) return;

        if(_needTarget)
        {
            if(IsTargetMonster())
            {
                UseCard();
            }
            else
            {
                CancelUse();
            }
        }
        else
        {
            UseCard();
        }
    }

    private void OnMouseExit()
    {
        if (IsUsing) return;
        HideBorder();
    }
 

    private bool IsTargetMonster()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // 마우스 위치와 겹치는 모든 Collider2D를 찾음
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mouseWorldPos2D);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Monster"))
            {
                return true;
            }
        }
        return false;
    }

    

    void HideCard()
    {
        DOTween.Kill(transform);
        SpriteRenderer[] CardSRs = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer Sr in CardSRs)
        {
            //알파값 0으로
        }
        TMPro.TMP_Text[] Texts = GetComponentsInChildren<TMPro.TMP_Text>();
        //텍스트들의 알파값도 모두 1로
    }

    


    void GlowBorder()
    {
        transform.DOScale(new Vector3(0.7f, 0.7f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); // 테두리 잔상을 빛나게
        _glowingBorder.DOColor(targetColor, Duration);
    }

    void HideBorder()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f), Duration);
        Color originalColor = _glowingBorder.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 테두리 잔상을 투명하게 안보이게
        _glowingBorder.DOColor(targetColor, Duration);
    }

    void MoveCardFront()
    {
        // 스프라이트 렌더러와 캔버스 정렬 순서 조정
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int j = 0; j < spriteRenderers.Length; j++)
        {
            spriteRenderers[j].sortingOrder = 2000 - j;
        }

        Canvas[] canvases = GetComponentsInChildren<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.sortingOrder = 2000 + 1;
        }
    }
    

    void CancelUse()
    {
        IsCanceled = true;
        Debug.Log("취소");
        IsUsing = false;
        
        HideBorder();
        BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //선을 숨깁니다.
        HandManager.Inst.ArrangeCards();
    }

    void UseCard()
    {
        Debug.Log("사용");
        IsUsing = false;
        HideBorder();
        BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //선을 숨깁니다.
        HandManager.Inst.ArrangeCards();
        //thisCardGO.UseCard();
    }
}
