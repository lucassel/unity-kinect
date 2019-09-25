using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthDebug : MonoBehaviour
{
    public GameObject MultiSourceManager;

    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;

    private MultiSourceManager _MultiManager;

    FrameDescription depthFrameDesc;
    CameraSpacePoint[] cameraSpacePoints;

    private int depthWidth;
    private int depthHeight;

    private Vector3[] points;

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
            Debug.Log("Width = " + depthWidth);
            Debug.Log("Total points = " + depthWidth * depthHeight);
            points = new Vector3[depthWidth * depthHeight];
            //_particleSystem = gameObject.GetComponent<ParticleSystem>();
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
            points[i] = new Vector3(cameraSpacePoints[i].X * scale, cameraSpacePoints[i].Y * scale, cameraSpacePoints[i].Z * scale);
        }

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

    void OnDrawGizmos()
    {
        if(points == null) return;

        foreach (var point in points)
        {
            //Gizmos.DrawWireSphere(point, .1f);
        }
    }
}