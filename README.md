# Protect Command Plugin

## Overview
The Protect Command Plugin is designed to restrict the usage of specific commands to designated regions in Terraria. This ensures that certain commands can only be executed within specified areas, enhancing control and security within the game.

## Features
- Restrict command usage to specific regions.
- Customizable messages for unauthorized command usage.
- Easy configuration through a JSON file.
- Bypass permissions for certain players.

## Installation
1. Download the plugin and place it in your TShock `ServerPlugins` directory.
2. Start your TShock server to generate the default configuration file.
3. Edit the `ProtectCommand.json` configuration file located in the TShock `tshock` directory to customize the plugin settings.

## Configuration
The configuration file `ProtectCommand.json` includes the following settings:

```json
{
    "Enable": true,
    "Not in the Region Message": "You are not in the correct region to use this command. Please use this command in {region}.",
    "Protected": {
        "cone": "Region1",
        "ctwo": "Region2"
    }
}
```

- `Enable`: Enables or disables the plugin.
- `Not in the Region Message`: The message displayed to players who attempt to use a protected command outside the designated region.
- `Protected`: A dictionary of commands and their corresponding regions.

## Usage
1. Define the regions in your TShock server where you want to restrict command usage.
2. Add the commands and their corresponding regions to the `Protected` dictionary in the configuration file.
3. Players attempting to use these commands outside the specified regions will receive the configured error message.

## Permissions
- `protectcommand.bypass`: Allows a player to bypass the region restrictions for commands.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.