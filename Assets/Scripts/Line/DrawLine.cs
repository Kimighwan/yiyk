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
    public int curLineLeght = 0;
    public float destroyLineTime = 5.0f;
    public List<Vector2> points = new List<Vector2>();

    private bool isStart = false;

    LineRenderer lineRenderer;
    EdgeCollider2D coll;
    private Queue<GameObject> line = new Queue<GameObject>();
    Vector3 mousePos;
    RaycastHit2D hit;
    private Queue<int> useLine = new Queue<int>();
    //////////////////////////////////////////////////////////////

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        Debug.DrawRay(mousePos, Vector3.forward * 100f, Color.red, 100f);
        hit = Physics2D.Raycast(mousePos, Vector3.forward, 100f, LayerMask.GetMask("Lever"));


        if (Input.GetMouseButtonDown(0)) // line °¹¼ö´Â 1°³ÀÓ
        {
            if (hit.collider != null)
                return;

            GameObject obj = Instantiate(linePrefab);
            lineRenderer = obj.GetComponent<LineRenderer>();
            coll = obj.GetComponent<EdgeCollider2D>();
            points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, points[0]);
            line.Enqueue(obj);
            curLineLeght = 0;
            isStart = true;
        }
        else if (Input.GetMouseButton(0) && isStart)
        {
            if (hit.collider != null)
            {
                GameObject obj = null;
                points.Clear();
                if (line.Count != 0)
                    obj = line.Dequeue();
                Destroy(obj, destroyLineTime);
                useLine.Enqueue(curLineLeght);
                //int preUseLineCount = useLine.Dequeue();
                StartCoroutine(LineUpdate(/*preUseLineCount*/));
                isStart = false;
                return;
            }

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(points[points.Count - 1], pos) > 0.1f && curLineCount <= maxLineCount)
            {
                points.Add(pos);
                curLineCount++;
                curLineLeght++;
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                coll.points = points.ToArray();
            }

        }
        else if (Input.GetMouseButtonUp(0) && isStart)
        {
            GameObject obj = null;
            points.Clear();
            if(line.Count != 0)
                obj = line.Dequeue();
            Destroy(obj, destroyLineTime);
            useLine.Enqueue(curLineLeght);
            //int preUseLineCount = useLine.Dequeue();
            StartCoroutine(LineUpdate(/*preUseLineCount*/));
            isStart = false;
        }
    }

    private IEnumerator LineUpdate(/*int count*/)
    {
        yield return new WaitForSeconds(destroyLineTime);
        curLineCount -= useLine.Dequeue();
    }
}
