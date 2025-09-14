# Visual Studio Discord Rich Presence Client

## Description
This package is made for display your activity from the Visual Studio development environment to the Discord.

Currently, three versions of Visual Studio are supported - Visual Studio 2019, Visual Studio 2022 and Visual Studio 2026.

Extensions can be installed directly from Visual Studio or from the Visual Studio Marketplace for each version.

[Discord RPC for Visual Studio 2019](https://marketplace.visualstudio.com/items?itemName=Ryavel.vsdrp2019)

[Discord RPC for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=Ryavel.vsdrp2022)

[Discord RPC for Visual Studio 2026](https://marketplace.visualstudio.com/items?itemName=Ryavel.vsdrp2026)

***After installing the extension, an option will appear in the "Extensions" menu to open the extension settings window.***

## Preview

![preview](https://github.com/Reavert/VisualStudioDiscordRPC/assets/55898777/13c38489-7ddf-4618-9a97-670727fed057)

## Features

### Flexible text display settings
- No text display;
- File name;
- Project name;
- Solution name;
- Git branch name;
- Visual Studio version.

### Flexible icon display settings
- No icon display;
- File extension icon;
- Visual Studio icon.

### Timer modes
- No timer (The timer is disabled);
- File mode (The timer shows how much time you have spent in the file);
- Project mode (The timer shows how much time you have spent in the project);
- Solution mode (The timer shows how much time you have spent in the solution);
- Application mode (The timer shows how much time you have spent in the editor).

### Repository button
You can select "Repository" as the button. 
Then a button will be displayed that leads to the remote repository of your solution.

![repository_button](https://github.com/Reavert/VisualStudioDiscordRPC/assets/55898777/417f9fcd-fbcf-4251-8436-3a3a895f61d4)

### Private repositories
You can mark the current repository of your solution as private. 
Then the button with a link to your remote repository will not be displayed.   

### Secret solutions
You can mark your current solution as secret. 
When you work in such solution, the icon will change to "lock", and information about your work will be hidden.

![secret_screen](https://github.com/Reavert/VisualStudioDiscordRPC/assets/55898777/f6532536-bbe6-4264-ae19-fb66ae62dd5a)

### Idling detection
You can enable idling detection.

If you do not switch to other windows or do not write anything in the text editor of the IDE, then after some time you will be marked as "Idling". The large image will change to the corresponding icon.   

You can set the time after which you are considered missing in the settings in the "Idling" block   

![image](https://github.com/user-attachments/assets/a947b8f1-62af-4061-845c-1e7e1186b1b5)

### Custom text display
You can create and customize your own text display using variables.

The available variables are shown in the table below.

|Variable|Description|
|--------|-----------|
|```file_name```|Name of the currently active file|
|```project_name```|Name of the currently active project|
|```solution_name```|Name of the currently active solution|
|```version```|Visual Studio version (2019 or 2022)|
|```edition```|Visual Stuido edition (Community, Professional or Enterprise)|
|```debug_mode```|Current debugging mode|
|```git_branch```|Current solution's git branch name|

### Localizations
The extension has the following localizations:
- English;
- Russian;
- Belarusian;
- Ukrainian;
- Czech;
- German;
- Spanish;
- French;
- Hindi;
- Italian;
- Korean;
- Polish;
- Portuguese;
- Turkish;
- Chinese Simplified;
- Chinese Traditional.

## Issues
If you encounter a problem or have suggestions for a project, feel free to add it to the [Issues](https://github.com/Reavert/VisualStudioDiscordRPC/issues) section.

## Contributing
If you want to help with development of this extension, feel free to make a [Pull Request](https://github.com/Reavert/VisualStudioDiscordRPC/pulls).

## Thanks
[Lachee](https://github.com/Lachee)/[discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp)

## License
This project is licensed under the [MIT License](https://github.com/Reavert/VisualStudioDiscordRPC/blob/main/LICENSE.txt).
