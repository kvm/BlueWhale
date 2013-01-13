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
            Login loginClient = new Login(1);
            //loginClient.Check_User_Exist(1,"pop","google");
            Credentials credentials = new Credentials();
            loginClient.Create_User(credentials);
            loginClient.Check_User_Exist(credentials);
            
            Console.ReadKey();
        }
    }
}
