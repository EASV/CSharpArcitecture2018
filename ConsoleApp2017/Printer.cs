using System;
using System.Collections.Generic;
using CustomerApp.Core.DomainService;
using CustomerApp.Core.Entity;
using CustomerApp.Infrastructure.Static.Data.Repositories;

namespace ConsoleApp2017
{
    #region Comments

    /* -- UI -- 
        Console.WriteLine
        Console.Readline
        dkd
    */
    //-- Infrastructue --
    // EF - Static List - Text File

    // --- Test --
    // Unit test for Core

    /*--- CORE -- 
        Customer - Entity - Core.Entity
        Domain Service - Repository / UOW - Core
        Application Service - Service - Core
    */
    #endregion

    public class Printer
    {
        #region Repository area

        private ICustomerRepository customerRepository;
        #endregion

        public Printer()
        {
            customerRepository = new CustomerRepository();
            //Move to Infrastructure Layer later 
            InitData();
            //Main UI start
            StartUI();

        }

        #region UI

        void StartUI()
        {
            string[] menuItems = {
                "List All Customers",
                "Add Customer",
                "Delete Customer",
                "Edit Customer",
                "Exit"
            };

            var selection = ShowMenu(menuItems);

            while (selection != 5)
            {
                switch (selection)
                {
                    case 1:
                        var customers = GetAllCustomers();
                        ListCustomers(customers);
                        break;
                    case 2:
                        var firstName = AskQuestion("Firstname: ");
                        var lastName = AskQuestion("Lastname: ");
                        var address = AskQuestion("Address: ");
                        var customer = CreateCustomer(firstName, lastName, address);
                        SaveCustomer(customer);
                        break;
                    case 3:
                        var idForDelete = PrintFindCustomeryId();
                        DeleteCustomer(idForDelete);
                        break;
                    case 4:
                        var idForEdit = PrintFindCustomeryId();
                        var customerToEdit = FindCustomerById(idForEdit);
                        Console.WriteLine("Updating " + customerToEdit.FirstName + " " + customerToEdit.LastName);
                        var newFirstName = AskQuestion("Firstname: ");
                        var newLastName = AskQuestion("Lastname: ");
                        var newAddress = AskQuestion("Address: ");
                        UpdateCustomer(idForEdit, newFirstName, newLastName, newAddress);
                        break;
                    default:
                        break;
                }
                selection = ShowMenu(menuItems);
            }
            Console.WriteLine("Bye bye!");

            Console.ReadLine();
        }



        int PrintFindCustomeryId()
        {
            Console.WriteLine("Insert Customer Id: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Please insert a number");
            }
            return id;
        }

        string AskQuestion(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        void ListCustomers(List<Customer> customers)
        {
            Console.WriteLine("\nList of Customers");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Id: {customer.Id} Name: {customer.FirstName} " +
                                $"{customer.LastName} " +
                                $"Adress: {customer.Address}");
            }
            Console.WriteLine("\n");

        }

        /// <summary>
        /// Shows the menu.
        /// </summary>
        /// <returns>Menu Choice as int</returns>
        /// <param name="menuItems">Menu items.</param>
        int ShowMenu(string[] menuItems)
        {
            Console.WriteLine("Select What you want to do:\n");

            for (int i = 0; i < menuItems.Length; i++)
            {
                //Console.WriteLine((i + 1) + ":" + menuItems[i]);
                Console.WriteLine($"{(i + 1)}: {menuItems[i]}");
            }

            int selection;
            while (!int.TryParse(Console.ReadLine(), out selection)
                || selection < 1
                || selection > 5)
            {
                Console.WriteLine("Please select a number between 1-5");
            }

            return selection;
        }
        #endregion

        #region Please move to Application Service

        void UpdateCustomer(int id, string newFirstName, string newLastName, string newAddress)
        {
            var customer = FindCustomerById(id);
            customer.FirstName = newFirstName;
            customer.LastName = newLastName;
            customer.Address = newAddress;

            //Save with repository!!
        }

        Customer FindCustomerById(int id)
        {
            return customerRepository.ReadyById(id);
        }

        List<Customer> GetAllCustomers()
        {
            return customerRepository.ReadAll();
        }

        void DeleteCustomer(int id)
        {
            customerRepository.Delete(id);
        }

        //Application Service
        Customer CreateCustomer(string firstName,
                         string lastName,
                         string address)
        {
            //Create - Application
            var cust = new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address
            };

            return cust;

        }

        //Application Service
        Customer SaveCustomer(Customer cust)
        {
            //Save in Data - Application
            return customerRepository.Create(cust);
        }

        #endregion

        #region Infrastructure layer / Initialization Layer

        void InitData()
        {
            var cust1 = new Customer()
            {
                FirstName = "Bob",
                LastName = "Dylan",
                Address = "BongoStreet 202"
            };
            customerRepository.Create(cust1);

            var cust2 = new Customer()
            {
                FirstName = "Lars",
                LastName = "Bilde",
                Address = "Ostestrasse 202"
            };
            customerRepository.Create(cust2);
        }

        #endregion

    }
}
