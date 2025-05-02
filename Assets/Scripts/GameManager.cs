using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Button btnShoot;
    public Slider sliderPower;
    public TMP_Text powerSelecet;


    [Header("3D")]
    public LineRenderer lineRenderer;
    public Transform start;
    public Transform end;
    public Transform splash;

    [Header("Settings")]
    public float powerMax = 30f;
    public float powerMin = 5f;
    public Vector3 wind = new Vector3(1, 0, 0);
    public int resolution = 20;
    public float simulationTime = 2f;

    private void Start()
    {
        btnShoot.onClick.AddListener(Shoot);
        SetupSlider();
    }

    private void SetupSlider()
    {
        sliderPower.maxValue = powerMax;
        sliderPower.minValue = powerMin;
        sliderPower.value = powerMin;
        StartCoroutine(SliderAutoRun());
    }

    private IEnumerator SliderAutoRun()
    {
        var wait = new WaitForSeconds(0.05f);
        var isIncrease = true;
        while (true)
        {
            if (isIncrease)
            {
                if (sliderPower.value == sliderPower.maxValue)
                    isIncrease = false;
                sliderPower.value += 1f;
            }
            else
            {
                if (sliderPower.value == sliderPower.minValue)
                    isIncrease = true;
                sliderPower.value -= 1f;
            }
            yield return wait;
        }
        yield return null;
    }

    private void Shoot()
    {
        var points = Calculate().ToArray();
        powerSelecet.text = sliderPower.value.ToString();
        ShowTrajectory(points);
    }

    private List<Vector3> Calculate()
    {
        var power = sliderPower.value;
        var points = new List<Vector3>();
        var direction = (end.position - start.position).normalized;
        var initialVelocity = direction * power;
        var stepTime = simulationTime / resolution;
        var hasIntersect = false;

        points.Add(start.position);

        for (int i = 1; i < resolution; i++)
        {
            var t = i * stepTime;
            var pointA = points[i - 1];
            var pointB = start.position +
                           initialVelocity * t +
                           0.5f * Vector3.down * t * t +
                           0.5f * wind * t * t;

            var lineDirection = (pointB - pointA).normalized;
            var lineLength = Vector3.Distance(pointA, pointB);
            var denominator = Vector3.Dot(end.up, lineDirection);

            if (Mathf.Abs(denominator) > 0.0001f)
            {
                Vector3 pointToPlane = end.position - pointA;
                var t2 = Vector3.Dot(end.up, pointToPlane) / denominator;

                if (t2 >= 0 && t2 <= lineLength)
                {
                    points.Add(pointA + lineDirection * t2);
                    hasIntersect = true;
                    break;
                }
                else
                {
                    points.Add(pointB);
                }
            }
        }
        if (hasIntersect)
        {
            splash.gameObject.SetActive(true);
            splash.position = points[points.Count - 1];
            splash.localPosition += Vector3.up * 0.00005f;
        }
        else
        {
            splash.gameObject.SetActive(false);
            Debug.LogWarning("Blow dart can't touch wall, please check configuration");
        }
        return points;
    }

    void ShowTrajectory(Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
