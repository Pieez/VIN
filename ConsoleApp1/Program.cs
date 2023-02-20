using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {



            var vin = VinGenerator.GetVin(2019, "Ford Motor Company");
            Console.WriteLine(vin);



            //Prints out the valid make names
            foreach (string key in VinGenerator.MakeToWmiDictionary.Keys)
            {
                Console.WriteLine(key);
            }
        }
    }

    public static class VinGenerator
    {

        public static Random rand = new Random();

        public static string allCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string vinCharacters = allCharacters.Replace("I", "").Replace("Q", "").Replace("O", "");

        public static string alphaCharacters = vinCharacters.Remove(vinCharacters.IndexOf('0'));

        public static string numericCharacters = vinCharacters.Substring(vinCharacters.IndexOf('0'));

        public static string yearCharacters = vinCharacters.Replace("0", "").Replace("U", "").Replace("Z", "");

        public static char ModelYearToLetter(int modelYear) => yearCharacters[modelYear % yearCharacters.Length];

        public static Dictionary<string, IList<string>> MakeToWmiDictionary = new Dictionary<string, IList<string>>();

        public static int[] Multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };

        public static int VinCharacterToNumber(char vinChar)
        {
            if (vinChar >= '0' && vinChar <= '9') return (int)vinChar - 48;
            if (vinChar >= 'S' && vinChar <= 'Z') return (allCharacters.IndexOf(vinChar) + 1) % 9 + 1;
            return allCharacters.IndexOf(vinChar) % 9 + 1;
        }

        public static string MakeValid(string vin)
        {
            var checkDigit = vin.Select(c => VinCharacterToNumber(c)).Zip(Multipliers, (a, b) => a * b).Aggregate(0, (x, y) => x + y, a => a % 11);
            return vin.Remove(8, 1).Insert(8, checkDigit == 10 ? "X" : checkDigit.ToString());
        }

        public static string GetWmi(string make)
        {
            try
            {
                var wmiList = MakeToWmiDictionary[make];
                return wmiList[rand.Next(wmiList.Count)];
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentException($"'{make}' is not a valid make identifier.", nameof(make));
            }
        }

        public static string GetVds() => Enumerable.Range(4, 5).Select(
            i =>
            {
                var charSet = "";
                switch (i)
                {
                    case 4:
                        charSet = alphaCharacters;
                        break;
                    case 5:
                        charSet = alphaCharacters;
                        break;
                    case 6:
                        charSet = numericCharacters;
                        break;
                    case 7:
                        charSet = numericCharacters;
                        break;
                    case 8:
                        charSet = vinCharacters;
                        break;
                    default:
                        charSet = vinCharacters;
                        break;
                }
                return charSet[rand.Next(charSet.Length)];
            })
            .Aggregate("", (a, b) => a + b, x => x) + rand.Next(10).ToString();

        public static string GetVis(int modelYear) => ModelYearToLetter(modelYear) + "A" + rand.Next(10000000).ToString().PadLeft(6, '0');

        public static string GetVin(int modelYear, string make) => MakeValid(GetWmi(make) + GetVds() + GetVis(modelYear));

        public static string ManufacturerMap =
@"'1B3': 'Dodge',
  '1C3': 'Chrysler',
  '1C4': 'Chrysler',
  '1C6': 'Chrysler',
  '1D3': 'Dodge',
  '1FA': 'Ford Motor Company',
  '1FB': 'Ford Motor Company',
  '1FC': 'Ford Motor Company',
  '1FD': 'Ford Motor Company',
  '1FM': 'Ford Motor Company',
  '1FT': 'Ford Motor Company',
  '1FU': 'Freightliner',
  '1FV': 'Freightliner',
  '1F9': 'FWD Corp.',
  '1G': 'General Motors',
  '1GC': 'Chevrolet Truck',
  '1GT': 'GMC Truck',
  '1G1': 'Chevrolet',
  '1G2': 'Pontiac',
  '1G3': 'Oldsmobile',
  '1G4': 'Buick',
  '1G6': 'Cadillac',
  '1G8': 'Saturn',
  '1GM': 'Pontiac',
  '1GY': 'Cadillac',
  '1H': 'Honda',
  '1HD': 'Harley-Davidson',
  '1J4': 'Jeep',
  '1J8': 'Jeep',
  '1L': 'Lincoln',
  '1ME': 'Mercury',
  '1M1': 'Mack Truck',
  '1M2': 'Mack Truck',
  '1M3': 'Mack Truck',
  '1M4': 'Mack Truck',
  '1M9': 'Mynatt Truck & Equipment',
  '1N': 'Nissan',
  '1NX': 'NUMMI',
  '1P3': 'Plymouth',
  '1R9': 'Roadrunner Hay Squeeze',
  '1VW': 'Volkswagen',
  '1XK': 'Kenworth',
  '1XP': 'Peterbilt',
  '1YV': 'Mazda (AutoAlliance International)',
  '1ZV': 'Ford (AutoAlliance International)',
  '2A4': 'Chrysler',
  '2BP': 'Bombardier Recreational Products',
  '2B3': 'Dodge',
  '2B7': 'Dodge',
  '2C3': 'Chrysler',
  '2CN': 'CAMI',
  '2D3': 'Dodge',
  '2FA': 'Ford Motor Company',
  '2FB': 'Ford Motor Company',
  '2FC': 'Ford Motor Company',
  '2FM': 'Ford Motor Company',
  '2FT': 'Ford Motor Company',
  '2FU': 'Freightliner',
  '2FV': 'Freightliner',
  '2FZ': 'Sterling',
  '2Gx': 'General Motors',
  '2G1': 'Chevrolet',
  '2G2': 'Pontiac',
  '2G3': 'Oldsmobile',
  '2G4': 'Buick',
  '2G9': 'Gnome Homes',
  '2HG': 'Honda',
  '2HK': 'Honda',
  '2HJ': 'Honda',
  '2HM': 'Hyundai',
  '2M': 'Mercury',
  '2NV': 'Nova Bus',
  '2P3': 'Plymouth',
  '2T': 'Toyota',
  '2TP': 'Triple E LTD',
  '2V4': 'Volkswagen',
  '2V8': 'Volkswagen',
  '2WK': 'Western Star',
  '2WL': 'Western Star',
  '2WM': 'Western Star',
  '3C4': 'Chrysler',
  '3D3': 'Dodge',
  '3D4': 'Dodge',
  '3FA': 'Ford Motor Company',
  '3FE': 'Ford Motor Company',
  '3G': 'General Motors',
  '3H': 'Honda',
  '3JB': 'BRP (all-terrain vehicles)',
  '3MD': 'Mazda',
  '3MZ': 'Mazda',
  '3N': 'Nissan',
  '3P3': 'Plymouth',
  '3VW': 'Volkswagen',
  '4F': 'Mazda',
  '4JG': 'Mercedes-Benz',
  '4M': 'Mercury',
  '4RK': 'Nova Bus',
  '4S': 'Subaru-Isuzu Automotive',
  '4T': 'Toyota',
  '4T9': 'Lumen Motors',
  '4UF': 'Arctic Cat Inc.',
  '4US': 'BMW',
  '4UZ': 'Frt-Thomas Bus',
  '4V1': 'Volvo',
  '4V2': 'Volvo',
  '4V3': 'Volvo',
  '4V4': 'Volvo',
  '4V5': 'Volvo',
  '4V6': 'Volvo',
  '4VL': 'Volvo',
  '4VM': 'Volvo',
  '4VZ': 'Volvo',
  '538': 'Zero Motorcycles',
  '5F': 'Honda Alabama',
  '5J': 'Honda Ohio',
  '5L': 'Lincoln',
  '5N1': 'Nissan',
  '5NP': 'Hyundai',
  '5T': 'Toyota - trucks',
  '5YJ': 'Tesla, Inc.',";

        static VinGenerator()
        {
            foreach (var line in ManufacturerMap.Split('\n'))
            {
                var lineParts = line.Trim().Replace("'", "").Replace(",", "").Replace(": ", ":").Split(':');
                var make = lineParts[1];
                var code = lineParts[0];
                if (code.Length == 2)
                {
                    code += "9";
                }
                if (!MakeToWmiDictionary.ContainsKey(make))
                {
                    MakeToWmiDictionary[make] = new List<string>();
                }
                MakeToWmiDictionary[make].Add(code);
            }
        }
    }
}