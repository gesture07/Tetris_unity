using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Block : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 1.0f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,]grid = new Transform[width,height];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1,0,0);
            if(!ValidMove())
            {
                transform.position -= new Vector3(-1,0,0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1,0,0);
            if(!ValidMove())
            {
                transform.position -= new Vector3(1,0,0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint),new Vector3(0,0,1),90);
            if(!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint),new Vector3(0,0,1),-90);
            }
        }

        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime/10 : fallTime))
        {
            transform.position += new Vector3(0,-1,0);
            if(!ValidMove())
            {
                transform.position -= new Vector3(0,-1,0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<spawnertetris>().NewTetris();
            }
            previousTime = Time.time;
        }
    }

    void CheckForLines()
    {
        for(int i = height -1;i >= 0; i++)
        {
            if(MasLine(i))
            {
                DeleteLine(i);
                rowDown(i);

            }
        }
    }

    bool MasLine(int i) //줄이 블록으로 곽 차 있는지 확인하기
    {
        for(int j =0;j < width;j++)// 줄을 0~9까지 검색한다.
        {
            if(grid[j, i] == null)//비어 있다면 false
                return false;
        }
        return true;//줄이 꽉 차 있으면 true
    }

    void DeleteLine(int i)//줄 삭제
    {
        for(int j=0;j < width;j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }

    void rowDown(int i)// 줄을 아래로 내린다
    {
        for(int y = i;y < height; y++)
        {
            for(int j=0; j < width;j++)
            {
                if(grid[j,y] != null)//윗 줄을 아랫 줄로 복사한다
                {
                    grid[j, y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j, y-1].transform.position += new Vector3(0,1,0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundx = Mathf.RoundToInt(children.transform.position.x);
            int roundy = Mathf.RoundToInt(children.transform.position.y);

            grid[roundx, roundy] = children;
        }
    }

    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundx = Mathf.RoundToInt(children.transform.position.x);
            int roundy = Mathf.RoundToInt(children.transform.position.y);

            if(roundx < 0 || roundx >= width || roundy < 0 ||roundy >= height)
            {
                return false;
            }

            if(grid[roundx, roundy] != null)
            {
            return false;
            }
        }

        return true;
    }
}
