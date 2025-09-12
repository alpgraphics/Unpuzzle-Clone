using UnityEngine;
using System.Collections.Generic;


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    public List <GameObject> activeBoxes = new List<GameObject>(); 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);  
            pool.Enqueue(obj);
        }
       
        Debug.Log($"Pool has been created size: {poolSize} ");
    } 
    
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();    
            obj.SetActive(true);
            activeBoxes.Add(obj);

            return obj;
        }
        Debug.Log("Pool is empty, Instantiating");
        return GameObject.Instantiate(prefab);
    }

    public void ReturnObject(GameObject obj)
    {
        if (!obj.gameObject.activeInHierarchy) return;
        activeBoxes.Remove(obj);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
