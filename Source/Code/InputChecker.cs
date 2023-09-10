using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static KCNProxy.ShowTipCode;

namespace KCNProxy
{
    public class InputChecker
    {
        /// <summary>
        /// Port输入判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsNumber(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);

            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Port输入判断
        /// </summary>
        /// <returns></returns>
        public static bool PortCheck(Control userControl)
        {
            if (string.IsNullOrEmpty(userControl.Text))
            {
                ShowTip("请填写端口号！", userControl);
                return false;
            }

            bool i = IsNumber(userControl.Text);
            if (i == false)
            {
                ShowTip("请输入正确的端口号！", userControl);
                return false;
            }

            int number = int.Parse(userControl.Text);
            userControl.Text = number.ToString();
            if (number > 65535)
            {
                ShowTip("端口号不得大于65535！", userControl);
                return false;
            }

            return true;
        }

        /// <summary>
        /// IP输入判断
        /// </summary>
        /// <param name="userControl"></param>
        /// <returns></returns>
        public static bool IPCheckClassic(Control userControl)
        {
            if (string.IsNullOrEmpty(userControl.Text))
            {
                ShowTip("地址不能为空！", userControl);
                return false;
            }

            var index0 = Regex.IsMatch(userControl.Text, "http://", RegexOptions.IgnoreCase);
            var index1 = Regex.IsMatch(userControl.Text, "https://", RegexOptions.IgnoreCase);
            if (index0.ToString() == "True" || index1.ToString() == "True")
            {
                ShowTip("输入的地址格式错误！请去掉http://或者https://", userControl);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 管理员模式判断
        /// </summary>
        /// <returns></returns>
        public static bool UserMode()
        {

            //获得当前登录的Windows用户标示
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                //是管理员
                return true;
            }

            return false;
        }


    }
}
