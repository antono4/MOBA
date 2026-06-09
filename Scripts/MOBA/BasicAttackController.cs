using UnityEngine;

/// <summary>
/// Script controller untuk basic attack yang menggunakan TargetLockOnSystem.
/// Menunjukkan implementasi nyata dari sistem lock-on.
/// </summary>
public class BasicAttackController : MonoBehaviour
{
    #region === REFERENCES ===
    
    [Header("Component References")]
    [Tooltip("Reference ke TargetLockOnSystem")]
    [SerializeField] private TargetLockOnSystem targetLockSystem;
    
    [Tooltip("Transform untuk spawn projectile")]
    [SerializeField] private Transform attackPoint;
    
    [Tooltip("Prefab projectile (optional)")]
    [SerializeField] private GameObject projectilePrefab;
    
    #endregion
    
    #region === ATTACK SETTINGS ===
    
    [Header("Attack Configuration")]
    [Tooltip("Base damage basic attack")]
    [SerializeField] private float baseDamage = 50f;
    
    [Tooltip("Cooldown attack dalam detik")]
    [SerializeField] private float attackCooldown = 1f;
    
    [Tooltip("Range attack (jarak maksimum)")]
    [SerializeField] private float attackRange = 5f;
    
    [Tooltip("Auto-attack range (otomatis attack jika enemy dalam range)")]
    [SerializeField] private float autoAttackRange = 8f;
    
    [Tooltip("Aktifkan auto-attack saat ada enemy dalam range")]
    [SerializeField] private bool enableAutoAttack = true;
    
    #endregion
    
    #region === PRIVATE VARIABLES ===
    
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private Animator animator;
    
    #endregion
    
    #region === UNITY LIFECYCLE ===
    
    private void Awake()
    {
        // Get animator jika ada
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // Update cooldown timer
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        
        // Handle input attack manual
        HandleAttackInput();
        
        // Handle auto-attack
        if (enableAutoAttack && attackTimer <= 0)
        {
            TryAutoAttack();
        }
    }
    
    #endregion
    
    #region === PUBLIC METHODS ===
    
    /// <summary>
    /// Execute basic attack.
    /// Bisa dipanggil dari button UI atau input lain.
    /// </summary>
    public void ExecuteAttack()
    {
        // Cek cooldown
        if (attackTimer > 0)
        {
            Debug.Log("[BasicAttack] Attack on cooldown");
            return;
        }
        
        // Cek apakah ada target
        if (!targetLockSystem.IsTargetLocked)
        {
            Debug.Log("[BasicAttack] No target locked");
            return;
        }
        
        // Cek range ke target
        GameObject target = targetLockSystem.CurrentTarget;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        if (distance > attackRange)
        {
            Debug.Log($"[BasicAttack] Target out of range ({distance:F1}m)");
            return;
        }
        
        // Execute attack
        PerformAttack(target);
    }
    
    /// <summary>
    /// Execute attack tanpa memerlukan target lock ( untuk auto-targeting)
    /// </summary>
    public void ExecuteAttackToTarget(GameObject target)
    {
        if (attackTimer > 0) return;
        if (target == null) return;
        
        PerformAttack(target);
    }
    
    #endregion
    
    #region === PRIVATE METHODS ===
    
    /// <summary>
    /// Handle input attack dari player
    /// </summary>
    private void HandleAttackInput()
    {
        // Klik kanan mouse untuk attack
        if (Input.GetMouseButtonDown(1))
        {
            ExecuteAttack();
        }
        
        // Spasi untuk attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteAttack();
        }
    }
    
    /// <summary>
    /// Coba auto-attack jika ada enemy dalam range
    /// </summary>
    private void TryAutoAttack()
    {
        // Cek apakah sudah ada target
        if (targetLockSystem.IsTargetLocked)
        {
            // Auto-attack target yang sudah dikunci
            ExecuteAttack();
            return;
        }
        
        // Cari enemy dalam auto-attack range
        var enemies = targetLockSystem.GetEnemiesInRange();
        
        if (enemies.Count > 0)
        {
            // Sort berdasarkan jarak (prioritas yang terdekat)
            enemies.Sort((a, b) => 
                Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position))
            );
            
            // Attack enemy terdekat
            GameObject nearestEnemy = enemies[0];
            float distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            
            if (distance <= autoAttackRange)
            {
                ExecuteAttackToTarget(nearestEnemy);
            }
        }
    }
    
    /// <summary>
    /// Perform attack logic
    /// </summary>
    private void PerformAttack(GameObject target)
    {
        // Set cooldown
        attackTimer = attackCooldown;
        
        // Set attacking state
        isAttacking = true;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Face target
        FaceTarget(target);
        
        // Apply damage
        ApplyDamage(target);
        
        // Spawn projectile jika ada
        if (projectilePrefab != null)
        {
            SpawnProjectile(target);
        }
        
        Debug.Log($"[BasicAttack] Attacking {target.name}");
        
        // Reset attacking state setelah delay
        StartCoroutine(ResetAttackingState(0.3f));
    }
    
    /// <summary>
    /// Putar karakter menghadap target
    /// </summary>
    private void FaceTarget(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0; // Keep horizontal only
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    /// <summary>
    /// Apply damage ke target
    /// </summary>
    private void ApplyDamage(GameObject target)
    {
        // Get EnemyHealth component
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        
        if (enemyHealth != null && !enemyHealth.IsDead)
        {
            // Calculate final damage (bisa ditambahkan damage scaling, critical, dll)
            float finalDamage = baseDamage;
            
            // Apply damage
            enemyHealth.TakeDamage(finalDamage, DamageType.Physical);
            
            Debug.Log($"[BasicAttack] Dealt {finalDamage} damage to {target.name}");
        }
        else
        {
            Debug.LogWarning($"[BasicAttack] Target {target.name} has no EnemyHealth component!");
        }
    }
    
    /// <summary>
    /// Spawn projectile menuju target
    /// </summary>
    private void SpawnProjectile(GameObject target)
    {
        if (attackPoint == null)
        {
            attackPoint = transform; // Fallback ke transform karakter
        }
        
        // Hitung arah projectile
        Vector3 direction = (target.transform.position - attackPoint.position).normalized;
        
        // Spawn projectile
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.LookRotation(direction));
        
        // Set target projectile
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Initialize(target, baseDamage);
        }
    }
    
    /// <summary>
    /// Reset attacking state setelah delay
    /// </summary>
    private System.Collections.IEnumerator ResetAttackingState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }
    
    #endregion
    
    #region === PUBLIC PROPERTIES ===
    
    /// <summary>
    /// Apakah sedang dalam state attacking
    /// </summary>
    public bool IsAttacking => isAttacking;
    
    /// <summary>
    /// Sisa cooldown attack
    /// </summary>
    public float AttackCooldownRemaining => attackTimer;
    
    /// <summary>
    /// Apakah bisa attack (cooldown sudah selesai)
    /// </summary>
    public bool CanAttack => attackTimer <= 0;
    
    #endregion
}