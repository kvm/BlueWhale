using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ApiCalls
{
    class Program
    {
        static void Main(string[] args)
        {
            Login loginClient = new Login(0,0);
            //loginClient.Check_User_Exist(1,"pop","google");
            Credentials credentials = new Credentials();
            Console.WriteLine(loginClient.Create_User(credentials).ToString());
            Console.WriteLine(loginClient.Check_User_Exist(credentials).ToString());
            Console.ReadKey();
        }
    }
}
