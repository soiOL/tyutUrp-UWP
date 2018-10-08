using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp.Util
{
    class UrpApi
    {
        public static readonly string BASEURL = "https://urp.soiol.cn/";

        //获取验证码
        public static readonly string GETCHECKCODE = "getCheckCode";

        //登录
        public static readonly string LOGIN = "login";

        //获取个人信息
        public static readonly string GETUSERINFO = "userInfo";

        //获取全部成绩
        public static readonly string GETCHENGJI = "getAllGradeView";

        //获取不及格成绩
        public static readonly string GETNOCHENGJI = "getBadGradeView";

        //获取绩点信息
        public static readonly string GETJIDIAN = "getGPA";

        //获取课表
        public static readonly string GETKEBIAO = "schedule";
    }
}
