using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MM
{
    public class Passwds
    {
        public List<Password> all;
        public string file;
        private bool keyChing=false;
        private MM owner;
        public Passwds(String f, MM o)
        {
            owner = o;
            load(f);
        }

        public int load(String f)
        {
            file=f;
            all = new List<Password>();
            StreamReader r = null;
            try
            {
                r = new StreamReader(f);
            }
            catch
            {
                return -1;
            }
            /*
            String pz = r.ReadLine();
            if (pz != null && !pz.Equals(""))
            {
                String[] sp = System.Text.RegularExpressions.Regex.Split(pz, "###");
                if (sp.Length > 1)
                {
                    if (sp[0].Length > 0)
                    {
                        this.owner.pChar = sp[0].ToCharArray()[0];
                    }
                    else
                    {
                        this.owner.pChar = '\0';
                    }
                    if (sp[1].Equals("0"))
                    {
                        this.owner.zClose = false;
                    }
                    else
                    {
                        this.owner.zClose = true;
                    }
                }
                if (sp.Length > 2)
                {
                    this.owner.skin = sp[2];
                }
                this.owner.setSkin();
                

            }
            */
            while (true)
            {
                String t = r.ReadLine();
                if (t == null || t.Equals(""))
                {
                    break;
                }
                String[] sp = System.Text.RegularExpressions.Regex.Split(t, "###");
                if (sp.Length < 4)
                {
                    continue;
                }
                all.Add(new Password(sp[0], sp[1], sp[2], sp[3]));
            }

            r.Close();
            return 1;
        }

        public void chKey()
        {
            keyChing = true;
            foreach (Password p in all)
            {
                p.Decrypt();
            }
            keyChing = false;
        }

        public void setNewKey()
        {
            foreach (Password p in all)
            {
                p.Encrypt();
            }
        }

        public List<Password> search(String s)
        {
            List<Password> li = new List<Password>();
            if (keyChing)
            {
                return li;
            }
            String ls=s.ToLower();
            foreach (Password p in all)
            {
                if (p.isRight() && (p.name.ToLower().Contains(ls) || p.id.ToLower().Contains(ls)))
                {
                    li.Add(p);
                }
            }
            return li;
        }

        public bool remove(Password p)
        {
            return all.Remove(p);
        }

        public void add(Password p)
        {
            all.Add(p);
        }

        public int sync()
        {
            FileStream f = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(f);
            /*
            String pz = this.owner.pChar + "###";
            if (this.owner.zClose)
            {
                pz += "1";
            }
            else
            {
                pz += "0";
            }
            sw.WriteLine(pz);
             * */
            foreach(Password p in all)
            {
                sw.WriteLine(p.getSync());
            }
            sw.Close();
            f.Close();
            return 1;
        }
    }
}
