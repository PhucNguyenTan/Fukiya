using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    public LineRenderer lineRenderer;

    public Transform start;
    public Transform end;
    public Button btnShoot;

    [Header("Settings")]
    public float power = 1f;
    public Vector3 wind = new Vector3(1, 0, 0);
    public int resolution = 20;
    public float simulationTime = 2f;

    private void Start()
    {
        btnShoot.onClick.AddListener(Shoot);
    }

    private void Shoot()
    {
        var points = Calculate();
        ShowTrajectory(points);
    }

    private Vector3[] Calculate()
    {
        var points = new Vector3[resolution];

        var direction = (end.position - start.position).normalized;
        var initialVelocity = direction * power;


        var stepTime = simulationTime / resolution;

        points[0] = start.position;

        for (int i = 1; i < resolution; i++)
        {
            var t = i * stepTime;
            points[i] = start.position +
                        initialVelocity * t +
                        0.5f * Vector3.down * t * t +
                        0.5f * wind * t * t;
        }

        return points;
    }

    private Vector3[] CalculateStraight()
    {
        var points = new Vector3[resolution];

        var direction = (start.position - end.position).normalized;
        var initialVelocity = direction * power;
        points[0] = start.position;

        for (int i = 1; i < resolution; i++)
        {
            var t = i / (float)resolution - simulationTime;
            points[i] = start.position +
                        initialVelocity * t +
                        0.5f * Vector3.down * t * t +
                        0.5f * wind * t * t;
        }

        return points;
    }

    void ShowTrajectory(Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
