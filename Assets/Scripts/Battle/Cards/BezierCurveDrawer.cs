using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int curveResolution = 20; // 곡선의 해상도, 더 높을수록 부드러운 곡선이 됩니다.
    public Vector3 ControlPoint;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = curveResolution; // 선의 점 개수 설정
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 누르고 있는 동안
        {
            DrawCurveFromScreenBottom();
        }
        else
        {
            lineRenderer.positionCount = 0; // 마우스를 놓으면 선을 숨깁니다.
        }
    }

    void DrawCurveFromScreenBottom()
    {
        Vector3 startPos = new Vector3(Screen.width / 2, 0, 0); // 화면 가운데 하단
        startPos = Camera.main.ScreenToWorldPoint(startPos + new Vector3(0, 0, 10)); // 화면 좌표를 월드 좌표로 변환

        Vector3 endPos = Input.mousePosition; // 마우스 위치
        endPos = Camera.main.ScreenToWorldPoint(endPos + new Vector3(0, 0, 10)); // 화면 좌표를 월드 좌표로 변환

        Vector3 controlPoint = ControlPoint; // 제어점 설정

        lineRenderer.positionCount = curveResolution;
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            Vector3 position = CalculateQuadraticBezierPoint(t, startPos, controlPoint, endPos);
            lineRenderer.SetPosition(i, position);
        }
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // 베지어 곡선 공식
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // 첫 번째 항
        p += 2 * u * t * p1; // 두 번째 항
        p += tt * p2; // 세 번째 항

        return p;
    }

}
