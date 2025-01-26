using UnityEngine;

namespace DefaultNamespace
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool followPosition = true;
        [SerializeField] private bool followRotation = true;
        [SerializeField] private bool followScale = true;

        void LateUpdate()
        {
            var targetPosition = target.position + offset;
            var targetRotation = target.rotation;
            var targetScale = target.lossyScale;

            if (followPosition)
            {
                transform.position = targetPosition;
            }

            if (followRotation)
            {
                transform.rotation = targetRotation;
            }

            if (followScale)
            {
                transform.localScale = targetScale;
            }
        }
    }
}