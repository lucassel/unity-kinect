using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthParticlize : MonoBehaviour
{
    public GameObject MultiSourceManager;

    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;

    private MultiSourceManager _MultiManager;

    FrameDescription depthFrameDesc;
    CameraSpacePoint[] cameraSpacePoints;

    private int depthWidth;
    private int depthHeight;

    ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;

    public Color color;
    public float size = 0.2f;
    public float scale = 10f;

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;

            depthFrameDesc = _Sensor.DepthFrameSource.FrameDescription;
            depthWidth = depthFrameDesc.Width;
            depthHeight = depthFrameDesc.Height;

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

            particles = new ParticleSystem.Particle[depthWidth * depthHeight];
            _particleSystem = gameObject.GetComponent<ParticleSystem>();
            _MultiManager = MultiSourceManager.GetComponent<MultiSourceManager>();

            cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];
            //gameObject.GetComponent<Renderer>().material.mainTexture = _MultiManager.GetColorTexture();
        }
    }


    void Update()
    {

        if (_Sensor == null) return;
        if (MultiSourceManager == null) return;
        if (_MultiManager == null) return;

        ushort[] rawdata = _MultiManager.GetDepthData();

        _Mapper.MapDepthFrameToCameraSpace(rawdata, cameraSpacePoints);

        for (int i = 0; i < cameraSpacePoints.Length; i++)
        {
            particles[i].position = new Vector3(cameraSpacePoints[i].X * scale, cameraSpacePoints[i].Y * scale, cameraSpacePoints[i].Z * scale);
            particles[i].startColor = color;
            particles[i].startSize = size;
            //if (rawdata[i] == 0) particles[i].startSize = 0;
        }

        _particleSystem.SetParticles(particles, particles.Length);

    }


    void OnApplicationQuit()
    {
        if (_Mapper != null)
        {
            _Mapper = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}