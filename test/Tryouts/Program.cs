using System;
using FastTests.Client.Subscriptions;

namespace Tryouts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 128; i++)
            {
                Console.WriteLine(i);

                using (var a = new SubscriptionOperationsSignaling())
                    a.SubscriptionInterruptionEventIsFiredWhenSubscriptionIsDeleted();
                }
            }
        }

    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ToDoTask
    {
        public string Id { get; set; }
        public string Task { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedTo { get; set; }
    }
}