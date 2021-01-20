using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombineMesh : MonoBehaviour
{
    public string       combinedMeshName;
    public GameObject[] gameObjects;

    [ContextMenu("Combine Mesh")]
    private void Combine()
    {
        List<CombineInstance> combineInstances = new List<CombineInstance>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            MeshFilter meshFilter = gameObjects[i].GetComponent<MeshFilter>();

            if (meshFilter == null)
            {
                continue;
            }

            combineInstances.Add(new CombineInstance()
            {
                // NOTE:
                // Needs to instantiate once because some mesh can not access.
                // Such mesh read/write parameter is disabled. Ex:Default Cube
                // This makes following error.
                // "Cannot combine mesh that does not allow access".

                mesh      = GameObject.Instantiate(meshFilter.sharedMesh),
                transform = gameObjects[i].transform.localToWorldMatrix
            });
        }

        Mesh combinedMesh      = new Mesh();
             combinedMesh.name = combinedMeshName;
             combinedMesh.CombineMeshes(combineInstances.ToArray());

        foreach (CombineInstance combineInstance in combineInstances)
        {
            DestroyImmediate(combineInstance.mesh);
        }

        AssetDatabase.CreateAsset(combinedMesh, "Assets/" + combinedMesh.name + ".asset");
    }
}