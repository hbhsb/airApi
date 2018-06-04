using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace airApi.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public bool IsDel { get; set; }
    }
}