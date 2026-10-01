# Ministry of Hajj and Umrah — Digital Twin & 3D Interactive Simulation

Interactive 3D simulation and Digital Twin environment built with **Unity 6 (6000.4.6f1)** utilizing the **High Definition Render Pipeline (HDRP 17.4.0)**.

---

## 🌟 Overview

This project provides a realistic, high-fidelity digital twin of the Ministry of Hajj and Umrah facilities and operational environments. It leverages cutting-edge real-time graphics, physically-based atmospheric lighting, and high-performance glTF/GLB runtime streaming to visualize architectural spaces, movement dynamics, and environmental conditions.

---

## 🚀 Key Features

- **Digital Twin 3D Visualization**: High-resolution 3D digital twin models (`DigitalTwinFinal.glb`) integrated with Unity's glTFast package for memory-efficient loading and rendering.
- **Physically-Based Lighting & Atmosphere**:
  - Custom HDRP Volume profiles (`MinistrySkyProfile`, `SunSkyFog_VolumeProfile`, `SkyandFogSettingsProfile`).
  - Automated editor tooling (`SunSkyFogPrefabCreator.cs`) for generating physically-accurate Sun (Directional Light), Sky, and Volumetric Fog presets.
- **Animated Character Dynamics**: Interactive 3D avatar (`business_man(1).glb`) with configured state-machine animator controller (`BusinessManWalk.controller`).
- **High Definition Render Pipeline (HDRP)**: Pre-configured quality tiers (`Balanced`, `Performant`, `High Fidelity`) tailored for desktop and workstation visual fidelity.
- **Modern Input Architecture**: Configured with Unity's new Input System (`com.unity.inputsystem`).

---

## 📁 Repository Structure

```text
├── Assets/
│   ├── Prefabs/             # Reusable HDRP Sun, Sky, and Fog prefabs
│   ├── Scripts/Editor/      # SunSkyFogPrefabCreator editor utility
│   ├── Settings/            # HDRP configurations & Ministry Volume profiles
│   ├── TutorialInfo/        # Project walkthrough assets
│   ├── BusinessManWalk.*    # Character animator controller
│   ├── business_man(1).glb  # 3D Avatar asset (Git LFS)
│   ├── DigitalTwinFinal.glb # Full Digital Twin 3D Model (Git LFS)
│   └── OutdoorsScene.unity  # Primary HDRP interactive scene
├── Packages/                # Unity Package Manager manifest & lock files
├── ProjectSettings/         # Engine & pipeline settings (HDRP, Quality, Input)
├── .gitattributes           # Git LFS tracking rules
└── .gitignore               # Unity-tailored ignore rules
```

---

## ⚙️ Requirements & Getting Started

### Prerequisites

1. **Unity Hub & Unity Editor**:
   - Unity Version: `6000.4.6f1` (or compatible Unity 6 release)
   - Modules: High Definition Render Pipeline (HDRP) support
2. **Git & Git LFS**:
   - Install [Git Large File Storage (LFS)](https://git-lfs.github.com/) to pull large 3D models (`DigitalTwinFinal.glb`).

### Cloning the Project

```bash
# Clone the repository
git clone https://github.com/MohamedMamdouhElmetwaly/MilitrayOfHajjandUmrah.git

# Navigate into the project folder
cd MilitrayOfHajjandUmrah

# Pull large assets via Git LFS
git lfs install
git lfs pull
```

### Opening in Unity

1. Open **Unity Hub**.
2. Click **Add** -> **Add project from disk**.
3. Select the cloned folder.
4. Launch with **Unity 6000.4.6f1**.
5. In the Project window, open `Assets/OutdoorsScene.unity`.

---

## 📄 License

Internal / Proprietary - Ministry of Hajj and Umrah Simulation Project.
