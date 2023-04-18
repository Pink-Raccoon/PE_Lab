using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.WSA;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine.UIElements;


public class CubeController : MonoBehaviour
{
    public Rigidbody cubeRomeo;
    public Rigidbody cubeJulia;
    public Rigidbody spring;

    public float springConstant; // N/m
    public float constantForce;
    public int springLength; // m
    public float cubeRomeoPosition;
    public float cubeStartPosition                                                                                                                                                                                                   

    private float currentTimeStep; // s
    private float cubeJuliaTimeStep;



    private List<List<float>> timeSeriesElasticCollision;
    private List<List<float>> timeSeriessInelasticCollision;
    // Start is called before the first frame update
    void Start()
    {
        cubeRomeo = GameObject.Find("Romeo").GetComponent<Rigidbody>();
        spring = GameObject.Find("spring").GetComponent<Rigidbody>();
        cubeJulia = GameObject.Find("Julia").GetComponent<Rigidbody>();

        timeSeriesElasticCollision = new List<List<float>>();
        timeSeriessInelasticCollision = new List<List<float>>();


    }

    // Update is called once per frame
    void Update()
    {

    }
    // FixedUpdate can be called multiple times per frame
    void FixedUpdate()
    {
        float springForceX = 0; // N
        float springPotentialEnergy = 0f;
        float cubeRomeoKinetic = 0f;
        float cubeRomeoImpulse = 0f;
        float cubeJuliaImpulse = 0f;
        float springDisplacement = 0.82f;
        float forceOnJulia = 0f;
        float velocityEnd = 0f;
        float cubeKineticEnd = 0f;



        // Move cube towards spring
        float springPosition = spring.transform.position.x;
        if (cubeRomeo.velocity.x <= 2)
        {
            cubeRomeo.AddForce(new Vector3(constantForce, 0f, 0f));
        }
        if (cubeRomeo.position.x >= springPosition - springLength)
        {
            springPotentialEnergy = (float)(0.5 * springConstant * Math.Pow(springLength, 2.0)); // 1/2k * x^2
            cubeRomeoKinetic = ((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0))); // 1/2*m*v^2
            springForceX = (springPotentialEnergy + cubeRomeoKinetic) / -(springLength);
            cubeRomeo.AddForce(new Vector3(springForceX, 0f, 0f));
            currentTimeStep += Time.deltaTime;
            timeSeriesElasticCollision.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoKinetic, springPotentialEnergy, cubeRomeoKinetic, springForceX });
        }
        else
        {

            cubeRomeoKinetic = ((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0))); // 1/2*m*v^2
            cubeRomeoImpulse = cubeRomeo.mass * cubeRomeo.velocity.x;
            cubeJuliaImpulse = cubeJulia.mass * cubeJulia.velocity.x;
            velocityEnd = (cubeRomeoImpulse + cubeJuliaImpulse) / (cubeRomeo.mass + cubeJulia.mass);
            cubeKineticEnd = (float)(0.5 * (cubeRomeo.mass + cubeJulia.mass) * Math.Pow(velocityEnd, 2.0));
            forceOnJulia = cubeJulia.mass * velocityEnd - cubeJulia.velocity.x;

            cubeRomeo.AddForce(new Vector3(springForceX, 0, 0));
            cubeJulia.AddForce(new Vector3(forceOnJulia, 0, 0));

        }
        cubeJuliaTimeStep += Time.deltaTime;
        timeSeriessInelasticCollision.Add(new List<float>() { cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia });
    }
    void OnApplicationQuit()
    {
        WriteTimeSeriesToCSV();
    }
    void WriteTimeSeriesToCSV()
    {
        using (var streamWriter = new StreamWriter("time_seriesElastic.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoKinetic, springPotentialEnergy, cubeRomeoKinetic, springForceX ");

            foreach (List<float> timeStep in timeSeriesElasticCollision)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
        using (var streamWriter = new StreamWriter("time_seriesInelastic.csv"))
        {
            streamWriter.WriteLine(" cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia");

            foreach (List<float> timeStep in timeSeriessInelasticCollision)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != cubeJulia) return;



        FixedJoint joint = gameObject.AddComponent<FixedJoint>();



        //Set the anchor point where the wand and blade collide
        ContactPoint contact = collision.contacts[0];
        joint.anchor = transform.InverseTransformPoint(contact.point);
        joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponent<Rigidbody>();



        // Stops objects from continuing to collide and creating more joints
        joint.enableCollision = false;
    }
}