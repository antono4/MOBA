using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script untuk sistem penguncian target otomatis pada karakter player.
/// Menemukan musuh terdekat, prioritas berdasarkan HP terendah, dan menampilkan indikator visual.
/// </summary>
public class TargetLockOnSystem : MonoBehaviour
{
    #region === KONFIGURASI DETEKSI ===
    
    [Header("Detection Settings")]
    [Tooltip("Radius deteksi untuk mencari musuh terdekat")]
    [SerializeField] private float detectionRadius = 10f;
    
    [Tooltip("Layer mask untuk mendeteksi musuh (pastikan enemy memiliki collider)")]
    [SerializeField] private LayerMask enemyLayer;
    
    [Tooltip("Interval refresh deteksi dalam detik")]
    [SerializeField] private float detectionInterval = 0.1f;
    
    #endregion
    
    #region === VISUAL SETTINGS ===
    
    [Header("Visual Indicator Settings")]
    [Tooltip("Warna material saat target terkunci")]
    [SerializeField] private Color lockedTargetColor = Color.red;
    
    [Tooltip("Warna default material enemy")]
    [SerializeField] private Color defaultTargetColor = Color.white;
    
    [Tooltip("Aktifkan debug log di console")]
    [SerializeField] private bool enableDebugLog = true;
    
    #endregion
    
    #region === PRIVATE VARIABLES ===
    
    // Target yang sedang dikunci
    private GameObject currentTarget;
    
    // Reference ke renderer target untuk reset warna
    private Renderer targetRenderer;
    
    // Material original dari target
    private Material originalMaterial;
    
    // Material highlight untuk target lock
    private Material highlightMaterial;
    
    // Daftar musuh yang sedang dalam detection range
    private List<GameObject> detectedEnemies = new List<GameObject>();
    
    // Timer untuk detection interval
    private float detectionTimer = 0f;
    
    // Apakah target sedang aktif
    private bool isTargetLocked = false;
    
    #endregion
    
    #region === PUBLIC PROPERTIES ===
    
    /// <summary>
    /// Mengembalikan target yang sedang dikunci (null jika tidak ada)
    /// </summary>
    public GameObject CurrentTarget => currentTarget;
    
    /// <summary>
    /// Apakah ada target yang sedang dikunci
    /// </summary>
    public bool IsTargetLocked => isTargetLocked && currentTarget != null;
    
    /// <summary>
    /// Jarak ke target yang sedang dikunci
    /// </summary>
    public float DistanceToTarget => 
        currentTarget != null ? Vector3.Distance(transform.position, currentTarget.transform.position) : float.MaxValue;
    
    #endregion
    
    #region === UNITY LIFECYCLE ===
    
    private void Awake()
    {
        // Buat material highlight dari warna yang ditentukan
        CreateHighlightMaterial();
    }
    
    private void Update()
    {
        // Update timer untuk detection interval
        detectionTimer += Time.deltaTime;
        
        // Cek input basic attack (klik kiri mouse)
        HandleBasicAttackInput();
        
        // Refresh detection secara periodik
        if (detectionTimer >= detectionInterval)
        {
            RefreshDetection();
            detectionTimer = 0f;
        }
        
        // Validasi target masih valid (belum mati atau di luar range)
        ValidateCurrentTarget();
    }
    
    #endregion
    
    #region === PUBLIC METHODS ===
    
    /// <summary>
    /// Metode utama untuk mengunci target.
    /// Dipanggil saat player menekan tombol Basic Attack.
    /// </summary>
    public void LockOnTarget()
    {
        // Deteksi semua musuh dalam radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        
        if (hitColliders.Length == 0)
        {
            LogDebug("Tidak ada musuh dalam radius deteksi");
            ClearTarget();
            return;
        }
        
        // Konversi hasil deteksi ke list dan filter yang sudah mati
        detectedEnemies.Clear();
        foreach (Collider collider in hitColliders)
        {
            // Skip jika tidak ada komponen EnemyHealth atau sudah mati
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                detectedEnemies.Add(collider.gameObject);
            }
        }
        
        if (detectedEnemies.Count == 0)
        {
            LogDebug("Semua musuh dalam radius sudah mati");
            ClearTarget();
            return;
        }
        
        // Urutkan berdasarkan HP terendah (prioritas utama)
        GameObject bestTarget = FindLowestHpTarget();
        
        if (bestTarget != null)
        {
            // Set target baru
            SetNewTarget(bestTarget);
            LogDebug($"Target dikunci: {currentTarget.name} dengan HP {GetTargetHp(currentTarget)}");
        }
    }
    
    /// <summary>
    /// Membatalkan penguncian target saat ini
    /// </summary>
    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            // Reset material target ke warna default
            ResetTargetMaterial();
            
            LogDebug($"Target dilepas: {currentTarget.name}");
        }
        
        currentTarget = null;
        targetRenderer = null;
        originalMaterial = null;
        isTargetLocked = false;
    }
    
    /// <summary>
    /// Mendapatkan semua musuh dalam radius deteksi
    /// </summary>
    public List<GameObject> GetEnemiesInRange()
    {
        List<GameObject> enemies = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        
        foreach (Collider collider in hitColliders)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                enemies.Add(collider.gameObject);
            }
        }
        
        return enemies;
    }
    
    #endregion
    
    #region === PRIVATE METHODS ===
    
    /// <summary>
    /// Membuat material highlight untuk efek lock-on
    /// </summary>
    private void CreateHighlightMaterial()
    {
        // Buat instance material baru agar tidak mengubah material original
        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = lockedTargetColor;
        
        // Set properti untuk efek glow/emisif sederhana
        highlightMaterial.EnableKeyword("_EMISSION");
        highlightMaterial.SetColor("_EmissionColor", lockedTargetColor * 0.5f);
    }
    
    /// <summary>
    /// Handle input dari player untuk basic attack
    /// </summary>
    private void HandleBasicAttackInput()
    {
        // Klik kiri mouse untuk basic attack
        if (Input.GetMouseButtonDown(0))
        {
            // Jika belum punya target, cari target baru
            if (!IsTargetLocked)
            {
                LockOnTarget();
            }
            else
            {
                // Jika sudah ada target, reset untuk mencari target baru
                // (Opsional: bisa dihapus jika ingin mempertahankan target)
                LockOnTarget();
            }
        }
        
        // Tekan Escape atau T untuk melepas target
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.T))
        {
            ClearTarget();
        }
    }
    
    /// <summary>
    /// Refresh daftar musuh yang terdetekasi
    /// </summary>
    private void RefreshDetection()
    {
        // Update daftar enemy dalam range
        detectedEnemies.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        
        foreach (Collider collider in hitColliders)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                detectedEnemies.Add(collider.gameObject);
            }
        }
    }
    
    /// <summary>
    /// Menemukan target dengan HP terendah dari daftar musuh
    /// </summary>
    private GameObject FindLowestHpTarget()
    {
        if (detectedEnemies.Count == 0) return null;
        
        GameObject lowestHpEnemy = null;
        float lowestHp = float.MaxValue;
        
        foreach (GameObject enemy in detectedEnemies)
        {
            // Skip jika enemy sudah mati
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyHealth.IsDead) continue;
            
            // Bandingkan HP
            float currentHp = enemyHealth != null ? enemyHealth.CurrentHp : float.MaxValue;
            if (currentHp < lowestHp)
            {
                lowestHp = currentHp;
                lowestHpEnemy = enemy;
            }
        }
        
        return lowestHpEnemy;
    }
    
    /// <summary>
    /// Menetapkan target baru dan apply visual indicator
    /// </summary>
    private void SetNewTarget(GameObject newTarget)
    {
        // Bersihkan target lama jika ada
        if (currentTarget != null && currentTarget != newTarget)
        {
            ResetTargetMaterial();
        }
        
        // Set target baru
        currentTarget = newTarget;
        isTargetLocked = true;
        
        // Get renderer dan simpan material original
        targetRenderer = currentTarget.GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            originalMaterial = targetRenderer.material;
            
            // Apply highlight material
            ApplyTargetHighlight();
        }
        
        // Event callback untuk sistem lain (audio, VFX, dll)
        OnTargetLocked(newTarget);
    }
    
    /// <summary>
    /// Apply efek highlight ke target
    /// </summary>
    private void ApplyTargetHighlight()
    {
        if (targetRenderer != null && highlightMaterial != null)
        {
            targetRenderer.material = highlightMaterial;
        }
    }
    
    /// <summary>
    /// Reset material target ke kondisi original
    /// </summary>
    private void ResetTargetMaterial()
    {
        if (targetRenderer != null && originalMaterial != null)
        {
            targetRenderer.material = originalMaterial;
        }
    }
    
    /// <summary>
    /// Validasi apakah target masih valid (hidup dan dalam range)
    /// </summary>
    private void ValidateCurrentTarget()
    {
        if (currentTarget == null)
        {
            isTargetLocked = false;
            return;
        }
        
        // Cek apakah target sudah mati
        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            LogDebug("Target sudah mati, melepas penguncian");
            ClearTarget();
            return;
        }
        
        // Cek apakah target di luar range
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > detectionRadius)
        {
            LogDebug("Target keluar dari range, melepas penguncian");
            ClearTarget();
            return;
        }
    }
    
    /// <summary>
    /// Mendapatkan HP saat ini dari target
    /// </summary>
    private float GetTargetHp(GameObject target)
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        return enemyHealth != null ? enemyHealth.CurrentHp : 0f;
    }
    
    /// <summary>
    /// Event callback saat target berhasil dikunci
    /// </summary>
    private void OnTargetLocked(GameObject target)
    {
        // Bisa ditambahkan efek lain seperti:
        // - Play sound effect
        // - Spawn VFX
        // - Trigger animasi
        // - Update UI
        
        LogDebug($"[EVENT] Target Locked: {target.name}");
    }
    
    /// <summary>
    /// Helper untuk debug log
    /// </summary>
    private void LogDebug(string message)
    {
        if (enableDebugLog)
        {
            Debug.Log($"[TargetLockOn] {message}");
        }
    }
    
    #endregion
    
    #region === GIZMOS VISUALIZATION ===
    
    private void OnDrawGizmosSelected()
    {
        // Visualisasi radius deteksi di Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Visualisasi line ke target saat ini
        if (currentTarget != null && isTargetLocked)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
            
            // Circle di sekitar target
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentTarget.transform.position, 1f);
        }
    }
    
    #endregion
}