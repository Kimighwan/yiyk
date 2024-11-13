using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    //private Coroutine checkInputTime;

    [SerializeField]
    [Header("------ Line Info ------")]
    public GameObject linePrefab; // line 프리펩
    public int maxLineCount = 180; // 라인 쵀대 길이
    public int curLineCount = 0; // 현재까지 사용한 길이
    public int curLineLeght = 0; // 지금 그리고 있는 캐찹 길이
    public float destroyLineTime = 5.0f; // 선 사라지는 딜레이
    public List<Vector2> points = new List<Vector2>();

    private bool isStart = false;

    LineRenderer lineRenderer;
    EdgeCollider2D coll;
    private Queue<GameObject> line = new Queue<GameObject>(); // 라인
    Vector3 mousePos;
    RaycastHit2D hitLever; // 레버인지 체크용
    RaycastHit2D hitEnemy; // 몬스터인지 체크용
    private Queue<int> useLine = new Queue<int>(); // 사용했던 라인들의 길이

    public GameObject settingUI;

    //////////////////////////////////////////////////////////////

    private void Start()
    {
        // AudioManager.Instance.PlayBGM(BGM.lobby);
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (settingUI.activeSelf) return;

        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        Debug.DrawRay(mousePos, Vector3.forward * 100f, Color.red, 100f);
        hitLever = Physics2D.Raycast(mousePos, Vector3.forward, 100f, LayerMask.GetMask("Lever"));
        hitEnemy = Physics2D.Raycast(mousePos, Vector3.forward, 100f, LayerMask.GetMask("Enemy"));


        if (Input.GetMouseButtonDown(0)) // line 갯수는 1개임
        {
            // 캐찹 인스턴스화
            GameObject ketChapp = Instantiate(Resources.Load<GameObject>("Ketchapp"), new Vector3(mousePos.x, mousePos.y, 15), Quaternion.identity);
            Destroy(ketChapp, 4.8f);

            AudioManager.Instance.PlaySFX(SFX.Mouseclick);

            // 레버 및 몬스터 위에는 라인이 그려지지 않음
            if (hitLever.collider != null || hitEnemy.collider != null)
                return;

            GameObject obj = Instantiate(linePrefab/*, new Vector3(mousePos.x, mousePos.y, 15), Quaternion.identity*/);
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
            // 레버 및 몬스터 위에 마우스가 올라가면...
            if (hitLever.collider != null || hitEnemy.collider != null)
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
            if (points.Count == 1)
            {
                coll.gameObject.SetActive(false); // 클릭만 했을 때
            }
            points.Clear();
            if (line.Count != 0)
                obj = line.Dequeue();
            Destroy(obj, destroyLineTime);
            useLine.Enqueue(curLineLeght);
            //int preUseLineCount = useLine.Dequeue();
            StartCoroutine(LineUpdate(/*preUseLineCount*/));
            isStart = false;
        }

        if(Input.GetMouseButtonDown(2)) // 모든 캐찹 삭제
        {
            
        }
    }

    private IEnumerator LineUpdate(/*int count*/)
    {
        yield return new WaitForSeconds(destroyLineTime);
        curLineCount -= useLine.Dequeue();
    }

}
