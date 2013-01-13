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
            Login loginClient = new Login();
            //loginClient.Check_User_Exist(1,"pop","google");
            loginClient.Create_User(12,"ankurkkhurana","ankur","khurana");
            Console.ReadKey();
        }
    }
}
