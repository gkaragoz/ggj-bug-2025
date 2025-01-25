using Car;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxSpeed = 20f; // Maksimum hız
    public float acceleration = 5f; // Hızlanma oranı
    public float deceleration = 7f; // Yavaşlama oranı
    public float detectionRange = 5f; // Engel algılama mesafesi

    public Transform[] wheels;
    private float currentSpeed = 0f; // Mevcut hız
    private bool isMoving = false;  // Hareket halinde olup olmadığını kontrol eder
    private bool isObstacleAhead = false; // Önünde engel olup olmadığını kontrol eder

    public LayerMask carLayer;
    
    public bool isLeftToRight = true;
    
    public Transform spawnPoint;
    
    public CarSpawnController carSpawnController;

    public bool hasInit = false;
    void Update()
    {
        if (!hasInit) return;
        
        DetectObstacle(); // Engel kontrolü yap
        UpdateSpeed();    // Hızı güncelle
        MoveCar();        // Aracı hareket ettir
        CheckCarPosition();
        CheckLigth();
        WheelMovement();
        
    }

    private void WheelMovement()
    {
        foreach (var wheel in wheels)
        {
            wheel.Rotate(Vector3.right,currentSpeed*Time.deltaTime*360);
        }
    }

    private void CheckLigth()
    {
        if (carSpawnController.trafficLightController.isHorizontalLight)
        {
            if (!isMoving)
            {
                Move();
            }
            return;
        };
        
        var nextStopPoint = isLeftToRight
            ? carSpawnController.trafficLightController.leftToRightStopPoints.Find(e=>e.position.x>transform.position.x)
            : carSpawnController.trafficLightController.rigthToLeftStopPoints.Find(e=>e.position.x<transform.position.x);
  
        if (nextStopPoint == null) return;

        var sholdStop = Mathf.Abs(transform.position.x - nextStopPoint.position.x) < 2;

        if (sholdStop)
        {
            Stop();
        }
    }

    private void CheckCarPosition()
    {
        if (Vector3.Distance(transform.position,spawnPoint.position)>300)
        {
            Destroy(gameObject);
        }
    }

    // Aracı hızlandır ve hareket ettir
    public void Move()
    {
        isMoving = true;
    }

    // Aracı durdur (yavaşlat)
    public void Stop()
    {
        isMoving = false;
    }

    // Hızı güncelleme işlemleri
    void UpdateSpeed()
    {
        if (isMoving && !isObstacleAhead) // Eğer hareket ediyor ve önünde engel yoksa
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else // Engel var veya durması gerektiğinde
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed < 0) currentSpeed = 0; // Hız negatif olmasın
            }
        }

        // Hızı sınırla
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    // Aracı hareket ettir
    void MoveCar()
    {
        transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }

    // Engel algılama
    void DetectObstacle()
    {
        RaycastHit hit;
        // Aracın önüne doğru bir ışın gönder
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, detectionRange,carLayer))
        {
            // Eğer ışın bir nesneye çarparsa engel var
            isObstacleAhead = true;
        }
        else
        {
            // Engel yok
            isObstacleAhead = false;
        }
    }

    // Debugging için Raycast'i görselleştir
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
