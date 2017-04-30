using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM
{
    class Password
    {
        String id;
        String name;
        String mPasswd;
        String descri;
        public Password(String id, String name, String mPasswd, String descri)
        {
            this.id = id;
            this.name = name;
            this.mPasswd = mPasswd;
            this.descri = descri;
        }
        public String getId()
        {
            return id;
        }
        public String getName()
        {
            return this.name;
        }
        public String getMPasswd()
        {
            return this.mPasswd;
        }
        public String getDescri()
        {
            return this.descri;
        }
    }
}
