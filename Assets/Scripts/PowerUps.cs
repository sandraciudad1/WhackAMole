using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] GameObject marcador;
    public GameObject[] objetos;
    GameObject randomPowerUp;
    GameObject instanciaPowerUp;
    List<GameObject> powerUpsGenerados = new List<GameObject>();

    float[] posX = { -7.58f, -6.53f, 1.72f, 2.76f, -2.67f, -1.6f, 6.26f, 7.3f, -6.2f, -5.1f, 2.3f, 3.4f };
    float[] posY = { -0.46f, -0.43f, -0.3f, -0.3f, -1.7f, -1.7f, -2f, -2f, -3.6f, -3.6f, -3.5f, -3.5f };
    
    public float timer = 60f, powerUpTimer = 1.5f; 
    bool generatePos = true, startCountdown=false;
    int lastPosition = -1, position;
    float positionX = 0f, positionY = 0f;

    private void Start()
    {
        powerUpsGenerados.AddRange(objetos);
    }

    void Update()
    {
        if (marcador.activeInHierarchy)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (Mathf.Floor(timer) % 10 == 0 && generatePos == false)
                {
                    generatePosition();
                    generatePos = true;
                }
                else if (Mathf.Floor(timer) % 10 != 0)
                {
                    generatePos = false;
                }

                if (startCountdown == true)
                {
                    countdown();
                    checkClick();
                    
                }

            }
            

        }
        
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
        Vector2 vectorPosition = new Vector2(positionX, positionY);

        if (powerUpsGenerados.Count > 0)
        {
            int index = Random.Range(0, powerUpsGenerados.Count);
            randomPowerUp = powerUpsGenerados[index];
            powerUpsGenerados.RemoveAt(index); 
        }

        instanciaPowerUp = Instantiate(randomPowerUp, vectorPosition, Quaternion.identity);
        powerUpTimer = 1.5f;
        startCountdown = true;
    }

    void countdown()
    {
        if (powerUpTimer >= 0)
        {
            powerUpTimer -= Time.deltaTime;
        } else
        {
            startCountdown = false;
            Destroy(instanciaPowerUp);
        }
    }

    void checkClick()
    {
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag("present"))
                {
                    powerUpTimer = 0;
                    modifyPoints(true);
                }
                else if (hitInfo.collider.gameObject.CompareTag("bomb"))
                {
                    powerUpTimer = 0;
                    modifyPoints(false);
                } 
                else if (hitInfo.collider.gameObject.CompareTag("slow"))
                {
                    modifySpeed(true, false);
                } 
                else if (hitInfo.collider.gameObject.CompareTag("fast"))
                {
                    modifySpeed(false, false);
                } 
                else if (hitInfo.collider.gameObject.CompareTag("freeze"))
                {
                    modifySpeed(false, true);
                }
            }
        }
    }

    void modifyPoints(bool increment)
    {
        MolesMovement mole = GameObject.FindWithTag("mole").GetComponent<MolesMovement>();
        if (mole != null)
        {
            mole.powerUpPoints(increment);
        }
    }

    void modifySpeed(bool slower, bool freeze)
    {
        MolesMovement mole = GameObject.FindWithTag("mole").GetComponent<MolesMovement>();
        if (mole != null)
        {
            mole.timerPowerUp = 5f;
            mole.changeSpeed(slower, freeze);
        }
    }
}
