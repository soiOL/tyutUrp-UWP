using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
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
        
        //去除字符串中的杂项
        public string getRealString(string value)
        {
            value = value.Replace("\r", "");
            value = value.Replace("\n", "");
            value = value.Replace("\t", "");
            value = value.Replace("&nbsp;", "");
            value = value.Trim();
            return value;
        }

        //获取学籍照片
        public async Task<BitmapImage> GetUserImage()
        {
            var userImage = await webUtil.GetImage(new Uri(UrpApi2.URL + UrpApi2.URL_ZP));
            return userImage;
        }

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
                                key = getRealString(key);
                            }
                            else
                            {
                                value = td[i].InnerText;
                                value = getRealString(value);
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

        //获取全部及格成绩,可直接用作webview
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

        //获取全部及格成绩列表
        public List<AllScore> getGoodScoreList(string html)
        {
            List<AllScore> allScores = new List<AllScore>();
            try
            {
                var doc = new HtmlDocument();
                Regex rgx = new Regex("</a>");
                var scores = rgx.Split(html);
                if (scores.Length > 1)
                {
                    for (int i = 1; i < scores.Length; i++)
                    {
                        var score = scores[i];
                        var allScore = new AllScore();
                        var scoreList = new List<ScoreStruct>();
                        doc.LoadHtml(score);
                        var head = doc.DocumentNode.SelectSingleNode("//b").InnerText;
                        head = getRealString(head);
                        allScore.Head = head;
                        var lable = doc.DocumentNode.SelectSingleNode("//td[@height='21']").InnerText;
                        lable = getRealString(lable);
                        allScore.Lable = lable;
                        var scoreTable = doc.DocumentNode.SelectSingleNode("//td[@class='pageAlign']/table[1]");
                        var scoreTrs = scoreTable.SelectNodes("./tr");
                        if (scoreTrs != null)
                        {
                            foreach (var scoreTr in scoreTrs)
                            {
                                var scoreTds = scoreTr.SelectNodes("./td");
                                var scoreStruct = new ScoreStruct();
                                for (int j = 0; j < scoreTds.Count; j++)
                                {
                                    var info = scoreTds[j].InnerText;
                                    info = getRealString(info);
                                    switch (j % 7)
                                    {
                                        case 0:
                                            scoreStruct.kechenghao = "课程号：" + info;
                                            break;
                                        case 1:
                                            scoreStruct.kexuhao = "课序号：" + info;
                                            break;
                                        case 2:
                                            scoreStruct.kechengming = info;
                                            break;
                                        case 3:
                                            scoreStruct.yingwenkechengming = info;
                                            break;
                                        case 4:
                                            scoreStruct.xuefen = "学分：" + info;
                                            break;
                                        case 5:
                                            scoreStruct.kechengshuxing = info;
                                            break;
                                        case 6:
                                            scoreStruct.chengji = info;
                                            break;

                                    }
                                }
                                scoreList.Add(scoreStruct);
                            }
                        }
                        allScore.ScoreList = scoreList;
                        allScores.Add(allScore);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return allScores;

        }

        //获取不及格成绩,可直接用作webview
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

        //获取不及格成绩列表
        public List<AllScore> getBadScoreList( string html)
        {
            List<AllScore> allScores = new List<AllScore>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var tables = doc.DocumentNode.SelectNodes("//td[@class='pageAlign']/table");
                //尚不及格列表
                var sTrs = tables[0].SelectNodes("./tr");
                //曾不及格列表
                var cTrs = tables[1].SelectNodes("./tr");
                if (sTrs != null)
                {
                    var scoreList1 = new List<ScoreStruct>();
                    foreach (var sTr in sTrs)
                    {
                        var sTds = sTr.SelectNodes("./td");
                        ScoreStruct scoreStruct1 = new ScoreStruct();
                        for (int i = 0; i < sTds.Count; i++)
                        {
                            var info = sTds[i].InnerText;
                            info = getRealString(info);
                            switch (i % 9)
                            {
                                case 0:
                                    scoreStruct1.kechenghao = "课程号：" + info;
                                    break;
                                case 1:
                                    scoreStruct1.kexuhao = "课序号：" + info;
                                    break;
                                case 2:
                                    scoreStruct1.kechengming = info;
                                    break;
                                case 3:
                                    scoreStruct1.yingwenkechengming = info;
                                    break;
                                case 4:
                                    scoreStruct1.xuefen = "学分：" + info;
                                    break;
                                case 5:
                                    scoreStruct1.kechengshuxing = info;
                                    break;
                                case 6:
                                    scoreStruct1.chengji = info;
                                    break;
                                case 7:
                                    scoreStruct1.time = "考试时间：\n" + info;
                                    break;
                                case 8:
                                    scoreStruct1.why = "未通过原因" + info;
                                    break;

                            }
                        }
                        scoreList1.Add(scoreStruct1);
                    }
                    allScores.Add(new AllScore()
                    {
                        Head = "尚不及格学科",
                        Lable = "",
                        ScoreList = scoreList1
                    });
                }
                if (cTrs != null)
                {
                    var scoreList2 = new List<ScoreStruct>();
                    foreach (var cTr in cTrs)
                    {
                        var cTds = cTr.SelectNodes("./td");
                        ScoreStruct scoreStruct2 = new ScoreStruct();
                        for (int i = 0; i < cTds.Count; i++)
                        {
                            var info = cTds[i].InnerText;
                            info = getRealString(info);
                            switch (i % 9)
                            {
                                case 0:
                                    scoreStruct2.kechenghao = "课程号：" + info;
                                    break;
                                case 1:
                                    scoreStruct2.kexuhao = "课序号：" + info;
                                    break;
                                case 2:
                                    scoreStruct2.kechengming = info;
                                    break;
                                case 3:
                                    scoreStruct2.yingwenkechengming = info;
                                    break;
                                case 4:
                                    scoreStruct2.xuefen = "学分：" + info;
                                    break;
                                case 5:
                                    scoreStruct2.kechengshuxing = info;
                                    break;
                                case 6:
                                    scoreStruct2.chengji = info;
                                    break;
                                case 7:
                                    scoreStruct2.time = "考试时间：\n" + info;
                                    break;
                                case 8:
                                    scoreStruct2.why = "未通过原因" + info;
                                    break;

                            }
                        }
                        scoreList2.Add(scoreStruct2);
                    }
                    allScores.Add(new AllScore()
                    {
                        Head = "曾不及格学科",
                        Lable = "",
                        ScoreList = scoreList2
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return allScores;
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

        //获取公告列表
        public async Task<Dictionary<string,string>> GetNotifyList()
        {
            try
            {
                var html = await webUtil.GetString(UrpApi2.URL_JWC + UrpApi2.URL_TZGG);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var gonggao = doc.DocumentNode.SelectNodes("//span[@class='n']");
                if (gonggao.Count > 0)
                {
                    var head = new StringBuilder();
                    string link = "";
                    var dictonary = new Dictionary<string, string>();
                    for (int i = 0; i < gonggao.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            var a = gonggao[i].ChildNodes[0];
                            head.Append(a.InnerText);
                            link = a.GetAttributeValue("href", "");
                        }
                        else
                        {
                            head.Append("(" + gonggao[i].InnerText + ")");
                            dictonary.Add(head.ToString(), link);
                            head.Clear();
                        }
                    }

                    return dictonary;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }

        //获取详细公告
        public async Task<string> GetNotifyContent(string url)
        {
            try
            {
                string html = await webUtil.GetString(url);
                StringBuilder valueBuilder = new StringBuilder("\n");
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var contentNode = doc.DocumentNode.SelectSingleNode("//div[@id='vsb_content']");
                var ps = contentNode.SelectNodes(".//p");
                foreach (var p in ps)
                {
                    var text = getRealString(p.InnerText);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        valueBuilder.Append(text);
                        valueBuilder.Append("\n");
                    }
                }

                valueBuilder.Append("\n");
                var value = valueBuilder.ToString();
                Regex regex = new Regex("<!--.*-->");
                value = regex.Replace(value, "");
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }
    }
}
