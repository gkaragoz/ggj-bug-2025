using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTraffic.Light
{
    public class TrafficLightController : MonoBehaviour
    {
        public List<TrafficLight> leftToRightTrafficLights;
        public List<TrafficLight> forwardTrafficLights;

        public List<Transform> leftToRightStopPoints;
        public List<Transform> rigthToLeftStopPoints;
    
        public bool isHorizontalLight = true;

        public Vector2 lightWaitTime;

        private void Start()
        {
            SetLeftToRightLights(true);
        }

        private void SetLeftToRightLights(bool isGreen)
        {
            StartCoroutine(Do());
            return;

            IEnumerator Do()
            {
                isHorizontalLight = false;

                foreach (var trafficLight in leftToRightTrafficLights)
                {
                    foreach (var lightTransform in trafficLight.redLight)
                    {
                        lightTransform.gameObject.SetActive(false);
                    }
                
                    foreach (var lightTransform in trafficLight.yellowLight)
                    {
                        lightTransform.gameObject.SetActive(true);
                    }
                
                    foreach (var lightTransform in trafficLight.greenLight)
                    {
                        lightTransform.gameObject.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(1);

                foreach (var trafficLight in leftToRightTrafficLights)
                {
                    foreach (var lightTransform in trafficLight.redLight)
                    {
                        lightTransform.gameObject.SetActive(!isGreen);
                    }
                
                    foreach (var lightTransform in trafficLight.yellowLight)
                    {
                        lightTransform.gameObject.SetActive(false);
                    }
                
                    foreach (var lightTransform in trafficLight.greenLight)
                    {
                        lightTransform.gameObject.SetActive(isGreen);
                    }
                }

                isHorizontalLight = isGreen;

                var waitTime = isGreen ? lightWaitTime.y : lightWaitTime.x;

                yield return new WaitForSeconds(waitTime);

                SetLeftToRightLights(!isGreen);
            }
        }
    
    }
}