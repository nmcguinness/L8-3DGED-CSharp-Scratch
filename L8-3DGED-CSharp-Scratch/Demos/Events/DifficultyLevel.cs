using System;

namespace L8_3DGED_CSharp_Scratch.Demos
{
    public class DifficultyLevel
    {
        public event Action<int> OnDifficultyChanged;

        private readonly int _minDifficulty, _maxDifficulty;
        private int _currentDifficulty;

        public DifficultyLevel(int minDifficulty, int maxDifficulty)
        {
            _minDifficulty = minDifficulty;
            _maxDifficulty = maxDifficulty;
        }

        public void SetDifficult(int delta)
        {
            //do some validation re min and max range
            _currentDifficulty += delta;

            //notify all interested parties
            OnDifficultyChanged?.Invoke(_currentDifficulty);
        }
    }
}
