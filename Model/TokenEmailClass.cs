using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TokenEmailClass
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; }

    }
}
