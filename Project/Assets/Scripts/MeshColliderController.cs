using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
[RequireComponent(typeof(MeshCollider))]

public class MeshColliderController : MonoBehaviour
{
    // Assigns the mesh from the MeshGenerator to the mesh collider of the object
    Mesh mesh;
    MeshGenerator meshGen;
    MeshCollider meshCol;

    void Start()
    {
        meshGen = GetComponent<MeshGenerator>();
        meshCol = GetComponent<MeshCollider>();
        mesh = meshGen.getMesh();

        meshCol.sharedMesh = mesh;
    }
}
