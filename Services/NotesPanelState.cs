namespace GymBudgetApp.Services
{
    public class NotesPanelState
    {
        private int _seasonId;

        public int SeasonId
        {
            get => _seasonId;
            set
            {
                if (_seasonId != value)
                {
                    _seasonId = value;
                    OnChange?.Invoke();
                }
            }
        }

        public event Action? OnChange;
    }
}
