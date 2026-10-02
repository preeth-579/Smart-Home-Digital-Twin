# 🏠 Smart Home Digital Twin

A cloud-connected **Smart Home Digital Twin** that combines **ESP32 sensor simulation in Wokwi**, **ThingSpeak cloud IoT**, and a **3D Unity environment**.

The system collects simulated smart-home sensor data, sends it to ThingSpeak through an ESP32, retrieves the latest cloud data in Unity, and uses that data to update a 3D digital representation of the house.

## 📌 Project Overview

This project demonstrates how an IoT-enabled smart home can be represented as a **Digital Twin**.

```text
WOKWI (ESP32 + Sensors)
          │
          │ HTTP / Wi-Fi
          ▼
      THINGSPEAK
       Cloud IoT
          │
          │ REST API
          ▼
        UNITY
     3D Digital Twin
```

## ✨ Features

- ESP32-based IoT simulation using Wokwi
- ThingSpeak cloud integration
- Unity 3D Smart Home Digital Twin
- Temperature-controlled fans
- LDR-controlled smart lighting
- Three smart lights controlled from the LDR
- Two fans controlled from temperature
- Directional light switching
- Ultrasonic-distance-controlled automatic door
- PIR-based motion/person visualization
- MQ-2 gas warning and alarm audio
- Soil-moisture monitoring / smart plant
- Four selectable camera viewpoints
- Unity sensor dashboard
- Live/stale ThingSpeak data detection

## 🧱 System Architecture

### 1. IoT Simulation — Wokwi

Wokwi simulates the ESP32 and sensors.

| Sensor | Purpose |
|---|---|
| DHT22 | Temperature and humidity |
| LDR / Photoresistor | Light level |
| PIR | Motion detection |
| HC-SR04 | Distance measurement |
| MQ-2 | Gas level |
| Soil Moisture Sensor | Soil moisture |

### 2. Cloud — ThingSpeak

Seven ThingSpeak fields are used:

| Field | Data | Unity Usage |
|---|---|---|
| Field 1 | Temperature | Fan control + UI |
| Field 2 | Humidity | UI |
| Field 3 | Distance | Door control + UI |
| Field 4 | Light | Light control + UI |
| Field 5 | Motion | Motion/person + UI |
| Field 6 | Gas | Gas alarm + UI |
| Field 7 | Soil Moisture | Smart plant + UI |

### 3. Unity — Digital Twin

`ThingSpeakManager` receives cloud data and passes it to individual controllers:

```text
ThingSpeakManager
├── FanController
├── LightController
├── DoorController
├── MotionController
├── GasAlarmController
└── SmartPlantController
```

## 🔌 Sensor-to-Digital-Twin Mapping

### 🌡️ Temperature → Two Fans

Current threshold:

```text
Temperature > 20°C  → Fans ON
Temperature ≤ 20°C  → Fans OFF
```

Both fan blade objects are controlled by the same `FanController`.

Fans rotate around the Y axis:

```csharp
fan.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
```

### 💡 LDR → Three Smart Lights

Current threshold:

```text
LDR < 30%  → Smart lights ON
LDR ≥ 30%  → Smart lights OFF
```

The Unity `LightController` can control all three room lights and their bulb emission.

When smart lights are ON:

```text
3 Smart Lights → ON
Bulb Emission  → ON
Directional Light → OFF
```

When smart lights are OFF:

```text
3 Smart Lights → OFF
Bulb Emission  → OFF
Directional Light → ON
```

### 🚪 HC-SR04 → Automatic Door

Current threshold:

```text
Distance < 50 cm  → Door OPEN
Distance ≥ 50 cm  → Door CLOSED
```

The Unity `DoorController` receives the distance through the ThingSpeak manager.

### 🚨 MQ-2 → Gas Alarm

Current threshold:

```text
Gas ≤ 60%  → Alarm OFF
Gas > 60%  → Alarm ON + Warning Audio
```

### 👤 PIR → Person Visualization

The PIR value is converted to a motion state:

```text
PIR >= 1 → Motion Detected
PIR < 1  → No Motion
```

The `MotionController` can show or hide a person GameObject in the house.

### 🌱 Soil Moisture → Smart Plant

Soil moisture is sent through Field 7 and passed to the smart-plant controller for visualization and monitoring.

## 🎥 Multiple Camera Viewpoints

Four fixed camera positions can be placed around the house:

```text
CameraPositions
├── CameraPosition1
├── CameraPosition2
├── CameraPosition3
└── CameraPosition4
```

The user can select:

```text
[ Camera 1 ] [ Camera 2 ]
[ Camera 3 ] [ Camera 4 ]
```

The Main Camera moves to the selected position and rotation.

## 🖥️ Recommended Unity Hierarchy

```text
SmartHome
│
├── House
│   ├── LivingRoom
│   │   ├── Light
│   │   └── Fan
│   ├── Bedroom
│   │   ├── Light
│   │   └── Fan
│   ├── Kitchen
│   │   └── Light
│   └── Entrance
│       └── Door
│
├── Directional Light
│
├── DigitalTwinManager
│   ├── ThingSpeakManager
│   ├── LightController
│   ├── FanController
│   ├── DoorController
│   ├── MotionController
│   ├── GasAlarmController
│   └── SmartPlantController
│
├── CameraManager
└── CameraPositions
    ├── CameraPosition1
    ├── CameraPosition2
    ├── CameraPosition3
    └── CameraPosition4
```

## 📁 Main Unity Scripts

### `ThingSpeakManager.cs`

Central cloud communication component.

Responsibilities:

- Requests ThingSpeak data
- Reads the latest feed
- Extracts seven fields
- Checks data freshness
- Updates controllers
- Updates UI
- Handles online/offline state

### `LightController.cs`

Responsibilities:

- Receives LDR value
- Controls three smart lights
- Controls bulb emission
- Turns Directional Light off when smart lights are on
- Supports keyboard testing

Keyboard testing:

```text
L → Lights ON
O → Lights OFF
```

### `FanController.cs`

Responsibilities:

- Receives temperature
- Controls two fans
- Rotates both fan blades
- Turns both fans ON/OFF

### `DoorController.cs`

Responsibilities:

- Receives ultrasonic distance
- Opens/closes the virtual door
- Uses the 50 cm threshold

### `MotionController.cs`

Responsibilities:

- Receives PIR motion state
- Shows/hides the person visualization

### `GasAlarmController.cs`

Responsibilities:

- Receives gas level
- Controls warning state
- Plays/stops alarm audio

### `SmartPlantController.cs`

Responsibilities:

- Receives soil-moisture data
- Updates the smart-plant visualization

### `CameraController.cs`

Responsibilities:

- Stores four camera positions
- Moves the Main Camera to the selected viewpoint

## ☁️ ThingSpeak Setup

Create a ThingSpeak channel with seven fields:

```text
Field 1 → Temperature
Field 2 → Humidity
Field 3 → Distance
Field 4 → Light
Field 5 → Motion
Field 6 → Gas
Field 7 → Soil Moisture
```

The ESP32 uploads the sensor values and Unity retrieves the latest data through the ThingSpeak REST API.

## 🔐 Configuration

Do **not** commit private credentials to GitHub.

Keep these values private:

```text
Wi-Fi SSID
Wi-Fi Password
ThingSpeak Channel ID
ThingSpeak Read API Key
ThingSpeak Write API Key
```

Use placeholders or local configuration files.

Example:

```text
CHANNEL_ID=YOUR_CHANNEL_ID
READ_API_KEY=YOUR_READ_API_KEY
WRITE_API_KEY=YOUR_WRITE_API_KEY
```

Recommended `.gitignore` entries:

```gitignore
.env
*.env
config.json
secrets.json

[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

.vscode/
.idea/
```

## ▶️ How to Run

### Step 1 — Start Wokwi

Open the ESP32 Wokwi project and start the simulation.

Verify:

- ESP32 is running
- Sensors are connected
- Wi-Fi is configured
- ThingSpeak credentials are configured

### Step 2 — Test Sensors

#### Fan

```text
25°C → Fans ON
15°C → Fans OFF
```

#### Lights

Change the LDR until the calculated light percentage crosses the configured threshold:

```text
LDR < 30% → Lights ON
LDR ≥ 30% → Lights OFF
```

#### Door

```text
30 cm → Door OPEN
100 cm → Door CLOSED
```

#### Gas

```text
50% → Safe
70% → Alarm
```

#### Motion

```text
Motion detected → Person visible
No motion → Person hidden
```

#### Soil

Change the soil-moisture value and observe the smart-plant response.

### Step 3 — Verify ThingSpeak

The ESP32 serial monitor should show successful uploads, for example:

```text
Sending data to ThingSpeak...
HTTP Response Code: 200
ThingSpeak update SUCCESS!
```

### Step 4 — Run Unity

1. Open the Unity project.
2. Load the main Smart Home scene.
3. Verify ThingSpeak settings.
4. Verify controller references.
5. Press **Play**.
6. Check the Unity Console.
7. Change Wokwi sensor values.
8. Wait for ThingSpeak to update.
9. Observe the 3D Digital Twin.

## 🔄 Example Data Flow

### Fan

```text
DHT22
  ↓
Temperature = 25°C
  ↓
ESP32
  ↓
ThingSpeak Field 1
  ↓
Unity ThingSpeakManager
  ↓
FanController.SetTemperature(25)
  ↓
Fan 1 ON + Fan 2 ON
  ↓
3D Fan Blades Rotate
```

### Lights

```text
LDR
  ↓
Low light
  ↓
ESP32
  ↓
ThingSpeak Field 4
  ↓
Unity ThingSpeakManager
  ↓
LightController
  ↓
3 Lights ON
Bulb Emission ON
Directional Light OFF
```

### Door

```text
HC-SR04
  ↓
Distance = 30 cm
  ↓
ESP32
  ↓
ThingSpeak Field 3
  ↓
Unity ThingSpeakManager
  ↓
DoorController.SetDistance(30)
  ↓
Door OPEN
```

### Gas

```text
MQ-2
  ↓
Gas = 70%
  ↓
ThingSpeak Field 6
  ↓
Unity ThingSpeakManager
  ↓
GasAlarmController
  ↓
Warning + Audio
```

## 🟢 Online / Offline Handling

Unity checks the timestamp of the latest ThingSpeak data.

```text
Fresh ThingSpeak data
        ↓
      ONLINE
        ↓
Update Digital Twin
```

If the latest data is too old:

```text
Old ThingSpeak data
        ↓
      OFFLINE
        ↓
Do not treat old values as current live data
```

This prevents the Digital Twin from appearing live when Wokwi/ESP32 has stopped uploading.

## 🧪 Threshold Summary

| System | Input | Threshold | Result |
|---|---|---:|---|
| Fan | Temperature | > 20°C | Both fans ON |
| Fan | Temperature | ≤ 20°C | Both fans OFF |
| Light | LDR | < 30% | Three lights ON |
| Light | LDR | ≥ 30% | Three lights OFF |
| Door | Distance | < 50 cm | Door OPEN |
| Door | Distance | ≥ 50 cm | Door CLOSED |
| Gas | Gas level | > 60% | Alarm ON |
| Gas | Gas level | ≤ 60% | Alarm OFF |
| Motion | PIR | ≥ 1 | Person detected |
| Motion | PIR | < 1 | No person detected |

## 🛠️ Technologies

- Unity
- C#
- Wokwi
- ESP32
- ThingSpeak
- HTTP / REST API
- DHT22
- LDR / Photoresistor
- PIR
- HC-SR04
- MQ-2
- Soil Moisture Sensor

## 📷 Screenshots

Add project screenshots to a `screenshots/` folder.

Suggested files:

```text
screenshots/
├── wokwi-circuit.png
├── thingspeak-dashboard.png
├── unity-smart-home.png
├── camera-views.png
└── gas-alarm.png
```

Then add them to this README, for example:

```markdown
![Wokwi Circuit](screenshots/wokwi-circuit.png)
![ThingSpeak Dashboard](screenshots/thingspeak-dashboard.png)
![Unity Smart Home](screenshots/unity-smart-home.png)
```

## 🎥 Demonstration Flow

A good project demonstration can follow this sequence:

```text
1. Start Wokwi
2. Show sensor values
3. Show ThingSpeak receiving data
4. Start Unity
5. Show Online status
6. Change temperature
7. Show both fans rotating
8. Change LDR
9. Show three lights and Directional Light changing
10. Change ultrasonic distance
11. Show door opening/closing
12. Change PIR
13. Show person visualization
14. Increase gas level
15. Show gas warning/audio
16. Change soil moisture
17. Show smart plant response
18. Demonstrate Camera 1–4
```

## 🛠️ Troubleshooting

### Unity does not receive data

Check:

- ThingSpeak Channel ID
- Read API Key
- Internet connection
- ThingSpeak channel data
- Unity Console
- `ThingSpeakManager`
- Data freshness/timeout settings

### Unity says OFFLINE

Check the timestamp of the latest ThingSpeak entry and make sure Wokwi is still uploading data.

### Fan rotates on the wrong axis

Use:

```csharp
fan.Rotate(
    Vector3.up,
    rotationSpeed * Time.deltaTime
);
```

If the direction is opposite to the desired direction, use `Vector3.down`.

Assign the **fan blade/pivot object**, not the entire fan assembly.

### Door does not open

Verify:

```text
HC-SR04
   ↓
ESP32 distance
   ↓
ThingSpeak Field 3
   ↓
ThingSpeakManager
   ↓
DoorController
```

Test:

```text
30 cm → OPEN
100 cm → CLOSED
```

### Lights do not turn ON

Check:

- LDR value sent by ESP32
- ThingSpeak Field 4
- LDR percentage conversion
- Light threshold
- Three Light references
- Three bulb Renderer references
- Directional Light reference

### Gas alarm does not play

Check:

- MQ-2 value
- ThingSpeak Field 6
- Gas threshold
- `GasAlarmController`
- AudioSource
- AudioClip
- Loop setting
- Unity audio volume

### Person does not appear

Check:

- PIR value
- ThingSpeak Field 5
- `MotionController`
- `personVisual` reference
- `showPersonWhenMotionDetected`

## 🚀 Future Improvements

Possible extensions:

- Individual control of each room light
- Individual fan control
- Mobile/web dashboard
- User authentication
- Historical sensor graphs
- Energy consumption monitoring
- Automatic irrigation
- Smart door access control
- More advanced Digital Twin animations
- Smooth camera transitions
- Additional rooms and appliances
- Database integration
- Alert notifications
- Real ESP32 hardware integration

## 🎓 Project Purpose

This project demonstrates the integration of:

**IoT + Cloud Computing + Simulation + 3D Visualization + Digital Twin**

The key concept is that sensor data does not stop at the cloud. Cloud data is used to update a virtual representation of the physical environment.

```text
Sensor Simulation
      ↓
    ESP32
      ↓
   ThingSpeak
      ↓
     Unity
      ↓
 Digital Twin
      ↓
Virtual Smart Home
```

## 📊 Project Status

- [x] ESP32 Wokwi simulation
- [x] DHT22 temperature/humidity
- [x] LDR light sensing
- [x] PIR motion sensing
- [x] HC-SR04 distance sensing
- [x] MQ-2 gas sensing
- [x] Soil moisture sensing
- [x] ThingSpeak cloud integration
- [x] Unity REST API integration
- [x] Sensor dashboard
- [x] Multi-light support
- [x] Multi-fan support
- [x] Automatic door
- [x] Motion/person visualization
- [x] Gas warning audio
- [x] Smart plant integration
- [x] Directional light switching
- [x] Multiple camera viewpoints
- [x] Live/stale data detection

## 📄 License

Add the license appropriate for your project.

For example:

```text
MIT License
```

For an academic project, use the license or terms required by your institution if applicable.

## 🙏 Acknowledgements

- Wokwi for ESP32 and IoT simulation
- ThingSpeak for cloud IoT data management
- Unity for 3D Digital Twin development
- ESP32 ecosystem for IoT development

---

## 📌 Quick Summary

```text
                         SMART HOME DIGITAL TWIN

Wokwi
  │
  │ Sensor Simulation
  ▼
ESP32
  │
  │ HTTP
  ▼
ThingSpeak
  │
  │ REST API
  ▼
Unity
  │
  ├── 🌡️ Temperature → 🌀 2 Fans
  ├── 💡 LDR → 💡 3 Lights
  │              └── ☀️ Directional Light
  ├── 📏 Distance → 🚪 Door
  ├── 👤 PIR → Person
  ├── 🚨 Gas → Alarm + Audio
  ├── 🌱 Soil → Smart Plant
  └── 🎥 4 Camera Views
```

**Smart Home Digital Twin — IoT sensor data transformed into a live 3D virtual environment.**
