using UnityEngine;

public class Bullet : MonoBehaviour
{
     private float damage;
     private Vector3 moveDirection;
     private float moveSpeed;

     public void Initialize(Vector3 moveDirection, float moveSpeed, float damage)
     {
          this.moveDirection = moveDirection;
          this.moveSpeed = moveSpeed;
          this.damage = damage;
     }
     
     private void Update()
     {
          transform.position += Time.deltaTime * moveSpeed *  moveDirection;
     }

     void OnTriggerEnter2D(Collider2D other)
     {
          Debug.Log(other.name);
          if (other.GetComponent<Health>() != null)
          {
               var healthComponent = other.GetComponent<Health>();
               Debug.Log(healthComponent);
               healthComponent.TakeDamage(damage);
          }
          
          if (!other.CompareTag("Player") && !other.CompareTag("Bullet")) Destroy(gameObject);
     }
}