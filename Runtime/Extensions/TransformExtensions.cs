using UnityEngine;

namespace Playrika.GameFoundation.Extensions
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (GameObject child in transform)
                Object.Destroy(child);
        }

        public static void SetX(this Transform transform, float x)
        {
            var currentPosition = transform.position;
            currentPosition.x = x;
            transform.position = currentPosition;
        }

        public static void SetY(this Transform transform, float y)
        {
            var currentPosition = transform.position;
            currentPosition.y = y;
            transform.position = currentPosition;
        }

        public static void SetZ(this Transform transform, float z)
        {
            var currentPosition = transform.position;
            currentPosition.z = z;
            transform.position = currentPosition;
        }

        public static void SetXY(this Transform transform, float x, float y)
        {
            var currentPosition = transform.position;
            currentPosition.x = x;
            currentPosition.y = y;
            transform.position = currentPosition;
        }

        public static void SetXZ(this Transform transform, float x, float z)
        {
            var currentPosition = transform.position;
            currentPosition.x = x;
            currentPosition.z = z;
            transform.position = currentPosition;
        }

        public static void SetYZ(this Transform transform, float y, float z)
        {
            var currentPosition = transform.position;
            currentPosition.y = y;
            currentPosition.z = z;
            transform.position = currentPosition;
        }
    }
}