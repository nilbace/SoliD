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
    [SerializeField] private bool IsCanceled; //��Ŭ������ ����ߴ���

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
        if (DOTween.IsTweening(transform)) // ���� ���� �� ���� ������Ʈ�� ���� Dotween�� ���� ���̶��
        {
            DOTween.Kill(transform); // �ش� Dotween�� ���
        }
        MoveCardFront();
    }

    void OnMouseDrag()
    {
        //��ҵƴٸ� �۵���������
        if (IsCanceled) return;

        if(!IsUsing)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // ���콺 ��ġ
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // ���� ��ǥ�� ��ȯ
            transform.position = objPosition; // ī���� transform�� ���콺�� ���󰡰� ��
        }

        //������� ��� �÷��� ����� ����
        if (!IsUsing && transform.position.y > YOffset)
        {
            Debug.Log("���������");
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
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // ���콺 ��ġ
                Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); // ���� ��ǥ�� ��ȯ
                transform.position = objPosition; // ī���� transform�� ���콺�� ���󰡰� ��
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
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // ���콺 ��ġ�� ��ġ�� ��� Collider2D�� ã��
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
            //���İ� 0����
        }
        TMPro.TMP_Text[] Texts = GetComponentsInChildren<TMPro.TMP_Text>();
        //�ؽ�Ʈ���� ���İ��� ��� 1��
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

    void MoveCardFront()
    {
        // ��������Ʈ �������� ĵ���� ���� ���� ����
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
        Debug.Log("���");
        IsUsing = false;
        
        HideBorder();
        BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //���� ����ϴ�.
        HandManager.Inst.ArrangeCards();
    }

    void UseCard()
    {
        Debug.Log("���");
        IsUsing = false;
        HideBorder();
        BezierCurveDrawer.Inst.lineRenderer.positionCount = 0; //���� ����ϴ�.
        HandManager.Inst.ArrangeCards();
        //thisCardGO.UseCard();
    }
}
