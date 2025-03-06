using UnityEngine;

namespace AI.Sensors
{
    public class BaseSensor<T> : MonoBehaviour
    {
        public virtual T[] Invoke()
        {
            return null;
        }
    }
}