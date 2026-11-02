# FoodSense Aspire — instructions for agents

## Communication
- Reply in Polish. Code, comments, commit messages in English.
- Report verified facts and numbers (before/after). If something cannot be verified, say so.
- Surface trade-offs explicitly (e.g. "12 px text is below WCAG touch-text guidance", "this class is used by 30 components").

## Start of every session (mandatory)
- Run `git status --short`, `git diff`, `git log --oneline -10` before touching anything.
- Sessions often leave uncommitted, half-finished work. If uncommitted changes exist, audit them first: what is done, what is missing, then tell the user. Do not silently redo work, and do not assume the diff is complete.
- Verify claims from the user or from a previous agent with commands (`rg`, `git show --stat`, build, tests) before acting on them.
- Check `TODO.md` for the task list and mark finished items.

## Never trust markup
- Every CSS class used in `.razor` markup must exist in `FoodSense.BlazorFrontend/wwwroot/app.css` or come from Bootstrap. A class that is not defined is dead code — defining missing classes is part of the task, not an optional extra.
- `bootstrap-utilities.css` is NOT linked in `Components/App.razor` (only `app.css`, which imports plain `bootstrap` CSS). So utility classes like `object-fit-contain`, `min-width-0`, `d-none`, `text-truncate` only work if defined locally in `app.css`.
- Tailwind is in `package.json` but its CSS is not loaded — do not use Tailwind classes.
- Check who else uses a class before changing it globally (`rg -l "\.class-name" FoodSense.BlazorFrontend/Components`).

## Repo facts
- Solution: `FoodSense Aspire.sln`. Projects: `FoodSense.API`, `FoodSense.BlazorFrontend`, `FoodSense.Common`, `FoodSense.ReceiptReader`, plus `*.Tests`.
- Build: `dotnet build "FoodSense Aspire.sln"` — must be 0 warnings, 0 errors.
- Tests: `dotnet test FoodSense.BlazorFrontend.Tests`, `dotnet test FoodSense.ReceiptReader.Tests`, `dotnet test FoodSense.API.Tests`.
- Known pre-existing failure, unrelated to UI work: `FoodSense.API.Tests.FoodLogServiceTests.LogConsumptionAndReducePantryAsync_ShouldLogNutrientsAndReduceMatchingPantryItem`. Do not fix it as a side effect of a UI task; report it as a separate issue.
- Run the app: `./Start.ps1` (API + Frontend in separate windows), or `dotnet run --project FoodSense.BlazorFrontend`.
- `gh` CLI is not installed. Open PRs through the link printed by `git push`: `https://github.com/adanylkov/FoodSense/compare/master...<branch>`.
- No CI in the repo; the build and tests you run are the only gate.

## Definition of done
1. Code change.
2. `dotnet build` clean.
3. `dotnet test` compared with the pre-change baseline; any new failure must be fixed.
4. UI changes verified in a browser (DevTools: mobile viewport 375 px, "Emulate a touch device", measure the real rendered height of the changed element, check `getComputedStyle` for the classes you added).
5. Commit on a feature branch. Never commit directly to `master`.
6. Push, then provide the PR link with a body: what changed, measured before/after values, trade-offs, known issues.
7. Add a follow-up commit for feedback rather than amending a pushed commit.

## Conventions
- Razor components: `FoodSense.BlazorFrontend/Components/`. View models: `FoodSense.BlazorFrontend/Services/<feature>/`, registered in DI; tests live in `FoodSense.BlazorFrontend.Tests` (xunit + FluentAssertions + NSubstitute).
- Bootstrap 5.3.3. Custom utility classes go in `wwwroot/app.css` after `@import "bootstrap"`. Existing helpers: `.extra-small` (0.75rem text, 2rem min-height on buttons), `.rounded-circle`, `.product-thumb` (56x56 px, 88x88 px from `md`).
- Mobile-first: base rules = phone, enlargements in `@media (min-width: 768px)`.
- Touch targets: minimum 2rem (32 px), target 2.75rem (44 px). Reducing font size alone is not a valid mobile change.
- Branch names: lowercase with hyphens (`product-list-compact`). Commits: `feat:`, `fix:`, `refactor:`.
