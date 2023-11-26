using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    [System.Serializable] //  (field) hoặc lớp (class) trở nên có khả năng serializable. Serialization (tích hợp)
    public class Pool
    {
        public Material tag;
        public GameObject prefab;
    }

    public List<Pool> pools;
    public Dictionary<Color, Queue<GameObject>> poolDictionary; //Tuy nhiên, bạn có thể chứa một Queue<T> (hoặc bất kỳ cấu trúc dữ liệu nào khác) bên trong một Dictionary bằng cách sử dụng kiểu giá trị (value type) của Dictionary là một kiểu tham chiếu (reference type). Chẳng hạn, bạn có thể sử dụng Queue<T> như một giá trị của Dictionary bằng cách sử dụng Queue như kiểu giá trị của Dictionary:
    [SerializeField] private GameObject prefabsUnUsing;

    public int size = 80;

    private void Awake()
    {
        Instance = this;
    }
    public void OnInit()
    {
        poolDictionary = new Dictionary<Color, Queue<GameObject>>(); //Khoi tao 1 Dictionary

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i <  size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                AddToEnqueue(obj, objectPool);
            }
            
            poolDictionary.Add(pool.tag.color, objectPool);
        }
    }
    public void AddToEnqueue(GameObject brick, Queue<GameObject> brickPool)
    {
        brick.transform.SetParent(prefabsUnUsing.transform);
        brick.SetActive(false);
        brickPool.Enqueue(brick);
    }

    // Object spawn from Pool to Target
    public GameObject SpawnFromPool(Color tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + " doesn's excist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); //Remove the element at 

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.SetParent(transform, false);

        return objectToSpawn;
        //IPoolObject pooledObj = objectToSpawn.GetComponent<IPoolObject>();

        //if (pooledObj != null)
        //{
        //    Debug.Log("k Rong");
        //    pooledObj.OnObjectSpanw();
        //}
        //poolDictionary[tag.color].Enqueue(objectToSpawn);// add back to our queue so that it can be reused later
        //return objectToSpawn;
    }
    //public void ReturnToPool(string tag, GameObject obj)
    //{
    //    if (poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
    //    {
    //        obj.SetActive(false);
    //        objectPool.Enqueue(obj);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Pool for " + tag + " not found. Object not returned to pool.");
    //    }
    //}
}
