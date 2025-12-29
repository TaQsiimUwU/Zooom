# Project Report: ZoooM

## 1. Game Title & Team Members

**Game Title:** ZoooM

**Team Members:**
*   **Abdallah Kassem Hassan** (320230213)
*   **Hesham Mahmoud Abdelsalam** (320320118)
*   **Mohamed Tamer Mohamed** (320230078)

---

## 2. Tools & Frameworks Used

The development of **ZoooM** utilized a modern suite of game development tools and libraries to achieve high-performance gameplay and responsive controls.

*   **Game Engine:** Unity 6 (Editor Version: 6000.3.0f1)
*   **Language:** C#
*   **Render Pipeline:** Universal Render Pipeline (URP)
*   **Input System:** Unity Input System
*   **Level Design:** ProBuilder
*   **UI/Text:** TextMesh Pro
*   **IDE:** JetBrains Rider / Visual Studio
*   **Version Control:** Git

---

## 3. Game Overview & Story/Objective

**Genre:** 3D First-Person Speed-running Platformer

**Overview:**
ZoooM is a fast-paced first-person platformer that emphasizes momentum and precision. Drawing inspiration from titles like *Titanfall 2*, *Mirror's Edge*, and *Neon White*, the game challenges players to traverse complex 3D environments using an arsenal of advanced movement mechanics.

**Objective:**
The core objective is to navigate from the start point to the goal trigger of each handcrafted level in the shortest time possible. Players must chain together wall-runs, slides, jumps, and grappling hook swings to maintain momentum.

**Key Mechanics:**
*   **Fluid Movement:** Physics-based acceleration with ground detection.
*   **Wall Running:** Automatic detection of runnable walls with camera tilt feedback.
*   **Grappling Hook:** A physics-driven grapple system allowing players to swing across gaps.
*   **Sliding:** Momentum-preserving slide mechanic, particularly effective on slopes.

---

## 4. Graphics Techniques Used

The visual experience of ZoooM is built upon Unity's Universal Render Pipeline (URP).

### Lighting
*   **Real-time Lighting:** Utilizes URP's lighting engine for dynamic shadows and depth.
*   **Post-Processing:** Uses a Global Volume Profile for Bloom and Tone Mapping to enhance the sci-fi aesthetic.

### Camera Transformations
*   **Dynamic Tilt:** The camera tilts on the Z-axis during wall-running to provide visual feedback.
*   **Procedural Sway:** The grappling gun and camera react to player movement and physics forces.
*   **FOV Manipulation:** Field of View changes dynamically based on speed and movement state.

### Texture Mapping
*   **Prototyping Grids:** High-contrast grid textures are used for level geometry to aid in distance estimation.
*   **UV Mapping:** ProBuilder's automatic UV unwrapping ensures textures tile correctly across complex geometry.

### Shaders
*   **URP Lit:** Standard shaders for level geometry.
*   **TextMesh Pro SDF:** Distance Field shaders for crisp UI text rendering.
*   **Line Renderer:** Unlit shaders for the grappling hook rope visualization.

### Animations
*   **Procedural Animation:**
    *   **Camera:** Code-driven tilts and shakes.
    *   **Gun:** Rotates towards the grapple point dynamically.
    *   **Crouch:** Smooth interpolation of player scale and camera height.

---

## 5. Screenshots of the game

*(Placeholders for screenshots)*

*   **Main Menu:** Entry point with navigation.
*   **Gameplay:** First-person view showing the grappling gun and HUD.
*   **Wall Running:** Action shot showing the camera tilt.
*   **Win Screen:** Victory UI with completion time.

---

## 6. How to run the game

1.  **Prerequisites:** Windows PC, Keyboard, Mouse.
2.  **From Build:**
    *   Extract the build zip file.
    *   Run `ZoooM.exe`.
3.  **From Editor:**
    *   Open the project in Unity 6 (6000.3.0f1).
    *   Open `Assets/Scenes/Menu.unity` or `Assets/Scenes/MainLevel.unity`.
    *   Press Play.

---

## 7. Division of work among team members

**Abdallah Kassem Hassan**
*   **Level Design:** Constructed levels using ProBuilder, designing the flow and obstacle placement.
*   **UI Implementation:** Designed and implemented the HUD, Main Menu, Win Screen, and Lose Screen systems.

**Hesham Mahmoud Abdelsalam**
*   **Player Movement:** Implemented the core movement physics (`PlayerMovement.cs`), including wall running (`WallRunning.cs`) and sliding mechanics.
*   **Grappling Mechanics:** Developed the grappling hook system (`GrapplingGun.cs`) and rope physics.

**Mohamed Tamer Mohamed**
*   **Audio System:** Implemented the `MusicManager.cs` and integrated all Sound Effects (SFX) for movement, interactions, and UI feedback.
