# simU-frontend

This holds the frontend repo of SimU, an AI mobile game built using Unity Mobile 2D where users can chat with their friends online / offline through personalities we built for them.

[Link to Figma](https://www.figma.com/file/C8lO2GUAnYuTL5eUbSuTjQ/Prototype%3A-Vico%2C-Tucker%2C-Jackson%2C-Evan%2C-Alan%2C-Lekina?type=design&node-id=547%3A435&mode=design&t=2vE4ODx9CQ4JZq1P-1)

## Architecture

We have 4 main scenes in this world and each scene includes multiple UI panels which users can interact with. 

Scenes:

1. Onboarding: The entire onboarding flow for a user from login/signup to answering questions about what they'd like their AI personalities to be like and selecting a character design.

2. MyWorlds: This scene is where a user can create a world, add an existing world to their list, join a world, and update their user's personality summary

3. MainMap: The map where users interact with one another. Here, users can chat with others, create AI agents, and owners can manage the players in their world (kick, etc.)

4. Chat: From the MainMap scene, users enter this chat scene to speak with other AI agents or users. If other users are offline, their bot takes over and answers questions / asks questions to keep the conversation going.

We use [HTTPClient](https://github.com/dartmouth-cs98-23f/jarvis-frontend/blob/main/Assets/Clients/HTTPClient.cs) to handle all CRUD requests with the backend.

We use [SignalRClient](https://github.com/dartmouth-cs98-23f/jarvis-frontend/blob/main/Assets/SignalRClient/SignalRClient.cs) to handle methods that require the server to prompt users so that every user can get live updates from the server rather than calling `GET` requests repeatedly:

1. Multiplayer movement
2. Checking if a user is online
3. Handling the adding and removing of users & agents in the world
4. Sending and updating the chat scene

#### Recording of Onboarding Flow
https://github.com/dartmouth-cs98-23f/jarvis-frontend/assets/37021321/b137bc5f-4c8f-4650-851d-f60fcf24ed23

#### Recording of creating an agent
https://github.com/dartmouth-cs98-23f/jarvis-frontend/assets/37021321/cca4128c-ce8c-4090-81cb-bf890aafb741

#### Recording of chatting with someone's bot
https://github.com/dartmouth-cs98-23f/jarvis-frontend/assets/37021321/a6bfec6d-f5c7-45e6-b575-209f7f34c2d8

## Setup

Install Unity 
Install XCode (if you want to deploy to app store)

## Deployment

1. `git pull https://github.com/dartmouth-cs98-23f/jarvis-frontend.git`
2. Open the Jarvis-Frontend folder using Unity
3. Connect with [backend](https://github.com/dartmouth-cs98-23f/SimU-GameService)
4. This app is deployed to the app store using Unity. Here is a [guide](https://learn.unity.com/tutorial/publishing-for-ios) to deploy to app store via Unity.

## Authors

Evan Phillips
Vico Lee

## Acknowledgments
Thanks to Tim Tregubov and Natalie Svoboda for their guidance throughout this project.

