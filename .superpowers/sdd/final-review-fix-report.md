Status: DONE

Files changed:
- `src/Crawler.Core/Cards/CardDefinition.cs`
- `src/Crawler.Content/CardCatalog.cs`
- `tests/Crawler.Content.Tests/CardCatalogTests.cs`
- `tests/Crawler.Core.Tests/MultiplierApplicatorTests.cs`

Commands run and outcomes:
- `Get-ChildItem -Force` -> confirmed workspace contents.
- `Get-Content` on the relevant skill files -> reviewed `using-superpowers`, `verification-before-completion`, `systematic-debugging`, and `test-driven-development`.
- `Get-Content` / `Select-String` / `ConvertFrom-Json` on the catalog and tests -> confirmed the missing rarity mapping, existing call sites, starter card rarities, and allowed rarity set.
- `C:\Users\Lenovo\.dotnet\dotnet.exe test Crawler.sln` -> passed, 0 failures.

Commit hash:
- Pending at report write time; commit created after this report is saved.

Self-review:
- Added `Rarity` to the core card model and wired the catalog loader to preserve it from JSON.
- Updated all direct `CardDefinition` constructors in tests to supply an explicit rarity.
- Strengthened catalog tests to assert the six starter-facing cards keep their expected rarity and that every card carries a nonblank rarity from the catalog's allowed set.

Concerns:
- None known. The fix is narrowly scoped and the solution test pass is green.
