using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Bound
{
    public float top;
    public float right;
    public float bottom;
    public float left;
}
public class DynamicLiquid : MonoBehaviour
{
    [Header("Liquid settings")]
    public Bound bound;
    public int quality;
    public Material material;

    private Vector3[] vertices;
    private Mesh mesh;

    [Header("Physics settings")]
    public float springConstant = 0.02f;
    public float damping = 0.1f;
    public float spread = 0.1f;
    public float collisionVelocityFactor = 0.04f;

    float[] velocities;
    float[] accelerations;
    float[] leftDeltas;
    float[] rightDeltas;

    private float timer;

    private void Start()
    {
        InitializePhysics();
        GenerateMesh();
        SetBoxCollider2D();
    }

    private void InitializePhysics()
    {
        velocities = new float[quality];
        accelerations = new float[quality];
        leftDeltas = new float[quality];
        rightDeltas = new float[quality];
    }

    private void Update()
    {
        // don't calculate all of those every frame;
        if (timer <= 0) return;
        timer -= Time.deltaTime;

        //updating physics
        for (int i = 0; i < quality; i++)
        {
            float force = springConstant * (vertices[i].y - bound.top) + velocities[i] * damping;
            accelerations[i] = - force;
            vertices[i].y += velocities[i]*Time.timeScale;
            velocities[i] += accelerations[i]* Time.timeScale;
        }

        for (int i = 0; i < quality; i++)
        {
            if(i>0)
            {
                leftDeltas[i] = spread * (vertices[i].y - vertices[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }
            if (i<quality-1)
            {
                rightDeltas[i] = spread * (vertices[i].y - vertices[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }
        //updating mesh
        mesh.vertices = vertices;
    }

    private void GenerateMesh()
    {
        float range = (bound.right - bound.left) / (quality-1);
        vertices = new Vector3[quality * 2];
        //generating vertices;
        //top vertices
        for (int i = 0; i < quality; i++)
        {
            vertices[i] = new Vector3(bound.left + (i * range), bound.top, 0);
        }
        //bottom vertices
        for (int i = 0; i < quality; i++)
        {
            vertices[i + quality] = new Vector2(bound.left + (i * range), bound.bottom);
        }
        //generating tris.
        int[] template = new int[6];
        template[0] = quality;
        template[1] = 0;
        template[2] = quality + 1;
        template[3] = 0;
        template[4] = 1;
        template[5] = quality + 1;

        int marker = 0;
        int[] tris = new int[((quality - 1) * 2) * 3];
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = template[marker++]++;
            if (marker >= 6) marker = 0;
        }

        //generate mesh
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (material) meshRenderer.sharedMaterial = material;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //set up mesh
        meshFilter.mesh = mesh;

    }
    private void SetBoxCollider2D()
    {
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb.isKinematic)
            Splash(collision, (-Game_Manager.Instance.startFloorSpeed) * collisionVelocityFactor);
        else Splash(collision, rb.velocity.y * collisionVelocityFactor);
    }
    public void Splash(Collider2D collision,float force)
    {
        timer = 4f;
        float radius = (collision.bounds.max.x - collision.bounds.min.x)/2; // max is positive, min is negative; precise radius isn't good sometimes;
        
        Vector2 center = new Vector2(collision.bounds.center.x-0.15f, bound.top); //0.15 is offset
        //applying physics
        for (int i = 0; i < quality; i++)
        {
            if (PointInsideCircle(vertices[i],center,radius))
            {
                velocities[i] = force;
            }
        }

    }
    bool PointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }

}
