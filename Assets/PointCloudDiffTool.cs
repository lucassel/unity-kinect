using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PointCloudDiffTool : MonoBehaviour
{
   public TextAsset PointCloud1;
   public TextAsset PointCloud2;

   public Material Material1;
   public Material Material2;

   void Start()
   {
      ConstructPointCloud(PointCloud1, Material1);
      ConstructPointCloud(PointCloud2, Material2);
      
   }


   void ConstructPointCloud(TextAsset file, Material mat)
   {
      var p = AssetDatabase.GetAssetPath(file);
      string[] lines = File.ReadAllLines(p);
      print($"mesh has {lines.Length} points");

      
      var vertices = new Vector3[lines.Length];
      var indices = new int[vertices.Length];

      for (var i = 0; i < vertices.Length; ++i)
      {
         indices[i] = i;
      }
     
      
      var mesh = new Mesh();


      for (int i = 0; i < lines.Length; i++)
      {
       
         var split = lines[i].Split('/');
         var pos = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
         vertices[i] = pos;
         
         if (i == 0)
         {
            print(pos);
         }
      }
      
      
      mesh.SetVertices(vertices);
      mesh.SetIndices(indices, MeshTopology.Points, 0);
      
      
      var go = new GameObject(file.name, typeof(MeshFilter), typeof(MeshRenderer));

      var mf = go.GetComponent<MeshFilter>();
      var mr = go.GetComponent<MeshRenderer>();

      mr.material = mat; 
      mf.mesh = mesh;
   }
}
