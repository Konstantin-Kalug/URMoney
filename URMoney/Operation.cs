using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URMoney
{
    public class Operation
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int PeopleId { get; set; }
        public int CategoryId { get; set; }
        public int ValuteId { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}
