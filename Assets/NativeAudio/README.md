Native Audio
Sirawat Pitaksarit / Exceed7 Experiments
Contact : 5argon@exceed7.com
----

# How to use

All of instructions to use this is in `HowToUse > HowToUse.zip`. Please unzip it somewhere not in your project or Unity will import it as one of your game's asset.

It is an offline version of this website : http://exceed7.com/native-audio

# Demo scene

Also there is a demo scene in `Demo` folder. All buttons do not work in editor, you have to test them in a real iOS/Android device.

# Extras

There is an Android Studio project in the **Extras** folder.

- I have prepared a Gradle task named `deployReleasePluginArchive`. Create a new configuration of that name to output `.aar` file in the `Output` folder. Edit the task's output folder as well as that is linked to a folder of my computer.
- Setup NDK and SDK appropriately, ideally from the one that came with Unity install.
- You may have to install `ninja` in order to use `cmake` specified in the `CMakeList.txt` file. (Like `brew install ninja` for macOS)

# Verbose logging at native side

They are left in the code for my own development use, but should you encounter any strange problems and could not afford to wait for me to look at it, enabling verbose logging might reveal something useful to you.

Verbose logging at native side can be enabled from [iOS] NativeAudio.mm `#define LOG_NATIVE_AUDIO` and [Android] NativeAudio.java `private final static boolean enableLogging = true;` + uncomment logging preprocessor at the top part of `.c` file. On Android you will have to use the project in `Extras > AndroidStudioProject`, edit it, and recompile the `.aar` file.

After enabling, [iOS] you can see it from Xcode console [Android] you can see it with `adb logcat`.