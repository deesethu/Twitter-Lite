using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TWSClient
{
    class Program
    {
        //static int t;
        static void Main(string[] args)
        {

            string auth;
            string baseaddress = "http://mrndtwitter.cloudapp.net";
            bool gotot = true;
            bool gotomenu = true;
            try
            {
                WebClient client = new WebClient();
                client.BaseAddress = baseaddress;
                //client.Proxy = new WebProxy("127.0.0.1", 8888);
                client.Headers.Add("Content-Type", "application/xml");
                while (gotomenu)
                {
                    Console.WriteLine("\nMenu\n-------------\n1.Authentication\n2.Create Tweet\n3.Delete Tweet\n4.Goto Tweet\n5.List a Tweet\n6.List all Tweets\n7.Mark Tweet as Offensive\n8.Statistics\n9.Log Out\n\nEnter your choice : ");
                    int ch;
                    ch = int.Parse(Console.ReadLine());
                    gotot = true;
                    switch(ch)
                    {
                        case 1:
                    
                                Console.WriteLine("Enter Userid : \n");
                                string UserId = Console.ReadLine();
                                 auth = client.DownloadString("/authenticate/"+UserId);
                                 client.Headers.Add("Auth-Token", UserId);
                                 Console.WriteLine(auth);
                                break;
                    
                        
                        case 2:

                                Console.WriteLine("Enter Tweet Name\n");                           
                                Console.WriteLine(client.UploadString("/tweets/createTweet/"+Console.ReadLine(), ""));                                
                                break;
                           
                        case 3:
                                Console.WriteLine("Enter Tweet Id : \n");
                                string Tweetid = Console.ReadLine();
                                string Dtweet = client.UploadString("/tweets/deleteTweet/" + Tweetid,"DELETE", "");
                                Console.WriteLine(Dtweet);
                                break;


                        case 4:
                                Console.WriteLine("Enter Tweet Id : \n");
                                string TweetID = Console.ReadLine();
                                string oneTweet = client.DownloadString("/tweets/gotoTweet/"+TweetID);
                                if (oneTweet.Contains("true"))
                                {
                                    while (gotot)
                                    {
                                        Console.WriteLine("\n\t1.Add Message\n\t2.Delete Message\n\t3.Reply to Message\n\t4.List a Message\n\t5.List all Messages\n\t6.Like a Message\n\t7.Mark Message as Offensive\n\t8.Tweet Statistics\n\t9.Back to Previous menu\nEnter your Choice : ");
                                        int cl;
                                        cl = int.Parse(Console.ReadLine());
                                        switch (cl)
                                        {
                                            case 1:
                                                Console.WriteLine("Enter your Message : \n");
                                                Console.WriteLine(client.UploadString("/message/addMessage/" + Console.ReadLine(), ""));
                                                break;

                                            case 2:
                                                Console.WriteLine("Enter the Message ID : \n");
                                                Console.WriteLine(client.UploadString("/message/deleteMessage/" + Console.ReadLine(), "DELETE",""));
                                                break;

                                            case 3:
                                                Console.WriteLine("Enter the Message ID : \n");
                                                string msgid = Console.ReadLine();
                                                Console.WriteLine("Enter your Message : \n");
                                                string msg = Console.ReadLine();
                                                Console.WriteLine(client.UploadString("/message/replyToMessage/" + msgid + "&" + msg, ""));
                                                break;

                                            case 4:
                                                Console.WriteLine("Enter the Message ID : \n");
                                                string v3 = client.DownloadString("/message/listAMessage/" + Console.ReadLine());
                                                byte[] vb3 = System.Text.Encoding.Default.GetBytes(v3);

                                                 using (MemoryStream stream = new MemoryStream(vb3))
                                                 {
                                                     DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                                     List<string> msgs = (List<string>)ser.ReadObject(stream);

                                                     foreach(string s in msgs)
                                                        Console.WriteLine(s);
                                                 }
                                                break;

                                            case 5:
                                                string v4 = client.DownloadString("/message/listAllMessages/");
                                                byte[] vb4 = System.Text.Encoding.Default.GetBytes(v4);

                                                 using (MemoryStream stream = new MemoryStream(vb4))
                                                 {
                                                     DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                                     List<string> msgs = (List<string>)ser.ReadObject(stream);

                                                     foreach(string s in msgs)
                                                        Console.WriteLine(s);
                                                 }
                                                break;

                                            case 6:
                                                Console.WriteLine("Enter the Message ID : \n");
                                                Console.WriteLine(client.UploadString("/message/likeMessages/" + Console.ReadLine(), ""));
                                                break;

                                            case 7:
                                                Console.WriteLine("Enter the Message ID : \n");
                                                Console.WriteLine(client.UploadString("/message/markMessageAsOffensive/" + Console.ReadLine(), ""));
                                                break;

                                            case 8:

                                                string v5 = (client.DownloadString("message/TweetStatistics/"));

                                                byte[] vb5 = System.Text.Encoding.Default.GetBytes(v5);

                                                using (MemoryStream stream = new MemoryStream(vb5))
                                                {
                                                     DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                                     List<string> msgs = (List<string>)ser.ReadObject(stream);

                                                     foreach(string s in msgs)
                                                        Console.WriteLine(s);
                                                }
                                                break;

                                            case 9:

                                                gotot = false;
                                                break;

                                            default:
                                                Console.WriteLine("Enter Correct Choice");
                                                break;

                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(oneTweet);
                                }
                                                    
                                break;
                           
                        case 5:
                                Console.WriteLine("\nEnter the Tweet ID : \n ");
                                string v1 = client.DownloadString("/tweets/listATweet/" + Console.ReadLine());
                                byte[] vb1 = System.Text.Encoding.Default.GetBytes(v1);
                                using (MemoryStream stream = new MemoryStream(vb1))
                                {
                                    DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                    List<string> tweets = (List<string>)ser.ReadObject(stream);

                                    foreach (string s in tweets)
                                        Console.WriteLine(s);
                                }
                                break;
                           
                        case 6: 
                                string value = client.DownloadString("/tweets/listAllTweets");

                                byte[] valueBytes = System.Text.Encoding.Default.GetBytes(value);

                                using (MemoryStream stream = new MemoryStream(valueBytes))
                                {
                                    DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                    List<string> tweets = (List<string>)ser.ReadObject(stream);

                                    foreach(string s in tweets)
                                    Console.WriteLine(s);
                                }
                                
                                break;
                            
                        case 7:
                                Console.WriteLine("\nEnter Tweet ID : \n");                                
                                Console.WriteLine(client.UploadString("/tweets/markTweetAsOffensive/"+Console.ReadLine(),""));                                
                                break;
                           
                        case 8:
                                string v2 = (client.DownloadString("/tweets/statistics/"));

                                byte[] vb2 = System.Text.Encoding.Default.GetBytes(v2);

                                using (MemoryStream stream = new MemoryStream(vb2))
                                {
                                    DataContractSerializer ser = new DataContractSerializer(typeof(List<string>));

                                    List<string> tweets = (List<string>)ser.ReadObject(stream);

                                    foreach(string s in tweets)
                                    Console.WriteLine(s);
                                }
                                break;
                            
                        case 9:
                                client.DownloadString("/tweets/logout/");
                                Console.WriteLine("Successfully Logged Out");
                                gotomenu = false;
                                break;

                        default:
                                Console.WriteLine("Enter Correct Choice");
                                break;

                        }
                                                          
                }
            }

            catch (WebException ex)
            {
                string message = string.Empty;
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        message = reader.ReadToEnd();
                    }
                }
                Console.WriteLine("Exception : {0} Message : {0}", ex, message);
            }
        }
    }
}
