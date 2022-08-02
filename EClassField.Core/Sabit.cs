using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public class Sabit
    {
        public static string imagehost = "http://image6.otowoop.com/";

       
        public static string webhost = "/";


       
    }
    public enum ProjeType
    {
        OtoKuafor = 0,
        ArabaIlan = 1,
        ArabaEvIlan = 2,
        Blog = 3,
        UcdModel = 4
    }

    public class SelectedCategories
    {

        public List<int> Ids { get; set; }
        public SelectedCategories()
        {
            Ids = new List<int>();
        }

        public ProjeType ProjeType { get; set; }

    }

}
