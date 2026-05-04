# Orc.FilterBuilder

Orc.FilterBuilder is a WPF component that helps users extract key insights from their data by adding complex filtering functionalities to any application. It is part of the [WildGums Open Source](http://opensource.wildgums.com) ecosystem and is built on top of [Catel](http://github.com/catel/catel).

Key capabilities:

- Build complex filter expressions at runtime via a UI or programmatically.
- Serialize and restore filter schemes (XML / JSON).
- Evaluate filters against in-memory collections using LINQ expressions.
- Fully localizable and extensible condition types.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains a stable public API. Breaking changes break downstream applications.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

Public API snapshots are verified by the test suite (`PublicApiFacts`). If you intentionally change a public API, update the corresponding `.verified.txt` snapshot files in `src/Orc.FilterBuilder.Tests/`.

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.FilterBuilder | `master` |
| Orc.FilterBuilder | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Solution Overview

```
Orc.FilterBuilder          => Core library (filter models, conditions, serialization, services)
Orc.FilterBuilder.Xaml     => WPF UI controls (filter editor views, converters, etc.)
Orc.FilterBuilder.Tests    => NUnit test project covering both assemblies
Orc.FilterBuilder.Example  => Demo WPF application
```

### Directory Guide

| Directory / File | Editable? | Notes |
|-----------------|-----------|-------|
| `*.generated.cs` | No | Leave as-is |
| `src/Orc.FilterBuilder/` | Yes | Core logic |
| `src/Orc.FilterBuilder.Xaml/` | Yes | WPF controls |
| `src/Orc.FilterBuilder.Tests/` | Yes | Tests |
| `src/Orc.FilterBuilder.Example/` | Yes | Demo app |
| `src/Orc.FilterBuilder.Tests/PublicApiFacts.Orc_FilterBuilder_HasNoBreakingChanges_Async.verified.txt` | Yes | Public API snapshot for core assembly |
| `src/Orc.FilterBuilder.Tests/PublicApiFacts.Orc_FilterBuilder_Xaml_HasNoBreakingChanges_Async.verified.txt` | Yes | Public API snapshot for XAML assembly |
| `deployment/` | No | Build / release scripts |

---

## Writing Code

### Code Style

- Follow existing conventions in each file.
- Do not reformat unrelated code in a commit.
- Nullable reference types are enabled — annotate correctly.

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests.
2. Group tests in a nested class named after the feature or method under test.
3. Use Pascal / Snake case for test method names (e.g. `Feature_Does_Work`).

```csharp
[TestFixture]
public class TheMyMethod
{
    [TestCase]
    public void ReturnsExpectedResult()
    {
        var result = new MyClass().MyMethod();

        Assert.That(result, Is.EqualTo(42));
    }
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Public API Snapshots

`PublicApiFacts` verifies that neither assembly has unexpected API changes. If you intentionally change the public API:

1. Delete (or update) the relevant `.verified.txt` file under `src/Orc.FilterBuilder.Tests/`.
2. Run the tests — the Verify framework will regenerate the snapshot file.
3. Commit the updated snapshot.

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Documentation portal | http://opensource.wildgums.com |
| Catel (base framework) | http://github.com/catel/catel |
