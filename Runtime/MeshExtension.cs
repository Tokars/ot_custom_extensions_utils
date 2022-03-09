using System.Linq;
using UnityEngine;

namespace OT.Extensions
{
    public static class MeshExtension
    {
        public static Vector3[] CalculateNormals(this Mesh mesh)
        {
            return mesh.normals.Distinct().ToArray();
        }
    }
}