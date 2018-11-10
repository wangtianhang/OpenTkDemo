using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Demo2
{
    public void LoadScene()
    {
        GameObject cube = new GameObject("cube");
        MeshFilter meshFilter = cube.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = cube.AddComponent<MeshRenderer>();

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f));
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f));
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));
        

        List<int> index = new List<int>();
        index.Add(0);
        index.Add(2);
        index.Add(3);
        index.Add(0);
        index.Add(3);
        index.Add(1);
        index.Add(8);
        index.Add(4);
        index.Add(5);
        index.Add(8);
        index.Add(5);
        index.Add(9);
        index.Add(10);
        index.Add(6);
        index.Add(7);
        index.Add(10);
        index.Add(7);
        index.Add(11);
        index.Add(12);
        index.Add(13);
        index.Add(14);
        index.Add(12);
        index.Add(14);
        index.Add(15);
        index.Add(16);
        index.Add(17);
        index.Add(18);
        index.Add(16);
        index.Add(18);
        index.Add(19);
        index.Add(20);
        index.Add(21);
        index.Add(22);
        index.Add(20);
        index.Add(22);
        index.Add(23);
        

        List<Vector3> normals = new List<Vector3>();
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, 1.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, 0.0f, -1.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(0.0f, -1.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));
        normals.Add(new Vector3(1.0f, 0.0f, 0.0f));

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = index.ToArray();
        mesh.normals = normals.ToArray();

        meshFilter.mesh = mesh;

        Shader shader = Shader.Find("Diffuse");
        Material material = new Material(shader);

        meshRenderer.material = material;

        GameObject cameraGo = new GameObject("Main Camera");
        Camera camera = cameraGo.AddComponent<Camera>();
        camera.transform.position = new Vector3(-2.36f, 4.47f, -4.57f);
        camera.transform.eulerAngles = new Vector3(45, 0, 0);

        GameObject lightGo = new GameObject("Directional light");
        lightGo.transform.eulerAngles = new Vector3(50, -30, 0);
        lightGo.AddComponent<Light>();
    }
}

