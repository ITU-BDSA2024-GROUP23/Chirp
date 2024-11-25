# Chirp

Chirp project, developed following the course Analysis, Design and Software Architecture (Autumn 2024).

Check out our documentation at <https://itu-bdsa2024-group23.github.io/chirp/>.

## Committing not working code

If the code that you are working on breaks other services, it should never end up in
the production environment. Instead use the `staging` branch until the code is working
and is in sync with the other services. Then create a pull request from `staging` to `main`
and wait for other group members to approve it.

## Using git hooks

Using git hooks can help you correct mistakes before pushing your code. This uses `dotnet format`
to mitigate the linting step in the workflow failing. You need to change the folder used for git hooks
for this to work since it normally is in `.git/hooks` wich is not commited to version control. Run the
following command to change what folder git uses for hooks.

```bash
git config --local core.hooksPath .githooks/
```

This only changes the local repository config. If you want to change to global config, you can use
`--global` instead of the flag `--local`.

## Branch naming conventions

All branches are all lowercase and uses `-` instead of spaces.

### Branches prefixes

What prefixes to use when creating a new branch.

If the operation is not here, wait for group meeting or start a conversation in issue.

| Branch purpose | Prefix |
|---|---|
| Refactor of feature | `refactor/` |
| Creating new feature | `feature/`|
| Feature enhancment | `enhancement/`|

## Development

This project uses .NET 7.0 to run. Usage of other versions is not recommended.

[![CodeFactor](https://www.codefactor.io/repository/github/itu-bdsa2024-group23/chirp/badge)](https://www.codefactor.io/repository/github/itu-bdsa2024-group23/chirp)
