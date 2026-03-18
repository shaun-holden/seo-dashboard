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
| `STRIPE_SECRET_KEY` | Stripe secret key for payments | *(none)* |
| `Stripe__PublishableKey` | Stripe publishable key | *(none)* |
| `Stripe__WebhookSecret` | Stripe webhook signing secret | *(none)* |
| `SMTP_EMAIL` | Gmail address for sending emails | *(none)* |
| `SMTP_PASSWORD` | Gmail App Password for SMTP | *(none)* |

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

### 11. Add Athletes and Invite Parents

- Click **Athletes** in the sidebar
- Select your season from the dropdown
- Click **+ Add Athlete**, enter their name and assign a team level
- Click **Invite** next to an athlete to send a parent invite
- Enter the parent's email and click **Send** — they'll receive an email with an invite code and a registration link
- To regenerate a code, click the reload button next to an existing code

### 12. Parent Portal

Parents register at the app and select **"Parent"** as their account type. After registering:

- Go to **Link Athlete** and enter the invite code provided by the admin
- The **My Athletes** page shows:
  - Fee breakdown (shared fees + required items + optional items)
  - Payment summary (total fees, paid, credits, balance)
  - 8-month payment plan with due dates on the 15th of each month
  - Three payment options: monthly installment, pay in full, or custom amount
  - Payment history with credits and payments
  - Upcoming events
- Parents can toggle optional items on/off to update their balance
- Required items and shared fees are locked and cannot be changed

### 13. Payments and Credits (Admin)

- Click **Payments** in the sidebar
- Select a season to view all athlete payment statuses
- Click **+ Apply Credit** to apply a credit to an athlete's account (e.g., overpayment, sibling discount)
- Enter the athlete, amount, and reason — the credit reduces their balance immediately
- Parents see credits in their payment history

### 14. Stripe Payments Setup

To enable online payments:

1. Get your API keys from [Stripe Dashboard](https://dashboard.stripe.com/apikeys)
2. Set environment variables:
   - `STRIPE_SECRET_KEY` — your secret key (`sk_live_...`)
   - `Stripe__PublishableKey` — your publishable key (`pk_live_...`)
3. Set up a webhook at [Stripe Webhooks](https://dashboard.stripe.com/webhooks):
   - Endpoint URL: `https://your-app-url/api/stripe-webhook`
   - Event: `checkout.session.completed`
   - Set `Stripe__WebhookSecret` to the signing secret (`whsec_...`)

### 15. Email Setup (Gmail SMTP)

To enable invite emails and password reset:

1. Go to [Google App Passwords](https://myaccount.google.com/apppasswords)
2. Generate an app password for your Gmail account
3. Set environment variables:
   - `SMTP_EMAIL` — your Gmail address (e.g., `deshaun@tntgym.org`)
   - `SMTP_PASSWORD` — the app password generated above

### 16. Password Reset

- On the **Login** page, click **"Forgot Password?"**
- Enter your email address and click **Send Reset Link**
- Check your email for a reset link
- Click the link and set a new password

### 17. Import Budget from Another User

- Click **Import Budget** in the sidebar
- Enter the other user's email address and click **Search**
- Select their season and your target season
- Review the preview and click **Import**

This copies the full structure **and** budget amounts from another user's account into yours.

## Deployment

The app deploys to Railway automatically when changes are pushed to the `master` branch. A `Dockerfile` is included for containerized deployment. Database migrations run automatically on startup.
