using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField]
    private bool isStart = false;
    private bool mouseRightBtnDown = false;
    private bool mouseRightCoolTime = false;

    public WaitForSeconds mouseRightClickDelay = new WaitForSeconds(1.0f);

    [SerializeField]
    [Header("------ Line Info ------")]
    public GameObject linePrefab; // line ������
    public int maxLineCount = 130; // ���� ���� ����
    public int usedLineLength = 0; // ������� ����� ����
    public int curLineLenght = 0; // ���� �׸��� �ִ� ĳ�� ����
    public float destroyLineTime = 5.0f; // �� ������� ������

    public List<Vector2> points = new List<Vector2>();
    private Queue<GameObject> line = new Queue<GameObject>(); // ����
    private Queue<int> usedLinesLength = new Queue<int>(); // ����ߴ� ���ε��� ����

    RaycastHit2D hitLever; // �������� üũ��
    RaycastHit2D hitEnemy; // �������� üũ��

    public GameObject settingUI;
    public Image gauge;

    LineRenderer lineRenderer;
    EdgeCollider2D coll;
    Vector3 mousePos;
    
    //////////////////////////////////////////////////////////////

    private void Start()
    {
        // AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    private void Update()
    {
        Draw();
        LineDelete();

        // KetChap Gauge
        gauge.fillAmount = (float)(maxLineCount - usedLineLength) / (float)maxLineCount;
    }

    private void Draw()
    {
        if (settingUI.activeSelf) return;

        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        Debug.DrawRay(mousePos, Vector3.forward * 100f, Color.red, 100f);
        hitLever = Physics2D.Raycast(mousePos, Vector3.forward, 100f, LayerMask.GetMask("Lever"));
        hitEnemy = Physics2D.Raycast(mousePos, Vector3.forward, 100f, LayerMask.GetMask("Enemy"));


        if (Input.GetMouseButtonDown(0)) // line ������ 1����
        {
            // ĳ�� �ν��Ͻ�ȭ
            GameObject ketChapp = Instantiate(Resources.Load<GameObject>("Ketchapp"), new Vector3(mousePos.x, mousePos.y, 15), Quaternion.identity);
            Destroy(ketChapp, 1.0f);

            AudioManager.Instance.PlaySFX(SFX.Mouseclick);

            // ���� �� ���� ������ ������ �׷����� ����
            if (hitLever.collider != null || hitEnemy.collider != null)
                return;

            GameObject obj = Instantiate(linePrefab/*, new Vector3(mousePos.x, mousePos.y, 15), Quaternion.identity*/);
            lineRenderer = obj.GetComponent<LineRenderer>();
            coll = obj.GetComponent<EdgeCollider2D>();
            points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, points[0]);
            line.Enqueue(obj);      // Enqueue
            curLineLenght = 0;
            isStart = true;
        }
        else if (Input.GetMouseButton(0) && isStart)
        {
            // ���� �� ���� ���� ���콺�� �ö󰡸�...
            if (hitLever.collider != null || hitEnemy.collider != null)
            {
                //GameObject obj = null;
                points.Clear();
                //if (line.Count != 0)
                //    obj = line.Dequeue();   // Dequeue
                usedLinesLength.Enqueue(curLineLenght);     // Enqueue

                Coroutine co = StartCoroutine("LineDestroy");

                isStart = false;

                //Destroy(obj, destroyLineTime);
                //StartCoroutine(LineUpdateCo(destroyLineTime));

                return;
            }

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(points[points.Count - 1], pos) > 0.1f && usedLineLength <= maxLineCount && !mouseRightBtnDown)
            {
                points.Add(pos);
                usedLineLength++;
                curLineLenght++;
                if(line.Count != 0)
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                    coll.points = points.ToArray();
                }
            }

        }
        else if (Input.GetMouseButtonUp(0) && isStart)
        {
            //GameObject obj = null;
            if (points.Count < 5)
            {
                coll.gameObject.SetActive(false); // Ŭ���� ���� ��
            }
            points.Clear();
            //if (line.Count != 0)
            //    obj = line.Dequeue();  // Dequeue
            usedLinesLength.Enqueue(curLineLenght); // Enqueue

            Coroutine co = StartCoroutine("LineDestroy");

            isStart = false;

            //Destroy(obj, destroyLineTime);
            //StartCoroutine(LineUpdateCo(destroyLineTime));
        }
    }

    private void LineDelete()
    {
        if (Input.GetMouseButtonDown(1) && !mouseRightBtnDown && !isStart && !mouseRightCoolTime) // ��� ĳ�� ����
        {
            if (line.Count == 0) return;

            StopAllCoroutines();

            mouseRightBtnDown = true; // ��Ŭ�� Ŭ��
            mouseRightCoolTime = true; // ��Ÿ�� üũ
            LineDirectDestroy();
        }
    }

    private IEnumerator LineDestroy() // �� ������ ���� ����
    {
        float chkTime = 0f;
        while(chkTime < destroyLineTime)// ������ ���� ��...
        {
            yield return null; 
            chkTime += Time.deltaTime;
            if (mouseRightBtnDown) yield break; // ���� ��Ŭ��, �� ��� ���� �����Ѵٸ� �׳� ����
        }

        GameObject obj = null;
        if (line.Count != 0)
            obj = line.Dequeue();  // Dequeue
        Destroy(obj); // ���� ����
        if(usedLinesLength.Count != 0)
        usedLineLength -= usedLinesLength.Dequeue(); // ����ߴ� ���� ���� ȸ��
    }

    private void LineDirectDestroy() // ��� �� �����
    {
        if(line.Count == 0) return;

        while (line.Count != 0)
        {
            GameObject obj = null;
            if (line.Count != 0)
                obj = line.Dequeue();  // Dequeue
            Destroy(obj); // ���� ����
            if (usedLinesLength.Count != 0)
                usedLineLength -= usedLinesLength.Dequeue(); // ����ߴ� ���� ���� ȸ��
        }

        mouseRightBtnDown = false;

        StartCoroutine("mouseRightClickDelayCo");
    }

    private IEnumerator mouseRightClickDelayCo() // ��Ŭ�� ��Ÿ��
    {
        yield return null; //mouseRightClickDelay;
        mouseRightCoolTime = false;
    }

    //private IEnumerator LineUpdateCo(float delay = 0)
    //{
    //    yield return new WaitForSeconds(delay);
    //    usedLineLength -= usedLinesLength.Dequeue();
    //}
}
