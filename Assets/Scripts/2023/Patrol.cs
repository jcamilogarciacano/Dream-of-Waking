using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform objective;
    public float x;
    public float y;

    public Vector2[] points;

    public int waitTime = 60;

    public float speed = 10f;

    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;

    public int counter;
    public bool goNext;
    public int getPoints;
    public bool newPoints;

    private Vector2 _nextPoint;

    public bool isPatrolling = true;

    // Start is called before the first frame update
    void Start()
    {
        goNext = false;
        if(points.Length == 0)
        {
           // Vector2[] tempPoins = new Vector2[3];
            points = new Vector2[3];
        }
        if (objective == null)
        {
            GameObject gameObject1 = new GameObject("Objective");
            gameObject1.transform.position = this.gameObject.transform.position;
            Transform tempT = gameObject1.transform;
            objective = tempT;
            //Destroy(gameObject1);
          
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartPatrol(isPatrolling);
    }

    public void StartPatrol( bool patrol)
    {
        if (patrol == true)
        {
            MoveTowards(goNext, getPoints);

            x = objective.position.x;
            y = objective.position.y;
            // print(objective.position.x + 'X');
            // print(objective.position.y + 'y');
            if (counter > waitTime)
            {
                if (getPoints == points.Length - 1)
                {
                    getPoints = -1;

                }
                if (getPoints == 0)
                {
                    NewPoints();
                }
                goNext = true;
                getPoints++;
                counter = 0;
            }
            else
            {
                //goNext = !goNext;
               // goNext = true;
                counter++;
            }

            if (counter > waitTime / 2)
            {
                //goNext = false;
                //goNext = true;
            }
        }
    }

    public void MoveTowards(bool move, int next)
    {
        float step = speed * Time.deltaTime;
        float diffX = Mathf.Abs(transform.position.x - points[next].x);
        float diffY = Mathf.Abs(transform.position.y - points[next].y);
        if (move == true)
        {
            SetNextPoint(points[next]);
            if(diffX > 0.01 && diffY > 0.01)
            {
                transform.position = Vector2.MoveTowards(transform.position, points[next], step);
            }
            else
            {
                goNext = false;
            }
            


        }
        
    }
    public void NewPoints()
    {
       
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector2(objective.position.x + Random.Range(xMin, xMax), objective.position.y + Random.Range(yMin, yMax));
        }
    }

    public bool IsMoving()
    {
        return goNext;
    }

    public void SetNextPoint(Vector2 nextPoint)
    {
        _nextPoint = nextPoint;
    }
    public Vector2 GetNextPoint()
    {
        return _nextPoint;
    }
    
}
