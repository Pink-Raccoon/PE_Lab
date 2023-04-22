using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    public Rigidbody cubeRomeo;
    public Rigidbody cubeJulia;
    public GameObject spring;

    private float currentTimeStep; // s
    private float cubeJuliaTimeStep;

    private List<List<float>> timeSeriesElasticCollision;
    private List<List<float>> timeSeriessInelasticCollision;

    private string filePath;
    private byte[] fileData;
    float springPotentialEnergy = 0f;
    float cubeRomeoKinetic = 0f;
    float cubeRomeoImpulse = 0f;
    float cubeJuliaImpulse = 0f;
    float forceOnJulia = 0f;
    float velocityEnd = 0f;
    float cubeKineticEnd = 0f;
    float constantForce = 4f;
    double startime = 0;
    private double accelarationTime = 1.0;
    float springConstant = 0f;
    float springMaxDeviation = 0f;
    

    float springContraction = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        startime = Time.fixedTimeAsDouble;

        timeSeriesElasticCollision = new List<List<float>>();
        timeSeriessInelasticCollision = new List<List<float>>();

        //Maximale Auslenkung gerechnet anhand der linken seite des Feders
        springMaxDeviation = spring.transform.position.x - spring.transform.localScale.y / 2;

        // Energieerhaltungsgesetz kinEnergie = PotEnergie : 1/2*m*v^2 = 1/2k * x^2
        springConstant = (float)((cubeRomeo.mass * Math.Pow(2.0, 2)) / (Math.Pow(springContraction, 2.0)));
    }

    // Update is called once per frame
    void Update()
    {
    }
    // FixedUpdate can be called multiple times per frame
    void FixedUpdate()
    {
        double currentTime = Time.fixedTimeAsDouble-startime;
        
        if (accelarationTime >= currentTime)
        {
            //accelaration = velocity / time; 
            constantForce = 4f;
            cubeRomeo.AddForce(new Vector3(constantForce, 0f, 0f));
        }

        cubeRomeoKinetic = Math.Abs((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0))); // 1/2*m*v^2
        springPotentialEnergy = Math.Abs((float)(0.5 * springConstant * Math.Pow(springContraction,2.0)));
        //currentTimeStep += Time.deltaTime;
        //timeSeriesElasticCollision.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoKinetic, springPotentialEnergy, cubeRomeoKinetic});

        float collisionPosition = cubeRomeo.transform.position.x + cubeRomeo.transform.localScale.x / 2;

        if (collisionPosition >= springMaxDeviation)
        {
            float springForceX = (collisionPosition - springMaxDeviation) * -springConstant;
            cubeRomeo.AddForce(new Vector3(springForceX, 0f, 0f));
            ChangeCubeTexture();
            currentTimeStep += Time.deltaTime;
            timeSeriesElasticCollision.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoKinetic, springPotentialEnergy, cubeRomeoKinetic, springForceX });
        }

        // 1/2*m*v^2
        cubeRomeoKinetic = Math.Abs((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0))); 
        cubeRomeoImpulse = Math.Abs(cubeRomeo.mass * cubeRomeo.velocity.x);
        cubeJuliaImpulse = Math.Abs(cubeJulia.mass * cubeJulia.velocity.x);
        velocityEnd = (cubeRomeoImpulse + cubeJuliaImpulse) / (cubeRomeo.mass + cubeJulia.mass);
        cubeKineticEnd = Math.Abs((float)(0.5 * (cubeRomeo.mass + cubeJulia.mass) * Math.Pow(velocityEnd, 2.0)));
        forceOnJulia = Math.Abs(cubeJulia.mass * velocityEnd - cubeJulia.velocity.x);

        cubeJuliaTimeStep += Time.deltaTime;
       timeSeriessInelasticCollision.Add(new List<float>() { cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x,cubeRomeo.mass, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x,cubeJulia.mass, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia });
    }
    void OnApplicationQuit()
    {
        WriteElasticTimeSeriesToCsv();
        WriteInelasticTimeSeriesToCsv();
    }
    void WriteElasticTimeSeriesToCsv()
    {
        using (var streamWriter = new StreamWriter("time_seriesElastic.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, cubeRomeoKinetic, springPotentialEnergy, cubeRomeoKinetic, springForceX");

            foreach (List<float> timeStep in timeSeriesElasticCollision)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }

    }

    void WriteInelasticTimeSeriesToCsv()
    {
        using (var streamWriter = new StreamWriter("time_seriesInelastic.csv"))
        {
            streamWriter.WriteLine("cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x,cubeRomeo.mass, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x,cubeJulia.mass, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia ");

            foreach (List<float> timeStep in timeSeriessInelasticCollision)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }

    void ChangeCubeTexture()
    {
        // the path of the image
        filePath = "Assets/Images/snoopy-flower-cynthia-t-thomas.jpg";
        // 1.read the bytes array
        fileData = File.ReadAllBytes(filePath);
        // 2.create a texture named tex
        Texture2D tex = new Texture2D(2, 2);
        // 3.load inside tx the bytes and use the correct image size
        tex.LoadImage(fileData);
        // 4.apply tex to material.mainTexture 
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != cubeJulia)
        {
            return;
        }
        if (collision.rigidbody == cubeJulia)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            ContactPoint[] contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            ContactPoint contact = contacts[0];
            joint.anchor = transform.InverseTransformPoint(contact.point);
            joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponent<Rigidbody>();


            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;
        }
    }
}