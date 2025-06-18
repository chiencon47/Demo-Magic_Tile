## 🚀 How to Run the Project

1. Open Unity Hub.  
2. Click "Add project" → navigate to the project folder.  
3. Go to `Assets/Scenes` and open `SampleScene`.  
4. Press `Play` to start experiencing the gameplay.

---

## 🕹️ Core Gameplay

- **Notes fall in sync with music**: Tiles are generated and fall according to the music timing.
- **Player interaction**: Tap/click at the right time to score points.
- **Scoring based on timing accuracy**:
  - 🟢 Perfect (±0.2s): +3 points  
  - 🟡 Great (±0.4s): +2 points  
  - 🔴 Good: +1 point
- **Game Over**: Occurs when any tile is missed and hits the bottom of the screen.

---

## 🌟 Additional Features Implemented

- ✅ **Dynamic background** that reacts to music rhythm.  
- ✅ **Visual effects** on note hits (scale, fade using DOTween).  
- ✅ **Scene reload** with a replay button.  
- ✅ **TouchManager** supports multi-touch and mouse input (for Editor testing).  
- ✅ **Note auto-scaling** for different screen resolutions.

---

## 🧠 Code Structure & Design Explanation

- `NoteGenerator`: Manages note spawning based on music timing and ensures no overlapping lanes.  
- `Note`, `ShortNote`, `LongNote`: Separated note types with specific interaction handling.  
- `GameManager`: Handles game states, music timing, and scoring.  
- `TouchManager`: Handles multi-touch input and mouse (using touch IDs).  
- `EffectManager`: Triggers visual feedback for Perfect/Great/Good hits.  
- `Object Pooling`: Used for spawning note objects and visual effects efficiently.

I used **DOTween** for smooth and optimized visual effects instead of coroutines.

---

## 🧾 Resources

- **DOTween** – Tween engine:  
  https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676  
- **Assets**: Provided in the test instructions.
- **Some visuals and effects** were custom-designed and created by me.

