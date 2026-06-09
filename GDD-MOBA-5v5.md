# GAME DESIGN DOCUMENT
## NEXUS ARENA — 5v5 Multiplayer Online Battle Arena

**Versi Dokumen:** 1.0  
**Tanggal:** 9 Juni 2026  
**Status:** Draft Awal — Ready for Review  
**Target Platform:** PC (Windows/macOS), Mobile (iOS/Android)

---

# BAGIAN 1: KONSEP UTAMA & TEMA

## 1.1 Executive Summary

Dokumen ini menyajikan rancangan game MOBA 5v5 dengan nama kode **NEXUS ARENA**. Game ini dirancang untuk memberikan pengalaman kompetitif yang seimbang antara gameplay strategis mendalam dan aksi cepat yang memuaskan. Fokus utama adalah pada sistem netcode yang adil, ekonomi yang transparan, dan karakter yang mudah dipahami namun sulit dikuasai.

---

## 1.2 Tema Game: Tiga Konsep Unik

Setiap tema di bawah ini menggabungkan dua genre berbeda untuk menciptakan identitas visual dan gameplay yang unik.

### 🎮 Tema 1: "AETHON FRONTIER" 
**Kombinasi Genre: Steampunk + Sihir Kuno**

> *"Ketika mesin uap bertemu mantra kuno di tanah yang dilupakan waktu."*

**Visual Concept:**
- Dunia pasca-apokaliptik di mana peradaban manusia bertahan dengan menggabungkan teknologi steam-engine dengan sihir arkanum
- Map berupa gurun pasca-industri dengan mesin raksasa yang menjadi landmark dan objective
- Karakter mengenakan armor brass-plate dengan glyph sihir yang bersinar saat kemampuan digunakan
- Warna dominan: Copper bronze, deep emerald, dan arcane purple

**Gameplay Hook:**
- Mekanik "Overcharge" — skill tertentu membutuhkan waktu charge menggunakan resource unik
- Item memiliki dual-stat (contoh: Steam Rifle = +40 Physical ATK + 15% CDR)
- Jungle penuh dengan "Ancient Machina" yang menggabungkan monster mekanis dengan entitas sihir

**Target Audience:** Player yang menikmati aesthetic vintage-industrial dengan depth gameplay meta yang kompleks

---

### 🎮 Tema 2: "KURALA CHRONICLES"
**Kombinasi Genre: Cyberpunk + Mitologi Nusantara**

> *"Di tahun 2187, dewa-dewa purba terbangun di antara tower megakorp."*

**Visual Concept:**
- Alternatif Indonesia masa depan — Jakarta 2187 setelah "The Great Convergence" menyatukan dimensi mitologi lokal dengan realita siber
- Map adalah Jakarta yang runtuh — flyover berkarat, tower korporasi yang menjulang, kanal-kanal yang berubah jadi arena perang
- Karakter memiliki desain "tradisional-mecha" — batik bercampur augmentasi siber, trisula dengan blade plasma
- Warna dominan: Neon orange, deep teal, dan gold mysticism

**Gameplay Hook:**
- Sistem "Karma Resonance" — killstreak memberikan buff unik sesuai "dosa" yang dilakukan (agresif, defensif, utility)
- Jungle boss adalah "Wayang Beasts" — entitas mitologi yang di-digitize menjadi boss mekanis
- Item memiliki efek "spiritual" yang berinteraksi dengan map objective

**Target Audience:** Player yang mencari identitas budaya kuat dengan setting futuristik segar

---

### 🎮 Tema 3: "VOID COLOSSEUM"  
**Kombinasi Genre: Dark Fantasy + Space Opera**

> *"Gladiator dari galaksi berbeda dipaksa bertarung di arena antar-dimensi."*

**Visual Concept:**
- Arena gladiator di ruang antar-dimensi — batu obsidian mengambang di void kosmis
- Map memiliki area "Gravity Zones" berbeda — beberapa area low-gravity mempengaruhi movement dan projectile
- Karakter adalah gladiator dari ras berbeda: knight armor minimalis, alien insectoid, cosmic entities
- Warna dominan: Void black, stellar white, dan blood crimson

**Gameplay Hook:**
- Sistem "Colosseum Tokens" — selain gold, ada token khusus untuk membeli exclusive items
- Setiap karakter memiliki "Chain Ability" — skill ultimate bisa dikombinasikan dengan ally untuk efek combo
- Boss adalah "Arena Champions" — boss previous season yang jadi playable boss dengan mechanics unik

**Target Audience:** Player yang mencari pengalaman MOBA dengan lore mendalam dan aesthetic dark-epic

---

## 1.3 Tema yang Dipilih untuk Pengembangan

**Direkomendasikan: Tema 2 — "KURALA CHRONICLES"**

**Alasan:**
1. Diferensiasi pasar kuat — tidak ada MOBA major dengan setting Southeast Asian Cyberpunk
2. Peluang partnership dengan IP lokal Indonesia
3. Sistem "Karma Resonance" memberikan uniqueness gameplay
4. Aesthetic yang sangat "Instagrammable" dan marketable untuk content creation

---

# BAGIAN 2: ARSITEKTUR MAP (PETA)

## 2.1 Overview Map Layout

```
                    ╔═══════════════════════════════╗
                    ║     ENEMY NEXUS (BASE)         ║
                    ║   ┌─────┐     ┌─────┐         ║
                    ║   │INHIB│     │INHIB│         ║
                    ║   └──┬──┘     └──┬──┘         ║
    ════════════════╬══════╧═══════════╧═══════╬═══════════════
    ║               │                       │               ║
    ║   ENEMY       │                       │     ALLY      ║
    ║   JUNGLE      │                       │     JUNGLE    ║
    ║               │                       │               ║
    ║   [RED]   [CRAB]  ════MID LANE════   [CRAB]  [BLUE]   ║
    ║               │       [RIVER]        │               ║
    ║               │                       │               ║
    ║   [WOLF]      │                       │      [WOLF]   ║
    ║               │                       │               ║
    ║   [RAPTOR]    │                       │    [RAPTOR]   ║
    ║               │                       │               ║
    ════════════════╬═══════╤═══════════════╧═══════════════
                    ║       │                  
                    ║   ┌───┴───┐              
                    ║   │ MID   │              
                    ║   │ TURRET│              
                    ║   └───┬───┘              
                    ║       │                  
    ════════════════╬═══════╧═══════════════════════════════
    ║               │                       │               
    ║   BOTTOM      │                       │     TOP       
    ║   LANE        │                       │     LANE      
    ║               │                       │               
    ║   [TURRET]════╧═══════════════════════╧════[TURRET]  
    ║               │                       │               
    ║   [TURRET]    │                       │     [TURRET]  
    ║               │                       │               
    ║               │                       │               
    ════════════════╬═══════════════════════╧═══════════════
                    ║
                    ║   ┌─────────────────┐
                    ║   │   ALLY NEXUS     │
                    ║   │     (BASE)       │
                    ║   └─────────────────┘
                    ╚═══════════════════════════════╝
```

## 2.2 Detail Lane System

### 2.2.1 Top Lane (Solo Lane)

**Karakteristik:**
- Distance: ~4500 units (terpanjang)
- Jumlah Turret: 2 (Outer + Inner)
- Experience Lane: Medium (solo XP bonus +15%)

**Strategic Role:**
- Dihuni oleh solo laner (Fighter/Tank/Mage)
- Map control penting untuk invade
- Tower diving risk tinggi karena distance dari base

**Minion Wave Pattern:**
- Spawn setiap 30 detik
- 3 Melee + 1 Ranged per wave
- Siege minion setiap 3 wave

---

### 2.2.2 Mid Lane (Carry Lane)

**Karakteristik:**
- Distance: ~3500 units (terpendek)
- Jumlah Turret: 2 (Outer + Inner)
- Experience Lane: High (central XP bonus +10%)

**Strategic Role:**
- Dihuni oleh Mage/Marksman
- Roaming hub untuk semua lane
- Objective control (Jungle Boss) lebih mudah diakses

**Minion Wave Pattern:**
- Spawn setiap 30 detik
- 3 Melee + 1 Ranged per wave
- Siege minion setiap 3 wave

---

### 2.2.3 Bottom Lane (Duo Lane)

**Karakteristik:**
- Distance: ~4000 units
- Jumlah Turret: 2 (Outer + Inner)
- Experience Lane: Split (ADC dapat lebih fokus farming)

**Strategic Role:**
- Dihuni oleh Marksman + Support
- Dragon proximity advantage
- Protect tower crucial untuk snowball

**Minion Wave Pattern:**
- Spawn setiap 30 detik
- 3 Melee + 1 Ranged per wave
- Siege minion setiap 3 wave

---

## 2.3 Jungle System

### 2.3.1 Jungle Layout

```
┌─────────────────────────────────────────────────────────┐
│                    ENEMY JUNGLE                        │
│                                                         │
│    ┌─────────┐          ┌─────────┐                      │
│    │  RED    │          │  BLUE  │                      │
│    │ BUFF    │          │  BUFF  │                      │
│    │(ASPD/   │          │ (CDR/  │                      │
│    │ HP REG) │          │ MANA   │                      │
│    └────┬────┘          │ REG)   │                      │
│         │               └────┬────┘                     │
│         ▼                    ▼                          │
│    ┌─────────┐          ┌─────────┐                     │
│    │ CRIMSON │          │ JADE    │                      │
│    │ CRAB    │          │  CRAB   │                      │
│    │(Early   │          │ (Vision │                      │
│    │gank     │          │ control)│                      │
│    │ tool)   │          └────┬────┘                     │
│    └────┬────┘               │                          │
│         │                    │                          │
│    ┌────┴────────────────────┴────┐                    │
│    │      KURALA WYRM (BOSS 1)      │                   │
│    │   Gold Bonus + Lane Pressure   │                   │
│    └───────────────────────────────┘                    │
│                         │                               │
│                    [RIVER]                              │
│                         │                               │
│    ┌───────────────────────────────┐                   │
│    │     VOID SENTINEL (BOSS 2)    │                   │
│    └───────────────────────────────┘                   │
│                         │                               │
│    ┌─────────┐          ┌─────────┐                     │
│    │ JADE    │          │CRIMSON │                     │
│    │  CRAB   │          │  CRAB   │                     │
│    └────┬────┘          └────┬────┘                    │
│         │                    │                          │
│    ┌────┴────┐          ┌────┴────┐                     │
│    │  BLUE   │          │  RED    │                     │
│    │  BUFF   │          │ BUFF    │                     │
│    │(CDR/    │          │ (ASPD/  │                     │
│    │ MANA    │          │ HP REG) │                     │
│    └─────────┘          └─────────┘                    │
│                   ALLY JUNGLE                          │
└─────────────────────────────────────────────────────────┘
```

### 2.3.2 Jungle Monsters

#### A. Buff Monsters

| Monster | Location | Respawn | Effect | Duration |
|---------|----------|---------|--------|----------|
| **Crimson Golem** (Red Buff) | Corner Jungle | 180s | +15% Attack Speed, +5% Lifesteal | 90s |
| **Jade Golem** (Blue Buff) | Opposite Corner | 180s | +20% CDR, +5 Mana Regen/5s | 90s |

**Mekanik Buff:**
- Buff lasts 90 detik setelah pickup
- Jika holder meninggal, buff drop dan bisa di-pickup ulang (tidak transfer)
- Visual indicator pada karakter menunjukkan buff aktif

#### B. Small Jungle Camps

| Camp | Monsters | HP (Level 1) | Gold | EXP | Respawn |
|------|----------|--------------|------|-----|---------|
| **Wolf Pack** | 3 Wolves | 800 total | 60g | 80 XP | 60s |
| **Raptor Nest** | 4 Raptors | 600 total | 80g | 100 XP | 60s |
| **River Crab** | 2 Crabs | 500 total | 40g | 50 XP | 45s |

#### C. Jungle Objectives

| Objective | Location | Respawn | Reward | Strategic Value |
|-----------|----------|---------|--------|-----------------|
| **Kurala Wyrm** | River Center | 240s | 300g + "Wyrm's Blessing" buff | Push lane pressure |
| **Void Sentinel** | River Center | 300s | 400g + "Sentinel's Aegis" buff | Team-wide defensive buff |

---

## 2.4 Sistem Bos (Epic Boss)

### 2.4.1 Boss 1: KURALA WYRM

**Lore Background:**
> "Naga kuno yang menjaga persimpangan antara dimensi manusia dan supranatural. Konvergensinya membuatnya menjadi野兽 yang terfragmentasi."

**Specifications:**
- HP: 12,000 (scaling +5% per minute)
- Armor: 80 (Physical), 60 (Magic)
- Attack: 250 (Physical)
- Attack Speed: 0.8

**Attack Patterns:**

| Attack | Damage | Effect | Telegraph Time |
|--------|--------|--------|----------------|
| **Tail Sweep** | 400 Physical | Knockback 300 units | 0.8s |
| **Void Breath** | 600 Magic (AoE) | -40% MS for 2s | 1.2s |
| **Summon Adds** | — | 2 Lesser Wyrms spawn | 3s cast |

**Battle Mechanics:**
- Wyrm immune to CC selama 5 detik pertama
- Damage threshold: Wyrm deals 50% increased damage jika tim < 3 player
- Wyrm akan "enrage" jika battle > 45 detik (Attack +50%, Attack Speed +30%)

**Rewards (untuk tim yang membunuh):**

| Reward | Effect | Duration |
|--------|--------|----------|
| **Wyrm's Blessing** | +25% Minion Damage, +15% Tower Damage | Until respawn atau killed |
| **Bonus Gold** | 300g split evenly | Instant |
| **Lane Pressure** | Semua minion dalam 2000 unit mendapat +30% MS dan +20% DMG | 60s |

**Strategic Implications:**
- Wyrm spawn di menit 5 (respawn setiap 4 menit)
- Melayani tim yang sudah winning (snowball tool)
- High risk-high reward untuk behind team
- Vision control crucial untuk contest

---

### 2.4.2 Boss 2: VOID SENTINEL

**Lore Background:**
> "Entitas penjaga gerbang antara realitas dan void. Diciptakan oleh convergent energy dari Kurala Convergence Event."

**Specifications:**
- HP: 18,000 (scaling +5% per minute)
- Armor: 100 (Physical), 100 (Magic)
- Attack: 200 (Magic)
- Attack Speed: 0.6

**Attack Patterns:**

| Attack | Damage | Effect | Telegraph Time |
|--------|--------|--------|----------------|
| **Gravity Pull** | 300 Magic | Pull semua enemy dalam 600 unit ke center | 1.5s |
| **Void Zone** | 150 Magic/s | AoE 400 unit, damage per detik | Continuous |
| **Shield Bash** | 500 Magic | Stun 1.5s pada target terdekat | 0.5s |
| **Dimension Rift** | 800 Magic | 3 pilar muncul, explode setelah 3s | 2s |

**Battle Mechanics:**
- Sentinel memiliki phases:
  - **Phase 1 (100%-50% HP):** Normal attacks
  - **Phase 2 (50%-0% HP):** Enrage — semua attacks +30% damage, add Void Zones
- Sentinel immune to damage selama 3 detik saat phase transition
- Jika < 2 player dalam radius 800 unit, Sentinel regen 5% HP/detik

**Rewards (untuk tim yang membunuh):**

| Reward | Effect | Duration |
|--------|--------|----------|
| **Sentinel's Aegis** | +40 Armor, +40 Magic Resist untuk semua allies | Until respawn atau killed |
| **Bonus Gold** | 400g split evenly | Instant |
| **Team Shield** | shield 1000 HP pada semua allies (instant) | Shield breakable |
| **Void Advantage** | Enemy tidak bisa melihat ally dalam jungle selama 30s | 30s |

**Strategic Implications:**
- Sentinel spawn di menit 10 (respawn setiap 5 menit)
- Game-changer objective — tim yang winning bisa freeze game
- Cocok untuk teamfight sekitar objective
- Support tankiness sangat important

---

## 2.5 Map Visual Design

**Overall Aesthetic: Kurala Cyberpunk Nusantara**

| Element | Design | Color Palette |
|---------|--------|---------------|
| Ground/Terrain | Rusty metal plating dengan circuit patterns | Dark steel, copper accents |
| Lane | Glowing pathway dengan rune inscriptions | Cyan glow, orange highlight |
| Jungle | Overgrown vegetation dengan tech artifacts | Emerald, jade green |
| River | Semiconducting fluid dengan floating data particles | Electric blue, violet |
| Towers | Art Deco style dengan energy shields | Gold, white energy |
| Nexus | Ancient temple dengan futuristic overlay | Purple, magenta energy |
| Fog of War | Dark cyber-haze dengan glitch effects | Dark grey, static noise |

---

# BAGIAN 3: ARSITEKTUR JARINGAN (ONLINE)

## 3.1 Overview Arsitektur Multiplayer

NEXUS ARENA dirancang dengan fokus pada **fairness** dan **low-latency experience**. Arsitektur ini menggunakan kombinasi teknologi yang telah teruji di industri MOBA modern.

---

## 3.2 Sistem Tick Rate

### 3.2.1 Tick Rate Configuration

| Component | Tick Rate | Interval | Justification |
|-----------|-----------|----------|---------------|
| **Game State Sync** | 20 Hz | 50ms | Balance antara accuracy dan bandwidth |
| **Player Movement** | 60 Hz | 16.67ms | Smooth interpolation untuk movement |
| **Combat/Ability** | 30 Hz | 33.33ms | Hit registration accuracy |
| **Projectile Sync** | 45 Hz | 22.22ms | Fast projectile tracking |
| **UI/Status Update** | 10 Hz | 100ms | Low priority, bandwidth saving |

### 3.2.2 Client-Side Prediction

```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT PREDICTION FLOW                   │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  [Input] → [Predict Local] → [Send to Server]             │
│              ↓                          ↓                   │
│         [Render Frame] ← [Receive Server] ← [Game State]   │
│              ↓                          ↓                   │
│         [Apply Correction] ← [Reconcile]                    │
│              ↓                                              │
│         [Smooth Interpolation]                              │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Prediction Window:** 100ms (2 tick ahead)  
**Correction Threshold:** 50ms deviation trigger snap  
**Interpolation Buffer:** 5 frames (83ms)

---

## 3.3 State Synchronization

### 3.3.1 Authority Model

```
┌─────────────────────────────────────────────────────────────┐
│                    AUTHORITY HIERARCHY                      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  SERVER (Authoritative)                                     │
│  ├── Game State Master                                      │
│  ├── Damage Calculation                                     │
│  ├── Ability Validation                                     │
│  ├── Position Reconciliation                               │
│  └── Anti-Cheat Validation                                   │
│                                                             │
│  CLIENT (Predictive)                                        │
│  ├── Local Input Processing                                 │
│  ├── Visual Prediction                                       │
│  ├── Client-Side Effects (particles, sounds)                │
│  └── Lag Compensation                                         │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 3.3.2 Synchronization Protocol

**Data Compression:**
- Position data: Delta compression, 8 bytes per entity
- Rotation data: Quantized, 2 bytes per entity
- State changes: Event-based, variable length
- Overall bandwidth target: < 56 Kbps per player

**Update Types:**

| Update Type | Frequency | Content | Priority |
|-------------|-----------|---------|----------|
| **Critical** | Every tick | Position, HP, mana | Highest |
| **Combat** | On event | Damage dealt, ability used | High |
| **Economic** | On event | Gold, items purchased | Medium |
| **Buff/Debuff** | On change | Status effects | Medium |
| **Death/Respawn** | On event | Kill notifications | High |
| **Objective** | On event | Tower, Boss status | High |

---

## 3.4 Rollback Netcode Implementation

### 3.4.1 Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    ROLLBACK NETCODE FLOW                    │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  TICK 0           TICK 1           TICK 2                  │
│    │                │                │                     │
│    ▼                ▼                ▼                     │
│  [Input] ──────→ [Input] ──────→ [Input]                   │
│    │                │                │                     │
│    ▼                ▼                ▼                     │
│  [Simulate] ────→ [Simulate] ────→ [Simulate]              │
│    │                │                │                     │
│    │    ┌───────────┘                │                     │
│    │    │  (Rollback if              │                     │
│    │    │   mismatch)                │                     │
│    ▼    ▼                            ▼                     │
│  [Render] ←── [Render] ←────────── [Render]               │
│                                                             │
│  Input Buffer: 6 frames (100ms)                            │
│  Rollback Window: 4 frames (66ms)                          │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 3.4.2 Rollback Parameters

| Parameter | Value | Notes |
|-----------|-------|-------|
| **Input Buffer Size** | 6 frames (100ms) | Allows 100ms latency compensation |
| **Rollback Window** | 4 frames (66ms) | Maximum frames to roll back |
| **Desync Detection** | Every 500ms | Hash comparison check |
| **Reconciliation Rate** | On desync | Immediate resync |
| **Ghost Count** | 2 frames | Prediction ghosts for smooth display |

### 3.4.3 Combat Rollback Specifics

**Ability Hit Detection:**
- Server authoritative untuk damage application
- Client predict damage numbers untuk responsiveness
- Rollback triggers jika server reject hit

**Example Scenario:**
```
Client Frame: Cast Skill → Predict 500 damage → Render hit effect
Server Frame: Validate → Distance check fail → Reject hit
Result: Client rollback → Remove damage → Show "miss" animation
```

---

## 3.5 Lag Compensation System

### 3.5.1 Client-Side Lag Handling

| Technique | Implementation | Benefit |
|-----------|---------------|---------|
| **Prediction Smoothing** | Lerp between positions over 100ms | Hide network jitter |
| **Ability Anticipation** | Show "charging" indicator at client | Hide ability delay |
| **Projectile Guidance** | Predictive path display | Better dodge feedback |
| **Death Recap** | Show actual killer stats on death screen | Fairness transparency |

### 3.5.2 Server-Side Lag Compensation

**Hit Registration Compensation:**
- Ability hit dicek pada **cast time position** (bukan current)
- Compensation window: 150ms untuk fast projectiles, 300ms untuk slow
- Maximum compensation: 500ms

**Example:**
```
Player A casts skill at T=0 (100ms latency)
Skill projectile travels
Player B moves at T=100ms
Hit calculated at T=0 position (Player B's position 100ms ago)
Hit registered if within 150ms compensation window
```

---

## 3.6 Anti-Cheat System

### 3.6.1 Server-Side Validation

| Check | Frequency | Action on Fail |
|-------|-----------|-----------------|
| **Movement Speed** | Every 200ms | Kick + 24h ban |
| **Ability Cooldown** | Every cast | Reject + warning |
| **Damage Calculation** | Every damage event | Correct + log |
| **Vision Range** | Every tick | Hide from enemy |
| **Gold/EXP Sources** | Every income event | Adjust + flag |

### 3.6.2 Client Hardening

```
┌─────────────────────────────────────────────────────────────┐
│                    ANTI-CHEAT LAYERS                        │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  Layer 1: Input Validation                                   │
│  ├── Validate input format                                   │
│  ├── Check input frequency limits                           │
│  └── Sanitize input data                                    │
│                                                             │
│  Layer 2: Game Logic Isolation                              │
│  ├── Critical calculations server-side only                 │
│  ├── Client only stores non-critical state                  │
│  └── No sensitive data exposed to client                   │
│                                                             │
│  Layer 3: Runtime Monitoring                                 │
│  ├── Detect memory modification                             │
│  ├── Detect code injection                                  │
│  └── Detect speed hack                                      │
│                                                             │
│  Layer 4: Behavioral Analysis                               │
│  ├── Track aim patterns (anti-aimbot)                       │
│  ├── Monitor APM anomalies                                  │
│  └── Flag suspicious play patterns                          │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 3.6.3 Fair Play Indicators

Player akan melihat:
- **Latency indicator** untuk semua player (ping merah jika >150ms)
- **Connection quality** meter
- **Reconnect status** saat disconnected
- **Desync warnings** jika ada network issues

---

## 3.7 Network Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                         NEXUS ARENA NETWORK                        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   [Client A] ──┐                                                    │
│   [Client B] ──┼──→ [Regional Server] ──→ [Game Manager]            │
│   [Client C] ──┤         │                      │                   │
│   [Client D] ──┤         │                      ▼                   │
│   [Client E] ──┤         │              [Matchmaking Server]       │
│   [Client F] ──┼──→ [Regional Server] ──→ [Database Cluster]        │
│   [Client G] ──┤         │                      │                   │
│   [Client H] ──┤         │                      ▼                   │
│   [Client I] ──┤         │              [Anti-Cheat Server]         │
│   [Client J] ──┘         │                      │                   │
│                          │                      ▼                   │
│                   [CDN Edge]              [Analytics Server]         │
│                                                                     │
│   Regions: SEA (SG), ASIA (JP), NA (US-W), EU (DE)                 │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

# BAGIAN 4: EKONOMI & PROGRES DALAM GAME

## 4.1 Sistem Mata Uang

### 4.1.1 Gold (Currency Utama)

**Fungsi:** Pembelian item, upgrade item, consumeables

**Sumber Gold:**

| Source | Amount | Notes |
|--------|--------|-------|
| **Minion Kill (Melee)** | 60g per minion | Main gold income |
| **Minion Kill (Ranged)** | 45g per minion | Slightly less |
| **Minion Kill (Siege)** | 90g per minion | High value target |
| **Monster Kill (Small)** | 40-80g | Jungle camps |
| **Monster Kill (Buff)** | 100g | Buff monsters |
| **Hero Kill** | 300g base | + bounty system |
| **Hero Assist** | 150g | Split dengan killer |
| **Passive Income** | 50g/minute | CS-free gold |
| **Boss Kill** | 300-400g | Split evenly |

**Bounty System (Kill Reward):**
```
Base Kill Bounty: 300g
+ Kill Streak Bonus:
  - First blood: +100g
  - Double kill: +50g
  - Triple kill: +100g
  - Quadra kill: +200g
  - Penta kill: +400g

− Death Penalty:
  - Setiap death mengurangi bounty next kill
  - Minimum bounty: 150g
  - Resets setelah 60s tanpa kill
```

### 4.1.2 Karma Points (Currency Sekunder)

**Fungsi:** Purchases cosmetic, emblems, profile customization

**Sumber Karma:**
- Match completion: 50-200 Karma (berdasarkan performa)
- Daily missions: 100-300 Karma
- Achievement: 500-2000 Karma
- Ranked season reward: 1000-5000 Karma

---

## 4.2 Sistem Experience (EXP)

### 4.2.1 EXP Sources

| Source | Base EXP | Notes |
|--------|----------|-------|
| **Minion Kill (Melee)** | 65 XP | Main EXP source |
| **Minion Kill (Ranged)** | 50 XP | Slightly less |
| **Minion Kill (Siege)** | 100 XP | High value |
| **Jungle Monster (Small)** | 60-100 XP | Jungle farming |
| **Jungle Monster (Buff)** | 150 XP | Buff camps |
| **Hero Kill** | 500 XP | Flat bonus |
| **Hero Assist** | 250 XP | Split |
| **Boss Kill** | 600 XP | Split |

### 4.2.2 Level Curve

| Level | Cumulative EXP | HP Growth/Level | Mana Growth/Level |
|-------|----------------|-----------------|-------------------|
| 1 | 0 | — | — |
| 2 | 280 | +120 | +30 |
| 3 | 620 | +130 | +32 |
| 4 | 1020 | +135 | +34 |
| 5 | 1500 | +140 | +36 |
| 6 | 2080 | +145 | +38 |
| 7 | 2780 | +150 | +40 |
| 8 | 3620 | +155 | +42 |
| 9 | 4620 | +160 | +44 |
| 10 | 5800 | +170 | +46 |
| 11 | 7180 | +180 | +48 |
| 12 | 8780 | +190 | +50 |
| 13 | 10620 | +200 | +52 |
| 14 | 12720 | +210 | +54 |
| 15 | 15120 | +220 | +56 |

**Max Level: 15**

### 4.2.3 Level Advantage Mechanics

- Level lead memberikan +5% damage per level difference (max +20%)
- Level disadvantage memberikan -3% damage received per level (min -15%)
- Visual indicator pada HP bar untuk level difference

---

## 4.3 Sistem Item Shop

### 4.3.1 Item Categories

```
┌─────────────────────────────────────────────────────────────┐
│                    ITEM SHOP STRUCTURE                      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │   ATTACK    │  │  DEFENSE    │  │    MAGIC    │        │
│  │             │  │             │  │             │        │
│  │ • Physical  │  │ • Armor     │  │ • Magic     │        │
│  │   Damage    │  │ • HP        │  │   Power     │        │
│  │ • Attack    │  │ • Resist    │  │ • Mana      │        │
│  │   Speed     │  │ • Shield    │  │   Regen     │        │
│  │ • Crit      │  │ • HP Regen  │  │ • CDR       │        │
│  │ • Lifesteal │  │ • Debuff    │  │ • Penetr    │        │
│  │             │  │   Reduction │  │             │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │  SUPPORT    │  │   MOVEMENT  │  │   STARTER   │        │
│  │             │  │             │  │             │        │
│  │ • Utility   │  │ • Boots     │  │ • Starting  │        │
│  │ • Vision    │  │ • MS items  │  │   items     │        │
│  │ • CC        │  │ • Engage    │  │ • Early     │        │
│  │   Reduction │  │   Tools     │  │   power    │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 4.3.2 Item Tiers

| Tier | Cost Range | Power Level | Item Count |
|------|------------|-------------|------------|
| **Starter** | 300-500g | Low | 8 items |
| **Basic** | 500-1000g | Medium | 12 items |
| **Advanced** | 1000-1800g | Medium-High | 10 items |
| **Legendary** | 1800-2500g | High | 8 items |
| **Ultimate** | 2500g+ | Very High | 4 items |

### 4.3.3 Item Progression (Contoh Build)

**Marksman Standard Build:**

```
STAGE 1: EARLY GAME (0-10 menit)
─────────────────────────────────
Starter Items:
├── Storm Blade (500g) — +20 Physical ATK, +15% Attack Speed
└── Swift Boots (350g) — +50 Movement Speed

Total: 850g
Power: Early lane dominance, farming tool

STAGE 2: CORE BUILD (10-20 menit)
──────────────────────────────────
Core Items:
├── Berserker's Fury (1850g) — +65 Physical ATK, +25% Crit Chance
├── Raptor Claws (1700g) — +30% Attack Speed, +20% Crit Damage
└── Swift Boots → Combat Boots (upgrade +100g) — +60 MS, +15% AS

Total: 3700g
Power: Sustained damage, crit scaling

STAGE 3: LATE GAME (20+ menit)
──────────────────────────────
Luxury Items (pilih sesuai situasional):
├── Void Piercer (2200g) — +55 Physical ATK, +40% Armor Penetration
├── Bloodthirster (2100g) — +50 Physical ATK, +20% Lifesteal, +300 HP
├── Infinity Edge (2500g) — +80 Physical ATK, +25% Crit Chance, +50% Crit Damage
└── Guardian Armor (1900g) — +200 Armor, +500 HP, +10% CDR

Final Build Value: ~6000-8000g
Power: High DPS, sustain, survivability
```

### 4.3.4 Item Stats Summary

| Stat | Description | Item Type |
|------|-------------|------------|
| **Physical ATK** | Damage untuk basic attacks | Attack |
| **Magic Power** | Damage untuk abilities (Magic) | Magic |
| **Attack Speed** | Attacks per second | Attack |
| **Critical Chance** | % chance untuk critical hit | Attack |
| **Critical Damage** | Multiplier untuk crit damage | Attack |
| **Armor Penetration** | % reduction enemy armor | Attack |
| **Magic Penetration** | % reduction enemy MR | Magic |
| **Lifesteal** | % damage converted ke HP | Attack |
| **Spell Vamp** | % ability damage converted ke HP | Magic |
| **Armor** | Physical damage reduction | Defense |
| **Magic Resist** | Magic damage reduction | Defense |
| **HP** | Health points | Defense |
| **HP Regen** | HP recovered per 5s | Defense |
| **Mana** | Ability resource | Magic |
| **Mana Regen** | Mana recovered per 5s | Magic |
| **Cooldown Reduction (CDR)** | % reduction skill cooldown | Utility |
| **Movement Speed** | Unit movement per second | Utility |

---

## 4.4 Economic Balance Philosophy

### 4.4.1 Gold Income Curve

```
Gold/minute
    │
 350│                    ╱╲
    │                   ╱  ╲
 300│                  ╱    ╲
    │                 ╱      ╲
 250│                ╱        ╲
    │               ╱          ╲
 200│              ╱            ╲
    │             ╱              ╲
 150│            ╱                ╲
    │           ╱                  ╲
 100│          ╱                    ╲
    │         ╱                      ╲
   0│────────╱────────────────────────╲──────────
     0      5      10      15      20    Minutes
     
     Minion farming: ~200g/min
     Kill/Bounty: Variable spike
     Objective: 50-100g bonus
```

### 4.4.2 Economy Design Principles

1. **Farming Proficiency Reward:** CS yang baik = significant gold lead
2. **Kill Bounty Risk-Reward:** Dying = memberikan gold ke enemy
3. **Objective Value:** Boss kill = 400g+ advantage
4. **Comeback Mechanics:** Behind team bisa catch up via bounty system
5. **Item Power Curve:** Item expensive = exponential power spike

---

# BAGIAN 5: DESAIN KARAKTER (ROLE)

## 5.1 Role Overview

### 5.1.1 Role Classification Matrix

```
┌────────────────────────────────────────────────────────────────────┐
│                      ROLE ARCHETYPE MATRIX                         │
├────────────────────────────────────────────────────────────────────┤
│                                                                    │
│  ROLE        │ PRIMARY     │ SECONDARY    │ IDEAL                │
│              │ FUNCTION    │ FUNCTION     │ PARTNER              │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ TANK         │ Initiation  │ Frontline    │ Marksman, Mage       │
│              │ CC Focus    │ Peel         │                      │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ FIGHTER      │ Duelist     │ Split-push   │ Assassin, Support    │
│              │ Sustained   │ Objective    │                      │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ ASSASSIN     │ Burst DMG   │ Backline     │ Fighter, Mage        │
│              │ Pick-off    │ assassin     │                      │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ MAGE         │ Teamfight   │ Zone control │ Marksman, Support    │
│              │ Burst/AoE   │ Poke         │                      │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ MARKSMAN     │ Primary DPS │ Objective    │ Support, Tank         │
│              │ Sustained   │ damage       │                      │
├──────────────┼─────────────┼──────────────┼───────────────────────┤
│ SUPPORT      │ Protection  │ Vision/Util  │ Marksman, Fighter    │
│              │ Sustain     │ Engage       │                      │
└──────────────┴─────────────┴──────────────┴───────────────────────┘
```

---

## 5.2 Role Detail Specifications

### 5.2.1 TANK

**Primary Function:** Frontline protection, initiation, crowd control

**Stats Base (Level 1):**
- HP: 850
- Mana: 300
- Physical ATK: 55
- Magic Power: 0
- Armor: 45
- Magic Resist: 40
- Movement Speed: 340

**Playstyle:**
- Absorb damage untuk tim
- Initiate teamfight dengan CC
- Peel untuk carries
- Zone control dengan AoE abilities

**Typical Items:** HP/Armor items, CDR, utility items

---

### 5.2.2 FIGHTER

**Primary Function:** Extended fights, split-push, sustained damage

**Stats Base (Level 1):**
- HP: 750
- Mana: 350
- Physical ATK: 62
- Magic Power: 20
- Armor: 35
- Magic Resist: 35
- Movement Speed: 350

**Playstyle:**
- Hybrid damage (mix physical/magic)
- Lane dominance via extended trades
- Objective control (Tower, Boss)
- Duel capability

**Typical Items:** Hybrid damage, HP, CDR, split-push items

---

### 5.2.3 ASSASSIN

**Primary Function:** Burst damage, backline elimination, pick-off

**Stats Base (Level 1):**
- HP: 650
- Mana: 250
- Physical ATK: 68
- Magic Power: 0
- Armor: 30
- Magic Resist: 30
- Movement Speed: 360

**Playstyle:**
- High burst damage
- Mobility-based gameplay
- Target priority (carries)
- Stealth/gap-closer tools

**Typical Items:** Damage, CDR, mobility, penetration items

---

### 5.2.4 MAGE

**Primary Function:** Teamfight impact, AoE damage, zone control

**Stats Base (Level 1):**
- HP: 600
- Mana: 450
- Physical ATK: 45
- Magic Power: 70
- Armor: 25
- Magic Resist: 32
- Movement Speed: 340

**Playstyle:**
- Ability-based damage
- AoE/zone control
- Burst combo
- Backline positioning

**Typical Items:** Magic Power, CDR, Mana, Magic Penetration

---

### 5.2.5 MARKSMAN

**Primary Function:** Primary DPS, objective damage, sustained damage

**Stats Base (Level 1):**
- HP: 550
- Mana: 280
- Physical ATK: 65
- Magic Power: 0
- Armor: 28
- Magic Resist: 28
- Movement Speed: 345

**Playstyle:**
- Basic attack focused
- High sustained DPS
- Positioning-dependent
- Squishy but high damage output

**Typical Items:** Attack Speed, Crit, Physical ATK, Lifesteal

---

### 5.2.6 SUPPORT

**Primary Function:** Ally protection, vision control, utility

**Stats Base (Level 1):**
- HP: 650
- Mana: 400
- Physical ATK: 48
- Magic Power: 45
- Armor: 32
- Magic Resist: 32
- Movement Speed: 345

**Playstyle:**
- Utility over damage
- Sustain/heal allies
- Vision control (wards)
- Engage/peel tools

**Typical Items:** Support items, CDR, utility, team buffs

---

## 5.3 Contoh Skill Kit: KAEL — The Void Sentinel's Chosen

**Role:** Mage  
**Lore:** "Dulu seorang arkeolog yang mencari artifact kuno, Kael sekarang menjadi vesselsentience dari Void Sentinel. Dia menggunakan kekuatan antar-dimensi untuk membalas dendam pada mereka yang merampas tanahnya."

### 5.3.1 Stats Overview

| Stat | Base (Lvl 1) | Growth/Level | Max (Lvl 15) |
|------|--------------|--------------|--------------|
| HP | 600 | +85 | 1790 |
| Mana | 450 | +45 | 1080 |
| Physical ATK | 45 | +2.5 | 80 |
| Magic Power | 70 | +6 | 154 |
| Armor | 25 | +3 | 67 |
| Magic Resist | 32 | +2.5 | 64.5 |
| Movement Speed | 340 | — | 340 |
| Attack Speed | 0.625 | +2% | 0.85 |

---

### 5.3.2 Passive Skill: VOID RESONANCE

**Effect:**
- Setiap ability yang mengenai enemy memberikan stack "Void Echo" (max 3 stacks)
- Setiap stack memberikan +4% Magic Penetration
- Setelah 3 stack, next ability trigger "Void Discharge" — AoE 300 radius, 150 (+40% Magic Power) Magic Damage
- Void Discharge consumes semua stacks

**Visual:** Purple energy spiral around Kael, intensity increases dengan stack  
**Audio:** Rising hum yang meningkat dengan stack count

**Strategic Use:**
- Combo-oriented gameplay
- Stack management crucial
- Discharge burst sebagai finisher

---

### 5.3.3 Skill 1: DIMENSIONAL RIFT (Q)

**Type:** Line Skillshot  
**Range:** 900 units  
**Cooldown:** 12/11/10/9/8 seconds

**Description:**
Kael merobek fabric dimensi, mengirim energy bolt yang melewati terrain. Bolt berhenti di first enemy hit atau max range.

**Damage:** 200/280/360/440/520 (+70% Magic Power) Magic Damage

**Effect on Hit:**
- Jika enemy memiliki "Void Echo" stack, Rift deals +50% damage dan consume 1 stack
- Apply 30% slow untuk 1.5 detik

**Visual:** Purple crack di udara, energy bolt dengan trailing particles  
**Audio:** Reality tearing sound, impact whoosh

**Interaction with Passive:**
- Rift adalah primary stacking tool
- Max damage ketika enemy sudah memiliki 1-2 stack (bonus +50% per stack consumed)

**Skillshot Properties:**
- Width: 80 units
- Speed: 1200 units/second
- Pierces through minions
- Stops on first champion hit

---

### 5.3.4 Skill 2: PHASE WALK (W)

**Type:** Dash + AoE  
**Range:** 600 units dash, 300 units AoE  
**Cooldown:** 16/14/12/10/8 seconds

**Description:**
Kael mematerialisasi ke shadow realm, become untargetable, lalu muncul di target location dengan AoE energy burst.

**Phase Duration:** 0.6 detik (dalam shadow realm)

**Damage on Emergence:** 120/180/240/300/360 (+60% Magic Power) Magic Damage ke semua enemy dalam 300 radius

**Effect on Emergence:**
- Allies dalam 300 radius receive shield 80/120/160/200/240 (+30% Magic Power) untuk 2 detik
- Kael gain 20% movement speed untuk 1.5 detik setelah emerge

**Visual:** Kael dissolve menjadi shadow particles, purple energy trail, emerge dengan explosion  
**Audio:** Whisper sound during phase, boom on emerge

**Strategic Use:**
- Dodge tool (bisa dodge skillshots selama 0.6s)
- Gap closer untuk engage
- Emergency escape
- Team shield untuk sustain

**Edge Cases:**
- Jika Kael menggunakan Phase Walk saat di-CC, CC effect tetap (tidak bisa cleanse)
- Jika target location out of range, Kael dash ke maximum range
- Bisa phase through walls (tidak ada wall collision)

---

### 5.3.5 Skill 3: VOID STORM (E)

**Type:** AoE Zone (Ground Target)  
**Range:** 550 units  
**Cooldown:** 18/16/14/12/10 seconds  
**Duration:** 4 detik

**Description:**
Kael menciptakan void vortex di target location. Vortex deals damage per detik dan apply stacking slow.

**Damage:** 60/90/120/150/180 (+35% Magic Power) Magic Damage per detik

**Effect:**
- Enemy dalam zone receive stacking slow (max 40% after 2 stacks)
- Setiap tick consume "Void Echo" stack dari affected enemies (bonus 20% damage per stack consumed)
- Zone persists selama 4 detik

**Visual:** Swirling purple energy zone, particles spiral inward, ground distortion  
**Audio:** Constant hum, intensifying dengan stack

**Strategic Use:**
- Zone control / zoning tool
- Stacking zone with passive untuk burst
- Catch / chase tool (stacking slow)
- Teamfight disruption

**Zone Properties:**
- Radius: 250 units
- Enemy can walk out (zone does not trap)
- Vision granted di dalam zone
- Can be placed on objectives (Towers, Boss)

---

### 5.3.6 Ultimate: SENTINEL'S JUDGMENT (R)

**Type:** Global / Charge-up  
**Range:** Global (target within 800 units of Kael)  
**Cooldown:** 120/100/80 seconds

**Description:**
Kael channel kekuatan Void Sentinel, selected enemy champion marked untuk "Judgment". Setelah 2.5 detik channel, target receive massive damage dan affected oleh "Void Collapse" effect.

**Channel Duration:** 2.5 detik

**Effect During Channel:**
- Target revealed through fog of war
- Target receive +15% damage from all sources selama channel
- Kael revealed (visible) tapi tidak bisa di-target

**Damage on Execution:** 600/900/1200 (+120% Magic Power) Magic Damage

**Void Collapse Effect (setelah damage):**
- Semua "Void Echo" stacks on target explode, dealing 80 damage per stack ke nearby enemies (250 radius)
- Target stunned untuk 1.5/2/2.5 detik
- Kael gain "Sentinel's Favor" buff: +30% Magic Power, +20% CDR untuk 6 detik

**Counterplay:**
- Target bisa bergerak keluar dari range 800 units (channel cancel)
- Target bisa menggunakan Cleanse/Immunity untuk avoid
- Team bisa interrupt channel dengan hard CC pada Kael

**Visual:** Massive energy beam dari sky, target marked dengan void runes, explosion on execution  
**Audio:** Building tension during channel, explosive release on execution

**Strategic Use:**
- High-risk, high-reward ultimate
- Pick-off tool untuk isolated enemies
- Teamfight game-changer (AoE explosion, self-buff)
- Backdoor / split-push punishment

**Damage Calculation Example (Level 6, 300 Magic Power):**
```
Base Damage: 900
Magic Power Bonus: 300 * 1.2 = 360
Total Damage: 1260 Magic Damage

+ Void Collapse (jika 3 stack):
  Explosion: 80 * 3 = 240 Magic Damage (AoE)
  Stun: 2 detik
```

---

### 5.3.7 Skill Rotation & Combos

**Basic Combo (Lane Phase):**
```
Rift (Q) → Auto Attack → Void Storm (E) → Phase Walk (W)
         [Stack 1]     [Stack 2]    [Stack 3] → Passive Discharge
```

**Full Burst Combo (Teamfight):**
```
Void Storm (E) [Zone placed] →
Phase Walk (W) [Dodge forward] →
Rift (Q) [Hit stacked enemy, consume 1] →
Auto Attack →
Ultimate (R) [Execute] → [Auto triggers Passive]
```

**Trade Pattern (Early Game):**
```
Phase Walk (W) → Rift (Q) → Phase Walk (W) retreat
[Engage]         [Damage]    [Disengage]
```

---

### 5.3.8 Ability Scaling Summary

| Ability | Base Damage | Scaling | Cooldown | Mana Cost |
|---------|-------------|---------|----------|-----------|
| **Passive (Discharge)** | 150 + 40% AP | AoE | — | — |
| **Q: Dimensional Rift** | 200-520 | +70% AP | 8-12s | 60/70/80/90/100 |
| **W: Phase Walk** | 120-360 | +60% AP | 8-16s | 80/90/100/110/120 |
| **E: Void Storm** | 60-180/tick | +35% AP | 10-18s | 70/80/90/100/110 |
| **R: Sentinel's Judgment** | 600-1200 | +120% AP | 80-120s | 150/175/200 |

---

## 5.4 Character Design Principles

### 5.4.1 Ability Design Guidelines

1. **Identifiable Strength:** Setiap skill harus punya clear use case
2. **Counterplay Window:** Semua skill harus punya counterplay (dodge, shield, immunity)
3. **Resource Management:** Skills harus menggunakan mana secara meaningful
4. **Synergy System:** Skills harus bisa dikombinasikan (tapi tidak broken)
5. **Visual Clarity:** Effect harus readable dalam teamfight chaos

### 5.4.2 Balance Philosophy

- **No dominant strategy:** Setiap role harus viable
- **Counter-pick matters:** Hero selection memberikan advantage tapi tidak guarantee win
- **Skill expression:** High skill ceiling untuk competitive play
- **Accessibility:** Hero playable di semua level
- **Progression reward:** Mastering hero memberikan satisfaction

---

# BAGIAN 6: DOKUMENTASI TEKNIS

## 6.1 Tech Stack Recommendation

| Component | Technology | Notes |
|-----------|------------|-------|
| **Game Engine** | Unreal Engine 5 | Graphics, rendering |
| **Network** | Custom UDP + TCP hybrid | Low-latency + reliability |
| **Physics** | Chaos Physics Engine | UE5 built-in |
| **Audio** | Wwise | Spatial audio |
| **UI Framework** | Slate/UMG | UE5 native |
| **Matchmaking** | Custom microservice | Docker-based |
| **Database** | PostgreSQL + Redis | Player data, session |
| **Anti-Cheat** | Custom + Third-party | Kernel-level protection |

## 6.2 Performance Targets

| Metric | Target | Notes |
|--------|--------|-------|
| **Frame Rate** | 60 FPS minimum | 144 FPS recommended |
| **Network Latency** | < 50ms | Regional servers |
| **Tick Rate** | 20 Hz (game), 60 Hz (input) | As described above |
| **Load Time** | < 30s | Map loading |
| **Memory Usage** | < 4GB RAM | Target spec |

---

# BAGIAN 7: PROJECT TIMELINE (HIGH-LEVEL)

| Phase | Duration | Deliverables |
|-------|----------|---------------|
| **Pre-Production** | 3 bulan | GDD final, Tech prototype, Art direction |
| **Alpha** | 6 bulan | Core gameplay, 3 maps, 20 heroes, Network infrastructure |
| **Beta** | 4 bulan | Full hero roster, Balance iteration, Anti-cheat |
| **Soft Launch** | 3 bulan | Limited region, Feedback collection |
| **Full Launch** | 2 bulan | Global release, Esports integration |

**Total Estimated Timeline: 18 bulan**

---

# BAGIAN 8: OPEN QUESTIONS

1. **Platform Priority:** Apakah fokus PC dulu atau simultaneous PC+Mobile?
2. **Monetization Model:** Free-to-play dengan cosmetic-only shop? Atau Battle Pass?
3. **Esports Plan:** Apakah langsung ada competitive scene atau organic growth?
4. **Hero Release Cadence:** 1 hero/month atau 2 hero/month?
5. **Cross-platform:** Apakah support cross-play antara PC dan Mobile?
6. **Voice Acting:** Full voice acting atau text-based dialogue?

---

# APPENDIX: REFERENCE LINKS

- Dota 2 Gamepedia: https://dota2.fandom.com/
- League of Legends Wiki: https://leagueoflegends.fandom.com/
- MOBA Balance Patterns: Academic papers on game balance
- Unreal Engine 5 Documentation: https://docs.unrealengine.com/

---

**Document End**

*Prepared by: Senior Game Designer & Systems Architect*  
*For: NEXUS ARENA Development Team*