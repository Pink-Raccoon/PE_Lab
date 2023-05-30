using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SwingRomeo : MonoBehaviour
{
    public Rigidbody Romeo;
    
    private List<List<float>> timeSeriessRopeSwingRomeo;


    double startime = 0;
    private float currentTimeStep; // s
    private Vector3 g= new Vector3(0f, -9.81f, 0f);
    float alphaRomeo = 0f; //Angle between crane and rope
    private bool rotationTriggered = false;
    float cCube = 1.1f;
    float constantAirFriction = 1.2f;
    float areaRomeo = 2.25f;
    float R = 6f; //Radius Rope

    // Start is called before the first frame update
    void Start()
    {
        timeSeriessRopeSwingRomeo = new List<List<float>>(); 
        startime = Time.fixedTimeAsDouble;
        // Romeo = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTimeStep += Time.deltaTime;

    }


    public Vector3 GetPostion()
    {
        return new Vector3(Romeo.position.x, Romeo.position.y + 6, Romeo.position.z);
    }

    public void MakeSwing(Vector3 connectedPos)
    {
        var ropeCubeRomeo = connectedPos - Romeo.position; // Endpunkt - Anfangspunkt
        Debug.Log("diff " + ropeCubeRomeo);
        alphaRomeo = (float)Math.Atan(ropeCubeRomeo.x / ropeCubeRomeo.y);
        Debug.Log("alpha " + alphaRomeo);
        var FG = Romeo.mass * g.magnitude * Math.Cos(alphaRomeo);
        var FZ = Romeo.mass * (Math.Pow(Romeo.velocity.magnitude, 2.0f)) / (R);
        var normalizedVelocityRomeo = Romeo.velocity.normalized;
        var FR = (float)(-0.5 * areaRomeo * constantAirFriction * cCube * Mathf.Pow(Romeo.velocity.magnitude, 2.0f)) * normalizedVelocityRomeo;
        var FH = (FG + FZ) * Math.Sin(alphaRomeo);
        var FV = (FG + FZ) * Math.Cos(alphaRomeo);
        var centripedalForceRomeo = new Vector3((float)FH, (float)FV, 0.0f) ;
        var force = centripedalForceRomeo + FR;
        Romeo.AddForce(force);
        var degree = ConvertRadiansToDegrees(alphaRomeo);
        //currentTimeStep += Time.deltaTime;
        timeSeriessRopeSwingRomeo.Add(new List<float>() { currentTimeStep, Romeo.position.x, Romeo.position.y, alphaRomeo, (float)degree, (float)FH, (float)FV, force.x, force.y, force.z });

    }

    void OnApplicationQuit()
    {
        WriteTimeSeriessRopeSwingRomeoToCsv();
    }
    void WriteTimeSeriessRopeSwingRomeoToCsv()
    {
        using (var streamWriter = new StreamWriter("timeSeriesRopeRomeo.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.position.y,alphaRomeo,degree,horizonForceRomeo,verticalForceRomeo, -frictionForceRomeo.x + horizonForceRomeo, -frictionForceRomeo.y + verticalForceRomeo, -frictionForceRomeo.z + zAxisForceRomeo");

            foreach (List<float> timeStep in timeSeriessRopeSwingRomeo)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }

    public static double ConvertRadiansToDegrees(double radians)
    {
        double degrees = (180 / Math.PI) * radians;
        return (degrees);
    }
}
