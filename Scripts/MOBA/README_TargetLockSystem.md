# 🎮 Target Lock-On System - Setup Guide

Dokumen ini menjelaskan cara setup dan menggunakan sistem Target Lock-On untuk game MOBA di Unity.

---

## 📁 File Structure

```
Scripts/
├── MOBA/
│   ├── TargetLockOnSystem.cs      # Sistem utama penguncian target
│   ├── BasicAttackController.cs   # Controller attack (contoh penggunaan)
│   └── Projectile.cs              # Script projectile (opsional)
└── Enemy/
    └── EnemyHealth.cs             # Component HP untuk enemy
```

---

## 🚀 Quick Setup

### Step 1: Setup Enemy Prefab

1. Buat prefab enemy baru atau gunakan yang sudah ada
2. Tambahkan komponen berikut:
   - **Collider** (CapsuleCollider, SphereCollider, atau BoxCollider)
   - **Rigidbody** (set Gravity = false)
   - **EnemyHealth** script

3. Set layer enemy ke **"Enemy"** (buat layer baru di Project Settings > Tags and Layers)

4. Konfigurasi EnemyHealth:
   ```
   Max Health: 100
   Physical Armor: 10
   Magic Armor: 10
   ```

### Step 2: Setup Player Character

1. Pada player character, tambahkan:
   - **TargetLockOnSystem** script
   - **BasicAttackController** script (opsional, untuk demo)

2. Konfigurasi TargetLockOnSystem:
   ```
   Detection Radius: 10 (sesuaikan dengan range attack karakter)
   Enemy Layer: [x] Enemy (pilih layer Enemy yang sudah dibuat)
   Detection Interval: 0.1
   Locked Target Color: Red (Color.red)
   Enable Debug Log: ✓
   ```

3. Konfigurasi BasicAttackController:
   ```
   Target Lock System: [drag player reference]
   Base Damage: 50
   Attack Cooldown: 1
   Attack Range: 5
   Auto Attack Range: 8
   Enable Auto Attack: ✓
   ```

### Step 3: Setup Layer Mask

1. Buka **Edit > Project Settings > Tags and Layers**
2. Buat layer baru dengan nama **"Enemy"** (biasanya index 8 atau 9)
3. Pada semua prefab enemy, set Layer ke **"Enemy"**
4. Pada TargetLockOnSystem, set Enemy Layer ke **"Enemy"**

---

## 🎮 Cara Penggunaan

### Input Controls

| Input | Action |
|-------|--------|
| **Left Mouse Click** | Lock target (mencari musuh dengan HP terendah) |
| **Right Mouse Click** | Execute basic attack ke target |
| **Space** | Execute basic attack ke target |
| **Escape / T** | Release current target |

### Sistem Prioritas Target

Target dikunci berdasarkan prioritas:
1. **HP Terendah** - Musuh dengan HP paling rendah akan dipilih
2. **Dalam Radius** - Hanya musuh dalam `detectionRadius` yang bisa dikunci
3. **Masih Hidup** - Musuh yang sudah mati tidak akan dipilih

---

## 🔧 API Reference

### TargetLockOnSystem

```csharp
// Public Properties
GameObject CurrentTarget    // Target yang sedang dikunci (null jika tidak ada)
bool IsTargetLocked          // Apakah ada target yang dikunci
float DistanceToTarget       // Jarak ke target

// Public Methods
void LockOnTarget()          // Mengunci target terdekat dengan HP terendah
void ClearTarget()          // Melepas target
List<GameObject> GetEnemiesInRange()  // Mendapatkan semua musuh dalam range
```

### Contoh Penggunaan Script

```csharp
// Cek apakah ada target
if (targetLockSystem.IsTargetLocked)
{
    GameObject target = targetLockSystem.CurrentTarget;
    Debug.Log($"Locked on: {target.name}");
}

// Mengunci target secara programatik
targetLockSystem.LockOnTarget();

// Melepas target
targetLockSystem.ClearTarget();

// Mendapatkan semua musuh untuk AI decision
var enemies = targetLockSystem.GetEnemiesInRange();
foreach (var enemy in enemies)
{
    Debug.Log($"Found enemy: {enemy.name}");
}
```

### EnemyHealth

```csharp
// Properties
float CurrentHp         // HP saat ini
float MaxHealth         // HP maksimum
bool IsDead              // Apakah sudah mati
float HealthPercentage  // Persentase HP (0-1)

// Methods
void TakeDamage(float amount, DamageType type)
void Heal(float amount)
void Revive(float healthPercent = 1f)

// Events
event Action<float, float> OnHealthChanged    // (currentHp, maxHp)
event Action OnDeath
event Action<float, DamageType> OnDamageTaken // (damage, type)

// Contoh penggunaan event
EnemyHealth enemy = GetComponent<EnemyHealth>();
enemy.OnHealthChanged += (current, max) => {
    Debug.Log($"HP: {current}/{max}");
};
enemy.OnDeath += () => {
    Debug.Log("Enemy died!");
};
```

---

## 🎨 Visual Indicator

### Default State
- Enemy memiliki material original (warna default)

### Locked State
- Material enemy berubah menjadi `lockedTargetColor` (merah default)
- Gizmo di Scene view menampilkan line dari player ke target

### Gizmo Visualization
- **Yellow Circle**: Detection radius
- **Red Line**: Line ke target
- **Red Circle**: Indicator di atas target

---

## ⚙️ Customization

### Mengubah Warna Highlight

```csharp
// Di Inspector TargetLockOnSystem
Locked Target Color: Color.yellow  // Ubah ke warna lain
```

### Mengubah Prioritas Target

Untuk mengubah prioritas (misalnya jarak terdekat bukan HP terendah):

```csharp
// Di TargetLockOnSystem.cs, modify method FindLowestHpTarget()
// Ganti dengan FindClosestTarget()

private GameObject FindClosestTarget()
{
    GameObject closest = null;
    float closestDistance = float.MaxValue;
    
    foreach (GameObject enemy in detectedEnemies)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance < closestDistance)
        {
            closestDistance = distance;
            closest = enemy;
        }
    }
    
    return closest;
}
```

### Menambahkan Custom Indicator (VFX/UI)

```csharp
// Di OnTargetLocked(), tambahkan:
private void OnTargetLocked(GameObject target)
{
    // Spawn VFX di atas target
    Vector3 pos = target.transform.position + Vector3.up * 2f;
    Instantiate(lockVFXPrefab, pos, Quaternion.identity);
    
    // Atau update UI
    targetIndicatorUI.Show(target);
}
```

---

## 🐛 Troubleshooting

### Enemy tidak terdeteksi?
1. ✅ Pastikan Enemy memiliki layer "Enemy"
2. ✅ Pastikan Enemy memiliki Collider
3. ✅ Pastikan Enemy memiliki EnemyHealth script
4. ✅ Cek console untuk error message

### Material tidak berubah?
1. ✅ Pastikan Enemy memiliki Renderer component
2. ✅ Cek apakah Shader "Standard" tersedia
3. ✅ Cek Debug Log untuk melihat status

### Target otomatis dilepas?
1. ✅ Cek detection radius sudah cukup besar
2. ✅ Pastikan target tidak mati
3. ✅ Cek console untuk "Target keluar dari range"

---

## 📝 Notes Tambahan

- Sistem ini menggunakan **OverlapSphere** untuk deteksi (performance friendly)
- Refresh detection setiap 0.1 detik untuk balance performa-akurasi
- Material highlight dibuat sebagai instance baru (tidak modify original)
- Sistem event-driven untuk extensibility

---

**Created for: NEXUS ARENA MOBA Project**  
**Unity Version: 2021.3 LTS or newer recommended**