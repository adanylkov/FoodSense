---
description: Commit, push, and prepare the pull request body for the current branch
argument-hint: "[commit message]"
---

Prepare the pull request for the current branch.

1. `git status --short` and `git diff --stat` — show exactly what will be committed. Do not commit build artifacts, `.dotnet`, `.build`, `.ms` (local run artifacts), `.vscode/launch.json`, or `package-lock.json` noise unless the change is intentional.
2. Branch must not be `master`. If it is, create a feature branch named after the change (lowercase, hyphens).
3. Commit message: `feat:` / `fix:` / `refactor:` + imperative English summary.
4. Run `dotnet build "FoodSense Aspire.sln"` and `dotnet test` for the affected projects. Any new failure must be fixed before pushing; report pre-existing failures separately.
5. `git push -u origin <branch>`.
6. Write the PR body to `PR-body.md` and show it to me. Include:
   - what changed (markup + CSS + commands run),
   - measured before/after values,
   - side effects on other components,
   - trade-offs that were not fixed,
   - known pre-existing failures.
   Use `**` emphasis for the key numbers and trade-offs.
7. Give me the PR link: `https://github.com/adanylkov/FoodSense/compare/master...<branch>` (`gh` is not installed, I open the PR manually).
