using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineJulia : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private SwingJulia swingJulia;
    private Rigidbody Julia;

    [SerializeField] private Transform[] cubeTransforms;
    private Vector3 initialPosition;

    private bool firstRun = true;
    private bool isSwinging = false;

    private GameObject ropeJulia;
    // Start is called before the first frame update
    void Start()
    {
        swingJulia = FindObjectOfType(typeof(SwingJulia)) as SwingJulia;
        lineRenderer = GetComponent<LineRenderer>();
        Julia = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineRenderer.positionCount = cubeTransforms.Length;
        for (int i = 0; i < cubeTransforms.Length; i++)
        {
            if (cubeTransforms[0].position.x <= -21.51f)
            {
                if (firstRun)
                {
                    initialPosition = swingJulia.GetPostion();
                    firstRun = false;
                    isSwinging = true;
                }
                lineRenderer.SetPosition(i, cubeTransforms[i].position);
                
            }

        }

        if (isSwinging)
        {
            swingJulia.MakeSwingJulia(initialPosition);
   
        }

    }
    
}