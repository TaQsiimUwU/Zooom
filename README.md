# **Project Zooom**

**Team Members:**
- Abdallah Kassem Hassan (320230213)
- Hesham Mahmoud Abdelsalam (320320118)
- Mohamed Tamer Mohamed (320230078)

---

## **Game Description**

**Genre:** 3D First-Person Speed-running Platformer

**Core Objective:** Navigate through handcrafted 3D levels from start to finish in the shortest time possible. Master advanced movement mechanics including wall-running, sliding, and grappling to achieve optimal times.

**Inspiration:** Fluid movement systems from *Titanfall 2*, parkour mechanics of *Mirror's Edge*, and time-attack gameplay of *Neon White*.

---

## **Implemented Features**

### **🎮 Movement System**
- **WASD Controls:** Fluid ground movement with physics-based acceleration
- **Wall Running:** Run along walls with automatic detection and camera tilt
  - Jump between walls for vertical mobility
  - Upward climbing with Left Shift
  - Downward sliding with Left Control
- **Sliding:** Crouch while moving to slide with momentum preservation
  - Enhanced speed on slopes (2.5x normal max speed)
  - Adjustable slide control for steering
- **Jumping:** Responsive jump mechanics with coyote time (0.2s grace period)
- **Grappling Hook:** Attach to designated surfaces and walls for swinging

### **📹 Camera System**
- **First-Person Perspective:** Immersive viewpoint for precision platforming
- **Mouse Look:** Smooth camera rotation with adjustable sensitivity
- **Dynamic Tilt:** Camera tilts during wall-running for enhanced feedback
- **Locked Cursor:** Automatic cursor lock during gameplay

### **🏃 Player Physics**
- **Ground Detection:** Reliable raycast-based ground checking
- **Slope Physics:** Dynamic friction system for realistic slope interaction
- **Counter-Movement:** Intelligent deceleration system
- **Max Speed Limits:** Context-aware speed caps (normal, sliding, wall-running)

### **🌍 Level Elements**
- **Static Platforms:** Core level geometry
- **Slippery Slopes:** Low-friction surfaces with downward force application
- **Moving Platforms:** Platforms that carry the player along their path
- **Death Zones:** Instant-kill triggers and fall detection
- **Goal Markers:** Level completion triggers

### **⏱️ Timer & Scoring**
- **Always-Visible Timer:** Real-time display in HUD (MM:SS.MS format)
- **Completion Time Tracking:** Records and displays final time
- **Survival Time Display:** Shows time survived on death

### **💀 Death System**
- **Fall Detection:** Automatic death when falling below threshold
- **Slow-Motion Effect:** Dramatic time slowdown (10% speed) on death
- **Visual Feedback:** White flash transitioning to black fade
- **Death Screen:** UI with survival time and restart options
- **Sound Effects:** Death and restart audio cues

### **🏆 Win System**
- **Goal Detection:** Automatic level completion on reaching goal
- **Win Screen:** Celebration UI with completion time
- **Level Progression:** Next level loading with delay
- **Sound Effects:** Victory and UI interaction sounds

### **🎵 Audio System**
- **Music Manager:** Persistent singleton across scenes
- **Dual Themes:** Separate menu and gameplay music tracks
- **Smooth Crossfade:** Seamless transitions between tracks (1s fade)
- **Toggle Control:** UI button to enable/disable music
- **Volume Control:** Adjustable music volume (0-100%)
- **Scene Detection:** Automatic track switching based on scene type

### **🎨 UI Elements**
- **Game HUD:** Real-time timer display
- **Win Screen:** Level completion panel with buttons
- **Lose Screen:** Death panel with restart options
- **Music Toggle:** On/off button with visual feedback
- **Crosshair:** Dynamic grappling hook targeting indicator

---

## **Controls**

| Action | Input |
|--------|-------|
| **Move Forward/Back** | W / S |
| **Strafe Left/Right** | A / D |
| **Jump** | Spacebar |
| **Crouch/Slide** | Left Ctrl (hold) |
| **Wall Run Up** | Left Shift (while wall-running) |
| **Wall Run Down** | Left Ctrl (while wall-running) |
| **Grappling Hook** | Mouse Button |
| **Camera Look** | Mouse Movement |

---

## **Technical Implementation**

### **Core Scripts**
- **PlayerMovement.cs:** Main player controller with physics-based movement
- **WallRunning.cs:** Wall detection and wall-running mechanics
- **FallDetector.cs:** Death trigger on falling
- **LoseScreen.cs:** Death screen management with effects
- **WinScreen.cs:** Victory screen and level progression
- **GoalTrigger.cs:** Level completion detection
- **LevelTimer.cs:** Singleton timer with HUD display
- **MusicManager.cs:** Persistent audio management
- **GrapplingGun.cs:** Grappling hook physics and detection

### **Platform Scripts**
- **Slope.cs:** Dynamic friction and downward force application
- **MovingPlatform.cs:** Platform movement with player carrying
- **DeathTrigger.cs:** Instant-kill zone implementation

### **Physics Features**
- Custom physics materials for low-friction surfaces
- Rigidbody-based player controller
- Raycast ground detection for reliability
- SpringJoint for grappling hook
- Force-based movement for responsive controls

---

## **Game Flow**

1. **Start:** Timer begins, music plays (gameplay theme)
2. **Gameplay:** Player navigates level using movement mechanics
3. **Death:** Fall below threshold → Slow-motion → Fade → Death screen
4. **Win:** Reach goal → Win screen → Display time → Next level option
5. **Restart:** Reload scene with sound effect
6. **Menu:** Return to menu → Music switches to menu theme

---

## **Used Technologies**

- **Engine:** Unity (2022.3+)
- **Language:** C#
- **Physics:** Unity Physics Engine with custom forces
- **UI:** Unity UI Toolkit & TextMeshPro
- **Audio:** Unity Audio System with AudioSource/AudioClip
- **Version Control:** Git

---

## **Future Enhancements**

- [ ] Leaderboards for fastest times
- [ ] Replay system to review runs
- [ ] Additional movement mechanics (double jump, dash)
- [ ] Multiple worlds with unique themes
- [ ] Custom level editor
- [ ] Multiplayer time trials
- [ ] Achievement system

---

## **Development Notes**

**Architecture Decisions:**
- Singleton pattern for global managers (Timer, Music)
- Component-based design for modular mechanics
- Scene-persistent managers using DontDestroyOnLoad
- Physics-based movement for natural feel

**Performance Optimizations:**
- Object pooling for frequently spawned effects
- Cached component references
- Efficient raycast usage
- Streaming audio for music

---

## **Credits**

Developed by Team Zooom as part of [Course/Project Name].

Special thanks to the Unity community and tutorial creators who inspired various mechanics.


