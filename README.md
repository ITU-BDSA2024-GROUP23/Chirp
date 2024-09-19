# Chirp

Chirp project, developed following the course Analysis, Design and Software Architecture (Autumn 2024).

## Example usage of Chirp CLI

To get an overview of available commands

```bash
chirp --help
```

List all cheeps

```bash
chirp read
```

Create a new cheep

```bash
chirp cheep "Hello, World!"
```

## Committing not working code

If the code that you are working on breaks other services, it should never end up in
the production environment. Instead use the `staging` branch until the code is working
and is in sync with the other services. Then create a pull request from `staging` to `main` 
and wait for other group members to approve it.

## Branch naming conventions

All branches are all lowercase and uses `-` instead of spaces.

#### Branches prefixes

What prefixes to use when creating a new branch.

If the operation is not here, wait for group meeting or start a conversation in issue.

| Branch purpose | Prefix |
|---|---|
| Refactor of feature | `refactor/` |
| Creating new feature | `feature/`|
| Feature enhancment | `enhancement/`|

## Development

This project uses .NET 7.0 to run. Usage of other versions is not recommended.
