using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class CubeController : MonoBehaviour
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

    private FixedJoint jointRomeo;
    private FixedJoint jointJulia;

    private string filePath;
    private byte[] fileData;
    float springPotentialEnergy = 0f;
    float cubeRomeoKinetic = 0f;
    float cubeRomeoImpulse = 0f;
    float cubeJuliaImpulse = 0f;
    float GesamtImpluls = 0f;
    float ImpulsCheck = 0f;
    float forceOnJulia = 0f;
    float velocityEnd = 0f;
    float cubeKineticEnd = 0f;
    float constantForce = 4f;
    double startime = 0;
    double accelarationTime = 1.0;
    float springConstant = 0f;
    float springMaxDeviation = 0f;
    float springContraction = 1.7f;
    float springLength = 0f;
    float R = 6f; //Radius Rope

    float g = 9.81f; //Gravity

    float alphaRomeo = 0f; //Angle between crane and rope
    float alphaJulia = 0f; //Angle between crane and rope

    float cCube = 1.1f;
    float constantAirFriction = 1.2f;

    float areaRomeo = 2.25f;
    float areaJulia = 2.25f;

    // Start is called before the first frame update
    void Start()
    {
        startime = Time.fixedTimeAsDouble;

        timeSeriesElasticCollision = new List<List<float>>();
        timeSeriessInelasticCollision = new List<List<float>>();


        springLength = spring.GetComponent<MeshFilter>().mesh.bounds.size.y * spring.transform.localScale.y;

        //Maximale Auslenkung gerechnet anhand der linken seite des Feders
        springMaxDeviation = spring.transform.position.x - springLength / 2;
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
            constantForce = 4f;
            cubeRomeo.AddForce(new Vector3(constantForce, 0f, 0f));
        }

        // 1/2*m*v^2
        cubeRomeoKinetic = Math.Abs((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0)));


        float collisionPosition = cubeRomeo.transform.position.x + cubeRomeo.transform.localScale.x / 2;

        if (collisionPosition >= springMaxDeviation)
        {
            float springForceX = (collisionPosition - springMaxDeviation) * -springConstant;
            springPotentialEnergy =(float)(0.5 * springConstant * Math.Pow(collisionPosition - springMaxDeviation, 2.0));
            cubeRomeo.AddForce(new Vector3(springForceX, 0f, 0f));
            ChangeCubeTexture();
            currentTimeStep += Time.deltaTime;
            timeSeriesElasticCollision.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x,  springPotentialEnergy, cubeRomeoKinetic, springForceX });
        }

        // 1/2*m*v^2
        cubeRomeoKinetic = Math.Abs((float)(0.5 * cubeRomeo.mass * Math.Pow(cubeRomeo.velocity.x, 2.0)));
        cubeRomeoImpulse = Math.Abs(cubeRomeo.mass * cubeRomeo.velocity.x);
        cubeJuliaImpulse = Math.Abs(cubeJulia.mass * cubeJulia.velocity.x);
        GesamtImpluls = cubeJuliaImpulse + cubeRomeoImpulse;
        velocityEnd = (cubeRomeoImpulse + cubeJuliaImpulse) / (cubeRomeo.mass + cubeJulia.mass);
        ImpulsCheck = (cubeRomeo.mass + cubeJulia.mass )* velocityEnd;
        cubeKineticEnd = Math.Abs((float)(0.5 * (cubeRomeo.mass + cubeJulia.mass) * Math.Pow(velocityEnd, 2.0)));
        forceOnJulia = Math.Abs(cubeJulia.mass * velocityEnd - cubeJulia.velocity.x);

        cubeJuliaTimeStep += Time.deltaTime;
        timeSeriessInelasticCollision.Add(new List<float>() { cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x,cubeRomeo.mass, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x,cubeJulia.mass, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia, GesamtImpluls, ImpulsCheck });

        /*
         * Teil 3 Seilschwung
         */

        ////Romeo

        //alphaRomeo = RopeRomeo.transform.position.y - cubeRomeo.position.x;
        ////Radial Gravity Rope Romeo
        //var radialGravityRopeRomeo = cubeRomeo.mass * g * Math.Cos(alphaRomeo);
        ////Centripedal force
        //var centriPedalForceRomeo = cubeRomeo.mass * (Math.Pow(cubeRomeo.velocity.x, 2.0)) / (R);

        ////Turbulent viskose Friction
        //var normalizedVelocityRomeo = cubeRomeo.velocity.normalized;
        //var frictionForceRomeo = (float)(-0.5 * areaRomeo * constantAirFriction * cCube * Math.Pow(cubeRomeo.velocity.x, 2.0)) * normalizedVelocityRomeo;

        //var horizonForceRomeo = radialGravityRopeRomeo + centriPedalForceRomeo * Math.Sin(alphaRomeo);
        //var verticalForceRomeo = radialGravityRopeRomeo + centriPedalForceRomeo * Math.Cos(alphaRomeo);
        //cubeRomeo.AddForce((float)(-frictionForceRomeo.x + horizonForceRomeo), (float)(-frictionForceRomeo.y + verticalForceRomeo), 0.0f);
        //currentTimeStep += Time.deltaTime;
        //timeSeriessRopeSwingRomeo.Add(new List<float>() { currentTimeStep, cubeRomeo.position.x, cubeRomeo.position.y, alphaRomeo, (float)horizonForceRomeo, (float)verticalForceRomeo, (float)(-frictionForceRomeo.x + horizonForceRomeo), (float)(-frictionForceRomeo.y + verticalForceRomeo) });

        ////Julia

        //alphaJulia = RopeJulia.transform.position.y - cubeJulia.position.x;
        ////Radial Gravity Rope Romeo
        //var radialGravityRopeJulia = cubeJulia.mass * g * Math.Cos(alphaJulia);
        ////Centripedal force
        //var centriPedalForceJulia = cubeRomeo.mass * (Math.Pow(cubeRomeo.velocity.x, 2.0)) / (R);
        ////Turbulent viskose Friction
        //var normalizedVelocityJuia = cubeJulia.velocity.normalized;
        //var frictionForceJulia = (float)(-0.5 * areaJulia * constantAirFriction * cCube * Math.Pow(cubeJulia.velocity.x, 2.0)) * normalizedVelocityJuia;

        //var horizonForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Sin(alphaJulia);
        //var verticalForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Cos(alphaJulia);
        //cubeJulia.AddForce((float)(-frictionForceJulia.x + horizonForceJulia), (float)(-frictionForceJulia.y + verticalForceJulia), 0.0f);
        //cubeJuliaTimeStep += Time.deltaTime;
        //timeSeriessRopeSwingJulia.Add(new List<float>() { currentTimeStep, cubeJulia.position.x, cubeJulia.position.y, alphaJulia, (float)horizonForceJulia, (float)verticalForceJulia, (float)(-frictionForceJulia.x + horizonForceJulia), (float)(-frictionForceJulia.y + verticalForceJulia) });
        
 
    }
    void OnApplicationQuit()
    {
        WriteElasticTimeSeriesToCsv();
        WriteInelasticTimeSeriesToCsv();
        WriteTimeSeriessRopeSwingRomeoToCsv();
        WriteTimeSeriessRopeSwingJuliaToCsv();

    }
    void WriteElasticTimeSeriesToCsv()
    {
        using (var streamWriter = new StreamWriter("time_seriesElastic.csv"))
        {
            streamWriter.WriteLine("currentTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x, springPotentialEnergy, cubeRomeoKinetic, springForceX");

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
            streamWriter.WriteLine("cubeJuliaTimeStep, cubeRomeo.position.x, cubeRomeo.velocity.x,cubeRomeo.mass, cubeRomeoImpulse, cubeRomeoKinetic, cubeJulia.position.x, cubeJulia.velocity.x,cubeJulia.mass, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia, GesamtImpluls, ImpulsCheck }");

            foreach (List<float> timeStep in timeSeriessInelasticCollision)
            {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
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

    //void onTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("RopeRomeo") && other.CompareTag("RopeJulia"))
    //    {
    //        //RopeJulia = other.GetComponent<Rigidbody>();
    //        RopeRomeo = other.GetComponent<Rigidbody>();
    //        if ((RopeRomeo != null && jointRomeo == null)/* && RopeJulia != null && jointJulia == null*/)
    //        {
    //            jointRomeo = cubeRomeo.AddComponent<FixedJoint>();
    //            jointRomeo.connectedBody = RopeRomeo;
    //            jointRomeo.connectedAnchor = Vector3.zero; ;
    //            //jointJulia = cubeJulia.AddComponent<FixedJoint>();
    //            //jointJulia.connectedBody = RopeJulia;
    //            //jointJulia.connectedAnchor = Vector3.zero;
    //        }

    //    }
    //}
}