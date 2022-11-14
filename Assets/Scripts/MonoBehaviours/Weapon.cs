using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{

    PhotonView view;

    bool isFiring;

    [HideInInspector]
    public Animator animator;
    Camera localCamera;
    float positiveSlope;
    float negativeSlope;

    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    public float weaponVelocity;


    public GameObject ammoPrefab;
    //List<GameObject> ammoPool;
    //public int poolSize;

    [Header("Attack Properties")]
    [SerializeField]
    private Transform _attackPoint;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private LayerMask _attackMask;
    [SerializeField]
    private int _attackDamage;

    private void OnDrawGizmos()
    {
        if (_attackPoint is null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private void Attack()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _attackMask);

        foreach (var obj in objs)
        {
            if (obj.TryGetComponent<Enemy>(out Enemy enemy) && obj is BoxCollider2D)
            {
                GetComponent<PhotonView>().RPC("DamageCoroutine", RpcTarget.All, enemy.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    void DamageCoroutine(int viewID)
    {
        if (gameObject != null && gameObject.activeSelf == true)
        {
            Enemy enemy = PhotonView.Find(viewID).gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.DamageCharacter(_attackDamage, 0.0f));
        }

    }


    void Awake()
    {



    }
    // Start is called before the first frame update
    void Start()
    {

        // if (ammoPool == null)
        // {
        //     ammoPool = new List<GameObject>();
        // }

        view = GetComponent<PhotonView>();


        // for (int i = 0; i < poolSize; i++)
        // {
        //     GameObject ammoObject = RPGGameManager.sharedInstance.InstantiateObject(ammoPrefab, Vector3.zero, Quaternion.identity);
        //     ammoObject.SetActive(false);
        //     ammoPool.Add(ammoObject);
        // }


        animator = GetComponent<Animator>();
        isFiring = false;
        localCamera = Camera.main;

        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isFiring = true;
                //FireAmmo();
            }
        }

        UpdateState();
    }

    // GameObject SpawnAmmo(Vector3 location)
    // {
    //     foreach (GameObject ammo in ammoPool)
    //     {
    //         if (ammo.activeSelf == false)
    //         {
    //             ammo.SetActive(true);
    //             ammo.transform.position = location;
    //             return ammo;
    //         }
    //     }
    //     return null;
    // }

    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = RPGGameManager.sharedInstance.InstantiateObject(ammoPrefab, transform.position, Quaternion.identity); //SpawnAmmo(transform.position);
        if (ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    void OnDestroy()
    {
        // foreach (GameObject ammo in ammoPool)
        // {
        //     RPGGameManager.sharedInstance.DestroyObject(ammo);
        // }

        // ammoPool = null;
    }

    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }

    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }

    Quadrant GetQuadrant()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = transform.position;
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);
        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }

    void UpdateState()
    {
        if (isFiring)
        {
            Vector2 quadrantVector;
            Quadrant quadEnum = GetQuadrant();
            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 1.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);

            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }
}
