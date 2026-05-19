# Sprite Flight - 2D Cross-Platform Space Arcade

**Sprite Flight** is an intense, space-themed 2D survival arcade game built using the modern **Unity 6** engine and the **UI Toolkit** framework. Navigating through a volatile asteroid field, players must test their reflexes against progressively accelerating hazards. 

This project expands upon the official Unity tutorial by implementing a fully integrated single-scene game loop, customized physics power-ups, and a robust health system.

---

## Play Online
**[Play Sprite Flight on Unity Play](https://play.unity.com/en/games/8289d06b-15cd-46db-8bf2-1faa4baad028/sprite-flight)**

---

## Gameplay & Controls

You control a rocket ship caught in a dense asteroid belt. Survival time increases your score. As time goes on, obstacles bounce off screen bounds with increasing physics-driven speed, demanding sharp navigation.

### Controls:
* **PC (Mouse):** Hold Left Mouse Button and move the cursor to steer the ship and engage the thrusters.
* **Mobile (Touch):** Tap and drag anywhere on the screen to rotate and apply continuous thrust.

---

## Custom Improvements & Features

This build includes several custom mechanics beyond the baseline Unity tutorial:

### 1. Unified Main Menu & Game Loop
* Migrated from a multi-scene structure to a smooth, single-scene architecture (`Game`).
* Implemented **Time.timeScale** manipulation to freeze background physics while the Main Menu is active, providing a seamless visual transition.

### 2. Upgraded Life & Recovery System
* Replaced instant death with a **3-Lives System** paired with visual UI heart tracking.
* Programmed flashing **Invincibility Frames (I-Frames)** via coroutines, granting a 2-second recovery window upon taking damage.

### 3. Dynamic Shield Power-Up Mechanics
* Created a procedural `PowerUpSpawner` that drops energy shield nodes at randomized time intervals and coordinates.
* Collecting a shield grants a 7-second immunity phase, turning the player into a battering ram that resets obstacle velocities and physically deflects them away with reverse `AddForce` and heavy angular torque.

### 4. Smart Game-Over Flow
* Features automated high-score persistence locally using `PlayerPrefs`.
* On Game Over, screen border colliders are dynamically disabled, allowing remaining obstacles to fly off-screen cleanly without cluttering the player's defeat screen.

---

## Tech Stack & Architecture

* **Engine:** Unity 6 (6000.4.0f1)
* **UI Framework:** UI Toolkit (Responsive UXML layouts for Main Menu, In-Game HUD, and Game Over overlays).
* **Input Architecture:** New Input System using universal Pointer Press/Position actions for native PC and mobile touch compatibility.
* **Physics:** 2D Rigidbody manipulation, Custom 2D Physics Materials (Bounciness > 1.0), and rigorous custom velocity clamping.
* **VFX & Audio:** 2D Box Particle System (ambient starfield), conditional audio looping for thrusters, and standalone explosion effects.

---

## Local Setup & Installation

1. Clone this repository to your local machine:
   ```bash
   git clone [https://github.com/Isjakuza/2D-project-Sprite-Flight.git](https://github.com/Isjakuza/2D-project-Sprite-Flight.git)