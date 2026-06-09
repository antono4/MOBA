using UnityEngine;

/// <summary>
/// Script untuk projectile yang bergerak menuju target.
/// Bisa digunakan untuk basic attack projectile atau skill projectiles.
/// </summary>
public class Projectile : MonoBehaviour
{
    #region === CONFIGURATION ===
    
    [Header("Projectile Settings")]
    [Tooltip("Kecepatan projectile (units per second)")]
    [SerializeField] private float speed = 15f;
    
    [Tooltip("Damage yang diberikan saat impact")]
    [SerializeField] private float damage = 50f;
    
    [Tooltip("Radius deteksi impact")]
    [SerializeField] private float impactRadius = 0.5f;
    
    [Tooltip("Apakah projectile menghilang saat hit enemy")]
    [SerializeField] private bool destroyOnHit = true;
    
    [Tooltip("Lifetime maximum projectile ( untuk miss)")]
    [SerializeField] private float maxLifetime = 3f;
    
    [Tooltip("Layer yang bisa di-hit")]
    [SerializeField] private LayerMask targetLayer;
    
    #endregion
    
    #region === VISUAL SETTINGS ===
    
    [Header("Visual Settings")]
    [Tooltip("Particle effect saat impact")]
    [SerializeField] private GameObject impactEffect;
    
    [Tooltip("Trail effect untuk projectile")]
    [SerializeField] private TrailRenderer trailRenderer;
    
    #endregion
    
    #region === PRIVATE VARIABLES ===
    
    private GameObject target;
    private Vector3 targetPosition;
    private float damageToDeal;
    private bool isInitialized = false;
    
    #endregion
    
    #region === UNITY LIFECYCLE ===
    
    private void Update()
    {
        if (!isInitialized) return;
        
        // Gerakkan projectile ke target
        MoveToTarget();
        
        // Cek lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            DestroyProjectile();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah collision dengan target layer
        if ((targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Cek apakah ini target yang dituju
            if (other.gameObject == target || other.gameObject.tag == "Enemy")
            {
                OnHit(other.gameObject);
            }
        }
    }
    
    #endregion
    
    #region === PUBLIC METHODS ===
    
    /// <summary>
    /// Inisialisasi projectile dengan target dan damage
    /// </summary>
    /// <param name="targetGameObject">GameObject target untuk dikejar</param>
    /// <param name="damageAmount">Damage yang akan diberikan</param>
    public void Initialize(GameObject targetGameObject, float damageAmount)
    {
        target = targetGameObject;
        targetPosition = target.transform.position;
        damageToDeal = damageAmount;
        isInitialized = true;
        
        // Set target position untuk tracking
        if (target != null)
        {
            StartCoroutine(UpdateTargetPosition());
        }
    }
    
    /// <summary>
    /// Inisialisasi projectile dengan posisi tetap ( untuk skillshot)
    /// </summary>
    /// <param name="position">Posisi target</param>
    /// <param name="damageAmount">Damage yang akan diberikan</param>
    public void InitializeAtPosition(Vector3 position, float damageAmount)
    {
        targetPosition = position;
        target = null;
        damageToDeal = damageAmount;
        isInitialized = true;
    }
    
    #endregion
    
    #region === PRIVATE METHODS ===
    
    /// <summary>
    /// Update posisi target setiap frame (untuk tracking target yang bergerak)
    /// </summary>
    private System.Collections.IEnumerator UpdateTargetPosition()
    {
        while (isInitialized && target != null)
        {
            targetPosition = target.transform.position;
            yield return null;
        }
    }
    
    /// <summary>
    /// Gerakkan projectile menuju target
    /// </summary>
    private void MoveToTarget()
    {
        // Hitung arah dan jarak
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);
        
        // Gerakkan projectile
        transform.position += direction * speed * Time.deltaTime;
        
        // Rotasi projectile menghadap arah gerak
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        // Cek apakah sudah dekat dengan target untuk trigger hit
        if (distance <= impactRadius)
        {
            OnHit(target);
        }
    }
    
    /// <summary>
    /// Handle saat projectile hit target
    /// </summary>
    private void OnHit(GameObject hitTarget)
    {
        // Apply damage
        EnemyHealth enemyHealth = hitTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageToDeal, DamageType.Physical);
            Debug.Log($"[Projectile] Hit {hitTarget.name} for {damageToDeal} damage");
        }
        
        // Spawn impact effect
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy projectile
        if (destroyOnHit)
        {
            DestroyProjectile();
        }
    }
    
    /// <summary>
    /// Hancurkan projectile
    /// </summary>
    private void DestroyProjectile()
    {
        // Stop all coroutines
        StopAllCoroutines();
        
        // Destroy gameobject
        Destroy(gameObject);
    }
    
    #endregion
}