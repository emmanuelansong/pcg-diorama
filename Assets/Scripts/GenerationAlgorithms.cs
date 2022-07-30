using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationAlgorithms : MonoBehaviour
{
	public float radius = 1;
	public Vector3 regionSize;
	public int rejectionSamples = 30;
	public List<Vector3> points;
	public float displayRadius = .01f;
	void OnValidate()
    {
		points = GeneratePoints(radius, regionSize, rejectionSamples);
    }

	void OnDrawGizmos()
    {
		Gizmos.DrawWireCube(regionSize / 2, regionSize);
		if(points != null)
        {
			foreach(Vector3 point in points)
            {
				Gizmos.DrawWireSphere(point, displayRadius);
				if(Physics.Raycast(point, Vector3.down, out RaycastHit hit))
				{
					if (hit.point.y > 0)
					{
						break;
					}
                }
            }
        }
    }

	//code from Sebastian Lague
	public List<Vector3> GeneratePoints(float radius, Vector3 sampleRegionSize, int numSamplesBeforeRejection = 30)
	{
		//square grid
		
		float cellSize = radius / Mathf.Sqrt(2);

		//number of times the cell size fits into sample region
		//grid tells us for reach cell, what is the index in that cell
		int[,,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize), Mathf.CeilToInt(sampleRegionSize.z / cellSize)];


		List<Vector3> points = new List<Vector3>();
		List<Vector3> spawnPoints = new List<Vector3>();

		spawnPoints.Add(sampleRegionSize / 2);
		while (spawnPoints.Count > 0)
		{
			//pick a random spawn point
			int spawnIndex = Random.Range(0, spawnPoints.Count);
			Vector3 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				//pick random direction
				float angle = Random.value * Mathf.PI * 2;
				Vector3 dir = new Vector3(Mathf.Sin(angle),0 ,Mathf.Cos(angle));
				Vector3 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

				if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
				{
					points.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize), (int)(candidate.z / cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		return points;
	}

	static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
	{
		//check if candidate is within sampleRegion
		if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y && candidate.z >= 0 && candidate.z < sampleRegionSize.z)
		{
			int cellX = (int)(candidate.x / cellSize);
			int cellY = (int)(candidate.y / cellSize);
			int cellZ = (int)(candidate.z / cellSize);

			int searchStartX = Mathf.Max(0, cellX - 2);
			int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

			int searchStartZ = Mathf.Max(0, cellZ - 2);
			int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);

			for (int x = searchStartX; x <= searchEndX; x++)
			{
				for (int y = searchStartY; y <= searchEndY; y++)
				{
					for (int z = searchStartZ; z <= searchEndZ; z++)
					{
						int pointIndex = grid[x, y, z] - 1;
						if (pointIndex != -1)
						{
							float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
							if (sqrDst < radius * radius)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
		return false;
	}

	public Vector3 RandomPointInRadius(Vector3 refPoint, float radius)
	{
		Terrain terrain = Terrain.activeTerrain;
		//Sample reference point + random offset * rad
		var sample = refPoint + Random.insideUnitSphere * radius;

		//Then set the y to sampled terrain height
		sample.y = terrain.SampleHeight(sample);
		//And return the sample!
		return sample;

	}

	public Vector3 RandomTerrainPosition(Terrain terrain)
	{
		//Get the terrain size in all 3 dimensions
		Vector3 terrainSize = terrain.terrainData.size;

		//Choose a uniformly random x and z to sample y
		float rX = Random.Range(0, terrainSize.x);
		float rZ = Random.Range(0, terrainSize.z);

		//Sample y at this point and put into an offset vec3
		Vector3 sample = new Vector3(rX, 0, rZ);
		sample.y = terrain.SampleHeight(sample);

		//terrain.terrainData.GetSteepness

		//points.Add(terrain.GetPosition() + sample);


		//Just return terrain pos + sample offset
		return terrain.GetPosition() + sample;
	}
}
