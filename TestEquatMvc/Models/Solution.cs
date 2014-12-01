using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEquatMvc.Models
{
    public class Solution
    {
        private List<char> _vars; //variables

        public List<char> Vars
        {
          get { return _vars; }
          set { _vars = value; }
        }

        private List<double> _sol;

        public List<double> Sol
        {
          get { return _sol; }
          set { _sol = value; }
        }

        public Solution()
        {
            _vars = new List<char>();
            _sol = new List<double>();
        }

        
    }
}