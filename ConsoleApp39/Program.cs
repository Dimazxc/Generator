using Bogus;
using Bogus.DataSets;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp38
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceStack.Text.CsvConfig.ItemSeperatorString = ";";

            int count = int.Parse(args[0]);
            CountryGenerator country = new CountryGenerator(args[1]);
            
            List<Person> users = new Faker<Person>(country.Locale)
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.Country, f => country.ViewCoutry(country.Region.Name, country.Region.NativeName))
                .RuleFor(p => p.Address, f => f.Address.StreetAddress() + ' ' + f.Address.City())
                .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber()).Generate(count).ToList();

            var str = CsvSerializer.SerializeToCsv<Person>(users);
            Console.WriteLine(str);
        }
    }


    class Person
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    class CountryGenerator
    {
        private Random countryID = new Random();
        private Dictionary<string, string> countries = new Dictionary<string, string>()
        {
            {"en_US", "en_US" },
            {"ru_RU", "ru" },
            {"uk_UA", "uk" }
        };

        public RegionInfo Region { get; set; }
        public string Locale { get; set; }
        public CountryGenerator(string country)
        {
            Region = new RegionInfo(country);
            Locale = countries[country];
        }

        public string ViewCoutry(params string[] names)
        {
            var ind = countryID.Next(0, names.Length);
            return names[ind];
        }
    }
}
