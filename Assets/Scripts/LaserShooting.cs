using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    public GameObject LinePrefab;
    private float maxDist = 100;
    [SerializeField] private int MaxLines = 3;
    List<GameObject> lines = new List<GameObject>();
    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int linesCount = 0;
        if (Input.GetMouseButton(0))
        {
            linesCount += LaserCalculation(transform.position + transform.up/2, transform.up, linesCount);
        }
        RemovingLines(linesCount);
    }
    int LaserCalculation(Vector3 startPosition, Vector3 direction, int lineIndex)
    {
        int result = 1;
        if (lineIndex < MaxLines)
        {
            RaycastHit hit;
            Ray ray = new Ray(startPosition, direction);
            bool intersect = Physics.Raycast(ray, out hit, maxDist);
            Vector3 hitPosition = hit.point;
            if (intersect)
            {
                result += LaserCalculation(hitPosition, Vector3.Reflect(direction, hit.normal), lineIndex + result);
            }
            else
            {
                hitPosition = startPosition + direction * maxDist;
            }
            DrawLine(startPosition, hitPosition, lineIndex);
        }
        return result;
    }
    void RemovingLines(int linesCount)
    {
        if (linesCount < lines.Count)
        {
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
            RemovingLines(linesCount);
        }
    }
    void DrawLine(Vector3 startPosition, Vector3 finishPosition, int lineIndex)
    {
        LineRenderer line = null;
        if (lineIndex < lines.Count)
        {
            line = lines[lineIndex].GetComponent<LineRenderer>();
        }
        else
        {
            GameObject go = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);
            line = go.GetComponent<LineRenderer>();
            lines.Add(go);
        }
        line.SetPosition(0, startPosition);
        line.SetPosition(1, finishPosition);
    }
}
