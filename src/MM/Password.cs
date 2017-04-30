using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM
{
    public class Password
    {

        public String id;
        public String name;
        public String Passwd;
        public String descri;
        private String mId;
        private String mName;
        private String mPasswd;
        private String mDescri;
        public Password(String id, String name, String Passwd, String descri)
        {
            this.id = this.name = this.Passwd = this.descri = "";
            this.mId = id;
            this.mName = name;
            this.mPasswd = Passwd;
            this.mDescri = descri;
        }

        public void reset(String id, String name, String Passwd, String descri)
        {
            this.mId = MM.Aes.Encrypt(id);
            this.mName = MM.Aes.Encrypt(name);
            this.mPasswd = MM.Aes.Encrypt(Passwd);
            this.mDescri = MM.Aes.Encrypt(descri);
            this.id = id;
            this.name = name;
            this.Passwd = Passwd;
            this.descri = descri;
        }

        public string getSync()
        {
            return mId + "###" + mName + "###" + mPasswd + "###" + mDescri;
        }

        public void Decrypt()
        {
            id = MM.Aes.Decrypt(mId);
            name = MM.Aes.Decrypt(mName);
            Passwd = MM.Aes.Decrypt(mPasswd);
            descri = MM.Aes.Decrypt(mDescri);
        }

        public void Encrypt()
        {
            bool ir=isRight();
            if (!ir)
            {
                return ;
            }
            mName = MM.Aes.Encrypt(name);
            mId = MM.Aes.Encrypt(id);
            mPasswd = MM.Aes.Encrypt(Passwd);
            mDescri = MM.Aes.Encrypt(descri);
        }

        public bool isRight()
        {
            if (name.Equals("????") || id.Equals("????") || descri.Equals("????") || Passwd.Equals("????")
                ||cocAscii(name) + cocAscii(id)+cocAscii(descri)+cocAscii(Passwd)<5)
            {
                return false;
            }
            return true;
        }

        private int cocAscii(String s)
        {
            int cn = 0;
            foreach (char c in s)
            {
                if (c >= 32 && c <= 126)
                {
                    cn++;
                }
            }
            return cn;
        }

        public override bool Equals(object o)
        {
            Password p = (Password)o;
            if (p == null)
            {
                return false;
            }
            if (p.mId.Equals(this.mId) && p.mName.Equals(this.mName))
            {
                return true;
            }
            return false;
        }
    }
}
