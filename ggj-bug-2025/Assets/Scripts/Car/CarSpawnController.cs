using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Car
{
    public class CarSpawnController : MonoBehaviour
    {
        public GameObject[] carPrefab;
        public Transform leftToRightSpawnPoint;
        public Transform rightToLeftSpawnPoint;


        public TrafficLightController trafficLightController;
        
        private void Start()
        {
            SpawnCar(leftToRightSpawnPoint, true);
            SpawnCar(rightToLeftSpawnPoint, false);
        }

        public void SpawnCar(Transform spawnPoint,bool isLeftToRight)
        {
            
            StartCoroutine(Do());
            return;
            IEnumerator Do()
            {
                var randomCar = carPrefab[Random.Range(0, carPrefab.Length)];
            
                var car = Instantiate(randomCar, spawnPoint.position, spawnPoint.rotation);
            
                car.transform.position = spawnPoint.position;
                
                car.transform.rotation = spawnPoint.rotation;
                
                var carController = car.GetComponent<CarController>();
            
                carController.spawnPoint = spawnPoint;
                
                carController.isLeftToRight = isLeftToRight;
                
                carController.carSpawnController = this;
                
                carController.hasInit = true;
                
                carController.Move();
                
                var waitTime = Random.Range(2, 10);
                
                yield return new WaitForSeconds(waitTime);

                while (!trafficLightController.isHorizontalLight)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                
                SpawnCar(spawnPoint,isLeftToRight);
            }
            
        }
    }
}
