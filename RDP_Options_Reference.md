# RDP Advanced Options Reference

This document lists the advanced RDP connection strings that can be used in the **Advanced Options** field of RemoteApp Tool v2.

### Format
Each advanced string must be written in the format: `name:type:value`
- **i**: Integer (e.g. `1` or `0`)
- **s**: String (e.g. `*` or `domain.com`)

Example: `audiomode:i:2`

---

## administrative session
**Syntax Example:** `administrative session:i:0`

Connect to the administrative session of the remote computer.

0 - Do not use the administrative session.
1 - Connect to the administrative session.

**Default Value:** `0`

---

## allow desktop composition
**Syntax Example:** `allow desktop composition:i:0`

Determines whether desktop composition (needed for Aero) is permitted when you log on to the remote computer.

0 - Disable desktop composition in the remote session.
1 - Desktop composition is permitted.

**Default Value:** `0`

---

## allow font smoothing
**Syntax Example:** `allow font smoothing:i:0`

Determines whether font smoothing may be used in the remote session.

0 - Disable font smoothing in the remote session.
1 - Font smoothing is permitted.

**Default Value:** `0`

---

## audiocapturemode
**Syntax Example:** `audiocapturemode:i:0`

Determines how sounds captured (recorded) on the local computer are handled when you are connected to the remote computer.

0 - Do not capture audio from the local computer.
1 - Capture audio from the local computer and send to the remote computer.

**Default Value:** `0`

---

## audiomode
**Syntax Example:** `audiomode:i:0`

Determines how sounds on a remote computer are handled when you are connected to the remote computer.

0 - Play sounds on the local computer.
1 - Play sounds on the remote computer.
2 - Do not play sounds.

**Default Value:** `0`

---

## audioqualitymode
**Syntax Example:** `audioqualitymode:i:0`

Determines the quality of the audio played in the remote session.

0 - Dynamically adjust audio quality based on available bandwidth.
1 - Always use medium audio quality.
2 - Always use uncompressed audio quality.

**Default Value:** `0`

---

## authentication level
**Syntax Example:** `authentication level:i:2`

Determines what should happen when server authentication fails.

0 - If server authentication fails, connect without giving a warning.
1 - If server authentication fails, do not connect.
2 - If server authentication fails, show a warning and allow the user to connect or not.
3 - Server authentication is not required.

**Default Value:** `2`

---

## autoreconnect max retries
**Syntax Example:** `autoreconnect max retries:i:20`

Determines the maximum number of times the client computer will try to reconnect to the remote computer if the connection is dropped.
Note: The maximum value Remote Desktop can handle is 200.

**Default Value:** `20`

---

## autoreconnection enabled
**Syntax Example:** `autoreconnection enabled:i:1`

Determines whether the client computer will automatically try to reconnect to the remote computer if the connection is dropped.

0 - Do not attempt to reconnect.
1 - Attempt to reconnect.

**Default Value:** `1`

---

## bandwidthautodetect
**Syntax Example:** `bandwidthautodetect:i:1`

Enables the option for automatic detection of the network type. Used in conjunction with networkautodetect. Also see connection type.

0 - Do not enable the option for automatic network detection.
1 - Enable the option for automatic network detection.

**Default Value:** `1`

---

## bitmapcachepersistenable
**Syntax Example:** `bitmapcachepersistenable:i:1`

Determines whether bitmaps are cached on the local computer (disk-based cache). Bitmap caching can improve the performance of your remote session.

0 - Do not cache bitmaps.
1 - Cache bitmaps.

**Default Value:** `1`

---

## bitmapcachesize
**Syntax Example:** `bitmapcachesize:i:1500`

Specifies the size in kilobytes of the memory-based bitmap cache. The maximum value is 32000.

**Default Value:** `1500`

---

## camerastoredirect
**Syntax Example:** `camerastoredirect:s:value`

Configures which cameras to redirect. This setting uses a semicolon-delimited list of KSCATEGORY_VIDEO_CAMERA interfaces of cameras enabled for redirection.

---

## compression
**Syntax Example:** `compression:i:1`

Determines whether the connection should use bulk compression.

0 - Do not use bulk compression.
1 - Use bulk compression.

**Default Value:** `1`

---

## connect to console
**Syntax Example:** `connect to console:i:0`

Connect to the console session of the remote computer.

0 - Connect to a normal session.
1 - Connect to the console screen.

**Default Value:** `0`

---

## connection type
**Syntax Example:** `connection type:i:2`

Specifies pre-defined performance settings for the Remote Desktop session.

1 - Modem (56 Kbps).
2 - Low-speed broadband (256 Kbps - 2 Mbps).
3 - Satellite (2 Mbps - 16 Mbps with high latency).
4 - High-speed broadband (2 Mbps - 10 Mbps).
5 - WAN (10 Mbps or higher with high latency).
6 - LAN (10 Mbps or higher).
7 - Automatic bandwidth detection. Requires bandwidthautodetect.

By itself, this setting does nothing. When selected in the RDC GUI, this option changes several performance related settings (themes, animation, font smoothing, etcetera). These separate settings always overrule the connection type setting.

**Default Value:** `2`

---

## desktop size id
**Syntax Example:** `desktop size id:i:0`

Specifies pre-defined dimensions of the remote session desktop.

0 - 640x480.
1 - 800x600.
2 - 1024x768.
3 - 1280x1024.
4 - 1600x1200.

This setting is ignored when either /w and /h, or desktopwidth and desktopheight are already specified.

**Default Value:** `0`

---

## desktopheight
**Syntax Example:** `desktopheight:i:600`

The height (in pixels) of the remote session desktop.

**Default Value:** `600`

---

## desktopscalefactor
**Syntax Example:** `desktopscalefactor:i:value`

Specifies the scale factor of the remote session to make the content appear larger.

Supported values:
Numerical value from the following list: 100, 125, 150, 175, 200, 250, 300, 400, 500

Note: The desktopscalefactor property is being deprecated and will soon be unavailable.

---

## desktopwidth
**Syntax Example:** `desktopwidth:i:800`

The width (in pixels) of the remote session desktop.

**Default Value:** `800`

---

## devicestoredirect
**Syntax Example:** `devicestoredirect:s:value`

Determines which supported Plug and Play devices on the client computer will be redirected and available in the remote session.

No value specified - Do not redirect any supported Plug and Play devices.
* - Redirect all supported Plug and Play devices, including ones that are connected later.
DynamicDevices - Redirect any supported Plug and Play devices that are connected later.
The hardware ID for one or more Plug and Play devices - Redirect the specified supported Plug and Play device(s).

---

## disable ctrl+alt+del
**Syntax Example:** `disable ctrl+alt+del:i:1`

Determines whether you have to press CTRL+ALT+DELETE before entering credentials after you are connected to the remote computer.

0 - CTRL+ALT+DELETE is required before logging in.
1 - CTRL+ALT+DELETE is not required. You can logon immediately.

Note: When disabled, this setting will also delay the autologin until the user has pressed CTRL+ALT+DELETE.

**Default Value:** `1`

---

## disable full window drag
**Syntax Example:** `disable full window drag:i:1`

Determines whether window content is displayed when you drag the window to a new location.

0 - Show the contents of the window while dragging.
1 - Show an outline of the window while dragging.

**Default Value:** `1`

---

## disable menu anims
**Syntax Example:** `disable menu anims:i:1`

Determines whether menus and windows can be displayed with animation effects in the remote session.

0 - Menu and window animation is permitted.
1 - No menu and window animation.

**Default Value:** `1`

---

## disable themes
**Syntax Example:** `disable themes:i:0`

Determines whether themes are permitted when you log on to the remote computer.

0 - Themes are permitted.
1 - Disable theme in the remote session.

**Default Value:** `0`

---

## disable wallpaper
**Syntax Example:** `disable wallpaper:i:1`

Determines whether the desktop background is displayed in the remote session.

0 - Display the wallpaper.
1 - Do not show any wallpaper.

**Default Value:** `1`

---

## disableconnectionsharing
**Syntax Example:** `disableconnectionsharing:i:0`

Determines whether a new Terminal Server session is started with every launch of a RemoteApp to the same computer and with the same credentials.

0 - No new session is started. The currently active session of the user is shared.
1 - A new login session is started for the RemoteApp.

**Default Value:** `0`

---

## disableremoteappcapscheck
**Syntax Example:** `disableremoteappcapscheck:i:0`

Specifies whether the Remote Desktop client should check the remote computer for RemoteApp capabilities.
0 - Check the remote computer for RemoteApp capabilities before logging in.
1 - Do not check the remote computer for RemoteApp capabilities.Note: This setting must be set to 1 when connecting to Windows XP SP3, Vista or 7 computers with RemoteApps configured on them.

**Default Value:** `0`

---

## displayconnectionbar
**Syntax Example:** `displayconnectionbar:i:1`

Determines whether the connection bar appears when you are in full screen mode.

0 - Do not show the connection bar.
1 - Show the connection bar.

**Default Value:** `1`

---

## domain
**Syntax Example:** `domain:s:value`

Specifies the name of the domain of the user.

---

## drivestoredirect
**Syntax Example:** `drivestoredirect:s:value`

Determines which local disk drives on the client computer will be redirected and available in the remote session.

No value specified - Do not redirect any drives.
* - Redirect all disk drives, including drives that are connected later.
DynamicDrives - Redirect any drives that are connected later.
The drive and labels for one or more drives - Redirect the specified drive(s).

---

## dynamic resolution
**Syntax Example:** `dynamic resolution:i:1`

Determines whether the resolution of a remote session is automatically updated when the local window is resized.

0 - Session resolution remains static during the session.
1 - Session resolution updates as the local window resizes.

**Default Value:** `1`

---

## enablecredsspsupport
**Syntax Example:** `enablecredsspsupport:i:1`

Determines whether Remote Desktop will use CredSSP for authentication if it's available.

0 - Do not use CredSSP, even if the operating system supports it.
1 - Use CredSSP, if the operating system supports it.

**Default Value:** `1`

---

## enablerdsaadauth
**Syntax Example:** `enablerdsaadauth:i:0`

Determines whether the client will use Microsoft Entra ID to authenticate to the remote PC. When used with Azure Virtual Desktop, this provides a single sign-on experience. This property replaces the property targetisaadjoined.

0 - Connections won't use Microsoft Entra authentication, even if the remote PC supports it.
1 - Connections will use Microsoft Entra authentication if the remote PC supports it.

**Default Value:** `0`

---

## enablesuperpan
**Syntax Example:** `enablesuperpan:i:0`

Determines whether SuperPan is enabled or disabled. SuperPan allows the user to navigate a remote desktop in full-screen mode without scroll bars, when the dimensions of the remote desktop are larger than the dimensions of the current client window. The user can point to the window border, and the desktop view will scroll automatically in that direction.

0 - Do not use SuperPan. The remote session window is sized to the client window size.
1 - Enable SuperPan. The remote session window is sized to the dimensions specified through /w and /h, or through desktopwidth and desktopheight.

**Default Value:** `0`

---

## encode redirected video capture
**Syntax Example:** `encode redirected video capture:i:1`

Enables or disables encoding of redirected video.

0 - Disable encoding of redirected video.

1 - Enable encoding of redirected video.

**Default Value:** `1`

---

## gatewaycredentialssource
**Syntax Example:** `gatewaycredentialssource:i:4`

Specifies the credentials that should be used to validate the connection with the RD Gateway.

0 - Ask for password (NTLM).
1 - Use smart card.
4 - Allow user to select later.

**Default Value:** `4`

---

## kdcproxyname
**Syntax Example:** `kdcproxyname:s:value`

Specifies the fully qualified domain name of a KDC proxy.

---

## keyboardhook
**Syntax Example:** `keyboardhook:i:2`

Determines how Windows key combinations are applied when you are connected to a remote computer.

0 - Windows key combinations are applied on the local computer.
1 - Windows key combinations are applied on the remote computer.
2 - Windows key combinations are applied in full-screen mode only.

**Default Value:** `2`

---

## maximizetocurrentdisplays
**Syntax Example:** `maximizetocurrentdisplays:i:0`

Determines which display a remote session uses for full screen on when maximizing. Requires use multimon set to 1. Only available on Windows App for Windows and the Remote Desktop app for Windows.

0 - Session is full screen on the displays initially selected when maximizing.
1 - Session dynamically is full screen on the displays the session window spans when maximizing.

**Default Value:** `0`

---

## negotiate security layer
**Syntax Example:** `negotiate security layer:i:1`

Determines whether the level of security is negotiated or not.

0 - Security layer negotiation is not enabled and the session is started by using Secure Sockets Layer (SSL).
1 - Security layer negotiation is enabled and the session is started by using x.224 encryption.

**Default Value:** `1`

---

## networkautodetect
**Syntax Example:** `networkautodetect:i:1`

Determines whether to use automatic network bandwidth detection or not. Requires the option bandwidthautodetect to be set and correlates with connection type 7.

0 - Use automatic network bandwitdh detection.
1 - Do not use automatic network bandwitdh detection.

**Default Value:** `1`

---

## pinconnectionbar
**Syntax Example:** `pinconnectionbar:i:1`

Determines whether or not the connection bar should be pinned to the top of the remote session upon connection when in full screen mode.

0 - The connection bar should not be pinned to the top of the remote session.
1 - The connection bar should be pinned to the top of the remote session.

**Default Value:** `1`

---

## prompt for credentials
**Syntax Example:** `prompt for credentials:i:0`

Determines whether Remote Desktop Connection will prompt for credentials when connecting to a remote computer for which the credentials have been previously saved.

0 - Remote Desktop will use the saved credentials and will not prompt for credentials.
1 - Remote Desktop will prompt for credentials.

**Default Value:** `0`

---

## prompt for credentials on client
**Syntax Example:** `prompt for credentials on client:i:0`

Determines whether Remote Desktop Connection will prompt for credentials when connecting to a server that does not support server authentication.

0 - Remote Desktop will not prompt for credentials.
1 - Remote Desktop will prompt for credentials.

**Default Value:** `0`

---

## promptcredentialonce
**Syntax Example:** `promptcredentialonce:i:1`

When connecting through an RD Gateway, determines whether RDC should use the same credentials for both the RD Gateway and the remote computer.

0 - Remote Desktop will not use the same credentials .
1 - Remote Desktop will use the same credentials for both the RD gateway and the remote computer.

**Default Value:** `1`

---

## public mode
**Syntax Example:** `public mode:i:0`

Determines whether Remote Desktop Connection will be started in public mode.

0 - Remote Desktop will not start in public mode .
1 - Remote Desktop will start in public mode and will not save any user data (credentials, bitmap cache, MRU) on the local machine.

**Default Value:** `0`

---

## redirectclipboard
**Syntax Example:** `redirectclipboard:i:1`

Determines whether the clipboard on the client computer will be redirected and available in the remote session and vice versa.

0 - Do not redirect the clipboard.
1 - Redirect the clipboard.

**Default Value:** `1`

---

## redirectcomports
**Syntax Example:** `redirectcomports:i:0`

Determines whether the COM (serial) ports on the client computer will be redirected and available in the remote session.

0 - The COM ports on the local computer are not available in the remote session.
1 - The COM ports on the local computer are available in the remote session.

**Default Value:** `0`

---

## redirectdirectx
**Syntax Example:** `redirectdirectx:i:1`

Determines whether DirectX will be enabled for the remote session.

0 - Do not enable DirectX rendering.
1 - Enable DirectX rendering in the remote session.

**Default Value:** `1`

---

## redirectdrives
**Syntax Example:** `redirectdrives:i:0`

Determines whether local disk drives on the client computer will be redirected and available in the remote session.

0 - The drives on the local computer are not available in the remote session.
1 - The drives on the local computer are available in the remote session.

Note: This setting is replaced by drivestoredirect from RDC 6.0 onward.

**Default Value:** `0`

---

## redirected video capture encoding quality
**Syntax Example:** `redirected video capture encoding quality:i:0`

Controls the quality of encoded video.

0 - High compression video. Quality may suffer when there's a lot of motion.
1 - Medium compression.
2 - Low compression video with high picture quality.

**Default Value:** `0`

---

## redirectlocation
**Syntax Example:** `redirectlocation:i:0`

Determines whether the location of the local device will be redirected and available in the remote session.

0 - The remote session uses the location of the remote computer.
1 - The remote session uses the location of the local device.

**Default Value:** `0`

---

## redirectposdevices
**Syntax Example:** `redirectposdevices:i:0`

Determines whether Microsoft Point of Service (POS) for .NET devices connected to the client computer will be redirected and available in the remote session.

0 - The POS devices from the local computer are not available in the remote session.
1 - The POS devices from the local computer are available in the remote session.

**Default Value:** `0`

---

## redirectprinters
**Syntax Example:** `redirectprinters:i:1`

Determines whether printers configured on the client computer will be redirected and available in the remote session.

0 - The printers on the local computer are not available in the remote session.
1 - The printers on the local computer are available in the remote session.

**Default Value:** `1`

---

## redirectsmartcards
**Syntax Example:** `redirectsmartcards:i:1`

Determines whether smart card devices on the client computer will be redirected and available in the remote session.

0 - The smart card device on the local computer is not available in the remote session.
1 - The smart card device on the local computer is available in the remote session.

**Default Value:** `1`

---

## redirectwebauthn
**Syntax Example:** `redirectwebauthn:i:1`

Determines whether WebAuthn requests on the remote computer will be redirected to the local computer allowing the use of local authenticators (such as Windows Hello for Business and security key).

0 - WebAuthn requests from the remote session aren't sent to the local computer for authentication and must be completed in the remote session.
1 - WebAuthn requests from the remote session are sent to the local computer for authentication.

**Default Value:** `1`

---

## remoteapplicationcmdline
**Syntax Example:** `remoteapplicationcmdline:s:value`

Optional command line parameters for the RemoteApp.

---

## remoteapplicationexpandworkingdir
**Syntax Example:** `remoteapplicationexpandworkingdir:i:0`

Determines whether environment variables contained in the RemoteApp working directory parameter should be expanded locally or remotely.

0 - Environment variables should be expanded to the values of the local computer.
1 - Environment variables should be expanded on the remote computer to the values of the remote computer.

Note: The RemoteApp working directory is specified through the shell working directory parameter.

**Default Value:** `0`

---

## remoteapplicationicon
**Syntax Example:** `remoteapplicationicon:s:value`

Specifies the file name of an icon file to be displayed in the Remote Desktop interface while starting the RemoteApp. By default RDC will show the standard Remote Desktop icon.

Note: Only .ico files are supported.

---

## screen mode id
**Syntax Example:** `screen mode id:i:2`

Determines whether the remote session window appears full screen when you connect to the remote computer.

1 - The remote session will appear in a window.
2 - The remote session will appear full screen.

**Default Value:** `2`

---

## selectedmonitors
**Syntax Example:** `selectedmonitors:s:value`

Specifies which local displays to use for the remote session. The selected displays must be contiguous. Requires use multimon to be set to 1.

Comma separated list of machine-specific display IDs. You can retrieve IDs by calling mstsc.exe /l. The first ID listed will be set as the primary display in the session. Defaults to all displays.

---

## session bpp
**Syntax Example:** `session bpp:i:32`

Determines the color depth (in bits) on the remote computer when you connect.

8 - 256 colors (8 bit).
15 - High color (15 bit).
16 - High color (16 bit).
24 - True color (24 bit).
32 - Highest quality (32 bit).

**Default Value:** `32`

---

## shell working directory
**Syntax Example:** `shell working directory:s:value`

The working directory on the remote computer to be used if an alternate shell is specified.

---

## signscope
**Syntax Example:** `signscope:s:value`

Comma-delimited list of .rdp file settings for which the signature is generated when using .rdp file signing.

---

## singlemoninwindowedmode
**Syntax Example:** `singlemoninwindowedmode:i:0`

Determines whether a multi display remote session automatically switches to single display when exiting full screen. Requires use multimon set to 1. Only available on Windows App for Windows and the Remote Desktop app for Windows.

0 - A remote session retains all displays when exiting full screen.
1 - A remote session switches to a single display when exiting full screen.

**Default Value:** `0`

---

## smart sizing
**Syntax Example:** `smart sizing:i:0`

Determines whether the client computer should scale the content on the remote computer to fit the window size of the client computer when the window is resized.

0 - The client window display will not be scaled when resized.
1 - The client window display will be scaled when resized.

**Default Value:** `0`

---

## span monitors
**Syntax Example:** `span monitors:i:0`

Determines whether the remote session window will be spanned across multiple monitors when you connect to the remote computer.

0 - Monitor spanning is not enabled.
1 - Monitor spanning is enabled.

Note: When using Remote Desktop Connection 7 (Windows 7/2008), the use multimon setting is recommended.

**Default Value:** `0`

---

## superpanaccelerationfactor
**Syntax Example:** `superpanaccelerationfactor:i:1`

Specifies the number of pixels that the screen view scrolls in a given direction for every pixel of mouse movement by the client when in SuperPan mode

**Default Value:** `1`

---

## targetisaadjoined
**Syntax Example:** `targetisaadjoined:i:0`

Allows connections to Microsoft Entra joined session hosts using a username and password. This property is only applicable to non-Windows clients and local Windows devices that aren't joined to Microsoft Entra.

0 - Connections to Microsoft Entra joined session hosts will succeed for Windows devices that meet the requirements, but other connections will fail.
1 - Connections to Microsoft Entra joined hosts will succeed but are restricted to entering user name and password credentials when connecting to session hosts.

Note: This property is being replaced by enablerdsaadauth.

**Default Value:** `0`

---

## usbdevicestoredirect
**Syntax Example:** `usbdevicestoredirect:s:value`

Determines which supported RemoteFX USB devices on the client computer will be redirected and available in the remote session when you connect to a remote session that supports RemoteFX USB redirection.

No value specified - Do not redirect any supported RemoteFX USB devices.
* - Redirect all supported RemoteFX USB devices for redirection that are not redirected by high-level redirection mechanisms.
{Device Setup Class GUID} - Redirect all supported RemoteFX USB devices that are members of the specified device setup class.
USB\InstanceID - Redirect the supported RemoteFX USB device specified by the given instance ID.
-USB\InstanceID - Do not redirect the supported RemoteFX USB device specified by the given instance ID, even if the device is in a device setup class that is redirected..

---

## use multimon
**Syntax Example:** `use multimon:i:0`

Determines whether the session should use true multiple monitor support when connecting to the remote computer.

0 - Do not enable multiple monitor support.
1 - Enable multiple monitor support.

**Default Value:** `0`

---

## username
**Syntax Example:** `username:s:value`

Specifies the name of the user account that will be used to log on to the remote computer.

---

## videoplaybackmode
**Syntax Example:** `videoplaybackmode:i:1`

Determines whether RDC will use RDP efficient multimedia streaming for video playback.

0 - Do not use RDP efficient multimedia streaming for video playback.
1 - Use RDP efficient multimedia streaming for video playback when possible.

**Default Value:** `1`

---

## winposstr
**Syntax Example:** `winposstr:s:0,3,0,0,800,600`

Specifies the position and dimensions of the session window on the client computer.

**Default Value:** `0,3,0,0,800,600`

---

## workspaceid
**Syntax Example:** `workspaceid:s:value`

This setting defines the RemoteApp and Desktop ID associated with the RDP file that contains this setting.

No

---

