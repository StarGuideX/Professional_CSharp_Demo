using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MenusSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        /// <summary>
        /// The following code snippet writes a relationship, a MenuCard containing Menu objects. Here, the MenuCard and Menu objects are instantiated.The bidirectional associations are assigned. With the Menu, the MenuCard property is assigned to the MenuCard, and with the MenuCard, the Menus property is filled with Menu objects. The MenuCard instance is added to the context invoking the Add method of the MenuCards property. 
        /// </summary>
        private static void AddRecords()
        {
            using (var context = new MenusContext())
            {
                var soupCard = new MenuCard();
                Menu[] soups =
                {
                    new Menu
                    {
                        Text = "Consommé Célestine (with shredded pancake)",
                        Price = 4.8m,
                        MenuCard = soupCard
                    },
                    new Menu
                    {
                        Text = "Baked Potato Soup",
                        Price = 4.8m,
                        MenuCard = soupCard
                    },
                    new Menu
                    {
                        Text = "Cheddar Broccoli Soup",
                        Price = 4.8m,
                        MenuCard = soupCard
                    },
                };
                soupCard.Title = "Soups";
                soupCard.Menus.AddRange(soups);
                context.MenuCards.Add(soupCard);
                ShowState(context);
                int records = context.SaveChanges();
                Console.WriteLine($"{records} added");
            }
        }

        /// <summary>
        /// The method ShowState that is invoked after adding the four objects to the context shows the state of all objects that are associated with the context. The DbContext class has a ChangeTracker associated that can be accessed using the ChangeTracker property. The Entries method of the ChangeTracker returns all the objects the change tracker knows about. With the foreach loop, every object including its state is written to the console
        /// </summary>
        /// <param name="context"></param>
        public static void ShowState(MenusContext context)
        {
            //ChangeTracker.Entries,returns all the objects the change tracker knows about. 
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine($"type: {entry.Entity.GetType().Name}," +
                $"state: {entry.State}, {entry.Entity}");
            }
            Console.WriteLine();
        }

        private static void ObjectTracking()
        {
            using (var context = new MenusContext())
            {
                //var m1 = (from m in context.Menus where m.Text.StartsWith("Con") select m).FirstOrDefault();
                //var m2 = (from m in context.Menus where m.Text.Contains("(") select m).FirstOrDefault();
                //if (object.ReferenceEquals(m1, m2))
                //{
                //    Console.WriteLine("the same object");
                //}
                //else
                //{
                //    Console.WriteLine("not the same");
                //}
                //ShowState(context);
            }
        }
    }
}
