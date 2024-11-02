using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    //private Coroutine checkInputTime;

    [SerializeField]
    [Header("------ Line Info ------")]
    public GameObject linePrefab;
    public int maxLineCount = 180;
    public int curLineCount = 0;
    public float destroyLineTime = 5.0f;
    public List<Vector2> points = new List<Vector2>();

    LineRenderer lineRenderer;
    EdgeCollider2D coll;
    private Queue<GameObject> line = new Queue<GameObject>();
    private Queue<int> useLine = new Queue<int>();
    //////////////////////////////////////////////////////////////

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // line °¹¼ö´Â 1°³ÀÓ
        {
            GameObject obj = Instantiate(linePrefab);
            lineRenderer = obj.GetComponent<LineRenderer>();
            coll = obj.GetComponent<EdgeCollider2D>();
            points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, points[0]);
            line.Enqueue(obj);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(points[points.Count - 1], pos) > 0.1f && curLineCount <= maxLineCount)
            {
                points.Add(pos);
                int useline = curLineCount++;
                useLine.Enqueue(useline);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                coll.points = points.ToArray();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            points.Clear();
            GameObject obj = line.Dequeue();
            Destroy(obj, destroyLineTime);
            int preUserLineCount = useLine.Dequeue();
            StartCoroutine(LineUpdate(preUserLineCount));
        }
    }

    private IEnumerator LineUpdate(int count)
    {
        yield return new WaitForSeconds(destroyLineTime);
        curLineCount -= count;
    }
}
