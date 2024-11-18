using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    //private Coroutine checkInputTime;

    [SerializeField]
    [Header("------ Line Info ------")]
    public GameObject linePrefab; // line ������
    public int maxLineCount = 180; // ���� ���� ����
    public int usedLineLength = 0; // ������� ����� ����
    public int curLineLenght = 0; // ���� �׸��� �ִ� ĳ�� ����
    public float destroyLineTime = 5.0f; // �� ������� ������
    public List<Vector2> points = new List<Vector2>();

    private bool isStart = false;
    private bool mouseRightBtnDown = false;

    LineRenderer lineRenderer;
    EdgeCollider2D coll;
    private Queue<GameObject> line = new Queue<GameObject>(); // ����
    Vector3 mousePos;
    RaycastHit2D hitLever; // �������� üũ��
    RaycastHit2D hitEnemy; // �������� üũ��
    private Queue<int> usedLinesLength = new Queue<int>(); // ����ߴ� ���ε��� ����

    public GameObject settingUI;

    //////////////////////////////////////////////////////////////

    private void Start()
    {
        // AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    private void Update()
    {
        Draw();
        LineDelete();
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
            Destroy(ketChapp, 4.8f);

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

                StartCoroutine(LineDestroyCo(/*obj*/));
                isStart = false;

                //Destroy(obj, destroyLineTime);
                //StartCoroutine(LineUpdateCo(destroyLineTime));

                return;
            }

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(points[points.Count - 1], pos) > 0.1f && usedLineLength <= maxLineCount)
            {
                points.Add(pos);
                usedLineLength++;
                curLineLenght++;
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                coll.points = points.ToArray();
            }

        }
        else if (Input.GetMouseButtonUp(0) && isStart)
        {
            //GameObject obj = null;
            if (points.Count == 1)
            {
                coll.gameObject.SetActive(false); // Ŭ���� ���� ��
            }
            points.Clear();
            //if (line.Count != 0)
            //    obj = line.Dequeue();  // Dequeue
            usedLinesLength.Enqueue(curLineLenght); // Enqueue

            StartCoroutine(LineDestroyCo(/*obj*/));
            isStart = false;

            //Destroy(obj, destroyLineTime);
            //StartCoroutine(LineUpdateCo(destroyLineTime));
        }
    }

    private void LineDelete()
    {
        if (Input.GetMouseButtonDown(1)) // ��� ĳ�� ����
        {
            Debug.Log("��Ŭ��!!");
            while (line.Count != 0)
            {
                mouseRightBtnDown = true;
            }
        }
    }

    private IEnumerator LineDestroyCo()
    {
        float chkTime = 0.0f;
        while(chkTime < destroyLineTime) // �⺻ ���� �ð����� ��Ŭ�� �ߴ��� Ȯ��
        {
            chkTime += Time.deltaTime;
            yield return null;
            if (mouseRightBtnDown) // ���콺 ��Ŭ���� �ߴٸ�
            {
                LineDestroy();
                mouseRightBtnDown = false;
                yield break;
            }
        }

        // �⺻ ���� �ð��� ������ ���� ���� ó��
        LineDestroy();

        yield return null;
    }

    private void LineDestroy()
    {
        GameObject obj = null;
        if (line.Count != 0)
            obj = line.Dequeue();  // Dequeue
        Destroy(obj); // ���� ����
        usedLineLength -= usedLinesLength.Dequeue(); // ����ߴ� ���� ���� ȸ��
    }


    //private IEnumerator LineUpdateCo(float delay = 0)
    //{
    //    yield return new WaitForSeconds(delay);
    //    usedLineLength -= usedLinesLength.Dequeue();
    //}
}
