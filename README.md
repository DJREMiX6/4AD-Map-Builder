# 4AD Map Builder
### Description
This application made with **Unity** aims to simplify the creation and management of **4AD** (Four Against Darkness) randomly generated dungeon.

The **core principle** is to make it more **fast to create rooms** and more **easy to manage the rooms content** that can easily become challenging when having a lot of rooms with a lot of informations inside them, especially whith corridors.

The project will be **Open Source** so any developer or user can collaborate to create a better SW for everyone by sending **Pull Request** or **Opening Issues** with **suggestions for new features and functionalities**.
**Feel free to join.**

### Screenshots
The current version of the SW generates one of the 6 possible entrances after starting up:

![image](https://github.com/DJREMiX6/4AD-Map-Builder/assets/35576682/4b0b399d-eabf-46b5-8c5f-715c08529e11)

Each **Room** is composed of one or more **Doors** and clicking on them will open the **Door Context Menu** with a list of options:

![image](https://github.com/DJREMiX6/4AD-Map-Builder/assets/35576682/d30574af-b8a1-4962-bc60-055f6deb349b)

Clicking on the **Add room** option will open a prompt that will ask the user of a **Room ID** (the same as the manual) but gives the user the option to **generate a random one** by clicking on the **Generate ID** button. The **Create room** button will use the inserted/generated **Room ID** and create the relative room attached to the clicked **Door**.

![image](https://github.com/DJREMiX6/4AD-Map-Builder/assets/35576682/a889ad5b-e70b-4e57-b365-bc45480ba751)

Some rooms may not have an **Explicit Door** but instead they have a kind of **Implicit Door** that simulates the fact that the door is not present but can still be clicked and have (for now) the same **Door Context Menu** as a normal **Door**:

![image](https://github.com/DJREMiX6/4AD-Map-Builder/assets/35576682/8b0217aa-c545-4973-ac6b-402bb16ea1e0)

