using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestHttpServer
{
    /// <summary>
    /// Serve服务器类
    /// </summary>
    class Program
    {
        static HttpListener httpListener = null;

        static void ResponseError(HttpListenerResponse response)
        {
            response.StatusCode = 200;
            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                writer.Write("参数错误!");
            }
        }
        
        static void Process()
        {
            Dictionary<string, string> userdata = new Dictionary<string, string>();//字典存储用户名，及用户密码
            Dictionary<string, string> userMsg = new Dictionary<string, string>();//通过用户信息
            int count = 1;
            while (true)
            {
                // 这句会等待客户端请求，同步操作（类似readkey,如无请求则会停滞在这一句等待）
                HttpListenerContext httpListenerContext = httpListener.GetContext();
                Console.WriteLine("-----------第{0}次访问，接收到客户端请求URL如下--------------",count);
                count++;//访问次数记录递增
                Console.WriteLine(httpListenerContext.Request.RawUrl);//输出客户端请求的URL信息（从端口号之后内容 /sava...）
                string urlstr = httpListenerContext.Request.RawUrl;
                if (urlstr.Length < 5)
                {
                    ResponseError(httpListenerContext.Response);
                    continue;
                }

                /*http://192.168.199.118:8080/save/?user=boss&data=heihei*/
                string urlmsg = urlstr.Substring(1, 4);//获取sava还是load
                if (urlmsg == "save")//如果是存档操作
                {
                    string user=null;
                    string password = null;
                    string userPass = null ;
                    string dateMsg = "0";

                    //var keys = httpListenerContext.Request.QueryString.AllKeys;//获取请求中包含的查询字符串如？后的id=1，为一个字符串数组
                    user = httpListenerContext.Request.QueryString.Get("user");
                    password = httpListenerContext.Request.QueryString.Get("password");
                   // dateMsg = httpListenerContext.Request.QueryString.Get("dateMsg") ;
                    //foreach (var key in keys)                                         //在服务端遍历输出请求URL中的参数，第一次key=user第二次key=data，value皆为传入的值。
                    //{
                    //    if (key == "user") { user = httpListenerContext.Request.QueryString[key]; }
                    //    else if (key == "password") { password = httpListenerContext.Request.QueryString[key]; }
                    //    else if (key == "dateMsg") { dateMsg = httpListenerContext.Request.QueryString[key]; }
                    //    //Console.WriteLine(key + ":"+ httpListenerContext.Request.QueryString[key]);
                    //}
                    userPass = user + password;                  
                    if (userdata.ContainsKey(user))//如果字典已存在同名user,直接修改该user的Value
                    {
                        using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream, Encoding.UTF8))
                        {
                            writer.Write("用户名已被使用");
                        }
                        //userdata[user] = data;
                    }
                    else
                    {
                        userdata.Add(user, password);//如果之前没有此user信息，userinf字典追加获得的用户信息
                        dateMsg = "1";
                        userMsg.Add(userPass, dateMsg);
                    }


                   
                    httpListenerContext.Response.StatusCode = 200;//返回请求已成功状态码

                    // -------------------------------------------------------------------------------
                    //将发送到客户端的字符流,字符编码为UTF8，http请求一定要有返回值，返回的是字符串
                    using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream, Encoding.UTF8))
                    {
                        //writer.WriteLine("这是服务端返回客户端的数据：");
                        if (user != null && password != null&&dateMsg!=null)
                        {
                            writer.Write("Success Saved!");//返回存档成功   

                        }
                        else
                        {
                            writer.Write("Failed Saved!");//返回存档成功   
                        }
                                            
                    }
                }
                else if (urlmsg == "load")//如果是读档操作
                {
                    string user = "";
                    string password = "";
                    string backMsg = "1";
                    //var keys = httpListenerContext.Request.QueryString.AllKeys;//获取请求中包含的查询字符串如？后的id=1，为一个字符串数组
                    user = httpListenerContext.Request.QueryString.Get("user");
                    password = httpListenerContext.Request.QueryString.Get("password");
                    //foreach (var key in keys)//在服务端遍历输出请求URL中的参数
                    //{
                    //    if (key == "user") { user = httpListenerContext.Request.QueryString[key]; }//如果收到客户端发来的key为user，将其value值赋给本地user变量
                    //    else if (key == "password") { password = httpListenerContext.Request.QueryString[key]; }
                    //    //Console.WriteLine(key + ":"+ httpListenerContext.Request.QueryString[key]);
                    //}
                    foreach (var userPassword in userMsg.Keys)
                    {
                        if (userPassword == user+password)//如果找到客户端发送来的用户，返回字典中该key的value
                        {
                            backMsg = userMsg[userPassword];
                        }
                    }
                    httpListenerContext.Response.StatusCode = 200;//返回请求已成功状态码
                    //将发送到客户端的字符流,字符编码为UTF8
                    using (StreamWriter writer = new StreamWriter(httpListenerContext.Response.OutputStream, Encoding.UTF8))
                    {
                        //writer.WriteLine("这是服务端返回客户端的数据：");    
                        if (backMsg != null)
                        {
                            writer.Write(backMsg);//返回玩家存储的数据
                        }
                        else
                        {
                            writer.Write("读取失败!");
                        }
                                              
                    }
                }
                else
                {
                    ResponseError(httpListenerContext.Response);
                }
            }
        }

        static void Main(string[] args)
        {
            httpListener = new HttpListener();//创建服务器监听
            //指定身份验证方式Anonymous为默认值，允许任意的客户机进行连接,不需要身份验证。
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //通过设置Prefixes属性来设置侦听URL及端口.
            httpListener.Prefixes.Add("http://+:8080/");//+代表本机IP,包含localhost（127.0.0.1）
            //httpListener.Prefixes.Add("http://localhost:8080/");
            //开启对指定URL和端口的监听,开始处理客户端输入请求。
            httpListener.Start();
            PrintPrefixes(httpListener);
            Thread thread = new Thread(new ThreadStart(Process));//新建子线程（要执行的方法内容）
            thread.Start();//启动子线程，由于子线程为while死循环，主线程即便后面没有代码或执行完毕也不会退出

            Console.WriteLine(1 + 1);
        }

        /// <summary>
        /// 打印监听地址信息测试
        /// </summary>
        /// <param name="listener"></param>
       static void PrintPrefixes(HttpListener listener)
        {
            HttpListenerPrefixCollection prefixes = listener.Prefixes;
            if (prefixes.Count == 0)
            {
                Console.WriteLine("There are no prefixes.");
            }
            foreach (string prefix in prefixes)
            {
                Console.WriteLine("服务器已启动，监听："+prefix);
            }
            // Show the listening state.
            if (listener.IsListening)
            {
                Console.WriteLine("The server is listening.");
            }

        }
    }
}
