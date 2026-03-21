# GymHub

Complete team management platform for gymnastics programs. Manage budgets, athletes, schedules, communications, payments, and more — with a dedicated parent portal for families.

## Features

### Communication

- **Announcements** — Post announcements to all parents or specific levels, with push notifications
- **Messages** — Direct messaging between admins and parents
- **Chat** — Real-time team chat rooms with emoji picker and profile pictures

### Team

- **Seasons** — Create and manage seasons with copy structure and import
- **Athletes** — Manage gymnasts, invite parents with invite codes, delete confirmation with parent link warnings
- **Roster** — Track apparel sizes and competition items per athlete
- **Manage Levels** — Organize athletes by team level with individualized budgets
- **Emergency Contacts** — View emergency contacts and medical notes for all athletes (employee accessible)

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
- **Payment Plans** — Auto-generated monthly installment plans with bulk level updates
- **Payment Plan Requests** — Parents can request alternate payment plans, admin reviews and approves
- **Stripe Integration** — Online payments (Pay Monthly, Pay in Full, Custom Amount, Auto-Pay)
- **iClassPro Billing** — Optional CSV import for billing integration
- **Auto-Expire Payments** — Abandoned Stripe payments auto-expire after 30 minutes

### Documents

- **Commitment Forms** — Digital forms for parent review and signature
- **Resources** — Upload and share documents with parents
- **Notes** — Internal admin notes per season

### Media

- **Gallery** — Photo albums shared with parents, auto-resize on upload (up to 10MB), lazy-loaded for performance

### Admin

- **User Management** — Roles (Admin, Coach, Parent) and permissions
- **Two-Factor Authentication** — Optional 2FA for account security
- **Audit Log** — Track all admin actions
- **Backups** — Daily automated database backups
- **Maintenance Mode** — Temporarily disable parent access during updates
- **Billing Report** — View billing preferences (Stripe, iClassPro, Manual) by athlete

## Parent Portal

A separate green-themed portal for parents with mobile PWA support and push notifications.

| Feature | Description |
| ------- | ----------- |
| **Fee Breakdown** | Shared fees, apparel, and competition costs per athlete |
| **Payment Plan** | Monthly installments with due dates on the 15th |
| **Stripe Payments** | Pay Monthly, Pay in Full, Custom Amount, or Auto-Pay |
| **Profile** | Display name, profile picture, push notification toggle |
| **Child Profiles** | Emergency contacts, medical info per linked athlete |
| **Payment Plan Requests** | Request alternate payment plans for review |
| **Announcements** | View team announcements filtered by level |
| **Push Notifications** | Get notified on phone for new announcements and events |
| **Schedule** | Upcoming meets and practices |
| **Calendar** | Full season calendar view |
| **Messages** | Direct messaging with admin |
| **Chat** | Team chat with emoji picker and profile pictures |
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
| **Billing Preferences** | Stripe Auto-Pay vs iClassPro vs Manual billing by athlete |

## Tech Stack

| Component | Technology |
| --------- | ---------- |
| Framework | Blazor Server (.NET 10) |
| Database | SQLite with Entity Framework Core |
| Auth | ASP.NET Identity (email/password, 2FA) |
| Payments | Stripe Checkout |
| Email | Resend API |
| Push Notifications | Web Push (VAPID) |
| PWA | Service worker with offline caching |
| Mobile App | Capacitor (iOS/Android wrapper) |
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
| `VAPID__PublicKey` | VAPID public key for web push notifications | *(none)* |
| `VAPID__PrivateKey` | VAPID private key for web push notifications | *(none)* |
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
- **Delete Safeguards** — Confirmation with parent link warnings before deleting athletes

## Deployment

The app deploys to Railway automatically when changes are pushed to the `master` branch. A `Dockerfile` is included for containerized deployment. Database migrations run automatically on startup.
