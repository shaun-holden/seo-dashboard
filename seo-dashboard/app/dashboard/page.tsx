export default function DashboardPage() {
  return (
    <div>
      <h1 className="text-2xl font-bold text-white mb-1">Dashboard</h1>
      <p className="text-gray-400 text-sm mb-8">Your SEO overview.</p>

      <div className="grid grid-cols-4 gap-4 mb-8">
        {[
          { label: 'Total Keywords', value: '1,284', change: '+12%' },
          { label: 'Avg. Position',  value: '14.3',  change: '-2.1' },
          { label: 'Tracked Pages',  value: '48',    change: '+3'   },
          { label: 'AI Suggestions', value: '27',    change: 'New'  },
        ].map((card) => (
          <div key={card.label} className="bg-gray-900 border border-gray-800 rounded-xl p-5">
            <p className="text-gray-400 text-xs uppercase tracking-wide">{card.label}</p>
            <p className="text-white text-2xl font-bold mt-1">{card.value}</p>
            <p className="text-blue-400 text-xs mt-1">{card.change}</p>
          </div>
        ))}
      </div>

      <div className="bg-gray-900 border border-gray-800 rounded-xl p-6">
        <h2 className="text-white font-semibold mb-4">Top Keywords</h2>
        <p className="text-gray-500 text-sm">Keyword table coming soon...</p>
      </div>
    </div>
  )
}