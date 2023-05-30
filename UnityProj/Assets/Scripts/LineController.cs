using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private SwingRomeo swingRomeo;
    private Rigidbody Romeo;

    [SerializeField] private Transform[] cubeTransforms;

    private Vector3 initialPosition;

    private bool firstRun = true;
    private bool isSwinging = false;

    // Start is called before the first frame update
    void Start()
    {
        swingRomeo = FindObjectOfType(typeof(SwingRomeo)) as SwingRomeo;
        lineRenderer = GetComponent<LineRenderer>();
       // Romeo = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineRenderer.positionCount = cubeTransforms.Length;
        for (int i = 0; i < cubeTransforms.Length; i++)
        {
            if (cubeTransforms[1].position.x <= -20.01f)
            {
                if (firstRun)
                {
                    initialPosition = swingRomeo.GetPostion();
                    firstRun = false;
                    Debug.Log($"initialpos {initialPosition}");
                    isSwinging = true;
                }
                
                lineRenderer.SetPosition(i, cubeTransforms[i].position);
                Debug.Log($"i:{i},trans.pos:{cubeTransforms[i].position}");
                Debug.Log($"linerend pos {lineRenderer.GetPosition(i)}");
               
            }
        }

        if (isSwinging)
        {
            swingRomeo.MakeSwing(initialPosition);
        }
        
    }

}
