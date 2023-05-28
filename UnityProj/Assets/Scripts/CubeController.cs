using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    public Rigidbody Romeo;
    public Rigidbody Julia;
    public GameObject spring;
    public GameObject ropeRomeo;
    public GameObject ropeJulia;

   // public Vector3 ropeAnchor;

    private float currentTimeStep; // s
    private float cubeJuliaTimeStep;

    private List<List<float>> timeSeriesElasticCollision;
    private List<List<float>> timeSeriessInelasticCollision;
    private List<List<float>> timeSeriessRopeSwingRomeo;
    private List<List<float>> timeSeriessRopeSwingJulia;

    private bool rotationTriggered = false;
 
    
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
    //public float rotationTrigger;

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
        
       
        Romeo = GetComponent<Rigidbody>();
        Julia = GetComponent<Rigidbody>();

        startime = Time.fixedTimeAsDouble;

        timeSeriesElasticCollision = new List<List<float>>();
        timeSeriessInelasticCollision = new List<List<float>>();
        timeSeriessRopeSwingRomeo = new List<List<float>>(); ;
        timeSeriessRopeSwingJulia = new List<List<float>>(); ;


    springLength = spring.GetComponent<MeshFilter>().mesh.bounds.size.y * spring.transform.localScale.y;

        //Maximale Auslenkung gerechnet anhand der linken seite des Feders
        springMaxDeviation = spring.transform.position.x - springLength / 2;
         // Energieerhaltungsgesetz kinEnergie = PotEnergie : 1/2*m*v^2 = 1/2k * x^2
        springConstant = (float)((Romeo.mass * Math.Pow(2.0, 2)) / (Math.Pow(springContraction, 2.0)));



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
            Romeo.AddForce(new Vector3(constantForce, 0f, 0f));
        }

        // 1/2*m*v^2
        cubeRomeoKinetic = Math.Abs((float)(0.5 * Romeo.mass * Math.Pow(Romeo.velocity.x, 2.0)));


        float collisionPosition = Romeo.transform.position.x + Romeo.transform.localScale.x / 2;

        if (collisionPosition >= springMaxDeviation)
        {
            float springForceX = (collisionPosition - springMaxDeviation) * -springConstant;
            springPotentialEnergy =(float)(0.5 * springConstant * Math.Pow(collisionPosition - springMaxDeviation, 2.0));
            Romeo.AddForce(new Vector3(springForceX, 0f, 0f));
            ChangeCubeTexture();
            currentTimeStep += Time.deltaTime;
            timeSeriesElasticCollision.Add(new List<float>() { currentTimeStep, Romeo.position.x, Romeo.velocity.x,  springPotentialEnergy, cubeRomeoKinetic, springForceX });
        }

        // 1/2*m*v^2
        cubeRomeoKinetic = Math.Abs((float)(0.5 * Romeo.mass * Math.Pow(Romeo.velocity.x, 2.0)));
        cubeRomeoImpulse = Math.Abs(Romeo.mass * Romeo.velocity.x);
        cubeJuliaImpulse = Math.Abs(Julia.mass * Julia.velocity.x);
        GesamtImpluls = cubeJuliaImpulse + cubeRomeoImpulse;
        velocityEnd = (cubeRomeoImpulse + cubeJuliaImpulse) / (Romeo.mass + Julia.mass);
        ImpulsCheck = (Romeo.mass + Julia.mass )* velocityEnd;
        cubeKineticEnd = Math.Abs((float)(0.5 * (Romeo.mass + Julia.mass) * Math.Pow(velocityEnd, 2.0)));
        forceOnJulia = Math.Abs(Julia.mass * velocityEnd - Julia.velocity.x);

        cubeJuliaTimeStep += Time.deltaTime;
        timeSeriessInelasticCollision.Add(new List<float>() { cubeJuliaTimeStep, Romeo.position.x, Romeo.velocity.x,Romeo.mass, cubeRomeoImpulse, cubeRomeoKinetic, Julia.position.x, Julia.velocity.x,Julia.mass, cubeJuliaImpulse, velocityEnd, cubeKineticEnd, forceOnJulia, GesamtImpluls, ImpulsCheck });
        



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
        if (collision.rigidbody != Julia)
        {
            return;
        }
        if (collision.rigidbody == Julia)
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
    //* Teil 3 Seilschwung
    //    */

    public void MakeSwing()
    {
        
        
        var ropeCubeRomeo = Romeo.position -ropeRomeo.transform.position ; 
        alphaRomeo = (float)Math.Atan2(ropeCubeRomeo.x , ropeCubeRomeo.y);
        Debug.Log("alphaRomeo is:" + alphaRomeo);

        //Radial Gravity Rope Romeo
        var FG = Romeo.mass * g * Math.Cos(alphaRomeo);
        //Centripedal force
        var FZ = Romeo.mass * (Math.Pow(Romeo.velocity.x, 2.0f)) / (R);
        var FH = FG + FZ * Math.Sin(alphaRomeo);
        var FV = FG + FZ * Math.Cos(alphaRomeo);
        //Turbulent viskose Friction
        var normalizedVelocityRomeo = Romeo.velocity.normalized;
        var frictionForceRomeo = (float)(-0.5 * areaRomeo * constantAirFriction * cCube * Math.Pow(Romeo.velocity.x, 2.0f)) * normalizedVelocityRomeo;
        Vector3 centripedalForceRomeo = new Vector3((float)FH, (float)FV, 0.0f);
        //Destroy(gameObject.GetComponent<FixedJoint>());
        Romeo.AddForce(centripedalForceRomeo + frictionForceRomeo);
        var force = centripedalForceRomeo + frictionForceRomeo;
        Debug.Log("swing Romeo" + (centripedalForceRomeo +frictionForceRomeo));
        currentTimeStep += Time.deltaTime;
        timeSeriessRopeSwingRomeo.Add(new List<float>() { currentTimeStep, Romeo.position.x, Romeo.position.y, alphaRomeo, (float)FH, (float)FV, force.x, force.y, force.z });


        //Julia

        var ropeCubeJulia = Julia.position - ropeJulia.transform.position;
        alphaJulia =(float) Math.Atan2(ropeCubeJulia.x , ropeCubeJulia.y);
        Debug.Log("alphaJulia is:" + alphaJulia);
        //Radial Gravity Rope Romeo
        var radialGravityRopeJulia = Julia.mass * g * Math.Cos(alphaJulia);
        //Centripedal force
        var centriPedalForceJulia = Julia.mass * (Math.Pow(Julia.velocity.x, 2.0f)) / (R);
        //Turbulent viskose Friction
        var normalizedVelocityJuia = Julia.velocity.normalized;
        var frictionForceJulia = (float)(-0.5 * areaJulia * constantAirFriction * cCube * Mathf.Pow(Julia.velocity.x, 2.0f)) * normalizedVelocityJuia;

        var horizonForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Sin(alphaJulia);
        var verticalForceJulia = radialGravityRopeJulia + centriPedalForceJulia * Math.Cos(alphaJulia);

        var centripedalForceJulia = new Vector3((float)horizonForceJulia, (float)verticalForceJulia, 0.0f);
        Julia.AddForce(centripedalForceJulia+frictionForceJulia);
        var forceJ = centripedalForceJulia + frictionForceJulia;
        Debug.Log("swing julia" + (centripedalForceJulia+ frictionForceJulia));
        
        cubeJuliaTimeStep += Time.deltaTime;
        timeSeriessRopeSwingJulia.Add(new List<float>() { currentTimeStep, Julia.position.x, Julia.position.y, alphaJulia, (float)horizonForceJulia, (float)verticalForceJulia, forceJ.x, forceJ.y, forceJ.z });
      
    }

}