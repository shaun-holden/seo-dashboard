namespace GymBudgetApp.Services
{
    public class BackupService : BackgroundService
    {
        private readonly ILogger<BackupService> _logger;
        private readonly IConfiguration _configuration;

        public BackupService(ILogger<BackupService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    CreateBackup();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Database backup failed");
                }

                // Run once per day
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        public void CreateBackup()
        {
            var dbFolder = Environment.GetEnvironmentVariable("DB_PATH")
                ?? Directory.GetCurrentDirectory();
            var dbPath = Path.Combine(dbFolder, "gymbudget.db");
            var backupFolder = Path.Combine(dbFolder, "backups");
            Directory.CreateDirectory(backupFolder);

            if (!File.Exists(dbPath))
            {
                _logger.LogWarning("Database file not found at {Path}", dbPath);
                return;
            }

            // Create dated backup
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var backupPath = Path.Combine(backupFolder, $"gymbudget-{timestamp}.db");
            File.Copy(dbPath, backupPath, overwrite: true);
            _logger.LogInformation("Database backed up to {Path}", backupPath);

            // Keep only last 7 backups
            var backups = Directory.GetFiles(backupFolder, "gymbudget-*.db")
                .OrderByDescending(f => f)
                .Skip(7)
                .ToList();
            foreach (var old in backups)
            {
                File.Delete(old);
                _logger.LogInformation("Deleted old backup {Path}", old);
            }
        }

        public string GetDbPath()
        {
            var dbFolder = Environment.GetEnvironmentVariable("DB_PATH")
                ?? Directory.GetCurrentDirectory();
            return Path.Combine(dbFolder, "gymbudget.db");
        }
    }
}
