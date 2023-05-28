using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject ropeRomeo;
    private CubeController cubeController;
    [SerializeField] private Transform[] cubeTransforms;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        cubeController = GameObject.FindObjectOfType(typeof(CubeController)) as CubeController;

        lineRenderer.positionCount = cubeTransforms.Length;
        //Debug.Log(lineRenderer.bounds); // Center -20.54,2.9,-1.5 Extenst: 1.03,2.65,0.5
        for (int i = 0; i < cubeTransforms.Length; i++)
        {
            if (cubeTransforms[0].position.x <= -20.01f)
            {
                lineRenderer.SetPosition(i, cubeTransforms[i].position);
                cubeController.MakeSwing();
            }

            //var pos = new Vector3(0f, 6f, 0f);
            //var posline = new Vector3(cubeTransforms[i].position.x, 0, 0f);
            //lineRenderer.SetPosition(0, pos);
            //lineRenderer.SetPosition(1, posline);
        }


    }

}
