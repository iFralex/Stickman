# Stickman
This Unity-based project, Stickman, was initially created in 2021 for a Fiverr client who sought an engaging online multiplayer game. Stickman is a two-player game where each player controls a stickman character competing to reach the center of the screen first. This technical overview delves into the critical aspects and components of the project.

# Technical Overview:

## 1. Unity Engine:
The game is developed using the Unity game engine, providing a robust platform for crafting interactive 3D experiences. It was primarily designed and tested with Unity version 2021.3.4f1.

**Please note**: Plugin folders such as PUN are not included in the repository. Download PUN2 from the [Unity Asset Store](https://assetstore.unity.com) and integrate it into the project.

## 2. Gameplay:
<img width="1072" alt="Gameplay" src="https://github.com/iFralex/Stickman/assets/61825057/940c0e5e-1ad8-4b3c-ad75-49422b9b3b53">

- **Character Animation:** Stickman characters execute a simple jumping animation when they move. These animations are key to conveying player actions.
- **Objective:** Players strive to get their stickman to the center of the screen before their opponent, making gameplay competitive and engaging.
<img width="1072" alt="Won game" src="https://github.com/iFralex/Stickman/assets/61825057/8a41788f-3f3d-4969-b621-e3066cb20a4e">

## 3. Multiplayer Integration with PUN2:
To support real-time online multiplayer functionality, the project effectively utilizes the Photon Unity Networking 2 (PUN2) framework. PUN2 facilitates seamless connections between players, ensuring they can interact and compete in real-time.

<img width="1072" alt="Wait a player" src="https://github.com/iFralex/Stickman/assets/61825057/89927fa3-7e17-43f3-a605-75eff9ea74f3">

## 4. User Interface:
- **Usernames:** Players can set their usernames, which are displayed during online matches.
<img width="1072" alt="Game menu" src="https://github.com/iFralex/Stickman/assets/61825057/ebd5206b-d9f8-4c53-9805-fefa032517b4">

- **Matchmaking:** The game offers options to search for random opponents, create custom rooms for friends, or join personalized rooms for multiplayer action.
<img width="1072" alt="Create private custom room" src="https://github.com/iFralex/Stickman/assets/61825057/49377d24-dbd9-41c0-b521-9aa005501fe6">
<img width="1072" alt="Enter in private custom room" src="https://github.com/iFralex/Stickman/assets/61825057/92d64982-fe6a-4dd2-a09e-7fbffc442fa7">

# C# Scripts
A Unity project has many files in addition to scripts that are usually not managed by the developer. If **you are only interested in seeing my work**, namely the C# scripts I wrote, go directly to the [assets folder](https://github.com/iFralex/Rompecabezas/tree/main/Assets) and open the C# files.

Unfortunately, the project was not developed with the intention of publication, so the file names, variables, and functions are in Italian, and no scripts are commented or written to facilitate reading by others.

Keep in mind that I programmed this video game when I was 15 years old, so you can imagine that the code is not perfect or optimized to the best, but it still works perfectly, and you can study it to understand how Unity and PUN 2 work.
