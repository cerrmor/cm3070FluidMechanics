using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMesh : MonoBehaviour
{
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    private Mesh OrigionalMesh, MeshClone;
    private MeshRenderer renderer;
    private BlobVertex[] bv;
    private Vector3[] vertexArray;


    // Start is called before the first frame update
    void Start()
    {
        OrigionalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OrigionalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();
        bv = new BlobVertex[MeshClone.vertices.Length];
        for(int i = 0; i < MeshClone.vertices.Length; i++)
        {
            bv[i] = new BlobVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertexArray = OrigionalMesh.vertices;
        for(int i = 0; i < bv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[bv[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            bv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(bv[i].Position);
            vertexArray[bv[i].ID] = Vector3.Lerp(vertexArray[bv[i].ID], target, intensity);
        }
        MeshClone.vertices = vertexArray;
    }
    public class BlobVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity,Force;
        public BlobVertex(int _id,Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < 0.001f)
            {
                Position = target;
            }
        }
    }
}
