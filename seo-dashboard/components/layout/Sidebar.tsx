'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import {
  LayoutDashboard,
  Search,
  BarChart2,
  FileText,
  Sparkles,
  Settings,
} from 'lucide-react'

const navItems = [
  { label: 'Dashboard',   href: '/dashboard', icon: LayoutDashboard },
  { label: 'Keywords',    href: '/keywords',  icon: Search },
  { label: 'SERP',        href: '/serp',      icon: BarChart2 },
  { label: 'Content',     href: '/content',   icon: FileText },
  { label: 'AI Insights', href: '/insights',  icon: Sparkles },
  { label: 'Settings',    href: '/settings',  icon: Settings },
]

export default function Sidebar() {
  const pathname = usePathname()

  return (
    <aside className="fixed left-0 top-0 h-screen w-56 bg-gray-900 border-r border-gray-800 flex flex-col">
      <div className="px-5 py-5 border-b border-gray-800">
        <span className="text-white font-semibold text-lg tracking-tight">
          SEO<span className="text-blue-400">Dash</span>
        </span>
        <p className="text-gray-500 text-xs mt-0.5">Keyword Intelligence</p>
      </div>

      <nav className="flex-1 px-3 py-4 space-y-1">
        {navItems.map(({ label, href, icon: Icon }) => {
          const active = pathname === href
          return (
            <Link
              key={href}
              href={href}
              className={`flex items-center gap-3 px-3 py-2 rounded-lg text-sm transition-colors ${
                active
                  ? 'bg-blue-600 text-white'
                  : 'text-gray-400 hover:text-white hover:bg-gray-800'
              }`}
            >
              <Icon size={16} />
              {label}
            </Link>
          )
        })}
      </nav>

      <div className="px-4 py-4 border-t border-gray-800">
        <p className="text-white text-sm font-medium">TNT Gym</p>
        <p className="text-gray-500 text-xs">Pro Plan</p>
      </div>
    </aside>
  )
}