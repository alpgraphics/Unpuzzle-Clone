using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class PuzzleBox : MonoBehaviour
{
    [Header("RAY Settings")] [SerializeField]
    private float rayDistance = 10f;


    public string directions;
    public string colors;
    private ObjectPool objectPool;
    private ParticlePool particlePool;
    private Renderer myRenderer;
    private MaterialPropertyBlock propBlock;


        
    RaycastHit hit;
    private Vector3 _defaultPosition;
    
    void Start()
    {   
        myRenderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }
    
    public void Initialize(BoxData data,Vector3 rotation, int index, ObjectPool objectPoolIn)
    {
        transform.name = "Box_" + index;
        transform.position = GridController.instance.grid[data.x, data.y].position;
        transform.rotation = Quaternion.Euler(rotation);
        
        _defaultPosition = transform.position;
        SetObjectPool(objectPoolIn);
        directions = data.direction;
        colors = data.color;
    }

    public void UpdateColor()
    {
        if (propBlock != null && myRenderer != null)
        {
            propBlock.SetColor("_Color", ConvertColor(colors));
            myRenderer.SetPropertyBlock(propBlock);
        }
        else
        {
            Debug.Log("Colors are not set");
        }
        
    }
    
    private void OnEnable()
    {
        Invoke(nameof(UpdateColor), 0.01f);
    }
    
    public void ChangeColorToRed()
    {
        // PropertyBlock'u geçici olarak temizle
        myRenderer.SetPropertyBlock(null);
    
        Color originalColor = ConvertColor(colors);
        myRenderer.material.DOColor(Color.red.gamma, 0.2f).OnComplete(() => {
            myRenderer.material.DOColor(originalColor, 0.8f);
        });
    }

    private void OnMouseDown()
    {
        transform.DOPunchScale(Vector3.one * -0.11f, 0.15f, 8, 2f).OnComplete(() =>{
        });
        ShootRay();
    }

    public void ShootRay()
    {

            Vector3 rayDirection = GetRayDirection();
            Collider myCollider = GetComponent<Collider>();
            Vector3 startPosition = myCollider.bounds.center;
            myCollider.enabled = false;

            Physics.Raycast(startPosition, rayDirection, out hit, rayDistance);

            Debug.DrawRay(startPosition, rayDirection * rayDistance, Color.red, 1f);
            myCollider.enabled = true;

            if (hit.collider != null)
            {
                Debug.Log($"{gameObject.name} → {hit.collider.name}");
                GameManager.Instance.MovesLeft--;

                PuzzleBox otherBox = hit.collider.GetComponent<PuzzleBox>();
                if (otherBox != null)
                {
                    MoveBox(otherBox);
                }
            }
            else
            {
                GameManager.Instance.CountLeft--;
                MoveBox(null);
                GameManager.Instance.MovesLeft--;

            }
    }
    
    
    
    public void MoveBox(PuzzleBox otherBox = null) 
    {

        float stopDistance = GridController.instance.cellSize-0.18f;
        Vector2 startPosition = transform.position;
        Vector2 direction = GetRayDirection();
        Vector3 punchDirection = direction.normalized;
        if (otherBox != null)
        {
            
            Vector2 targetPosition = hit.point;
            Sequence sequence = DOTween.Sequence();
            if (hit.distance < stopDistance * 2f)
            {
                otherBox.ChangeColorToRed();
                ShakeAllBoxesInDirection(transform.position, direction);

            }
            else
            {
                ParticlePool.Instance.PlayParticle(startPosition, this.transform, direction, 0.2f,colors);
                transform.DOMove(targetPosition - (direction * stopDistance), 0.2f).SetEase(Ease.InOutQuad).OnComplete(() => { 
                    ShakeAllBoxesInDirection(transform.position, direction);});
            }
            Settings.Instance?.Haptic();
            /*
            sequence.OnComplete(() => {
                otherBox.ShootRay();
            });*/
        }
        else
        {
            Vector2 targetPosition = startPosition + (direction * (rayDistance+10));
            ParticlePool.Instance.PlayParticle(startPosition,this.transform,direction,0.6f,colors);
            GetComponent<Collider>().enabled = false;
                transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
            {   
                GetComponent<Collider>().enabled = true;
                objectPool.ReturnObject(this.gameObject);
            });
        }
    }

    private void ShakeAllBoxesInDirection(Vector3 startPos, Vector3 dir)
    {
        _defaultPosition = transform.position;
        float maxDistance = 1.2f;
        RaycastHit shakeHit;
        if (Physics.Raycast(startPos, dir, out shakeHit, maxDistance))
        {
            PuzzleBox box = shakeHit.collider.GetComponent<PuzzleBox>();
            Debug.DrawRay(startPos, dir, Color.magenta, 2f);
            Debug.Log(shakeHit.collider.name);
            if (box != null && box != this)
            {
                Debug.Log(dir);
                box.transform.DOKill();
                box.transform.DOPunchPosition(dir * 0.07f, 0.2f, 8, 3).OnComplete(() =>
                {
                    box.ReturnToDefaultPosition();
                });
                // Distance kontrolü ile chain reaction
                if (shakeHit.distance < 0.8f)
                {
                    StartCoroutine(DelayedAction());
                    IEnumerator DelayedAction()
                    {
                        yield return new WaitForSeconds(0.1f);
                        ShakeAllBoxesInDirection(box.transform.position, dir);
                    }
                }
            }
        }
    }

    private void ReturnToDefaultPosition()
    {
        transform.position = _defaultPosition;
    }

    private Color ConvertColor(string color)
    {
        switch (color)
        {
            case "red": 
                ColorUtility.TryParseHtmlString("#ff6b6b", out Color redColor);
                return redColor;
            case "green": 
                ColorUtility.TryParseHtmlString("#6ede8a", out Color greenColor);
                return greenColor;
            case "yellow": 
                ColorUtility.TryParseHtmlString("#ffe66d", out Color yellowColor);
                return yellowColor;
            case "blue": 
                ColorUtility.TryParseHtmlString("#4e66cd", out Color blueColor);
                return blueColor;
            default:
                ColorUtility.TryParseHtmlString("#ff6b6b", out Color defaultColor);
                return defaultColor;
        }
    }
    
    public void SetObjectPool(ObjectPool pool)
    {
        objectPool = pool;
    }
    Vector3 GetRayDirection()
    {
        switch (directions)
        {
            case "Up":
                return Vector3.up;
            case "Down":
                return Vector3.down;
            case "Left":
                return Vector3.left;
            case "Right":
                return Vector3.right;
            default:
                Debug.Log("Couldn't accesed to the direction data.");
                return Vector3.zero;
        }
    }

    
}





