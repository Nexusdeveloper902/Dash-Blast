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
          if (other.GetComponent<Health>() != null || other.GetComponentInChildren<Health>() != null || other.GetComponentInParent<Health>() != null)
          {
               var healthComponent = other.GetComponent<Health>();
               healthComponent = other.GetComponentInParent<Health>();
               healthComponent = other.GetComponentInChildren<Health>();
               
               healthComponent.TakeDamage(damage);
          }
     }
}