# Project Description
This discord bot was created on .NET 8 to track the necessary tags from e621 and post the necessary content in channels, there is also a search by e621, and a blocklist system.
In the future there will be a tracking update of the user gallery in furaffinity

## How to deploy a bot on Linux
### First you need to install dotnet-runtime
1. Download the dotnet installation script
```bash
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
```

2. We make the script executable and give it these rights
```bash
sudo chmod +x ./dotnet-install.sh
```

3. Install dotnet-runtime 
```bash
./dotnet-install.sh --version latest –runtime dotnet
```

4. After installing dotnet runtime, we need to add environment variables, here it is written in detail how to do this
[Set environment variables system-wide](https://learn.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual#set-environment-variables-system-wide)

### Deploying a bot
1. Now download **`hornyDragonProject.tar`**
```bash
wget https://github.com/SylvesterFox/HornyDragonBot/releases/download/0.0.1.5-alpha/HornyDragonBot-0.0.1.5.tar -o hornyDragonProject.tar
```

2. Let's unzip the archive
```bash
tar –xfv hornyDragonProject.tar
```

3. Go to the `hornybot` directory and create a `.env` file to configure the bot
```bash
cd hornybot
nano .env
```

- in the .env file we register our tokens from the discord bot and e621
```bash
TOKEN_BOT = <discord token>
TOKEN_E621 = <token e621>
USER_E621 = <username e621>
```

4. After setting up the .env file, launch the bot
```bash
dotnet HorryDragonProject.dll
```
