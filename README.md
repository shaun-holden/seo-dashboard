# GymHub

Complete team management platform for gymnastics programs. Manage budgets, athletes, schedules, communications, payments, and more — with a dedicated parent portal for families.

## Features

### Communication

- **Announcements** — Post announcements to all parents or specific levels
- **Messages** — Direct messaging between admins and parents
- **Chat** — Real-time team chat rooms

### Team

- **Seasons** — Create and manage seasons with copy structure and import
- **Athletes** — Manage gymnasts, invite parents with invite codes
- **Roster** — Track apparel sizes and competition items per athlete
- **Manage Levels** — Organize athletes by team level with individualized budgets

### Schedule

- **Calendar** — Season calendar with meets, practices, and events
- **Meets** — Competition scheduling with venue details, GPS coordinates, and day-by-day schedules
- **Practices** — Recurring practice schedules by level

### Finance

- **Budget** — Track revenue and expenses by category with budget vs. actual
- **Budget Calculator** — What-if scenario modeling for per-athlete costs
- **Apparel** — Pricing and assignment for team apparel items
- **Competitions** — Competition entry fees and travel costs
- **Payments** — Track payments and apply credits per athlete
- **Payment Plans** — Auto-generated monthly installment plans
- **Stripe Integration** — Online payments (Pay Monthly, Pay in Full, Custom Amount, Auto-Pay)
- **iClassPro Billing** — Optional CSV import for billing integration

### Documents

- **Commitment Forms** — Digital forms for parent review and signature
- **Resources** — Upload and share documents with parents
- **Notes** — Internal admin notes per season

### Media

- **Gallery** — Photo albums shared with parents

### Admin

- **User Management** — Roles (Admin, Coach, Parent) and permissions
- **Two-Factor Authentication** — Optional 2FA for account security
- **Audit Log** — Track all admin actions
- **Backups** — Daily automated database backups
- **Maintenance Mode** — Temporarily disable parent access during updates

## Parent Portal

A separate green-themed portal for parents with mobile PWA support.

| Feature | Description |
| ------- | ----------- |
| **Fee Breakdown** | Shared fees, apparel, and competition costs per athlete |
| **Payment Plan** | Monthly installments with due dates on the 15th |
| **Stripe Payments** | Pay Monthly, Pay in Full, Custom Amount, or Auto-Pay |
| **Profile** | Emergency contacts, medical info, and apparel sizes |
| **Announcements** | View team announcements filtered by level |
| **Schedule** | Upcoming meets and practices |
| **Calendar** | Full season calendar view |
| **Messages** | Direct messaging with admin |
| **Chat** | Team chat participation |
| **Gallery** | View team photo albums |
| **Commitment Form** | Review and sign commitment forms digitally |

## Reports

All reports support CSV export.

| Report | Description |
| ------ | ----------- |
| **Budget vs Actual** | Compare budgeted and actual amounts by category |
| **Apparel** | Gymnast names, sizes, and item assignments |
| **Outstanding Balances** | Athletes with remaining balances and payment status |
| **Level Summary** | Financial summary per team level |
| **Attendance** | Meet and practice attendance tracking |
| **Payment Summary** | All payments received with date and method |
| **Parent Engagement** | Parent login activity and portal usage |

## Tech Stack

| Component | Technology |
| --------- | ---------- |
| Framework | Blazor Server (.NET 10) |
| Database | SQLite with Entity Framework Core |
| Auth | ASP.NET Identity (email/password, 2FA) |
| Payments | Stripe Checkout |
| Email | Resend API |
| Deployment | Railway (auto-deploy from `master`) |

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
| -------- | ----------- | ------- |
| `DB_PATH` | Path to SQLite database file | `gymbudget.db` |
| `STRIPE_SECRET_KEY` | Stripe secret key for payments | *(none)* |
| `Stripe__PublishableKey` | Stripe publishable key | *(none)* |
| `STRIPE_WEBHOOK_SECRET` | Stripe webhook signing secret | *(none)* |
| `RESEND_API_KEY` | Resend API key for sending emails | *(none)* |
| `RESEND_FROM_EMAIL` | From address for outgoing emails | *(none)* |
| `AdminEmail` | Default admin email address | `deshaun@tntgym.org` |
| `NotificationEmails` | JSON array of emails for admin notifications | *(none)* |
| `MaintenanceMode` | Set to `true` to block parent portal access | `false` |

## Security

- **Password Policy** — 8+ characters with uppercase, lowercase, and digit required
- **Account Lockout** — Automatic lockout after failed login attempts
- **Session Timeout** — Configurable session expiration
- **Security Headers** — CSP, HSTS, and other security headers enabled
- **Two-Factor Authentication** — Optional TOTP-based 2FA
- **Audit Logging** — All admin actions recorded with timestamps
- **Daily Backups** — Automated database backups with retention

## Deployment

The app deploys to Railway automatically when changes are pushed to the `master` branch. A `Dockerfile` is included for containerized deployment. Database migrations run automatically on startup.
