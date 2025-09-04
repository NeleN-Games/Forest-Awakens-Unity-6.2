# 🧱 Unity Isometric Survival Game Prototype

A **solo prototype project** of a top-down **isometric survival game** made in **Unity**.  
The project demonstrates **gameplay programming**, **game architecture**, and **clean code practices**, without relying on advanced assets or networking.

---

## 🚀 Features (Work-in-Progress)

### 🎮 Core Gameplay
- 🧭 Smooth isometric player movement (camera-relative).  
- 🎥 Camera system with **zoom**, **rotation**, and **follow/free modes** (Cinemachine + CameraManager).  
- 🪓 Resource gathering with health-based depletion (wood, stone).  
- 🎒 Inventory system (optimized updates, tooltips, drag & drop).  
- 🏗️ Building placement with transparent preview and overlap checks.  
- 🪓 Tools & equipment system (pickaxe, equippables, consumables).  
- 🌗 Dynamic day/night lighting.  
- ❤️ Health, ⚡ Energy, and 🍗 Hunger stats with modifiers and timed effects.  
- 👾 Basic enemy AI (chase & patrol — in progress).  

### 🛠 Systems & Architecture
- 📦 Crafting system with categories, availability checks, and UI feedback.  
- 🔨 Building system with size handling & transparent placement previews.  
- ⚙️ Equipment manager with slot-based equipping and inventory sync.  
- 🎛️ Stat system (Health, Energy, Hunger) with modifiers and timed effects.  
- 🧩 Editor tools: Generic Database Editor Window (mesh/material/size support), Source/Item editors.  
- 🌱 **Future:** Random map generation with resources, tech tree progression, and NPC management.

### 🎨 Visuals
- Placeholder 3D models (logs, stones, tools, buildings).  
- Tilemap ground + mesh-based ground textures.  
- DOTween-based UI animations and interpolations.

---

## 🗺️ Roadmap

✅ Implement player movement & camera follow  
✅ Resource gathering & inventory basics  
✅ Crafting & building placement system  
✅ Stats (health, hunger, energy) with modifiers  
✅ Equipment system with equipping logic  

🔲 Enemy AI behaviors (combat, patrol, spawn points)  
🔲 Advanced multi-tile building & rotate/snap UX  
🔲 Save/Load system  
🔲 Sound & visual polish (VFX, SFX)  
🔲 Random map generation with resources & procedural placement  
🔲 Tech tree system & progression  
🔲 NPC control & management (spawn, pathing, interactions)

---

## 🧭 Vertical Slice — Scope & Plan

**Goal:** produce a short, fully playable gameplay loop that demonstrates the core pillars of the game: **move → gather → craft → build → survive**.  

### Scope (minimum for the vertical slice)
- Player movement (camera-relative isometric). ✅  
- Camera with zoom/rotation + follow/free modes. ✅  
- At least 2 resource node types (Wood, Stone) with step-based depletion. ✅  
- Hold-to-collect mechanic + visual feedback (resource health states). ✅  
- Inventory UI showing counts and icons (optimized refresh). ✅  
- Crafting UI allowing to craft at least one tool (Pickaxe). ✅  
- Building placement for at least one building type (transparent preview, overlap check). ✅  
- One basic enemy encounter (spawn → patrol → chase) — minimal combat loop. 🔲  
- HUD for Health / Energy / Hunger and stat modifier effects. ✅  
- One short showcase recording (GIF or video clip) of a full loop. 🔲  
- 🌱 **Future vertical slice upgrades:** procedural map generation, tech tree demo, and basic NPC interactions. 🔲  

> ✅ = implemented, 🔲 = pending/future  

### Assets to prepare (simple / reuse-friendly)
- Player model (low-poly / flat-color) or isometric sprite (single idle/walk).  
- Tool model: Pickaxe (already added). ✅  
- Resource models/prefabs: Log, Stone (already added). ✅  
- Ground tile atlas (128/256 px) + tilemap setup. ✅  
- Building placeholder meshes/prefabs (single-tile + multi-tile variants).  
- Basic UI icons (inventory, craft, build) — use colored 32px placeholders.  
- Minimal VFX: collect particle, place ghost effect.  
- Minimal SFX: collect, craft, place, hit, UI click.  
- Optional: short looping ambient music.  

### Systems / Code polish to finish
- Tuning stat tick rates & hunger/energy/health balance (StatManager / HealthSystem).  
- Ensure CraftingUI↔Crafter flow is reliable for both ItemCrafter and BuildingCrafter (events & subscription fixed). ✅  
- Improve BuildingPlacer UX: rotation keys, snapping, cancel/confirm, visual occlusion toggles. ✅  
- Enemy AI: spawn points and simple combat; ensure player damage and death flow hooked to HealthSystem. 🔲  
- Save/Load stub (serialize crafted & placed buildings + inventory) — optional for slice. 🔲  
- Polish tooltip raycast bugfixes and DOTween animations for UI (already improved). ✅  
- Final pass for null-checks / event unsubscriptions (prevent runtime errors). ✅  
- 🌱 **Future:** Random map generation, tech tree progression, and NPC management modules. 🔲  

### Milestones / Checklist
- [x] Core movement + camera (Cinemachine & CameraManager integrated)  
- [x] Tilemap ground + resource prefabs (wood, stone)  
- [x] Collecting system with SourceHealth & visual state transitions  
- [x] Inventory UI (optimized refresh; DOTween count animation)  
- [x] Crafting UI + CraftManager + craft lookup registration  
- [x] BuildingPlacer with size support and overlap checks  
- [x] EquipmentManager & equip-slot drag/drop  
- [ ] Enemy spawn & encounter integrated into gameplay loop  
- [ ] One short GIF/video showing the full loop for portfolio  
- [ ] 🌱 Random map generation + tech tree demo  
- [ ] 🌱 NPC control and interactions  

---

## 🎯 Objective

Survive as long as possible by **gathering resources**, **crafting tools/buildings**, **managing your stats**, and **avoiding threats**.  
Future version will also include **exploration of random maps, tech progression, and NPC management**.  

---

## 🛠️ Tools & Tech

- **Unity** 2022 or newer  
- **C#**  
- **Cinemachine** (camera)  
- **DOTween** (UI & animation)  
- **Input System** (new Unity Input System)  
- **Git** for version control  

---

## 📜 License

This project is open-source under the [MIT License](LICENSE).  

---

⚠️ This project is part of my personal portfolio and is shared for review purposes only.  
❌ Please do not reuse, fork, or redistribute any part of this code without explicit permission.  
📫 Feel free to contact me if you'd like to collaborate or learn more.  

---

*Thank you for checking out my project!*
