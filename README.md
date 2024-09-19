# Chirp

Chirp project, developed following the course Analysis, Design and Software Architecture (Autumn 2024).

Create cheeps and list others cheeps.

## Commiting not working code

If the code that you are working on breaks other services, it should never end up in
the production environment. Instead use the `staging` branch until the code is working
and is in sync with the other services. Then create a merge request from `staging` to `main`.

## Branch naming conventions

All branches are all lowercase and uses `-` instead of spaces.

#### Branches prefixes

What prefixes to use when creating a new branch.

If the operation is not here, wait for group meeting or start a conversation in issue.

| Branch purpose | Prefix |
|---|---|
| Refactor of feature | `refactor/` |
| Creating new feature | `feature/`|
