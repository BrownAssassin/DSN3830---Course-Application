using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer001.Model
{
    class Result
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public int WinCount { get; set; }

        public Result(int Id,int userid,int totalcount,int wincount)
        {
            this.Id = Id;
            this.UserId = userid;
            this.TotalCount = totalcount;
            this.WinCount = WinCount;
        }
    }
}
