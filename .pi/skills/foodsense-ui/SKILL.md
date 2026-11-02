---
name: foodsense-ui
description: Mobile-first UI/CSS rules for FoodSense.BlazorFrontend. Use when changing Razor markup, Bootstrap classes, component sizing, spacing, typography, colors, or responsive behaviour.
---

# Blazor UI / CSS skill

## Before editing markup
1. Read the component and its parent (`ProductList` -> `ProductListItem`, `ProductCard`).
2. `rg -n 'class="[^"]*"'` in the component, then check every class:
   - defined in `FoodSense.BlazorFrontend/wwwroot/app.css`,
   - or a real Bootstrap 5.3 class available in `bootstrap.css` (imported by `App.razor`),
   - otherwise the class is dead and must be defined in `app.css`.
3. `rg -l "\.<class>" FoodSense.BlazorFrontend/Components` to see every component sharing the class. Changing a shared class changes them all — report that.
4. Check responsive behaviour of the same markup: mobile base, `md`/`lg` overrides, `d-lg-inline` style visibility rules.
5. Measure the current size in the browser (DevTools: `getBoundingClientRect().height` on the element, e.g. `.product-item`). Record the number as the baseline.

## Mobile-first sizing rules
- Base rules target phones, enlargements go into `@media (min-width: 768px)`.
- Fixed-size media must be sized explicitly (e.g. `.product-thumb { width: 56px; height: 56px }` + `img { object-fit: contain }`), otherwise images render at their natural size.
- Text-only shrinking is not a mobile layout. Keep the container width constraint (`min-width: 0` plus `min-width-0` class on grid children) so long words cannot widen the container.
- Preserve touch targets: `.btn.btn-sm.extra-small` and `.btn.btn-sm.rounded-circle` keep `min-height: 2rem`. Add `min-width: 2rem` to icon-only circle buttons.
- Use `min-height`, not `height`, so content can grow.
- Keep visual hierarchy: title readable, metadata small, numeric badges large.

## Verification (required for UI tasks)
- `dotnet build "FoodSense Aspire.sln"` — 0 warnings, 0 errors.
- `dotnet test` on all three test projects, compared with the baseline.
- Browser at 375 px viewport with touch emulation, dark mode, long product names, images from external URLs (measure the row height at 56 px, 80 px, 145 px source widths).
- Check `md`+ layout is unchanged: measure at 992 px.
- Verify with DevTools that the new rules apply (`getComputedStyle`) and that no Bootstrap utility you rely on is missing.
- Record measured before/after heights and font sizes; put them in the commit and PR body.

## Known constraints
- `App.razor` loads `app.css` and the bundle stylesheet only. Bootstrap utility CSS is not loaded, so `object-fit-contain`, `min-width-0`, `d-none` need local definitions.
- `package.json` has Tailwind, `tailwind.css`, `tailwind.config.js`, PostCSS build scripts, and a `wwwroot/app.tailwind.css`, but the Tailwind stylesheet is not linked; `.App-Root` in `app.css` uses Tailwind-style class names that are currently dead.
- Images use external URLs with `aspect-ratio-1x1`-style classes that are not defined; keep `object-fit: contain` for thumbnails.
- Bootstrap 5.3 `btn-circle`-style sizing: `.btn` has a `min-height` baseline of 2rem for `btn-sm`, 2.5rem for default, 3.2rem for `btn-lg`.

## Report
In the PR/commit message include:
- what markup and CSS changed,
- measured before/after values,
- side effects on other components,
- trade-offs that were not fixed (touch-target sizes, text contrast, Tailwind dead code),
- known pre-existing test failures.
