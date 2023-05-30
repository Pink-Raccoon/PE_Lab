using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SwingJulia : MonoBehaviour
{
   
    public Rigidbody Julia;


    private float cubeJuliaTimeStep;
    private List<List<float>> timeSeriessRopeSwingJulia;
    private Vector3 g = new Vector3(0f, -9.81f, 0f);
    private bool rotationTriggered = false;
    double startime = 0;
    float R = 6f; //Radius Ropefloat g = 9.81f; //Gravity
    float alphaJulia = 0f; //Angle between crane and rope
    float cCube = 1.1f;
    float constantAirFriction = 1.2f;float areaJulia = 2.25f;

    // Start is called before the first frame update
    void Start()
    {
        timeSeriessRopeSwingJulia = new List<List<float>>();
        startime = Time.fixedTimeAsDouble;
        //Julia = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cubeJuliaTimeStep += Time.deltaTime;
    }
    public Vector3 GetPostion()
    {
        return new Vector3(Julia.position.x, Julia.position.y + 6, Julia.position.z);
    }
    public void MakeSwingJulia(Vector3 connectedPos)
    {
        var ropeCubeJulia = connectedPos - Julia.position; // Endpunkt - Anfangspunkt
        alphaJulia = (float)Math.Atan(ropeCubeJulia.x / ropeCubeJulia.y);
        var FG = Julia.mass * g.magnitude * Math.Cos(alphaJulia);
        var FZ = Julia.mass * (Math.Pow(Julia.velocity.magnitude, 2.0f)) / (R);
        var normalizedVelocityJulia = Julia.velocity.normalized;
        var FR = (float)(-0.5 * areaJulia * constantAirFriction * cCube * Mathf.Pow(Julia.velocity.magnitude, 2.0f)) * normalizedVelocityJulia;
        var FH = (FG + FZ) * Math.Sin(alphaJulia);
        var FV = (FG + FZ) * Math.Cos(alphaJulia);
        var centripedalForceJulia = new Vector3((float)FH, (float)FV, 0.0f) ;
        var forceJ = centripedalForceJulia;
        Julia.AddForce(forceJ);
        var degree = ConvertRadiansToDegrees(alphaJulia);
        cubeJuliaTimeStep += Time.deltaTime;
        timeSeriessRopeSwingJulia.Add(new List<float>() { cubeJuliaTimeStep, Julia.position.x, Julia.position.y, alphaJulia,(float)degree, (float)FH, (float)FV, forceJ.x, forceJ.y, forceJ.z });
    }

    void OnApplicationQuit()
    {
        WriteTimeSeriessRopeSwingJuliaToCsv();
    }
    void WriteTimeSeriessRopeSwingJuliaToCsv()
    {
        using (var streamWriter = new StreamWriter("timeSeriesRopeJulia.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeJulia.position.x, cubeJulia.position.y, alphaJulia, degree, horizonForceJulia, verticalForceJulia, -frictionForceJulia.x + horizonForceJulia, -frictionForceJulia.y + verticalForceJulia, -frictionForceJulia.z + zAxisForceJulia");

            foreach (List<float> timeStep in timeSeriessRopeSwingJulia)
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
