using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MolesMovement : MonoBehaviour
{
    [SerializeField] GameObject mole;
    [SerializeField] TextMeshProUGUI points;
    [SerializeField] TextMeshProUGUI totalpoints;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] GameObject marcador;
    [SerializeField] GameObject initialScreen;
    [SerializeField] GameObject pointsScreen;
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;
    public int pointsCounter = 0;

    int lastPosition = -1, position;
    float positionX = 0f, positionY = 0f;
    float[] posX = { -7.6f, -6.34f, 1.67f, 2.81f, -2.67f, -1.42f, 6.24f, 7.4f, -6.18f, -5.1f, 2.27f, 3.4f };
    float[] posY = { -1.1f, -1.14f, -0.94f, -0.94f, -2.34f, -2.39f, -2.63f, -2.63f, -4.26f, -4.26f, -4.13f, -4.13f };

    bool startGame = false, canStart = false;
    float globalTimer = 60f, moleTimer = 2f;
    bool moleTimerReduced = false;
    bool initUp = true, initDown = false;
    public int counterDecrease = 1;
    public float speed = 1;
    public float timerPowerUp = 5f;
    bool startDecrease = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (marcador.activeInHierarchy)
        {
            if (globalTimer > 0)
            {
                globalTimer -= Time.deltaTime * speed;
                time.text = Mathf.FloorToInt(globalTimer).ToString();

                if (timerPowerUp > 0 && startDecrease==true)
                {
                    timerPowerUp -= Time.deltaTime;
                }
                else
                {
                    startDecrease = false;
                    timerPowerUp = 5f;
                    speed = 1;
                }
                
                
                

                if (Mathf.Floor(globalTimer) % 10 == 0 && moleTimer > 0.3f && moleTimerReduced == false)
                {
                    counterDecrease++;
                    moleTimerReduced = true;
                }
                else if (Mathf.Floor(globalTimer) % 10 != 0)
                {
                    moleTimerReduced = false;
                }

                if (canStart == false)
                {
                    generatePosition();
                }
                else
                {
                    moleMovement();
                }
            }
            else
            {
                globalTimer = 0f;
                time.text = Mathf.FloorToInt(globalTimer).ToString();
                totalpoints.text = points.text;
                pointsScreen.SetActive(true);
                checkPoints();
                marcador.SetActive(false);
                //startGame = false;
            }
        }
        
    }

    void checkPoints()
    {
        //max points 64
        if (pointsCounter>0 && pointsCounter < 20)
        {
            star2.SetActive(true);
        }
        else if(pointsCounter>=20 && pointsCounter < 50)
        {
            star1.SetActive(true);
            star2.SetActive(true);
        } 
        else if (pointsCounter >= 50)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
    }

    public void startBtnClick()
    {
        initialScreen.SetActive(false);
        pointsScreen.SetActive(false);
        marcador.SetActive(true);
        globalTimer = 60f;
        canStart = false;
    }

    void generatePosition()
    {
        int newPosition;
        do
        {
            newPosition = UnityEngine.Random.Range(0, 12);
        } while (newPosition == lastPosition); 

        lastPosition = newPosition; 
        position = newPosition;

        positionX = posX[position];
        positionY = posY[position];
        mole.transform.position = new Vector2(positionX, positionY);
        canStart = true;
        initUp = true;
        moleTimer = 2f - (0.3f*counterDecrease);
        
    }

    void moleMovement()
    {
        if (initUp && positionY <= (posY[position] + 0.75f)) //mole up
        {
            positionY += 0.015f;
            mole.transform.position = new Vector2(positionX, positionY);
        }
        else 
        {
            initUp = false;
            initDown = true;
            if (moleTimer >= 0) // wait 
            {
                moleTimer -= Time.deltaTime;
            }
            else
            {
                if (initDown && positionY >= posY[position]) // mole down
                {
                    positionY -= 0.015f;
                    mole.transform.position = new Vector2(positionX, positionY);
                }
                else
                {
                    initDown = false;
                    canStart = false;
                }

            }
        }
    }

    private void OnMouseDown()
    {
        pointsCounter += 1;
        points.text = pointsCounter.ToString();
        initUp = false;
        moleTimer = 0f;
        initDown = false;
        canStart = true;
        
    }

    public void powerUpPoints(bool increment)
    {
        if (increment == true)
        {
            pointsCounter += 10;
            points.text = pointsCounter.ToString();
        }
        else
        {
            pointsCounter -= 10;
            points.text = pointsCounter.ToString();
        }
        
    }

    public void changeSpeed(bool slower, bool freeze)
    {
        startDecrease = true;
        if (freeze == true)
        {
            speed = 0f;
        } 
        else
        {
            if (slower == true)
            {
                speed = 0.5f;
            }
            else
            {
                speed = 2f;
            }
        }
        
    }
}
