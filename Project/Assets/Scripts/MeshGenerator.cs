using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{    
    Mesh mesh;
    
    private enum SurfaceType { DiniSurface, Hourglass, Snail, DentedCube, Torus, FlowerBall, KleinBottle}

    [SerializeField] private SurfaceType surfaceType;

    // generates the objects on awake
    void Awake()
    {
        switch (surfaceType)
        {
            case SurfaceType.DiniSurface:
                Generate(DiniSurface);
                break;
            case SurfaceType.Hourglass:
                Generate(Hourglass);
                break;
            case SurfaceType.Snail:
                Generate(Snail);
                break;
            case SurfaceType.DentedCube:
                Generate(DentedCube);
                break;
            case SurfaceType.Torus:
                Generate(Torus);
                break;
            case SurfaceType.FlowerBall:
                Generate(FlowerBall);
                break;
            case SurfaceType.KleinBottle:
                Generate(KleinBottle);
                break;
            default:
                throw new ArgumentException();
        }
    }

    // the actual generator
    private void Generate(Func<Vector2, Vector3> functor)
    {
        mesh = new Mesh();

        var subdivisions = new Vector2Int(50, 50);
        var vertexSize = subdivisions + new Vector2Int(1, 1);

        var vertices = new Vector3[vertexSize.x * vertexSize.y];
        var uvs = new Vector2[vertices.Length];

        for (var y = 0; y < vertexSize.y; y++)
        {
            var v = (1f / subdivisions.y) * y;

            for (var x = 0; x < vertexSize.x; x++)
            {
                var u = (1f / subdivisions.x) * x;

                var uv = new Vector2(u, v);

                var vertex = functor(uv);

                var arrayIndex = x + y * vertexSize.x;
                vertices[arrayIndex] = vertex;
                uvs[arrayIndex] = uv;

            }
        }

        var triangles = new int[subdivisions.x * subdivisions.y * 6];

        for (int i = 0; i < subdivisions.x * subdivisions.y; i++)
        {
            var triangleIndex = (i % subdivisions.x)
                                + (i / subdivisions.x) * subdivisions.x;
            var indexer = i * 6;


            triangles[indexer] = triangleIndex;
            triangles[indexer + 1] = triangleIndex + subdivisions.x + 1;
            triangles[indexer + 2] = triangleIndex + 1;

            triangles[indexer + 3] = triangleIndex + 1;
            triangles[indexer + 4] = triangleIndex + subdivisions.x + 1;
            triangles[indexer + 5] = triangleIndex + subdivisions.x + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // formulas for the objects
    private Vector3 DiniSurface(Vector2 uv)
    {
        uv.x = uv.x * 4 * Mathf.PI;
        uv.y += 0.01f;

        var xPos = 0.5f * Mathf.Cos(uv.x) * Mathf.Cos(uv.y);
        var yPos = 0.5f * Mathf.Sin(uv.x) * Mathf.Sin(uv.y);
        var zPos = 0.5f * (Mathf.Cos(uv.y) + Mathf.Log(Mathf.Tan(0.5f * uv.y))) + 0.1f * uv.x;

        return new Vector3(xPos, yPos, zPos);
    }

    private Vector3 Hourglass(Vector2 uv)
    {
        uv.x = uv.x * 2 * Mathf.PI;
        uv.y = uv.y * -2 * Mathf.PI;
        
        var xPos = 7 * Mathf.Cos(uv.x) * Mathf.Sin(uv.y);
        var yPos = 7 * Mathf.Sin(uv.x) * Mathf.Sin(uv.y);
        var zPos = 7 * Mathf.Sin(uv.y);

        return new Vector3(xPos, yPos, zPos);
    }
    
    private Vector3 Snail(Vector2 uv)
    {
        uv.x = uv.x * 2 * Mathf.PI;
        uv.y = uv.y * -2 * Mathf.PI;

        var xPos = (float) ((1 - uv.y / (2 * Mathf.PI)) * Mathf.Sin(3 * uv.y) * (1 + Mathf.Cos(uv.x)) +
                            0.5 * Mathf.Sin(3 * uv.y));
        var yPos = (float) ((1-uv.y/(2*Mathf.PI))*Mathf.Cos(3*uv.y)*(1+Mathf.Cos(uv.x))+0.5*Mathf.Cos(3*uv.y));
        var zPos = 6*uv.y/(2*Mathf.PI)+(1-uv.y/(2*Mathf.PI))*Mathf.Sin(uv.x);

        return new Vector3(xPos, yPos, zPos);
    }
    
    private Vector3 DentedCube(Vector2 uv)
    {
        uv.x = uv.x * 4 * Mathf.PI -2 * Mathf.PI;
        uv.y = uv.y * 4 * Mathf.PI -2 * Mathf.PI;

        var xPos = Mathf.Sin(uv.x);
        var yPos = Mathf.Sin(uv.y);
        var zPos = Mathf.Sin(uv.x+uv.y);

        return new Vector3(xPos, yPos, zPos);
    }

    private Vector3 Torus(Vector2 uv)
    {
        uv.x = uv.x * 2 * Mathf.PI + 5f;
        uv.y = uv.y * -2 * Mathf.PI;

        var xPos = 3 * Mathf.Cos(uv.y) + Mathf.Cos(uv.y) * Mathf.Cos(uv.x);
        var yPos = 3 * Mathf.Sin(uv.y) + Mathf.Sin(uv.y) * Mathf.Cos(uv.x);
        var zPos = Mathf.Sin(uv.x);

        return new Vector3(xPos, yPos, zPos);
    }

    private Vector3 FlowerBall(Vector2 uv)
    {
        uv.x = uv.x * 2 * Mathf.PI;
        uv.y = uv.y * Mathf.PI;

        var r = 2 + Mathf.Sin(7 * uv.x + 5 * uv.y);
        var xPos = r * Mathf.Cos(uv.x) * Mathf.Sin(uv.y);
        var yPos = r * Mathf.Sin(uv.x) * Mathf.Sin(uv.y);
        var zPos = r * Mathf.Cos(uv.y);

        return new Vector3(xPos, yPos, zPos);
    }

    private Vector3 KleinBottle(Vector2 uv)
    {
        uv.x = uv.x * 2 * Mathf.PI;
        uv.y = uv.y * 4 * Mathf.PI;

        var xPos = uv.y < 2 * Mathf.PI ? (2.5 - 1.5 * Mathf.Cos(uv.y)) * Mathf.Cos(uv.x)
            : (uv.y < 3 * Mathf.PI ? -2 + (2 + Mathf.Cos(uv.x)) * Mathf.Cos(uv.y)
            : -2 + 2 * Mathf.Cos(uv.x) - Mathf.Cos(uv.x));

        var yPos = uv.y < 2 * Mathf.PI ? (2.5 - 1.5 * Mathf.Cos(uv.y)) * Mathf.Sin(uv.x) : Mathf.Sin(uv.x);

        var zPos = uv.y < Mathf.PI ? -2.5 * Mathf.Sin(uv.y)
            : (uv.y < 2 * Mathf.PI ? 3 * uv.y - 3 * Mathf.PI
            : (uv.y < 3 * Mathf.PI ? (2 + Mathf.Cos(uv.x)) * Mathf.Sin(uv.y) + 3 * Mathf.PI
            : -3 * uv.y + 12 * Mathf.PI));

        return new Vector3((float)xPos, (float)yPos, (float)zPos);
    }

    public Mesh getMesh() {
        return mesh;
    }
}



