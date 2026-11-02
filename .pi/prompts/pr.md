---
description: Commit, push, and create the pull request for the current branch (gh CLI, with manual fallback)
argument-hint: "[commit message]"
---

Prepare the pull request for the current branch.

1. `git status --short` and `git diff --stat` — show exactly what will be committed. Do not commit build artifacts, `.dotnet`, `.build`, `.ms` (local run artifacts), `.vscode/launch.json`, or `package-lock.json` noise unless the change is intentional.
2. Branch must not be `master`. If it is, create a feature branch named after the change (lowercase, hyphens).
3. Commit message: `feat:` / `fix:` / `refactor:` + imperative English summary.
4. Run `dotnet build "FoodSense Aspire.sln"` and `dotnet test` for the affected projects. Any new failure must be fixed before pushing; report pre-existing failures separately.
5. `git push -u origin <branch>`.
6. Write the PR body to `PR-body.md` (gitignored) and show it to me. Include:
   - what changed (markup + CSS + commands run),
   - measured before/after values,
   - side effects on other components,
   - trade-offs that were not fixed,
   - known pre-existing failures.
   Use `**` emphasis for the key numbers and trade-offs.
7. Use the `gh` CLI. The PATH entry added by winget is not visible in a fresh tool call, so prefix the command:
   `export PATH="$PATH:/c/Program Files/GitHub CLI" && gh auth status`
   - If a `github.com` account is logged in, create the PR from the body file:
     `gh pr create -B master -H <branch> -F PR-body.md -t "<commit summary>"`
     Report the URL that `gh` prints. Add `--dry-run` first if the body is long or the branch is new.
   - If `gh` is missing, or `gh auth status` reports no logged-in host, do not try to log in (the login flow is interactive). Fall back to the link printed by `git push`:
     `https://github.com/adanylkov/FoodSense/compare/master...<branch>`
     and say explicitly that the PR was not created automatically and that `gh auth login` is needed.
8. After creating the PR, verify it: `gh pr view --json url,title,state,baseRefName,headRefName,files,additions,deletions` — report the measured additions/deletions. `gh pr comment <number> -F PR-body.md` for follow-up commits, never an amended pushed commit.
