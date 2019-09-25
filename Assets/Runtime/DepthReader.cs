using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class DepthReader : MonoBehaviour
{

    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;

    private MultiSourceManager _MultiManager;
    private MultiSourceFrameReader _multiSourceFrameReader;

    private FrameDescription _depthFrameDesc;
    FrameDescription _colorFrameDesc;
    CameraSpacePoint[] _cameraSpacePoints;
    ColorSpacePoint[] _colorSpacePoints;

    ushort[] depthFrameData;
    byte[] colorFrameData;
    int bytesPerPixel = 4;

    
    public int NumPoints;
    int depthWidth;
    int depthHeight;

    [HideInInspector]
    public Vector3[] vertices;

    public Color[] Colors;

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            _multiSourceFrameReader = _Sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);

            _depthFrameDesc = _Sensor.DepthFrameSource.FrameDescription;
            _colorFrameDesc = _Sensor.ColorFrameSource.FrameDescription;
            depthWidth = _depthFrameDesc.Width;
            depthHeight = _depthFrameDesc.Height;
            
            NumPoints = depthWidth * depthHeight;

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
            _MultiManager = GetComponent<MultiSourceManager>();

            depthFrameData = new ushort[depthWidth * depthHeight];
            _colorSpacePoints = new ColorSpacePoint[depthWidth * depthHeight];
            _cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];
            
            _cameraSpacePoints = new CameraSpacePoint[depthWidth * depthHeight];
            vertices = new Vector3[depthWidth * depthHeight];
            
            
            int colorWidth = _colorFrameDesc.Width;
            int colorHeight = _colorFrameDesc.Height;
            Colors = new Color[depthWidth * depthHeight];
            colorFrameData = new byte[colorWidth * colorHeight * bytesPerPixel];


        }


    }

    


    void Update()
    {

        if (_Sensor == null) return;
       
        if (_MultiManager == null) return;
       

        ushort[] rawdata = _MultiManager.GetDepthData();

        _Mapper.MapDepthFrameToColorSpace(rawdata, _colorSpacePoints);
        _Mapper.MapDepthFrameToCameraSpace(rawdata, _cameraSpacePoints);

        for (int i = 0; i < _cameraSpacePoints.Length; i++)
        {
            vertices[i] = new Vector3(_cameraSpacePoints[i].X, _cameraSpacePoints[i].Y, _cameraSpacePoints[i].Z);
            Colors[i] = Color.white;
            
        }
        
        
       
    }
    
    


    void OnApplicationQuit()
    {
        _multiSourceFrameReader.Dispose();
        _multiSourceFrameReader = null;

        
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