# RiverBooks Documentation

This folder contains the Hugo-based documentation site for RiverBooks, built with the [Doks](https://getdoks.org/) theme.

## Running Locally

```bash
cd docs
npm install
npm run dev
```

The site will be available at `http://localhost:1313/`.

## Content Standards

### Section Navigation

The Doks theme renders section headers (like "Architecture" or "Getting Started") as expand/collapse toggles in the sidebar, not as clickable navigation links. To ensure users can access section landing pages:

**Every section must have an `overview.md` page** with `weight: 1` that contains the main section content.

#### Correct Structure

```
content/docs/
├── architecture/
│   ├── _index.md           # Section metadata only (brief description)
│   ├── overview.md         # Main content (weight: 1) ← Users click this
│   ├── dependencies.md     # (weight: 2)
│   └── module-diagrams.md  # (weight: 3)
├── getting-started/
│   ├── _index.md           # Section metadata only
│   ├── overview.md         # Main content (weight: 1)
│   └── ef-migrations.md    # (weight: 2)
```

#### _index.md Template

Section `_index.md` files should contain only frontmatter and a brief description:

```markdown
---
title: "Section Name"
description: "Brief description for SEO"
summary: "Brief summary"
date: 2024-12-04
weight: 1
---

Brief description of this section.
```

> **Why keep `_index.md`?** Although `overview.md` contains the main content, `_index.md` is still required:
> - **Section definition** - Hugo requires it to treat the folder as a content section (branch bundle)
> - **Sidebar header** - The `title` becomes the section header text in navigation
> - **Section ordering** - The `weight` controls where sections appear relative to each other
> - **SEO metadata** - `description` and `summary` are used for search engines and previews
> - **Direct URL fallback** - If someone navigates directly to `/docs/architecture/`, Hugo renders this page

#### overview.md Template

The `overview.md` file contains the full section content:

```markdown
---
title: "Overview"
description: "Same as _index.md description"
summary: "Same as _index.md summary"
date: 2024-12-04
weight: 1
---

Full section content goes here...
```

### Page Weights

- `overview.md` should always have `weight: 1`
- Subsequent pages in the section should have `weight: 2`, `weight: 3`, etc.
- The `_index.md` weight controls section ordering in the sidebar (e.g., Getting Started = 1, Architecture = 2)

## Folder Structure

```
docs/
├── assets/           # CSS, JS, and images for theme customization
├── config/           # Hugo configuration
│   ├── _default/     # Default settings
│   └── production/   # Production overrides
├── content/          # Markdown content
│   └── docs/         # Documentation pages
├── diagrams/         # Source files for diagrams
├── layouts/          # Hugo layout overrides
├── static/           # Static assets (images, diagrams)
└── README.md         # This file
```

## Building for Production

```bash
npm run build
```

Output is generated in the `public/` folder.

## Deploying

The site is deployed to [riverbooks.ardalis.com](https://riverbooks.ardalis.com) via GitHub Pages or your preferred hosting.
