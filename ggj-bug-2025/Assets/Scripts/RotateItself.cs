using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RotateItself : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;

        private void Update()
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
    }
}