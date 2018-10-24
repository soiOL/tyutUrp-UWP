using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using HtmlAgilityPack;
using Newtonsoft.Json;
using urp.Struct;

namespace urp.Util
{
    class UrpUtil
    {
        private static readonly int SUCCESS = 1;
        private static readonly int TIMEOUT = 0;
        private static readonly int FAIL = -1;
        private WebUtil webUtil;
        private ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public UrpUtil()
        {
            webUtil = new WebUtil();

        }

        //登录综合教务系统


        //获取学籍信息
        public async Task<int> GetUserInfo(Dictionary<string, string> map)
        {
            try
            {
                string result = await webUtil.GetString(UrpApi2.URL + UrpApi2.URL_XJXX);
                var doc = new HtmlDocument();
                doc.LoadHtml(result);
                result = doc.DocumentNode.SelectSingleNode("//title").InnerText;
                if (result.Equals("学籍查询"))
                {
                    string key = null;
                    string value = null;
                    var trs = doc.DocumentNode.SelectNodes("//table[@id='tblView']/tr");
                    foreach (var tr in trs)
                    {
                        var td = tr.SelectNodes("./td");
                        for (int i = 0; i < td.Count; i++)
                        {
                            if (i % 2 == 0)
                            {
                                key = td[i].InnerText;
                                key = key.Replace("\r", "");
                                key = key.Replace("\n", "");
                                key = key.Replace("\t", "");
                                key = key.Replace("&nbsp;", "");
                            }
                            else
                            {
                                value = td[i].InnerText;
                                value = value.Replace("\r", "");
                                value = value.Replace("\n", "");
                                value = value.Replace("\t", "");
                                value = value.Replace(" ", "");
                                value = value.Replace("&nbsp;", "");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                                    map.Add(key, value);
                            }
                        }
                    }

                    return SUCCESS;
                }

                return TIMEOUT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return TIMEOUT;
            }
        }

        //获取全部及格成绩
        public async Task<string> getGoodScore()
        {
            string result = await webUtil.GetString(UrpApi2.URL + UrpApi2.URL_QB);
            if (result.Contains("两学期"))
            {
                Regex rgx = new Regex("border=\"0\".*((class=\"titleTop2\")|(id=\"user\"))");
                result = rgx.Replace(result, "border=\"1\"");
                return result;
            }

            return "SESSION";
        }

        //获取不及格成绩
        public async Task<string> getBadScore()
        {
            string result = await webUtil.GetString(UrpApi2.URL + UrpApi2.URL_BJG);
            if (result.Contains("不及格"))
            {
                Regex rgx = new Regex("历年成绩[\\s\\S]*不及格成绩查询");
                result = rgx.Replace(result, "");
                rgx = new Regex("border=\"0\".*((class=\"titleTop2\")|(id=\"user\"))");
                result = rgx.Replace(result, "border=\"1\"");
                return result;
            }
            return "SESSION";
        }

        //登录教学管理系统
        public async Task<int> loginStu(StuUser user)
        {
            try
            {
                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                paramList.Add(new KeyValuePair<string, string>("u", user.userName));
                paramList.Add(new KeyValuePair<string, string>("p", user.passWord));
                paramList.Add(new KeyValuePair<string, string>("r", "on"));
                string result = await webUtil.PostString(UrpApi2.STUURL + UrpApi2.URL_LOGINSTU, paramList);
                var jsonStruct = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (jsonStruct["Msg"].Equals("登陆成功"))
                {
                    return SUCCESS;
                }
                return FAIL;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return FAIL;
            }
        }

        //获取学分绩点
        public async Task<int> getJiDian(List<JidianInfo> strList,StuUser user)
        {
            try
            {
                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                paramList.Clear();
                paramList.Add(new KeyValuePair<string, string>("xh", user.userName));
                paramList.Add(new KeyValuePair<string, string>("sort", "jqzypm,xh"));
                paramList.Add(new KeyValuePair<string, string>("do", "xsgrcj"));
                string result = await webUtil.PostString(UrpApi2.STUURL + UrpApi2.URL_XFJD, paramList);
                if (result.Contains("xm"))
                {
                    result = result.Substring(1, result.Length - 2);
                    var jidianStruct = JsonConvert.DeserializeObject<JidianStruct>(result);
                    strList.Add(new JidianInfo() { values = "学号：" + jidianStruct.xh });
                    strList.Add(new JidianInfo() { values = "姓名：" + jidianStruct.xm });
                    strList.Add(new JidianInfo() { values = "班级：" + jidianStruct.bjh });
                    strList.Add(new JidianInfo() { values = "要求总学分：" + jidianStruct.zxf });
                    strList.Add(new JidianInfo() { values = "已修课程学分：" + jidianStruct.yxzxf });
                    strList.Add(new JidianInfo() { values = "已修自主实践学分：" + jidianStruct.yxzzsjxf });
                    strList.Add(new JidianInfo() { values = "曾不及格学分：" + jidianStruct.cbjgxf });
                    strList.Add(new JidianInfo() { values = "尚不及格学分：" + jidianStruct.sbjgxf });
                    strList.Add(new JidianInfo() { values = "GPA：" + jidianStruct.pjxfjd });
                    strList.Add(new JidianInfo() { values = "GPA班级排名：" + jidianStruct.gpabjpm });
                    strList.Add(new JidianInfo() { values = "GPA专业排名：" + jidianStruct.gpazypm });
                    strList.Add(new JidianInfo() { values = "GPA大类排名：" + jidianStruct.gpadlpm });
                    strList.Add(new JidianInfo() { values = "加权学分成绩：" + jidianStruct.jqxfcj });
                    strList.Add(new JidianInfo() { values = "加权班级排名：" + jidianStruct.jqbjpm });
                    strList.Add(new JidianInfo() { values = "加权专业排名：" + jidianStruct.jqzypm });
                    strList.Add(new JidianInfo() { values = "平均成绩：" + jidianStruct.pjcj });
                    strList.Add(new JidianInfo() { values = "平均成绩班级排名：" + jidianStruct.pjcjbjpm });
                    strList.Add(new JidianInfo() { values = "平均成绩专业排名：" + jidianStruct.pjcjzypm });
                    strList.Add(new JidianInfo() { values = "统计时间：" + jidianStruct.tjsj });
                    return SUCCESS;
                }
                
                return FAIL;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return FAIL;
            }
        }
    }
}
