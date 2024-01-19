using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;

    static List<GameObject> ammoPool;

    public int poolSize;

    public float weaponVelocity;

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

    private void Awake()
    {
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for (int i=0; i<poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    void Start()
    {
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            FireAmmo();
        }

        UpdateState();
    }


    float GetSlope(Vector2 point1, Vector2 point2)
    {
        return (point2.y - point1.y) / (point2.x - point1.x);
    }


    void UpdateState()
    {
        if (isFiring)
        {
            Vector2 quandrantVector;
            Quadrant quadEnum = GetQuadrant();

            switch (quadEnum)
            {
                case Quadrant.East:
                    quandrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quandrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quandrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrant.North:
                    quandrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quandrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);

            animator.SetFloat("fireXDir", quandrantVector.x);
            animator.SetFloat("fireYDir", quandrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }
    }

    Quadrant GetQuadrant()
    {
        Vector2 playerPosition = transform.position;
        Vector2 mousePosition = Input.mousePosition;

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
        else return Quadrant.North;
    }

    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        // b = y - mx
        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        // b = y - mx
        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }

    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject ammo = SpawnAmmo(transform.position);

        if (ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    private void OnDestroy()
    {
        ammoPool = null;
    }

}
