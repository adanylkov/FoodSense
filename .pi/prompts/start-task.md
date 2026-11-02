---
description: Start a task — audit the repo state before changing any code
argument-hint: "[task]"
---

Zadanie: ${1:-kontynuuj ostatnią niedokończoną pracę}.

Before editing anything:

1. `git status --short`, `git diff`, `git log --oneline -10`, `git branch --show-current`.
2. If there are uncommitted changes, audit them: which part of the task is already done, which is missing, and whether the markup references CSS classes that exist in `wwwroot/app.css` (classes used in `.razor` are often undefined and therefore dead).
3. Report the audit to me in Polish: what is done, what is missing, what is broken, with measured numbers.
4. Then propose the plan (files, commands, verification steps) and state the trade-offs you are not sure about. Wait for my confirmation before writing code.

Use the `foodsense-ui` skill for any UI/CSS work. Do not trust my description of the previous session — verify it with commands.
