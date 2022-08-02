using EClassField.Core;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Directory;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EClassField.API.Models
{
    public class CreateType
    {

        public string FieldName { get; set; }
        public Type FieldType { get; set; }
    }
    public static class MyTypeBuilder
    {
        public static List<CreateType> yourListOfFields = new List<CreateType>();
        public static void CreateNewObject()
        {
            var myType = CompileResultType();
            var myObject = Activator.CreateInstance(myType);
        }
        public static Type CompileResultType()
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
    public class Function
    {

      

     
        public static string GetTimeDiffrent(DateTime d1, DateTime d2)
        {

            DateTime baslamaTarihi = d1;
            DateTime bitisTarihi = d2;

            TimeSpan kalangun = bitisTarihi - baslamaTarihi;


            string s = "";
            if (kalangun.Days >= 1)
                s += kalangun.Days + " gün ";
            else if (kalangun.Hours >= 1)
                s += kalangun.Hours + " saat ";
            else if (kalangun.Minutes >= 1)
                s += kalangun.Minutes + " dakika";
            else if (kalangun.Seconds >= 1)
                s += kalangun.Seconds + " saniye";


            return s;
        }
        public static string FullLoc(City city, Town town, Area area, Neighborhood neighboard)
        {
            string loc = "";
            if (city != null)
                loc += city.Name;
            if (town != null)
                loc += " / " + town.Name;
            if (area != null)
                loc += " / " + area.Name;
            if (neighboard != null)
                loc += " / " + neighboard.Name;

            return loc;

        }
        public static List<Category> getKategoriYol(int ID)
        {
           
            ClassFieldDbContext ctx = new ClassFieldDbContext();
        
            List<Category> cats = new List<Category>();
            Category cat = Cache.Categories.Find(d => d.Id == ID && d.IsActive == true);
            if (cat != null)
            {
                cats.Add(cat);
                while (cat != null)
                {
                    cat = Cache.Categories.Find(d => d.Id == cat.ParentCategoryId);
                    if (cat != null)
                    {
                        cats.Add(cat);
                    }
                }

            }


            cats = cats.OrderBy(d => d.Order).ToList();


            return cats;

        }

        public static string getValue(string fullstr, string name)

        {
            string[] inputs = fullstr.Split(';');
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].Split('=')[0].ToLower() == name.ToLower())
                    return inputs[i].Split('=')[1] + "";

            }

            return "";


        }
        public static string getValuePlayer(string fullstr, string name)

        {
            string[] inputs = fullstr.Split('&');
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].Split('=')[0].ToLower() == name.ToLower())
                    return inputs[i].Split('=')[1] + "";

            }

            return "";


        }

        public static List<Ordertip> GetOrderFilterList(int katID)
        {
            int[] cats = getKategoriYol(katID).Select(d => d.Id).ToArray();


            return OrderList.GetOrderTip().FindAll(d => d.KatID.Any(x=>cats.Contains(x)));

        }


        public static string MD5Sifrele(string metin)
        {
            // MD5CryptoServiceProvider nesnenin yeni bir instance'sını oluşturalım.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Girilen veriyi bir byte dizisine dönüştürelim ve hash hesaplamasını yapalım.
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            //byte'ları biriktirmek için yeni bir StringBuilder ve string oluşturalım.
            StringBuilder sb = new StringBuilder();


            //hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım.
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürelim.
            return sb.ToString();
        }
    }
}