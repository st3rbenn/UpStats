# 📜 What's this?
UpStats provide a *stats system based on your interaction with the world of Terraria* such as:

- [x] Minning
- [x] Fishing
- [x] Woodcutting
- [~] Fighting
- [ ] More coming soon




# ⚙️ How do I build this?

Below is a step by step instruction for that. It only assumes that you know at least how to use command prompts, shells, or terminals. You should.

- Get TModLoader from [Steam](https://store.steampowered.com/app/1281930/tModLoader) or [GitHub](https://github.com/tModLoader/tModLoader/releases).
- Get Git from [git-scm](https://git-scm.com/download) or from a Linux package manager. Most defaults suffice in the installer.
- Get the [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK.
- Clone the mod into `%userprofile%/Documents/My games/Terraria/tModLoader/ModSources`.
The git command for that would be `git clone https://github.com/St3rbenn/UpStats -b trunk`, where `dev` is the branch you want to clone.
- Build the mod by running `dotnet build` in the cloned folder.

That's all. Use `git pull` to pull new commits, and `git reset origin/trunk --hard` to force-reset your local repository.
