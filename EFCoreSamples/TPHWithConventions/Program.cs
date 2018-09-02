using System;
using System.Linq;

namespace TPHWithConventions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static void AddSampleData()
        {
            using (var context = new BankContext())
            {
                context.CashPayments.Add(
                new CashPayment { Name = "Donald", Amount = 0.5M });
                context.CashPayments.Add(
                new CashPayment { Name = "Scrooge", Amount = 20000M });
                context.CreditcardPayments.Add(
                new CreditcardPayment
                {
                    Name = "Gus Goose",
                    Amount = 300M,
                    CreditcardNumber = "987654321"
                });
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 要查询层次结构中的特定类型，可以使用OfType扩展方法。 
        /// </summary>
        private static void QuerySample()
        {
            using (var context = new BankContext())
            {
                var creditcardPayments =
                context.Payments.OfType<CreditcardPayment>();
                foreach (var payment in creditcardPayments)
                {
                    Console.WriteLine($"{payment.Name}, {payment.Amount}");
                }
            }
        }
    }
}
