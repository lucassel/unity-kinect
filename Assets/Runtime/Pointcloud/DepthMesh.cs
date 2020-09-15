using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DepthMesh : MonoBehaviour
{
  public GameObject DepthReader;
  public bool UseJobSystem;

  public float size = 0.2f;
  public float scale = 10f;

  public bool Use32BitMesh;
  public bool UpdateDepth;

  public TextAsset TextFile;

  public void Start()
  {
    _reader = DepthReader.GetComponent<DepthReader>();
    _filter = GetComponent<MeshFilter>();
    _renderer = GetComponent<MeshRenderer>();
    _material = _renderer.sharedMaterial;

    _mesh = new Mesh();
    //_mesh.MarkDynamic();
    if (Use32BitMesh)
    {
      _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    _filter.sharedMesh = _mesh;
  }

  void CreateMesh()
  {
    _vertices = new Vector3[_reader.NumPoints];
    _indices = new int[_vertices.Length];

    for (var i = 0; i < _vertices.Length; ++i)
    {
      _indices[i] = i;
    }

    _mesh.SetVertices(_vertices);
    _mesh.SetIndices(_indices, MeshTopology.Points, 0);
    _mesh.bounds = new Bounds(transform.position, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));

    _meshCreated = true;
    Debug.Log("Mesh created.");
    print($"mesh has {_vertices.Length} points");
    //print(_material.GetFloat(PointSize));
  }


  void Update()
  {
    if (_reader.vertices.Length == 0) return;

    if (_meshCreated == false)
    {
      CreateMesh();
    }
    else
    {
      if (!UpdateDepth)
      {
        return;
      }

      UpdateMesh();
    }


    _material.SetFloat(PointSize, size);
  }

  public void WritePointCloud()
  {
    UpdateDepth = false;
    UpdateMesh();
    var p = AssetDatabase.GetAssetPath(TextFile);
    var writer = new StreamWriter(p, false);
    Debug.Log($"writing to {p}!");
    var wrote = 0;
    for (var i = 0; i < _reader.vertices.Length; i++)
    {
      if (!float.IsNegativeInfinity(_reader.vertices[i].x))
      {
        writer.WriteLine($"{_reader.vertices[i].x}/{_reader.vertices[i].y}/{_reader.vertices[i].z}");
        wrote++;
      }
    }

    Debug.Log($"Wrote {wrote}/{_reader.vertices.Length} points away");

    writer.Close();
    UpdateDepth = true;
  }

   void UpdateMesh()
  {
    var buffer = new Vector3[_reader.NumPoints];

    if (UseJobSystem)
    {
      ExecutePointCloudJob(_mesh.vertices);
    }

    else
    {
      for (var i = 0; i < buffer.Length; i++)
      {
        buffer[i] = _reader.vertices[i];
      }
    }

    _mesh.SetVertices(buffer);
  }

  private void ExecutePointCloudJob(Vector3[] copy)
  {
    var vertArray = new NativeArray<Vector3>(copy, Allocator.TempJob);
    var job = new PointCloudMeshJob
    {
      Vertices = vertArray,
      Reader = _reader
    };
    var jobHandle = job.Schedule(_vertices.Length, 500);
    jobHandle.Complete();

    vertArray.CopyTo(_vertices);
    vertArray.Dispose();
  }

  DepthReader _reader;
  Mesh _mesh;
  MeshFilter _filter;
  MeshRenderer _renderer;
  Material _material;

  Vector3[] _vertices;
  int[] _indices;

  bool _meshCreated;

  static readonly int PointSize = Shader.PropertyToID("_PointSize");
}