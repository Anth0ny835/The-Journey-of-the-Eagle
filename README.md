# The Journey of the Eagle 🦅

**University academic project · Unity 6 · C# · Android augmented reality**

The Journey of the Eagle is a university augmented reality (AR) game that uses image tracking to select preset eagle appearances. Players scan a supported eagle image using an Android device, confirm the selected appearance, and control an eagle in a jungle environment featuring animal targets, attacks, scoring, and respawning.

**Development contribution:** I independently programmed and implemented the Unity application for a university assignment. The game uses third-party Unity assets and packages for art and other supporting functionality; those materials are not claimed as my original creations.

## Screenshots

| Main menu | AR image scanning |
| --- | --- |
| ![Main menu](docs/screenshots/main%20menu.png) | ![AR scanning](docs/screenshots/AR%20cam%20scan.png) |
| **Color confirmation** | **Main game / combat** |
| ![Color confirmation](docs/screenshots/color%20confirmation.png) | ![Gameplay combat](docs/screenshots/main%20game%20combat%20scene.png) |

## Main features

- **AR image tracking:** Recognizes predefined eagle images using AR Foundation to select corresponding eagle variants (rather than recoloring an arbitrary drawing in real time).
- **Appearance confirmation:** Preview and confirm a selected appearance or return to scan again.
- **Eagle movement and camera:** On-screen joystick controls and camera tracking.
- **Combat:** Attack animal targets, reduce health, and increase the kill count when targets are defeated.
- **Respawning:** Defeated targets can respawn following a delay.
- **Scene transitions:** Main Menu → AR Camera → Color Confirmation → Main Game.

## Technology

| Component | Project version |
| --- | --- |
| Game engine | Unity 6000.0.27f1 (Unity 6) |
| Programming language | C# |
| Target platform | Android |
| AR Foundation | 6.0.4 |
| Google ARCore XR Plugin | 6.0.4 |
| XR Interaction Toolkit | 3.0.6 |
| Universal Render Pipeline | 17.0.3 |

Versions are from the original Unity project's configuration, not a fresh build verification.

## Gameplay steps

1. Tap **PLAY** from the main menu.
2. Point the rear camera at a supported eagle picture to select a matching appearance.
3. Choose **YES** to continue or **NO** to scan again.
4. Move using the joystick and use the attack button against animal targets.

## AR scanning pictures

The provided images are in [`scanning-images/`](scanning-images/):
[`eagleBlue.jpg`](scanning-images/eagleBlue.jpg), [`eagleRed.jpg`](scanning-images/eagleRed.jpg), [`eagleRGB.jpg`](scanning-images/eagleRGB.jpg), and [`EagleImage.png`](scanning-images/EagleImage.png).

To try scanning, show or print one of the supported target images and aim the phone camera at the full image in good lighting. The project selects from predefined eagle variants.

## Running the Android build

The Android APK is available from the [v1.0.0 GitHub Release](https://github.com/Anth0ny835/The-Journey-of-the-Eagle/releases/tag/v1.0.0).

### Installation

1. Download the APK from the release assets.
2. Install it on a compatible ARCore-supported Android device.
3. Allow camera access when prompted.
4. Download or print the eagle images from the [`scanning-images/`](scanning-images/) folder.
5. Launch the game, scan a supported eagle image, confirm the selected appearance, and begin playing.

**Note:** This APK was developed for a university assignment. Compatibility with all Android devices has not been verified.

## Source code

[`Scripts/`](Scripts/) contains **13 unmodified C# gameplay scripts from my university implementation**, organized by their original Unity script folder names:

- `AR cam/` — AR image handling and eagle appearance selection
- `MainGame/` — movement, combat, camera, scoring, and respawning
- `MainMenu/` — menu navigation and eagle animation behavior

**Important:** This public repository is a **source-code and documentation showcase**, **not a complete Unity project**. It deliberately omits Unity Asset Store raw assets, scenes, prefabs, and other dependencies. It cannot be opened or rebuilt as a standalone project simply by cloning it. The complete Unity source remains in the privately retained university project.

## User manual

Read the [user manual](docs/User%20Manual%20for%20The%20Journey%20of%20the%20Eagle.pdf) for the gameplay flow and controls.

## APK and demo

The first release of **The Journey of the Eagle** is available on GitHub.

| Download | Description |
|---|---|
| [Download Android APK](https://github.com/Anth0ny835/The-Journey-of-the-Eagle/releases/download/v1.0.0/The.Jouney.of.the.Eagle.apk) | Installable Android game |
| [Download Gameplay Demo](https://github.com/Anth0ny835/The-Journey-of-the-Eagle/releases/download/v1.0.0/Game.Demo.mp4) | Recorded demonstration of gameplay |
| [View v1.0.0 Release](https://github.com/Anth0ny835/The-Journey-of-the-Eagle/releases/tag/v1.0.0) | Release notes and downloadable files |

The APK and gameplay demonstration were produced as part of the university assignment. The public repository contains original gameplay scripts and documentation, while third-party Unity Asset Store source assets are excluded.

## Third-party content

The original game incorporates assets obtained from the Unity Asset Store. The public repository intentionally does not include raw Asset Store packages, character models, textures, prefabs, or third-party scripts. Ownership and licence rights remain with their respective creators. This repository does not offer a blanket open-source licence over third-party game content.

## Project status

Completed university assignment; public documentation and original gameplay code preserved without refactoring. No fresh build or Android device validation was performed during repository preparation.
