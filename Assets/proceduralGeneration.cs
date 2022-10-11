using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGeneration : MonoBehaviour
{ //new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
    Vector3 rememberedPosition;
    float movespeed = 1f;
    public void generateMap(Vector2 seed)
    {
        movespeed = Random.Range(0.5f, 2f);
        rememberedPosition = transform.position;
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshCollider mc = GetComponent<MeshCollider>();
        Vector3[] verticies = mf.mesh.vertices;
        for(int i = 0; i < verticies.Length; i++)
        {
            Vector3 vert = verticies[i];
            float finalHeight = 0;
            float height = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 100 + seed.x, (transform.position.z + vert.z) / 100 + seed.y)) * 10f;
            float height_a = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 20 + seed.x, (transform.position.z + vert.z) / 20 + seed.y)) * 10f;
            float height_b = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 2 + seed.x, (transform.position.z + vert.z) / 2 + seed.y));
            float ocean = 3f - Mathf.PerlinNoise((transform.position.x + vert.x) / 100 + seed.x, (transform.position.z + vert.z) / 100 + seed.y) * 3f;

            finalHeight += Mathf.Pow(2, height);
            finalHeight += height_a;
            finalHeight += height_b;
            finalHeight += ocean;
            finalHeight -= Mathf.Pow(2, ocean);

            vert.y = finalHeight;
            verticies[i] = vert;
        }
        mf.mesh.vertices = verticies;
        mf.mesh.RecalculateNormals();
        mc.sharedMesh = mf.mesh;
        transform.position -= new Vector3(0, 30, 0);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, rememberedPosition, 7 * Time.deltaTime * movespeed);
    }

    public void removeChunk()
    {
        StartCoroutine(dstry());
    }

    IEnumerator dstry()
    {
        rememberedPosition -= new Vector3(0, 30, 0);
        float sp = movespeed;
        movespeed = 0.1f;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        movespeed = sp;
        yield return new WaitForSeconds(0.7f / movespeed);
        Destroy(gameObject);
    }
}
