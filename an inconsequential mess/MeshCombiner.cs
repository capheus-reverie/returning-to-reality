using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<MeshFilter> sourceMeshFilters;
    [SerializeField] private MeshFilter targetMeshFilter;

    [ContextMenu(itemName: "Combine Meshes")]

    #endregion

    #region Unity Methods

    private void CombineMeshes()
    {
        var combine = new CombineInstance[sourceMeshFilters.Count];

        for(var i = 0; i < sourceMeshFilters.Count; i++)
		{
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
		}

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        targetMeshFilter.mesh = mesh;
    }

    #endregion

}
