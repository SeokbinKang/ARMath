using System.Linq;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Test class for drawing a polygon mesh
/// </summary>
public class TestPolygon : MonoBehaviour
{
    public Material Mattt;
    private void Start()
    {
        // Create Vector2 vertices
        var vertices2D = new[] {
            new Vector2(0, 0),
            new Vector2(0, 500),
            new Vector2(500, 500),
            new Vector2(500, 1000),
            new Vector2(0, 1000),
            new Vector2(0, 1500),
            new Vector2(1500, 1500),
            new Vector2(1500, 1000),
            new Vector2(1000, 1000),
            new Vector2(1000, 500),
            new Vector2(1500, 500),
            new Vector2(1500, 0),
        };

        var vertices3D = vertices2D.ToVector3();

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => Random.ColorHSV())
            .ToArray();

        // Create the mesh
        var mesh = new Mesh {
            vertices = vertices3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set up game object with mesh;
        /*
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;*/

        CanvasRenderer pCanvasRenderer = gameObject.AddComponent<CanvasRenderer>();
        pCanvasRenderer.Clear();        
        Material pMat = new Material(Shader.Find("Sprites/Default"));
        pMat.SetColor("_TintColor", Color.red);
        pCanvasRenderer.SetMaterial(Mattt, null);
        pCanvasRenderer.SetMesh(mesh);
    }
}