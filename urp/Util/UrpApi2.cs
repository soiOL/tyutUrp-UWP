using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urp.Util
{
    class UrpApi2
    {
        public static string JWCURL = "http://219.226.101.61/jiawu3.html";
        //主url
        public static string URL = "http://202.207.247.49/";
        public static string STUURL = "http://202.207.247.60/";
        public static string DEFAULTURL = "http://202.207.247.49/";
        public static string DEFAULTSTUURL = "http://202.207.247.60/";
        //    public static String URL3 = "http://202.207.247.44:8065/";
        //    public static String URL4 = "http://202.207.247.44:8069/";
        //登录urp
        public readonly static string URL_LOGIN = "loginAction.do";
        //注销登录
        public readonly static string URL_LOGOUT = "logout.do";
        public readonly static string URL_LOGINSTU = "Hander/LoginAjax.ashx";
        //学籍信息
        public readonly static string URL_XJXX = "xjInfoAction.do?oper=xjxx";
        //学分绩点
        public readonly static string URL_XFJD = "Hander/Cj/CjAjax.ashx";
        //实践成绩
        public readonly static string URL_ZJSJ = "xszzsjcjbAction.do?oper=viewByStudent";
        ////本学期课表
        public readonly static string URL_KB = "xkAction.do?actionType=6";

        //验证码
        public readonly static string URL_YZM = "validateCodeAction.do";
        //照片
        public readonly static string URL_ZP = "xjInfoAction.do?oper=img";
        //方案成绩
        public readonly static string URL_FA = "gradeLnAllAction.do?type=ln&oper=fainfo&fajhh=1734";
        //全部及格成绩
        public readonly static string URL_QB = "gradeLnAllAction.do?type=ln&oper=qbinfo";
        //不及格成绩
        public readonly static string URL_BJG = "gradeLnAllAction.do?type=ln&oper=bjg";
        //通知公告
        public readonly static string URL_TZGG = "tzgg.htm";
        //jwc网站
        public readonly static string URL_JWC = "http://jwc.tyut.edu.cn/";
        //评教列表
        public readonly static string URL_JXPG_LIST = "jxpgXsAction.do?oper=listWj&pageSize=300";
        //具体评估
        public readonly static string URL_JXPG = "jxpgXsAction.do?oper=wjpg";
        //评估页面
        public readonly static string URL_PG = "jxpgXsAction.do";
        
        //学生卡记录查询
            //加载页面(参数：terminal:Android ，POST方法)
        public readonly static string URL_ECARD_PREPARE =
            "http://111.231.107.11:18001/card/cardClientService/getLoadingPage.ib";
            //登录URL(参数：stucode: ,stupsw:默认123456 ,GET方法)
        public static string URL_ECARD_LOGIN = "http://202.207.245.234:9090/1001.json";
            //查询流水URL(参数：stucode: ,startdate: 2018-12-15 ,enddate: , GET方法)
        public static string URL_ECARD_SEARCH = "http://202.207.245.234:9090/0005.json";
            //查询余额URL(参数：stucode:, GET方法)
        public static string URL_ECARD_MONEY = "http://202.207.245.234:9090/0002.json";

        public static string StuCode = "";

        public static string StuPsw = "";

        public static string StartDate = "";

        public static string EndDate = "";

    }
}
