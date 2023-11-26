using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawn : MonoBehaviour
{
    public static BrickSpawn Instance;
    ObjectPooler objectPooler;
   
    public Vector3 origin;
    public List<Material> listTag;

    [SerializeField] private List<GameObject> planes;
    public List<GameObject> Planes { get => planes; }

    private void Awake()
    {
        Instance = this;
    }
    public void OnInit()
    {
        objectPooler = ObjectPooler.Instance;
        SpawnerBrick();

    }

    private Color RandomTag()
    {
        int indexTagInList =  Random.Range(0, listTag.Count);
        return listTag[indexTagInList].color;
    }

    private void SpawnerBrick()
    {
        foreach (GameObject plane in planes)
        {
            int width = plane.transform.GetComponent<Plane>().Width;
            int height = plane.transform.GetComponent<Plane>().Height;
            for (int i = -width / 2; i < width / 2; i++)
            {
                for (int j = -height / 2; j < height / 2; j++)
                {
                    origin = new Vector3(plane.transform.position.x + i, plane.transform.position.y + 0.6f, plane.transform.position.z + j);
                    Spawn(origin, plane);
                }
            }
        }
    }
    public void Spawn(Vector3 posSpawn, GameObject planeSpawn)
    {
        Color randomTag = RandomTag();

        while (objectPooler.poolDictionary[randomTag].Count == 0)
        {
            randomTag = RandomTag();
        }
        GameObject brick = objectPooler.SpawnFromPool(randomTag, posSpawn, Quaternion.identity);

        brick.transform.SetParent(planeSpawn.transform);
    }


}
