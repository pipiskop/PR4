using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PR4
{
    public class Money
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Ammount { get; set; }
        public bool IsIncome { get; set; }
        public DateTime CurrentDate;

        public Money(DateTime entryDate, string entryName, string entryType, int entryMoney, bool isIncome)
        {
            CurrentDate = entryDate;
            Name = entryName;
            Type = entryType;
            Ammount = entryMoney;
            IsIncome = isIncome;
        }
    }
}
