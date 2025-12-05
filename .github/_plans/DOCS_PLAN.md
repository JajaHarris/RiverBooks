# RiverBooks Documentation Site Plan

This document outlines the complete plan for setting up a documentation site for RiverBooks using Hugo with the Doks theme, hosted on GitHub Pages.

## Overview

- **Framework:** Hugo (static site generator)
- **Theme:** Doks (by Thulite) - a modern documentation theme
- **Hosting:** GitHub Pages
- **Deployment:** GitHub Actions (automatic on push to main/docs branch)
- **URL:** `https://ardalis.github.io/RiverBooks/` (or custom domain if desired)

---

## Prerequisites

### 🤖 Copilot Will Handle
- Creating the Doks project structure in `docs/` folder
- Setting up the GitHub Actions workflow for deployment
- Configuring Hugo/Doks for the RiverBooks project
- Migrating existing content (like the dynamic diagram HTML)
- Creating initial documentation pages

### 👤 You Need to Have/Do
- **Node.js** (LTS version, v20+) installed locally for development
- **Hugo Extended** (latest version) installed locally for development
- **GitHub repository write access** to configure Pages settings

---

## Implementation Steps

### Phase 1: Project Setup

#### Step 1.1 - Create Doks Project Structure
**🤖 Copilot will do this**

Create a new Doks documentation site in the `docs/` folder with:
- Hugo configuration files
- Doks theme as npm dependency
- Basic content structure
- Package.json with build scripts

#### Step 1.2 - Configure for GitHub Pages
**🤖 Copilot will do this**

- Set `baseURL` in production config to `https://ardalis.github.io/RiverBooks/`
- Configure paths to work with GitHub Pages subdirectory hosting

#### Step 1.3 - Create GitHub Actions Workflow
**🤖 Copilot will do this**

Create `.github/workflows/docs-deploy.yml` with:
- Trigger on push to `main` branch (or configurable)
- Hugo Extended installation
- Node.js setup and npm install
- Build and deploy to GitHub Pages

---

### Phase 2: Content Migration & Setup

#### Step 2.1 - Migrate Existing Diagram
**🤖 Copilot will do this**

- Move `docs/riverbooks.dynamic.html` to appropriate location in new structure
- Ensure it's accessible at a predictable URL
- Update README.md link to point to GitHub Pages URL

#### Step 2.2 - Create Initial Documentation Pages
**🤖 Copilot will do this**

Suggested initial structure:
```
docs/
├── content/
│   ├── _index.md                 # Home page
│   ├── getting-started/
│   │   ├── _index.md             # Overview
│   │   ├── installation.md       # Setup instructions
│   │   └── ef-migrations.md      # EF migration content from README
│   └── architecture/
│       ├── _index.md             # Architecture overview
│       └── dependencies.md       # Dependency diagrams page
```

---

### Phase 3: GitHub Configuration

#### Step 3.1 - Enable GitHub Pages
**👤 You need to do this**

1. Go to repository **Settings** → **Pages**
2. Under "Build and deployment":
   - **Source:** Select "GitHub Actions"
3. Save changes

> **Note:** This is a one-time setup. The GitHub Actions workflow will handle deployments automatically after this.

#### Step 3.2 - Verify GitHub Actions Permissions
**👤 You need to verify this**

1. Go to repository **Settings** → **Actions** → **General**
2. Under "Workflow permissions":
   - Ensure "Read and write permissions" is selected
   - OR ensure "Read repository contents and packages permissions" is selected with Pages write access

---

### Phase 4: Deployment & Verification

#### Step 4.1 - Push Changes and Deploy
**👤 You need to do this**

1. Review the changes Copilot has made
2. Commit and push to the `main` branch (or merge the docs branch)
3. Monitor the GitHub Actions workflow in the "Actions" tab

#### Step 4.2 - Verify Deployment
**👤 You need to do this**

1. Wait for the GitHub Action to complete (~1-2 minutes)
2. Visit `https://ardalis.github.io/RiverBooks/`
3. Verify the documentation site loads correctly
4. Check that the dependency diagram is accessible

---

### Phase 5: Optional - Custom Domain Setup

**👤 You need to do this (if desired)**

If you want to use a custom domain like `docs.riverbooks.com`:

#### Step 5.1 - DNS Configuration
Add one of the following DNS records at your domain provider:

**For apex domain (riverbooks.com):**
```
Type: A
Name: @
Value: 185.199.108.153
       185.199.109.153
       185.199.110.153
       185.199.111.153
```

**For subdomain (docs.riverbooks.com):**
```
Type: CNAME
Name: docs
Value: ardalis.github.io
```

#### Step 5.2 - GitHub Pages Custom Domain
1. Go to repository **Settings** → **Pages**
2. Enter your custom domain in "Custom domain"
3. Check "Enforce HTTPS" (after DNS propagates)

#### Step 5.3 - Update Hugo Config
**🤖 Copilot can help with this**
- Update `baseURL` in production config to the custom domain

---

## Local Development

### First Time Setup
```powershell
cd docs
npm install
```

### Run Development Server
```powershell
cd docs
npm run dev
```
Site will be available at `http://localhost:1313/RiverBooks/`

### Build for Production
```powershell
cd docs
npm run build
```
Output will be in `docs/public/`

---

## File Structure After Implementation

```
RiverBooks/
├── .github/
│   └── workflows/
│       └── docs-deploy.yml          # GitHub Actions workflow
├── docs/
│   ├── assets/                      # Custom CSS/JS
│   ├── config/
│   │   ├── _default/
│   │   │   ├── hugo.toml            # Base Hugo config
│   │   │   ├── menus.toml           # Navigation menus
│   │   │   └── params.toml          # Theme parameters
│   │   └── production/
│   │       └── hugo.toml            # Production overrides (baseURL)
│   ├── content/
│   │   ├── _index.md                # Home page
│   │   ├── getting-started/         # Getting started section
│   │   └── architecture/            # Architecture documentation
│   ├── static/
│   │   └── diagrams/                # Static files (SVGs, HTML diagrams)
│   │       └── riverbooks.dynamic.html
│   ├── package.json
│   └── package-lock.json
├── src/                             # Existing source code
├── README.md                        # Updated with docs link
└── DOCS_PLAN.md                     # This file
```

---

## Summary Checklist

| Step | Description | Who |
|------|-------------|-----|
| 1 | Create Doks project structure | 🤖 Copilot |
| 2 | Configure GitHub Actions workflow | 🤖 Copilot |
| 3 | Create initial documentation content | 🤖 Copilot |
| 4 | Migrate existing diagram | 🤖 Copilot |
| 5 | Enable GitHub Pages in repo settings | 👤 You |
| 6 | Verify Actions permissions | 👤 You |
| 7 | Push changes / merge PR | 👤 You |
| 8 | Verify deployment | 👤 You |
| 9 | (Optional) Configure custom domain DNS | 👤 You |
| 10 | (Optional) Set custom domain in GitHub | 👤 You |

---

## Next Steps

Once you've reviewed this plan, let me know and I'll proceed with:
1. Scaffolding the Doks documentation site
2. Creating the GitHub Actions workflow
3. Setting up initial content pages
4. Migrating your existing diagram

Ready to proceed? Just say "go" or let me know if you'd like any adjustments to the plan!
