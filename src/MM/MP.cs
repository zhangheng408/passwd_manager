using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
/*
 * Model 
 * Manage password
 * */
namespace MM
{
    class MP
    {
        Password []p;
        public MP()
        {
            int i;
            String t;
            StreamReader r = null;

            try
            {
                r = new StreamReader("test.rmx");
            }
            catch
            {
                p = null;
                return;
            }

            for (i = 0; ; i++)
            {
                t = r.ReadLine();
                if (null == t || t.Equals(""))
                {
                    break;
                }
            }
            r.Close();
            if (i < 1)
            {
                p = null;
                return;
            }
            p = new Password[i];
            String[] sp;
            r = new StreamReader("test.rmx");
            for (i = 0; ; i++)
            {
                 t= r.ReadLine();
                if (null == t || t.Equals(""))
                {
                    break;
                }
                //sp=t.Split("##");
                sp = Regex.Split(t, "###");
                try
                {
                    p[i] = new Password(sp[0], sp[1], sp[2], sp[3]);
                }
                catch (Exception)
                {
                    p[i] = null;
                }
            }
            r.Close();
        }
        public ArrayList search(String input)
        {
            ArrayList a = new ArrayList();
            for (int i=0; p!=null &&i < p.Length; i++)
            {
                if (p[i].getId().ToLower().Contains(input.ToLower())||p[i].getName().ToLower().Contains(input.ToLower()))
                {
                    a.Add(p[i]);
                }
            }
            return a;
        }
        public static int addPass(string ap)
        {
            FileStream f = new FileStream("test.rmx", FileMode.Append);
            StreamWriter sw = new StreamWriter(f);
            sw.WriteLine(ap);
            sw.Close();
            f.Close();
            return 1;
        }
    }
}
