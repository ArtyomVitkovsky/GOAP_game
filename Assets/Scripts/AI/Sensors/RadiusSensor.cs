using UnityEngine;

namespace AI.Sensors
{
    public class RadiusSensor : BaseSensor<RaycastHit>
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float radius;

        private RaycastHit[] _raycastHits = new RaycastHit[32];
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public override RaycastHit[] Invoke()
        {
            Physics.SphereCastNonAlloc(
                transform.position, radius, Vector3.up, _raycastHits, 0, layerMask
            );

            return _raycastHits;
        }
    }
}
