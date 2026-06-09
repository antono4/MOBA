using UnityEngine;
using System;

/// <summary>
/// Script untuk mengelola Health Point (HP) dan status kematian enemy.
/// Menjadi dependency untuk TargetLockOnSystem.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    #region === CONFIGURATION ===
    
    [Header("Health Configuration")]
    [Tooltip("HP maksimum enemy")]
    [SerializeField] private float maxHealth = 100f;
    
    [Tooltip("Armor fisik (mengurangi damage fisik)")]
    [SerializeField] private float physicalArmor = 10f;
    
    [Tooltip("Armor magic (mengurangi damage magic)")]
    [SerializeField] private float magicArmor = 10f;
    
    #endregion
    
    #region === EVENTS ===
    
    // Event saat HP berubah
    public event Action<float, float> OnHealthChanged; // (currentHp, maxHp)
    
    // Event saat enemy mati
    public event Action OnDeath;
    
    // Event saat damage diterima
    public event Action<float, DamageType> OnDamageTaken; // (damage, type)
    
    #endregion
    
    #region === PRIVATE VARIABLES ===
    
    private float currentHealth;
    private bool isDead = false;
    
    #endregion
    
    #region === PUBLIC PROPERTIES ===
    
    /// <summary>
    /// HP saat ini enemy
    /// </summary>
    public float CurrentHp => currentHealth;
    
    /// <summary>
    /// HP maksimum enemy
    /// </summary>
    public float MaxHealth => maxHealth;
    
    /// <summary>
    /// Apakah enemy sudah mati
    /// </summary>
    public bool IsDead => isDead;
    
    /// <summary>
    /// Persentase HP (0-1)
    /// </summary>
    public float HealthPercentage => maxHealth > 0 ? currentHealth / maxHealth : 0f;
    
    /// <summary>
    /// Armor fisik enemy
    /// </summary>
    public float PhysicalArmor => physicalArmor;
    
    /// <summary>
    /// Armor magic enemy
    /// </summary>
    public float MagicArmor => magicArmor;
    
    #endregion
    
    #region === UNITY LIFECYCLE ===
    
    private void Awake()
    {
        // Initialize HP ke maksimum saat spawn
        currentHealth = maxHealth;
    }
    
    #endregion
    
    #region === PUBLIC METHODS ===
    
    /// <summary>
    /// Memberikan damage ke enemy.
    /// Menghitung reduction dari armor terlebih dahulu.
    /// </summary>
    /// <param name="amount">Jumlah damage sebelum armor reduction</param>
    /// <param name="damageType">Tipe damage (Physical/Magic/True)</param>
    public void TakeDamage(float amount, DamageType damageType = DamageType.Physical)
    {
        // Skip jika enemy sudah mati
        if (isDead) return;
        
        // Hitung damage setelah reduction armor
        float actualDamage = CalculateDamageAfterArmor(amount, damageType);
        
        // Kurangi HP
        currentHealth -= actualDamage;
        
        // Clamp HP tidak boleh kurang dari 0
        currentHealth = Mathf.Max(0f, currentHealth);
        
        // Trigger event damage
        OnDamageTaken?.Invoke(actualDamage, damageType);
        
        // Trigger event HP change
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Log untuk debug
        Debug.Log($"[EnemyHealth] {gameObject.name} took {actualDamage} {damageType} damage. HP: {currentHealth}/{maxHealth}");
        
        // Cek kematian
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Memberikan healing ke enemy (untuk enemy yang bisa di-heal)
    /// </summary>
    /// <param name="amount">Jumlah heal</param>
    public void Heal(float amount)
    {
        if (isDead) return;
        
        currentHealth += amount;
        
        // Clamp HP tidak boleh lebih dari max
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        // Trigger event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        Debug.Log($"[EnemyHealth] {gameObject.name} healed for {amount}. HP: {currentHealth}/{maxHealth}");
    }
    
    /// <summary>
    /// Memulihkan enemy dari kematian (untuk respawn system)
    /// </summary>
    /// <param name="healthPercent">Persentase HP saat respawn (0-1)</param>
    public void Revive(float healthPercent = 1f)
    {
        if (!isDead) return;
        
        isDead = false;
        currentHealth = maxHealth * healthPercent;
        
        // Trigger event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        Debug.Log($"[EnemyHealth] {gameObject.name} revived with {currentHealth} HP");
    }
    
    /// <summary>
    /// Modifier armor secara dinamis
    /// </summary>
    /// <param name="physical">Armor fisik baru (-1 untuk disable)</param>
    /// <param name="magic">Armor magic baru (-1 untuk disable)</param>
    public void ModifyArmor(float physical, float magic)
    {
        physicalArmor = physical >= 0 ? physical : physicalArmor;
        magicArmor = magic >= 0 ? magic : magicArmor;
    }
    
    #endregion
    
    #region === PRIVATE METHODS ===
    
    /// <summary>
    /// Menghitung damage setelah dikurangi armor.
    /// Menggunakan formula: Damage * (100 / (100 + Armor))
    /// </summary>
    private float CalculateDamageAfterArmor(float damage, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Physical:
                // Formula pengurangan armor untuk damage fisik
                float physicalReduction = 100f / (100f + physicalArmor);
                return damage * physicalReduction;
                
            case DamageType.Magic:
                // Formula pengurangan armor untuk damage magic
                float magicReduction = 100f / (100f + magicArmor);
                return damage * magicReduction;
                
            case DamageType.True:
                // True damage tidak berkurang oleh armor
                return damage;
                
            default:
                return damage;
        }
    }
    
    /// <summary>
    /// Handle kematian enemy
    /// </summary>
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        Debug.Log($"[EnemyHealth] {gameObject.name} has died!");
        
        // Trigger death event
        OnDeath?.Invoke();
        
        // Nonaktifkan collider untuk prevent further damage
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Optional: spawn particle effect atau play death animation
        // Disini bisa ditambahkan logic untuk object pooling
    }
    
    #endregion
    
    #region === EDITOR VISUALIZATION ===
    
    // Visualisasi HP di inspector (untuk debugging)
    private void OnGUI()
    {
        #if UNITY_EDITOR
        // Hanya tampilkan di Editor mode
        if (!Application.isPlaying) return;
        
        // Posisi di atas enemy
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        
        // Hitung lebar bar HP berdasarkan persentase
        float hpPercent = HealthPercentage;
        float barWidth = 60f;
        float barHeight = 8f;
        
        // Background bar (merah untuk HP)
        GUI.color = Color.red;
        GUI.DrawTexture(new Rect(screenPos.x - barWidth/2, Screen.height - screenPos.y, barWidth, barHeight), Texture2D.whiteTexture);
        
        // Foreground bar (hijau untuk HP remaining)
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(screenPos.x - barWidth/2, Screen.height - screenPos.y, barWidth * hpPercent, barHeight), Texture2D.whiteTexture);
        
        // Border
        GUI.color = Color.white;
        GUI.Box(new Rect(screenPos.x - barWidth/2 - 1, Screen.height - screenPos.y - 1, barWidth + 2, barHeight + 2), "");
        #endif
    }
    
    #endregion
}

/// <summary>
/// Enum untuk tipe damage
/// </summary>
public enum DamageType
{
    Physical,
    Magic,
    True
}