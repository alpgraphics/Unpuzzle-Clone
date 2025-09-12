using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;
   
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int poolSize = 5;
   
    private Queue<GameObject> pool = new Queue<GameObject>();
   
    void Awake()
    {
        Instance = this;
       
        for (int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.SetActive(false);
            pool.Enqueue(particle);
        }
    }
    
    public void PlayParticle(Vector2 startPos, Transform target, Vector3 direction, float duration,string color)
    {
        GameObject particle = GetParticle();
        particle.transform.position = startPos;
        particle.SetActive(true);
        particle.transform.SetParent(target);
        
        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;

            Color particleColor = GetColorFromString(color);
            main.startColor = particleColor;
        }
        
        Vector3 pos = particle.transform.position; 
        pos.z -= 0.5f;
        particle.transform.position = pos;
        
        if (direction == Vector3.up)
        {
            particle.transform.rotation = Quaternion.Euler(90f, 90,0);  
        }
        else if (direction == Vector3.down)
        {
            particle.transform.rotation = Quaternion.Euler(-90f, 90,0);  
        }
        else if (direction == Vector3.left)
        {
            particle.transform.rotation = Quaternion.Euler(0f, 90,0);  
        }
        else if (direction == Vector3.right)
        {
            particle.transform.rotation = Quaternion.Euler(-180f, 90,0);  
        }
            
        
        DOVirtual.DelayedCall(duration, () => {
            particle.transform.SetParent(null);
            ReturnParticle(particle);
        });
    }
   
    GameObject GetParticle()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return Instantiate(particlePrefab);
        }
    }
    private Color GetColorFromString(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red": return Color.red;
            case "blue": return Color.blue;
            case "green": return Color.green;
            case "yellow": return Color.yellow;
            default: return Color.white;
        }
    }
    void ReturnParticle(GameObject particle)
    {
        particle.SetActive(false);
        pool.Enqueue(particle);
    }
}
