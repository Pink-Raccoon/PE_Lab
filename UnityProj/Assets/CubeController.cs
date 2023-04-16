﻿
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
    private Rigidbody cubeRomeo;
    private Rigidbody cubeJulia;
    private Rigidbody spring;

    public float springConstant; // N/m
    public float constantForce;
    public int springLength; // m

    private float currentTimeStep; // s
    private float cubeJuliaTimeStep;



    private List<List<float>> timeSeries;

    private bool hasSpeed;
    private bool hasElasticImpulse;

    // Start is called before the first frame update
    void Start()
    {
        cubeRomeo = GameObject.Find("Romeo").GetComponent<Rigidbody>();
        spring = GameObject.Find("spring").GetComponent<Rigidbody>();
        cubeJulia = GameObject.Find("Julia").GetComponent<Rigidbody>();

        timeSeries = new List<List<float>>(); 

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
        float cubeRomeKineticEnergy = 0f;
        float cubeRomeoImpulse = 0f;
        float cubeJuliaImpulse = 0f;




        // Move cube towards spring
        float springPosition = spring.transform.position.x;
        if (cubeRomeo.velocity.x < 2)
        {
            cubeRomeo.AddForce(new Vector3(constantForce,0f,0f));
            Vector3 rightMovement = transform.right * 2 * Time.fixedDeltaTime;
            cubeRomeo.MovePosition(cubeRomeo.position + rightMovement);

        }

        if (cubeRomeo.velocity.x == 2 && !hasElasticImpulse)
        {
            currentTimeStep += Time.deltaTime;

            timeSeries.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeKineticEnergy });
        }
        if ( cubeRomeo.position.x >= springPosition)
        {
            hasElasticImpulse = true;
           
            springPotentialEnergy = (float)(0.5 * springConstant * Math.Pow(cubeRomeo.position.x-springLength, 2.0)); // 1/2k * x^2
            cubeRomeKineticEnergy = ((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0))); // 1/2*m*v^2
            springForceX = (springPotentialEnergy + cubeRomeKineticEnergy) / springLength;//-k * x

            //cubeRomeo.AddForce(new Vector3(cubeRomeKineticEnergy, 0f, 0f));
            cubeRomeo.AddForce(new Vector3(springForceX, 0f, 0f));

            currentTimeStep += Time.deltaTime;
            timeSeries.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x });
        }
        else
        {
            cubeRomeoImpulse = cubeRomeo.mass * cubeRomeo.velocity.x;
            cubeJuliaImpulse = cubeJulia.mass * cubeJulia.velocity.x;
            hasElasticImpulse = false;
            cubeRomeo.AddForce(new Vector3(cubeRomeoImpulse,0,0));
            cubeJulia.AddForce(new Vector3(cubeJuliaImpulse,0,0));
            
        }

        currentTimeStep += Time.deltaTime;
        currentTimeStep += Time.deltaTime;


        timeSeries.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoImpulse,cubeJuliaTimeStep, cubeJulia.position.x, cubeJulia.velocity.x, cubeJuliaImpulse }); } void OnApplicationQuit()
    {
        WriteTimeSeriesToCSV();
    }
    void WriteTimeSeriesToCSV()
    {
        using (var streamWriter = new StreamWriter("time_series.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeJuliaTimeStep, cubeJulia.position.x, cubeJulia.velocity.x ");

            foreach (List<float> timeStep in timeSeries)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
}
