using DefaultNamespace.ScriptableEvents;
using UnityEngine;
using Variables;
using Random = UnityEngine.Random;

namespace Asteroids
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private ScriptableEventInt onAsteroidDestroyed;

        [Header("Config:")]
        public float minForce;
        public float maxForce;
        public float minSize;
        public float maxSize;
        public float minTorque;
        public float maxTorque;
        public int minChildAmount = 0;
        public int maxChildAmount = 0;

        [Header("References:")]
        [SerializeField] private Transform shape;

        private Rigidbody2D _rigidbody;
        private Vector3 _direction;
        private int _instanceId;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _instanceId = GetInstanceID();
            
            SetDirection();
            AddForce();
            AddTorque();
            SetSize();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.Equals(other.tag, "Laser"))
            {
               HitByLaser();
            }
        }

        private void HitByLaser()
        {
            onAsteroidDestroyed.Raise(_instanceId);
            var childAmount = Random.Range(minChildAmount, maxChildAmount + 1);

            for (int i = 0; i < childAmount; i++)
            {
                var tempAsteroid = Instantiate(this, transform.position, Quaternion.identity).GetComponent<Asteroid>();
                tempAsteroid.minForce = minForce / 2;
                tempAsteroid.maxForce = maxForce / 2;
                tempAsteroid.minSize = minSize / 2;
                tempAsteroid.maxSize = maxSize / 2;
                tempAsteroid.minTorque = minTorque / 2;
                tempAsteroid.maxTorque = maxTorque / 2;
                tempAsteroid.minChildAmount = 0;
                tempAsteroid.maxChildAmount = 0;
            }
            
            Destroy(gameObject);
        }

        // TODO Can we move this to a single listener, something like an AsteroidDestroyer?
        public void OnHitByLaser(IntReference asteroidId)
        {
            if (_instanceId == asteroidId.GetValue())
            {
                Destroy(gameObject);
            }
        }
        
        public void OnHitByLaserInt(int asteroidId)
        {
            if (_instanceId == asteroidId)
            {
                Destroy(gameObject);
            }
        }
        
        private void SetDirection()
        {
            var size = new Vector2(3f, 3f);
            var target = new Vector3
            (
                Random.Range(-size.x, size.x),
                Random.Range(-size.y, size.y)
            );

            _direction = (target - transform.position).normalized;
        }

        private void AddForce()
        {
            var force = Random.Range(minForce, maxForce);
            _rigidbody.AddForce( _direction * force, ForceMode2D.Impulse);
        }

        private void AddTorque()
        {
            var torque = Random.Range(minTorque, maxTorque);
            var roll = Random.Range(0, 2);

            if (roll == 0)
                torque = -torque;
            
            _rigidbody.AddTorque(torque, ForceMode2D.Impulse);
        }

        private void SetSize()
        {
            var size = Random.Range(minSize, maxSize);
            shape.localScale = new Vector3(size, size, 0f);
        }
    }
}
