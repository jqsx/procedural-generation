using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proceduralGeneration : MonoBehaviour
{ //new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
    public GameObject[] grassPrefabs;
    public GameObject[] treePrefabs;
    Vector3 rememberedPosition;
    float movespeed = 1f;
    public void generateMap(Vector2 seed, float scale, float heightMultiplier)
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
            float height = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 100 * scale + seed.x, (transform.position.z + vert.z) / 100 * scale + seed.y)) * 10f;
            float height_a = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 20 * scale + seed.x, (transform.position.z + vert.z) / 20 * scale + seed.y)) * 10f * heightMultiplier;
            float height_b = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 2 * scale + seed.x, (transform.position.z + vert.z) / 2 * scale + seed.y)) * heightMultiplier;
            float ocean = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 100 * scale + seed.x, (transform.position.z + vert.z) / 100 * scale + seed.y)) * 3f * heightMultiplier;
            float islandMap = (0.5f - Mathf.PerlinNoise((transform.position.x + vert.x) / 1000 * scale - seed.x, (transform.position.z + vert.z) / 1000 * scale - seed.y)) * 5f * heightMultiplier;
            if (islandMap <= 0)
            {
                islandMap = -Mathf.Pow(3, Mathf.Abs(islandMap));
            }

            finalHeight += Mathf.Pow(2, height);
            finalHeight += height_a;
            finalHeight += height_b;
            finalHeight += ocean;
            finalHeight += islandMap;
            finalHeight -= Mathf.Pow(2, ocean);

            vert.y = finalHeight;
            verticies[i] = vert;

            float grassMap = Mathf.PerlinNoise((-transform.position.x + vert.x) / 2 + seed.x, (-transform.position.z + vert.z) / 2 + seed.y);
            if (grassMap > 0.7 && finalHeight >= 0 && grassMap < 0.9f)
            {
                int rng = (int)Mathf.Floor(Mathf.Abs(Mathf.Cos(finalHeight)) * (grassPrefabs.Length - 1));
                GameObject _grassInstance = Instantiate(grassPrefabs[rng], vert + transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                _grassInstance.transform.localScale *= Random.Range(1f, 3f);
                _grassInstance.transform.parent = transform;

            }
            else if (grassMap >= 0.9f && finalHeight >= 0)
            {
                int rng = (int)Mathf.Floor(Mathf.Abs(Mathf.Cos(finalHeight)) * (treePrefabs.Length - 1));
                GameObject _treeInstance = Instantiate(treePrefabs[rng], vert + transform.position, Quaternion.identity);
                _treeInstance.transform.localScale *= Random.Range(1f, 2f);
                _treeInstance.transform.parent = transform;
            }
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
