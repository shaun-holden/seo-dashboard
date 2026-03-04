# GymBudgetApp

A budget tracking application for gymnastics programs and other project types. Track revenue, expenses, payroll, and financial metrics across seasons.

## Features

- **Project Types** — Choose between Competition (gym-specific defaults) or Custom (define your own labels and features)
- **Season Management** — Create and manage budget seasons (e.g., 2026-2027)
- **Budget Tracking** — Track revenue and expenses by category with budget vs. actual comparison
- **Coach Management** — Manage coaching staff and assignments (or custom-labeled staff)
- **Meet Scheduling** — Track competitions with travel type (drive/fly) (or custom-labeled events)
- **Per Diem** — Calculate meal allowances per coach per meet (optional per project type)
- **Mileage Tracking** — Track travel mileage and reimbursement (optional per project type)
- **Team Levels** — Organize athletes by level with payment plan calculations (optional per project type)
- **Reports** — Generate financial reports by season or meet
- **Copy Structure** — Copy coaches, meets, and team levels from one season to another
- **Import Budget** — Import a full season's data from another user's account

## Tech Stack

- **Framework**: Blazor Server (.NET 10)
- **Database**: SQLite with Entity Framework Core
- **Auth**: ASP.NET Identity (email/password)
- **Deployment**: Railway (auto-deploy from `master`)

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run Locally

```bash
git clone https://github.com/shaun-holden/GymBudgetApp.git
cd GymBudgetApp
dotnet run
```

The app will be available at `http://localhost:5224`.

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_PATH` | Path to SQLite database file | `gymbudget.db` |

## How to Use

### 1. Create an Account

- Open the app and click **Register**
- Enter your email and a password (8+ characters, must include uppercase, lowercase, and a number)
- Log in with your new credentials

### 2. Create a Season

- Click **Seasons** in the sidebar
- Click **+ New Season**
- Enter a season name (e.g., "2026-2027"), total athlete count, and check "Active Season"
- Choose a **Project Type**:
  - **Competition** — Uses default gym terminology (Meets, Coaches, Team Levels, Athlete Items) with all features enabled
  - **Custom** — Define your own labels for each entity and choose which features to enable (Team Levels, Per Diem, Mileage)
- Click **Save**

### 3. Add Coaches

- Click **Coaches** in the sidebar
- Select your season from the dropdown at the top
- Click **+ Add Coach**
- Enter the coach's name and role (e.g., "Head Coach", "Compulsory 1")
- Click **Save**

### 4. Add Meets

- Click **Meets** in the sidebar
- Select your season from the dropdown
- Click **+ Add Meet**
- Enter the meet name, date, and travel type (Drive or Fly)
- Click **Save**

### 5. Enter Budget Data

- Click **Budget** in the sidebar
- Select your season from the dropdown
- Click **+ Add Item**
- Choose a category (Team Entry, Flight, Hotel, Car Rental, Per Diem, etc.)
- Enter the rate, quantity, and whether this is a budgeted or actual expense
- Optionally link it to a specific meet or coach
- Click **Save**

### 6. Track Per Diem

- Click **Per Diem** in the sidebar
- Select a meet from the dropdown
- Click **+ Add Entry**
- Choose a coach, meal type (Breakfast, Lunch, Dinner), daily rate, and number of days
- Mark as budgeted or actual
- Click **Save**

### 7. Track Mileage

- Click **Mileage** in the sidebar
- Select a meet from the dropdown
- Click **+ Add Entry**
- Choose a coach, enter miles driven and rate per mile
- Mark as budgeted or actual
- Click **Save**

### 8. Set Up Team Levels

- Click **Team Levels** in the sidebar
- Select your season from the dropdown
- Click **+ Add Team Level**
- Enter the level name (e.g., "Gym Stars", "Elite/HOPE"), athlete count, and payment plan months
- Click **Save**
- Add athlete items (fees, uniforms, etc.) under each level with name, cost, and whether it's required

### 9. View Reports

- Click **Reports** in the sidebar
- Select a season and optionally filter by meet or category
- View budget vs. actual comparisons across all your data

### 10. Copy a Season's Structure

When starting a new season, you don't have to re-enter everything from scratch:

- Go to **Seasons** in the sidebar
- Find the season you want to copy from
- Click the **Copy Structure** button
- Select the target season to copy into
- Review the preview (coaches, meets, team levels)
- Click **Copy**

This copies coaches, meets, team levels, and athlete items — but **not** budget amounts, per diem, or mileage, so you start fresh with a clean slate.

### 11. Import Budget from Another User

- Click **Import Budget** in the sidebar
- Enter the other user's email address and click **Search**
- Select their season and your target season
- Review the preview and click **Import**

This copies the full structure **and** budget amounts from another user's account into yours.

## Deployment

The app deploys to Railway automatically when changes are pushed to the `master` branch. A `Dockerfile` is included for containerized deployment. Database migrations run automatically on startup.
