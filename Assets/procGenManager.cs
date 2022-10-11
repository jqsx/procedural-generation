using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procGenManager : MonoBehaviour
{
    public GameObject map;
    public int size = 5;
    List<GameObject> chunks = new List<GameObject>();
    Vector2 seed;
    void Start()
    {
        seed = getNewSeed();
        StartCoroutine(spawnChunks());
    }

    IEnumerator spawnChunks()
    {
        for (int y = 0; y < size * 2; y++)
        {
            for(int x = 0; x < size * 2; x++)
            {
                bool haschunk = false;
                Vector3 chunkPosition = new Vector3((-size + x) * 10 + Mathf.Round(transform.position.x / 10f) * 10f, 0, (-size + y) * 10 + +Mathf.Round(transform.position.z / 10f) * 10f);
                for (int i = 0; i < chunks.Count; i++)
                {
                    Vector2 loopchunkpos = new Vector2(chunks[i].transform.position.x, chunks[i].transform.position.z);
                    if (loopchunkpos == new Vector2(chunkPosition.x, chunkPosition.z))//isInRange(new Vector2(chunks[i].transform.position.x, chunks[i].transform.position.z), new Vector2(chunkPosition.x, chunkPosition.z), 0f))
                    {
                        haschunk = true;
                    }
                }
                if (!haschunk)
                {
                    GameObject inst = Instantiate(map, chunkPosition, Quaternion.identity);
                    chunks.Add(inst);
                    inst.GetComponent<proceduralGeneration>().generateMap(seed);
                    yield return new WaitForSeconds(0.001f);
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.1f);
        loop();
    }

    public Vector2 getNewSeed()
    {
        return new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
    }

    void loop()
    {
        List<GameObject> validated = new List<GameObject>();
        for (int i = 0; i < chunks.Count; i++)
        {
            if (isInRange(new Vector2(transform.position.x, transform.position.z), new Vector2(chunks[i].transform.position.x, chunks[i].transform.position.z), size))
            {
                validated.Add(chunks[i]);
            }
            else
            {
                chunks[i].GetComponent<proceduralGeneration>().removeChunk();
            }
        }
        chunks = validated;
        StartCoroutine(spawnChunks());
    }

    bool isInRange(Vector2 self, Vector2 other, float range)
    {
        range += 1;
        range *= 10;
        return other.x <= self.x + range && other.x >= self.x - range && other.y <= self.y + range && other.y >= self.y - range;
    }
}
