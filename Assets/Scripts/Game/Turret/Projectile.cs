using System;
using Game.Character.Components;
using Game.NpcSystem;
using UnityEngine;
using Zenject;

namespace Game.Turret
{
    public class Projectile : MonoBehaviour, IDamageDealer
    {
        // public static Action<DiContainer> GetContextDecorator(IWorldMember sender)
        // {
        //     return diContainer =>
        //     {
        //         diContainer.BindInstance(sender);
        //     };
        // }
        
        [Inject] private ICombatService CombatService;

        [Inject] private IWorldMember sender;
        
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private GameObject explosion;
        [SerializeField] private float speed = 100;
        [SerializeField] private int damage = 10;

        public IWorldMember Sender => sender;
        public int Damage => damage;

        public void Launch()
        {
            rigidbody.AddForce(transform.forward * speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            var contact = other.GetContact(0);
            
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Instantiate(explosion, position, rotation);

            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                CombatService.ProcessAttack(this, damagable);
            }
            
            Destroy(gameObject);
        }

    }
}