# Create New Repository Structure

RiverBooks is only a reference repository for modeling the folder layout and repository conventions of a new project template. It is not a framework, starter kit, shared platform, or required base repository for the new project. The reference repo can be found at [ardalis/RiverBooks](https://github.com/ardalis/RiverBooks).

Use this document as an agent-facing scaffold guide for creating a new repository that follows the overall RiverBooks folder structure and repository organization.

This file is about the whole repo layout.

It is intentionally separate from application architecture guidance such as module boundaries, use cases, and contracts. The goal here is to help an agent create the repository shell, top-level folders, project layout, and support assets in a consistent way before filling in feature code.

## Goal

Create a repository that has:

- A clean root folder with clear separation between source code, documentation, planning, and automation
- A `src/` folder that contains the actual application solution and projects
- A `docs/` folder for a publishable documentation site
- A `plans/` or agent-planning area for implementation planning artifacts
- Repository automation/configuration files at the root
- Minimal clutter at the root level

## RiverBooks Repo Shape

At a high level, RiverBooks is organized like this:

```text
RiverBooks/
├── .agents/
├── .github/
├── .gitignore
├── aspire.config.json
├── README.md
├── docs/
├── icon.png
├── plans/
├── src/
└── supporting markdown artifacts
```

That layout is worth preserving because it separates concerns cleanly:

- root-level repo operations and metadata
- source code under `src/`
- long-form docs under `docs/`
- lightweight planning artifacts outside the codebase

## Root Folder Rules

Use these rules when setting up a new repository.

### Keep the root shallow

The repository root should contain only:

- repo metadata
- automation/configuration files
- top-level human-readable docs
- a small number of purpose-driven folders

Do not dump project internals directly into the root.

### Reserve `src/` for executable application code

All solution files, app hosts, libraries, modules, tests, and code projects belong under `src/`.

Do not place implementation projects beside `README.md` at the root.

### Keep docs outside `src/`

RiverBooks uses a dedicated `docs/` site rather than mixing product documentation into source folders.

Preserve that pattern:

- engineering docs that should be browsed or published belong in `docs/`
- code comments and per-project notes stay near the code

### Keep planning artifacts outside `src/`

RiverBooks keeps planning separate from implementation. Preserve that separation.

Short-lived execution plans should not live inside application project folders.

## Recommended Top-Level Structure

Use a repository structure like this:

```text
NewProject/
├── .agents/                           # agent operating docs and plan conventions
├── .github/                           # GitHub automation and repo settings
├── .gitignore                         # repo ignore rules
├── README.md                          # primary repo entrypoint
├── aspire.config.json                 # Aspire/Codex local orchestration metadata if used
├── docs/                              # documentation website or docs hub
├── plans/                             # temporary or curated implementation plans
├── src/                               # all source code and solution files
├── icon.png                           # optional repo/app icon
└── Create New Application Project.md  # optional scaffold briefs for future agents
```

If the repo does not use Aspire, omit `aspire.config.json`.

If the repo does not use an icon or visual branding, omit `icon.png`.

## Folder-by-Folder Intent

### `.agents/`

Purpose:

- store agent-facing instructions that are specific to this repository
- define planning conventions
- preserve reusable workflow notes for future agent runs

Recommended contents:

```text
.agents/
├── PLANS.md
└── plans/
    ├── 2026-05-22-bootstrap.md
    └── completed/
```

Recommended rules:

- `PLANS.md` defines how plans are written and updated
- active plans live in `.agents/plans/`
- completed plans move to `.agents/plans/completed/`
- plan filenames should be date-prefixed and descriptive

RiverBooks currently has a root `plans/` folder plus an `.agents/PLANS.md` convention file. For a new repo, it is reasonable to standardize on either:

1. `plans/` for human-facing plans and `.agents/` for agent policy only
2. `.agents/plans/` for agent execution plans and `plans/` for higher-level roadmap documents

Pick one convention early and keep it consistent.

### `.github/`

Purpose:

- GitHub repository automation
- dependency update configuration
- workflows
- issue and pull request templates if desired

RiverBooks currently has a minimal `.github/` with `dependabot.yml`.

Recommended contents for a new repo:

```text
.github/
├── dependabot.yml
├── workflows/
│   ├── build.yml
│   ├── test.yml
│   └── docs.yml
└── pull_request_template.md
```

Keep this folder small and operational.

### `docs/`

Purpose:

- documentation site
- architecture writeups
- getting-started guides
- diagrams and static assets

RiverBooks uses Hugo with the Doks theme. You do not have to use the same stack, but you should preserve the separation between source code and documentation site assets.

Recommended docs structure:

```text
docs/
├── README.md
├── package.json
├── package-lock.json
├── .gitignore
├── assets/
├── config/
│   ├── _default/
│   └── production/
├── content/
│   ├── _index.md
│   └── docs/
├── diagrams/
├── layouts/
└── static/
```

Folder intent:

- `assets/`: custom CSS, JS, theme assets
- `config/`: docs site configuration
- `content/`: markdown pages
- `diagrams/`: source diagrams or generated diagram inputs
- `layouts/`: site template overrides
- `static/`: static images and already-built assets

If you are not creating a full docs site yet, you can still create a slim version:

```text
docs/
├── README.md
├── architecture/
├── getting-started/
└── images/
```

### `plans/`

Purpose:

- temporary project planning artifacts
- migration plans
- feature rollout notes
- staged implementation checklists

RiverBooks currently uses:

```text
plans/
└── add-aspire-plan.md
```

Recommended use:

- keep only active or noteworthy plans here
- avoid turning this into permanent documentation
- once a plan is finished, either archive it in `.agents/plans/completed/` or delete it if it has no enduring value

### `src/`

Purpose:

- all application source code
- solution files
- project-level code style configuration
- runtime hosts
- modules
- tests

This is the most important folder in the repo and should stay focused on implementation.

Recommended `src/` structure for a new modular monolith:

```text
src/
├── .editorconfig
├── Directory.Build.props
├── NewProject.slnx
├── NewProject.AppHost/
├── NewProject.Api/
├── NewProject.Blazor/
├── NewProject.Sdk/
├── NewProject.ServiceDefaults/
├── NewProject.SharedKernel/
├── NewProject.ModuleA/
├── NewProject.ModuleA.Contracts/
├── NewProject.ModuleB/
├── NewProject.ModuleB.Contracts/
├── NewProject.ModuleA.Tests/
└── NewProject.ArchitectureTests/
```

For a RiverBooks-like backend without the Blazor/Kiota additions, the host might be `NewProject.Web`. For future projects in this repo family, prefer:

- `NewProject.Api`
- `NewProject.Blazor`
- `NewProject.Sdk`

That naming makes the frontend/backend boundary explicit.

## Files That Belong at the Root

### `README.md`

The root `README.md` should be the primary entrypoint for humans.

It should include:

- what the project is
- why it exists
- core architecture summary
- how to build/run
- where to find full docs

### `.gitignore`

The root `.gitignore` should handle:

- .NET build outputs
- IDE artifacts
- docs-site build outputs
- local secrets or environment files as appropriate

RiverBooks uses a broad Visual Studio oriented ignore file, which is a solid base for a .NET repo.

### `aspire.config.json`

Include this only if the repo uses Aspire and local orchestration tooling that depends on it.

### Visual assets

Optional files such as `icon.png` are fine at the root when they represent repo/app branding and are used by tooling or documentation.

### Scaffold docs

If the repo is meant to be a template or repeated by agents, keeping scaffold instructions at the root is reasonable.

Examples:

- `Create New Application Project.md`
- `Create New Repository Structure.md`

## Source Folder Standards

Inside `src/`, keep a few repo-wide files near the solution:

### `.editorconfig`

Purpose:

- repository-wide code style
- formatting and naming rules

This should live in `src/` if it is intended to govern source projects specifically, as RiverBooks does.

### `Directory.Build.props`

Purpose:

- shared MSBuild properties for every project

Use this for:

- target framework
- nullable settings
- implicit usings
- shared build defaults

### Solution file

Prefer placing the main solution file in `src/`.

Example:

```text
src/NewProject.slnx
```

This keeps the root cleaner and makes `src/` the obvious place to build from.

## Recommended Repo Creation Order for Agents

When bootstrapping a new repo, the agent should create folders in this order.

### Phase 1: Root shell

Create:

- `.github/`
- `.agents/`
- `docs/`
- `plans/`
- `src/`
- `README.md`
- `.gitignore`

### Phase 2: Source shell

Inside `src/`, create:

- `.editorconfig`
- `Directory.Build.props`
- main solution file
- app host projects
- shared projects
- initial business modules
- test projects

### Phase 3: Documentation shell

Inside `docs/`, create:

- docs README
- content structure
- asset/config folders
- architecture and getting-started sections

### Phase 4: Automation shell

Inside `.github/`, create:

- dependency update config
- CI workflows
- docs publishing workflow if the docs site is published

### Phase 5: Agent operations shell

Inside `.agents/`, create:

- `PLANS.md`
- optional `.agents/plans/`
- repo-specific operating notes if needed

## Recommended Agent Instructions

When using this repository structure as a template, the agent should follow these rules.

### Rule 1: Create a clean root

Do not create unnecessary top-level directories.

Prefer a root that is easy to scan in Finder, GitHub, and terminal output.

### Rule 2: Put all code under `src/`

Do not place projects, test assemblies, or solution support files at the repository root unless there is a compelling tool constraint.

### Rule 3: Separate docs from code

If content is meant to teach, explain, onboard, or publish, put it in `docs/`.

If content is meant to compile, run, or test, put it in `src/`.

### Rule 4: Separate plans from docs

Plans are execution artifacts, not stable product documentation.

Do not mix plans into architecture docs unless they are intentionally promoted into permanent documentation.

### Rule 5: Make the repo self-describing

A new contributor or agent should be able to infer:

- where the code lives
- where docs live
- where planning artifacts live
- where repo automation lives

just by looking at the root folders.

## Template Tree for a New Repo

Use this as the default target:

```text
NewProject/
├── .agents/
│   ├── PLANS.md
│   └── plans/
│       └── completed/
├── .github/
│   ├── dependabot.yml
│   └── workflows/
├── .gitignore
├── README.md
├── aspire.config.json
├── docs/
│   ├── README.md
│   ├── assets/
│   ├── config/
│   ├── content/
│   ├── diagrams/
│   ├── layouts/
│   └── static/
├── plans/
├── src/
│   ├── .editorconfig
│   ├── Directory.Build.props
│   ├── NewProject.slnx
│   ├── NewProject.AppHost/
│   ├── NewProject.Api/
│   ├── NewProject.Blazor/
│   ├── NewProject.Sdk/
│   ├── NewProject.ServiceDefaults/
│   ├── NewProject.SharedKernel/
│   ├── NewProject.ModuleA/
│   ├── NewProject.ModuleA.Contracts/
│   ├── NewProject.ModuleB/
│   ├── NewProject.ModuleB.Contracts/
│   ├── NewProject.ModuleA.Tests/
│   └── NewProject.ArchitectureTests/
├── icon.png
├── Create New Application Project.md
└── Create New Repository Structure.md
```

## What to Preserve from RiverBooks Specifically

If using RiverBooks as the model, preserve these repo-level decisions:

- source code is centralized under `src/`
- solution-wide defaults live in `src/Directory.Build.props`
- code style lives in `src/.editorconfig`
- documentation is treated as a real product under `docs/`
- planning is separate from implementation
- repo automation is lightweight and lives in `.github/`
- root-level clutter is minimal

## What to Modernize for a New Repo

For future projects, improve a few things while keeping the same spirit:

- standardize whether plans live in `plans/`, `.agents/plans/`, or both
- rename `Web` to `Api` when the host is primarily an HTTP backend
- add `Blazor` and `Sdk` projects explicitly under `src/`
- add CI workflows in `.github/workflows/`
- include a docs publishing workflow if docs are first-class

## Deliverable Checklist

The repository shell is ready when:

- the root folder is clean and purpose-driven
- `src/` contains all code projects and the solution
- `docs/` is ready for long-form documentation
- `.github/` holds repo automation
- `.agents/` holds agent operating conventions
- planning artifacts have a clear home
- `README.md` explains how to orient in the repo

## Summary

RiverBooks has a strong repo shape because it is simple and disciplined:

- code in `src/`
- docs in `docs/`
- plans outside code
- automation in `.github/`
- conventions near the root

That is the pattern to replicate for a new project.
