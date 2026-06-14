# RemoteApp Tool (v2 Modernized Edition)

With Microsoft RemoteApp technology, you can seamlessly use an application that is running on another computer.

**RemoteApp Tool v2** is a completely modernized and redesigned utility that allows you to create/manage RemoteApps hosted on Windows as well as generate RDP and MSI files for clients. This version has been fully rewritten in modern **C# (.NET 8)** and features a beautiful **WPF Material Design** interface, leaving behind the legacy VB.NET code.

## Download v2 (Modern)

**Latest Installer:**

👉 [Descargar Instalador de RemoteApp Tool v2 (.exe)](https://github.com/stivencp32-ux/remoteapptool/raw/v2-modernization/RemoteAppTool-v2-Setup.exe)

*(Este instalador incluye internamente todo el runtime de .NET 8, por lo que no necesitas instalar ningún framework previo en tus servidores. Simplemente descarga, instala y disfruta).*

## Features

* **Modern UI:** Material Design interface with Dark Mode and dynamic dialogs.
* **Fully Native .NET 8 Core:** Much faster and completely decoupled from legacy dependencies.
* Create and manage RemoteApps on Windows desktops and servers.
* Generate RDP files instantly.
* Generate MSI installers (requires WiX Toolset for building MSI files).
* Manage connections, icons, and command-line arguments.

## Requirements for building from source

If you want to compile the code yourself:
* Microsoft .NET 8 SDK
* Windows OS (due to Windows Registry and WPF requirements)

---

*(Forked and Modernized from the original Kim Knight's RemoteApp Tool)*
