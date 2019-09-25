using UnityEngine;
using System.Collections;
 
 
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GeometryShaderDebug : MonoBehaviour {
 
    private Mesh mesh;
    int numPoints = 200000;
 
    // Use this for initialization
    void Start () {
       
        CreateMesh();
    }
 
    void CreateMesh() {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        GetComponent<MeshFilter>().mesh = mesh;
        Vector3[] points = new Vector3[numPoints];
        int[] indecies = new int[numPoints];
        Color[] colors = new Color[numPoints];
        for(int i=0;i<points.Length;++i) {
            points[i] = new Vector3(Random.Range(-10f,10f), Random.Range (-10f,10f), Random.Range (-10f,10f));
            indecies[i] = i;
            colors[i] = new Color(Random.Range(0.0f,1.0f),Random.Range (0.0f,1.0f),Random.Range(0.0f,1.0f),1.0f);
        }
 
        mesh.vertices = points;
        mesh.colors = colors;
        mesh.SetIndices(indecies, MeshTopology.Points,0);
 
    }
}