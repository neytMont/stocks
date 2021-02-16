using System;
using System.Threading;


namespace Stocks
{
    public class Stock
    {
        public event EventHandler<StockNotification> StockEvent;
        private readonly Thread _thread;
        public string StockName { get; set; }
        public int InitialValue { get; set; }
        public int CurrentValue { get; set; }
        public int MaxChange { get; set; }
        public int Threshold { get; set; }
        public int NumChanges { get; set; }

        public Stock(string name, int startingValue, int maxChange, int threshold)//constructor
        {
            StockName = name;
            InitialValue = startingValue;
            MaxChange = maxChange;
            Threshold = threshold;
            CurrentValue = InitialValue;
            _thread = new Thread(new ThreadStart(Activate));//make an instance of the thread
            _thread.Start();//start the thread
        }

        public void Activate()
        {
            for (int i = 0; i < 25; i++)
            {
                Thread.Sleep(500);
                ChangeStockValue();
            }
        }

        public void ChangeStockValue()
        {
            var random = new Random();
            CurrentValue += random.Next(1, MaxChange - 1);
            NumChanges++;
            if (CurrentValue - InitialValue > Threshold)
            {
                StockNotification cArgs = new StockNotification(StockName, CurrentValue, NumChanges);
                StockEvent?.Invoke(this, cArgs);
            }
        }
    }
}
