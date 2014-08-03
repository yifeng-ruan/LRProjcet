using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Utils.Crypt
{
    //MD5
    public class DESEncrypt
    {
        public static string Encrypt(string strText)
        {
            byte [] data=System.Text.Encoding.Unicode.GetBytes (strText.ToCharArray());
            System.Security.Cryptography.MD5 MD5=new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte [] result=MD5.ComputeHash(data);
            string sResult=System.Text.Encoding.Unicode.GetString(result);
            string EnPwd=System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strText.ToString(),"MD5");
            return EnPwd;
        }
    }
}
