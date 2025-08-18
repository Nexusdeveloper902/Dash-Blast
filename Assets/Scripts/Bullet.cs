using UnityEngine;

public class Bullet : MonoBehaviour
{
     private float damage;
     private Vector3 moveDirection;
     private float moveSpeed;
     private GameObject owner;

     public void Initialize(Vector3 moveDirection, float moveSpeed, float damage, GameObject owner)
     {
          this.moveDirection = moveDirection;
          this.moveSpeed = moveSpeed;
          this.damage = damage;
          this.owner = owner;
     }
     
     private void Update()
     {
          transform.position += Time.deltaTime * moveSpeed *  moveDirection;
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          if (other.gameObject == owner) return;
          Debug.Log(other.name);
          if (other.GetComponent<Health>() != null && other.name != owner.name)
          {
               var healthComponent = other.GetComponent<Health>();
               Debug.Log(healthComponent);
               healthComponent.TakeDamage(damage);
          }
          
          if (!other.CompareTag("Player") && !other.CompareTag("Bullet")) Destroy(gameObject);
     }
}