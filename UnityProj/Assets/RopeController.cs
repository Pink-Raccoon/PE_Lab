using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Rigidbody cubeRomeo;
    public Rigidbody cubeJulia;
    public GameObject spring;
    public Rigidbody RopeRomeo;
    public Rigidbody RopeJulia;
    private float currentTimeStep; // s
    private float cubeJuliaTimeStep;

    private List<List<float>> timeSeriesElasticCollision;
    private List<List<float>> timeSeriessInelasticCollision;
    private List<List<float>> timeSeriessRopeSwingRomeo;
    private List<List<float>> timeSeriessRopeSwingJulia;


    float R = 6f; //Radius Rope

    float g = 9.81f; //Gravity

    float alphaRomeo = 0f; //Angle between crane and rope
    float alphaJulia = 0f; //Angle between crane and rope

    float cCube = 1.1f;
    float constantAirFriction = 1.2f;

    float areaRomeo = 2.25f;
    float areaJulia = 2.25f;

    private FixedJoint jointRomeo;
    private FixedJoint jointJulia;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //Romeo
        if (jointRomeo != null)
        {
            alphaRomeo = RopeRomeo.transform.position.y - cubeRomeo.position.x;
            //Radial Gravity Rope Romeo
            var radialGravityRopeRomeo = cubeRomeo.mass * g * Math.Cos(alphaRomeo);
            //Centripedal force
            var centriPedalForceRomeo = cubeRomeo.mass * (Math.Pow(cubeRomeo.velocity.x, 2.0)) / (R);

            //Turbulent viskose Friction
            var normalizedVelocityRomeo = cubeRomeo.velocity.normalized;
            var frictionForceRomeo = (float)(-0.5 * areaRomeo * constantAirFriction * cCube * Math.Pow(cubeRomeo.velocity.x, 2.0)) * normalizedVelocityRomeo;

            var horizonForceRomeo = radialGravityRopeRomeo + centriPedalForceRomeo * Math.Sin(alphaRomeo);
            var verticalForceRomeo = radialGravityRopeRomeo + centriPedalForceRomeo * Math.Cos(alphaRomeo);
            cubeRomeo.AddForce((float)(-frictionForceRomeo.x + horizonForceRomeo), (float)(-frictionForceRomeo.y + verticalForceRomeo), 0.0f);
            currentTimeStep += Time.deltaTime;
            timeSeriessRopeSwingRomeo.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.position.y, alphaRomeo, (float)horizonForceRomeo, (float)verticalForceRomeo, (float)(-frictionForceRomeo.x + horizonForceRomeo), (float)(-frictionForceRomeo.y + verticalForceRomeo) });
        }

        if (jointJulia != null)
        {
            //Julia

            alphaJulia = RopeJulia.transform.position.y - cubeJulia.position.x;
            //Radial Gravity Rope Romeo
            var radialGravityRopeJulia = cubeJulia.mass * g * Math.Cos(alphaJulia);
            //Centripedal force
            var centriPedalForceJulia = cubeRomeo.mass * (Math.Pow(cubeRomeo.velocity.x, 2.0)) / (R);
            //Turbulent viskose Friction
            var normalizedVelocityJuia = cubeJulia.velocity.normalized;
            var frictionForceJulia = (float)(-0.5 * areaJulia * constantAirFriction * cCube * Math.Pow(cubeJulia.velocity.x, 2.0)) * normalizedVelocityJuia;

            var horizonForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Sin(alphaJulia);
            var verticalForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Cos(alphaJulia);
            cubeJulia.AddForce((float)(-frictionForceJulia.x + horizonForceJulia), (float)(-frictionForceJulia.y + verticalForceJulia), 0.0f);
            cubeJuliaTimeStep += Time.deltaTime;
            timeSeriessRopeSwingJulia.Add(new List<float>() { currentTimeStep, cubeJulia.position.x, cubeJulia.position.y, alphaJulia, (float)horizonForceJulia, (float)verticalForceJulia, (float)(-frictionForceJulia.x + horizonForceJulia), (float)(-frictionForceJulia.y + verticalForceJulia) });
        }
        

    }
    void OnApplicationQuit()
    {

        WriteTimeSeriessRopeSwingRomeoToCsv();
        WriteTimeSeriessRopeSwingJuliaToCsv();

    }
    void WriteTimeSeriessRopeSwingRomeoToCsv()
    {
        using (var streamWriter = new StreamWriter("timeSeriesRopeRomeo.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.position.y,alphaRomeo,horizonForceRomeo,verticalForceRomeo,-frictionForceRomeo.x + horizonForceRomeo, -frictionForceRomeo.y + verticalForceRomeo");

            foreach (List<float> timeStep in timeSeriessRopeSwingRomeo)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }

    void WriteTimeSeriessRopeSwingJuliaToCsv()
    {
        using (var streamWriter = new StreamWriter("timeSeriesRopJulia.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeJulia.position.x, cubeJulia.position.y,alphaJulia,horizonForceJulia,verticalForceJulia,-frictionForceJulia.x + horizonForceJulia, -frictionForceJulia.y + verticalForceJulia");

            foreach (List<float> timeStep in timeSeriessRopeSwingJulia)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == cubeRomeo)
        {
            RopeRomeo = other.GetComponent<Rigidbody>();
            if (RopeRomeo != null && jointRomeo == null)
            {
                jointRomeo = cubeRomeo.gameObject.AddComponent<FixedJoint>();
                jointRomeo.connectedBody = RopeRomeo;
                jointRomeo.connectedAnchor = Vector3.zero;
            }
        }
        else if (other.attachedRigidbody == cubeJulia)
        {
            RopeJulia = other.GetComponent<Rigidbody>();
            if (RopeJulia != null && jointJulia == null)
            {
                jointJulia = cubeJulia.gameObject.AddComponent<FixedJoint>();
                jointJulia.connectedBody = RopeJulia;
                jointJulia.connectedAnchor = Vector3.zero;
            }
        }
    }


}
