using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Stocks
{
    public class StockBroker
    {
        //getters and setters
        public string BrokerName { get; set; }

        public List<Stock> stocks = new List<Stock>();
        public static ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
        public string titles = "Broker".PadRight(10) + "Stock".PadRight(12) + "Value".PadRight(7) + "Changes";

        public StockBroker(string name)
        {
            BrokerName = name;
        }

        public void AddStock(Stock stock)
        {
            stocks.Add(stock);
            stock.StockEvent += EventHandler;
        }

        void EventHandler(Object sender, StockNotification e)
        {
            myLock.EnterWriteLock();
            Stock nStock = (Stock)sender;
            string output = BrokerName.PadRight(10) + nStock.StockName.PadRight(12) + nStock.CurrentValue.ToString().PadRight(7) + nStock.NumChanges;
            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\neyt\source\repos\Stocks\output.txt", true))
            {
                if (GlobalHelper.writeTitle)
                {
                    GlobalHelper.writeTitle = false;
                    Console.WriteLine(titles);
                    outputFile.WriteLine(titles);
                }
                outputFile.WriteLine(output);
            }
            Console.WriteLine(output);
            myLock.ExitWriteLock();
        }
    }

    public static class GlobalHelper
    {
        public static Boolean writeTitle = true;
    }
}
