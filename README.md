# 🚚 MBGRush

MBGRush adalah game 2D platformer yang dibuat menggunakan **Unity 6** sebagai tugas kelompok. Pemain mengendalikan mobil pengangkut bantuan gizi untuk mengumpulkan seluruh koin, menghindari rintangan, dan mencapai garis finish.

---

## 👥 Tim Pengembang

- Iqbal Maulana
- Muhammad Nur Syafii
- Rizal Anggoro

---

# 🛠️ Teknologi

- Unity 6 (6000.5.1f1)
- C#
- Git
- GitHub

---

# 📦 Cara Menjalankan Project

## 1. Install Software

Pastikan sudah menginstall:

- Unity Hub
- Unity Editor **6000.5.1f1**
- Git atau GitHub Desktop

---

## 2. Clone Repository

```bash
git clone https://github.com/iqblmlnf/MBGRush.git
```

Masuk ke folder project

```bash
cd MBGRush
```

---

## 3. Buka Project

- Buka **Unity Hub**
- Klik **Add Project**
- Pilih folder **MBGRush**
- Tunggu Unity melakukan import asset.

---

## 4. Jalankan Game

Buka Scene utama (Menu Utama) terlebih dahulu:

```
Assets/Scenes/MainMenu.unity
```

Lalu tekan tombol **Play** pada Unity.

---

# 🎮 Kontrol

| Tombol | Fungsi |
|---------|---------|
| A / ← | Bergerak mundur / ke kiri |
| D / → | Bergerak maju / ke kanan |

### 🛠️ Fitur Pengujian (Developer Shortcuts)
Saat menjalankan game di dalam Unity Editor, Anda dapat menggunakan tombol pintas keyboard berikut untuk mempermudah pengujian panel UI:
*   Tekan **`K`** ➡️ Memicu kondisi **Game Over** secara instan.
*   Tekan **`L`** ➡️ Memicu kondisi **Misi Selesai / Victory** secara instan.

---

# 🎯 Gameplay

- Kumpulkan seluruh koin.
- Hindari batu.
- Jika menabrak batu, level akan diulang.
- Semua koin wajib diambil sebelum finish.
- Setelah seluruh koin terkumpul, pemain dapat menyelesaikan level.

---

# 📁 Struktur Project

```
Assets/
│
├── Animations/
├── Audio/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Camera/
│   ├── Collectible/
│   ├── Managers/
│   ├── Obstacle/
│   ├── Player/
│   └── UI/
│
├── Sprites/
└── Materials/
```

---

# 👨‍💻 Cara Berkontribusi

## Pertama kali

Clone repository

```bash
git clone https://github.com/iqblmlnf/MBGRush.git
```

---

## Sebelum mulai mengerjakan

Selalu update project terlebih dahulu.

```bash
git pull
```

---

## Setelah selesai mengerjakan

Tambahkan perubahan

```bash
git add .
```

Commit

```bash
git commit -m "Menambahkan fitur ..."
```

Push

```bash
git push
```

---

# ⚠️ Penting

JANGAN menghapus atau mengubah file berikut tanpa koordinasi:

- Packages/
- ProjectSettings/
- .gitignore

---

JANGAN upload folder berikut:

- Library/
- Temp/
- Logs/
- UserSettings/

Folder tersebut dibuat otomatis oleh Unity.

---

# 📌 Aturan Kerja Kelompok

Sebelum mulai coding:

```bash
git pull
```

Sesudah selesai coding:

```bash
git add .
git commit -m "Deskripsi perubahan"
git push
```

---

# 📄 License

Project ini dibuat untuk keperluan akademik Universitas Amikom Yogyakarta.
