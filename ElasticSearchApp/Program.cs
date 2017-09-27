using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchApp
{
    class Program
    {
        private static ElasticSearchDataStore esStore;
        private static IndexManager indexManager;
        private static SampleData sampleDataManager;

        static void Main(string[] args)
        {
            var esUrl = ConfigurationManager.AppSettings["esurl"]; ;
            esStore = new ElasticSearchDataStore(esUrl, new ESLogger(esUrl));
            indexManager = new IndexManager(esUrl, new ESLogger(esUrl));
            sampleDataManager = new SampleData(esUrl);
            var option = string.Empty;
            do
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine("Press 1 to search hotel \nPress 2 to add hotel \nPress 3 to create index \nPress 4 to delete index \nPress 5 to populate data \nPress 6 to exit");
                Console.WriteLine("--------------------------------\n");
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        var hotels = SearchHotels();
                        if (hotels.Count > 0)
                            DisplayHotels(hotels);
                        else
                            Console.WriteLine("No hotels found");
                        break;
                    case "2":
                        AddHotelToIndex();
                        break;
                    case "3":
                        CreateIndex();
                        break;
                    case "4":
                        DeleteIndex();
                        break;
                    case "5":
                        PopulateSapmleData();
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            } while (option.Equals("6") == false);
        }

        private static void DisplayHotels(List<Hotel> hotels)
        {
            foreach(var hotel in hotels)
            {
                Console.WriteLine(hotel.ToString());
            }
        }

        private static void PopulateSapmleData()
        {
            var index = GetIndexName("in which you want to populate samples");
            sampleDataManager.PopulateSampleDataInEs(index);
        }

        private static void DeleteIndex()
        {
            var index = GetIndexName("you want to delete");
            indexManager.DeleteIndex(index);
        }

        private static void CreateIndex()
        {
            var index = GetIndexName("you want to create");
            indexManager.CreateIndex(index);
        }

        private static void AddHotelToIndex()
        {
            var index = GetIndexName("in which you want to add hotel");
            var hotel = new Hotel();
            Console.WriteLine("\nEnter the hotel details-----\n");
            Console.WriteLine("Enter hotel id");
            hotel.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nEnter the hotel name:");
            hotel.Name = Console.ReadLine();
            Console.WriteLine("\nEnter hotel type:");
            hotel.Type = Console.ReadLine();
            Console.WriteLine("\nEnter the description:");
            hotel.Description = Console.ReadLine();
            esStore.AddHotel(index, hotel);
        }

        private static List<Hotel> SearchHotels()
        {
            var index = GetIndexName("in which you want to search hotel");
            Console.WriteLine("Enter the hotel name you want to search");
            var searchedHotelName = Console.ReadLine();
            var hotels = esStore.Search(index, searchedHotelName);
            return hotels;
        }

        private static string GetIndexName(string text)
        {
            string index = string.Empty;
            do
            {
                Console.WriteLine($"Enter the index name {text}:");
                index = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(index));
            return index;
        }
    }
}
