using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public struct PointCloudMeshJob : IJobParallelFor
{
	public NativeArray<Vector3> Vertices;
	public DepthReader Reader;


	public void Execute(int index)
	{	
		var vert = Vertices[index];

		vert.x = Reader.vertices[index].x;
		vert.y = Reader.vertices[index].y;
		vert.z = Reader.vertices[index].z;

		Vertices[index] = vert;
	}

}