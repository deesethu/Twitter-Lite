using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;
using System.ServiceModel.Channels;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Xml;


namespace TweetAzure
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        public static string dataFilesDirectory = "";
        static Hashtable hashTweet = new Hashtable();
        static Hashtable hashAuthenticateUsers = new Hashtable();
        static Hashtable hashTweetUsers = new Hashtable();
        static Hashtable hashUserDetails = new Hashtable();
        //static int tweetid=0;
        static bool serverStarted = true;
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}
        //this gets the Ip of the client

        public string getIp()
        {
            RemoteEndpointMessageProperty endpoint =
                OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            return ip;
        }
        // this function checks whether the username is valid or not
        public bool isValid(string userId)                                             
        {
            char[] userID = userId.ToArray();
            if (userID.Length == 6)
            {
                if ((userID[0] == 'm') && (userID[1] == 'r') && (userID[2] == 'n') && (userID[3] == 'd'))
                {
                    if ((userID[4] == '0') && (userID[5] == '0'))
                    {
                        return false;
                    }

                    if ((userID[4] == '5') && (userID[5] == '0'))
                    {
                        return true;
                    }

                    if (((int)userID[4] >= 48) && ((int)userID[5] >= 48) && ((int)userID[4] <= 52) && ((int)userID[5] <= 57))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string getUserName()
        {

            return WebOperationContext.Current.IncomingRequest.Headers["Auth-Token"];

        }

        //whether user already exists or not

        bool userAlreadyExists(string userId)
        {
            foreach (string ip in hashAuthenticateUsers.Keys)
            {
                if (hashAuthenticateUsers[ip].ToString() == userId)
                {
                    return true;
                }
            }
            return false;
        }


        //this is the function that authenticates the user

        public string authenticate(string userId)
        {
            if (serverStarted)
            {
                //Tweet tweet1 = new Tweet(0, "", 0, "", false);
                //hashTweet.Add(0, tweet1);
                //writeToFileTweet("F:\\Tweets.xml");
                readFromFile();
                serverStarted = false;
            }
            if (isValid(userId.ToLower()))
            {
                if (!hashAuthenticateUsers.ContainsKey(userId.ToLower()))
                {
                    if (hashAuthenticateUsers.ContainsKey(userId))
                    {
                        return "Already a user logged on from the remote system,Please Logout and then login again";
                    }
                    hashAuthenticateUsers[userId.ToLower()] = true;
                    if (!hashUserDetails.ContainsKey(userId))
                    {
                        hashUserDetails[userId] = new UserDetais(userId, 0, 0, 0, 0);
                    }
                    hashTweetUsers[userId.ToLower()] = null;
                    WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
                    return "authenticated";
                }
            }

            return "not authenticated";
        }


        bool isAuthenticated(string ip)
        {
            if (hashAuthenticateUsers.ContainsKey(ip))
            {
                return true;
            }
            return false;
        }
        // create Tweet function


        public string createTweet(string tweetName)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            string userId = getUserName();
            if (hashTweet.Count <= 50)
            {
                Tweet obj = (Tweet)hashTweet[0];
                int tweetid = obj.tweetId;
                tweetid++;
                obj.tweetId++;
                Tweet newTweet = new Tweet(tweetid, tweetName, 0, userId, false);
                Message messageObject = new Message(tweetid, 0, "", "", false, 0, 0, false);
                newTweet.noOfMessages++;
                newTweet.message.Add(0, messageObject);
                hashTweet.Add(tweetid, newTweet);
                UserDetais ud = (UserDetais)hashUserDetails[userId];
                ud.tweetCount++;
                ud.points = ud.points + 5;
                writeToFileUserDetails("D:\\Users.xml");
                writeToFileTweet("D:\\Tweets.xml");
                writeToFileMessage("D:\\Message.xml");
                return "tweet created with ID :"+tweetid;
            }
            return "Exceeds Max Tweet Limit";
        }

        //this is used to delete a tweet

        public string deleteTweet(string tweetId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            int id = int.Parse(tweetId);
            string userId = getUserName();
            if (hashTweet.ContainsKey(id))
            {
                Tweet tweet = (Tweet)hashTweet[id];
                if (tweet.userId == userId)
                {
                    hashTweet.Remove(id);
                    UserDetais ud = (UserDetais)hashUserDetails[userId];
                    ud.tweetCount--;
                    ud.points = ud.points - 5;
                    writeToFileUserDetails("D:\\Users.xml");
                    writeToFileTweet("D:\\Tweets.xml");
                    writeToFileMessage("D:\\Message.xml");
                    return "Deleted Successfully";
                }
                else
                {
                    return "you are not authenticated to delete the tweet";
                }
            }
            else
            {
                return "TweetId doesn't exists";
            }
        }

        public string gotoTweet(string tweetId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            int id = int.Parse(tweetId);
            string userId = getUserName();
            if (hashTweet.ContainsKey(id))
            {
                hashTweetUsers[userId] = id;
                return "true";
            }
            else
            {
                return "Requested TweetId does not exist!";
            }
        }


        //exiting from the application will delete the binding with ip address

        public void exitFromApp()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");

            if (hashAuthenticateUsers.ContainsKey(getUserName()))
            {
                hashAuthenticateUsers.Remove(getUserName());
            }
        }

        // Mark a tweet as Offensive

        public string markTweetAsOffensive(string tweetId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            int id = int.Parse(tweetId);
            string userId = getUserName();
            if (hashTweet.ContainsKey(id))
            {
                Tweet tweet = (Tweet)hashTweet[id];
                tweet.isOffensive = true;
                hashTweet.Remove(id);
                hashTweet.Add(id, tweet);
                writeToFileTweet("D:\\Tweets.xml");
                writeToFileMessage("D:\\Message.xml");
                return "Tweet Marked Offensive";
            }
            return "Tweet ID dosen't exists";
        }

        public List<string> getTweet(string tweetId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            int id = int.Parse(tweetId);
            List<string> myList = new List<string>();
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                myList.Add("You are not authenticated yet");
                //return "you are not authenticated yet";
                return myList;
            }
            
            if (hashTweet.ContainsKey(id) && id != 0)
            {
                Tweet tweet = (Tweet)hashTweet[id];
                myList.Add("TID\tNo.of Msgs\t\tTName");
                myList.Add(id + "\t" + (tweet.message.Count - 1) + "\t\t\t" + tweet.tweetName);
                //return id + "\t" + tweet.tweetName + "\t" + (tweet.message.Count - 1);
                return myList;
            }
            else
            {
                myList.Add("Tweet ID does not exist!");
                //return "Tweet Id doesn't Exists";
                return myList;
            }
        }
       
        public List<string> getAlltweets()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            List<string> myList = new List<string>();
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                myList.Add("You are not authenticated yet");
                //return "you are not authenticated yet";
                return myList;
            }
            
            int count = hashTweet.Count;
            //int addObject = 0;
            myList.Add("TID\tNo.of Msgs\t\tTName");
            foreach (int keyValue in hashTweet.Keys)
            {
                if (keyValue != 0)
                {
                    Tweet tweet = (Tweet)hashTweet[keyValue];
                    //myList.Add(tweet);
                    myList.Add(keyValue + "\t" + (tweet.message.Count - 1) + "\t\t\t" + tweet.tweetName );
                }
            }
            //the below code also can be used for listing all the tweets
            /*for (int i = 1; addObject < count; i++)
            {
                if (hashTweet.ContainsKey(i))
                {
                    tweets[addObject] = (Tweet)hashTweet[i];
                    addObject++;
                }
            }*/
            return myList;
        }


        public string addMessage(string msg)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            if (tweet.message.Count <= 1001)
            {
                Message g_idObject = (Message)tweet.message[0];
                int g_id = g_idObject.messageId;
                g_id++;
                Message newObject = new Message(tweetId, g_id, msg, userId, true, 0, 0, false);
                tweet.noOfMessages++;
                tweet.message.Add(g_id, newObject);
                tweet.message.Remove(0);
                Message g_Object = new Message(tweetId, g_id, "", "", false, 0, 0, false);
                tweet.message.Add(0,g_Object);
                UserDetais ud = (UserDetais)hashUserDetails[userId];
                ud.msgCount++;
                ud.points = ud.points + 3;
                writeToFileUserDetails("D:\\Users.xml");
                writeToFileTweet("D:\\Tweets.xml");
                writeToFileMessage("D:\\Message.xml");
                return "message Added successfully with id:"+g_id;
            }
            else
            {
                return "Max Message Limit exceeded";
            }
        }

        // Delete a Message 

        public string deleteMessage(string msgId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            int id = int.Parse(msgId);
            int msg_count = tweet.noOfMessages;
            if (tweet.message.ContainsKey(id))
            {
                Message msg = (Message)tweet.message[id];
                if (msg.userId == userId)
                {
                    tweet.message.Remove(id);
                    UserDetais ud = (UserDetais)hashUserDetails[userId];
                    ud.msgCount--;
                    ud.points = ud.points - 3;
                    tweet.noOfMessages--;
                    for (int i = 0; i < msg_count; i++)
                    {
                        foreach (int key in tweet.message.Keys)
                        {
                            Message msg1 = (Message)tweet.message[key];
                            if (msg1.replyTo == id)
                            {
                                tweet.message.Remove(msg1.messageId);
                                ud = (UserDetais)hashUserDetails[msg1.userId];
                                ud.msgCount--;
                                ud.points = ud.points - 3;
                                tweet.noOfMessages--;
                                break;
                            }
                        }
                    }
                    writeToFileUserDetails("D:\\Users.xml");
                    writeToFileTweet("D:\\Tweets.xml");
                    writeToFileMessage("D:\\Message.xml");
                    return "Message successfully Deleted";
                }
                else
                {
                    return "you are not autherized to delete the message";
                }
            }
                return "Message Id doesn't Exists";

        }

        //reply to a Message 

        public string replyToMessage(string msgId, string msg)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");

            if (!(hashAuthenticateUsers.ContainsKey(getUserName())))
            {
                return "you are not authenticated yet";
            }
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            int id = int.Parse(msgId);
            if (tweet.message.ContainsKey(id))
            {
                Message msgObject = (Message)tweet.message[id];
                if (msgObject.isReply)
                {
                    if (tweet.message.Count <= 1001)
                    {
                        Message g_idObject = (Message)tweet.message[0];
                        int g_id = g_idObject.messageId;
                        g_id++;
                        Message newObject = new Message(tweetId, g_id, msg, userId, false, id, 0, false);
                        tweet.message.Add(g_id, newObject);
                        tweet.noOfMessages++;
                        tweet.message.Remove(0);
                        Message g_Object = new Message(tweetId, g_id, "", "", false, 0, 0, false);
                        tweet.message.Add(0, g_Object);
                        UserDetais ud = (UserDetais)hashUserDetails[userId];
                        ud.msgCount++;
                        ud.points = ud.points + 3;
                        writeToFileUserDetails("D:\\Users.xml");
                        writeToFileTweet("D:\\Tweets.xml");
                        writeToFileMessage("D:\\Message.xml");
                        return "reply Added successfully to id:"+id;
                    }
                    else
                    {
                        return "Max Message Limit exceeded";
                    }
                }
                else
                {
                    return "you cannot reply to this because it is a reply";
                }
            }
            return "Id doesn't Exists";
        }


        public List<string> listAMessage(string msgId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            List<string> myList = new List<string>();
            myList.Add("MID\tLikeCount\t\tMSG");
            int m_id, numberOfLikes;
            string messageContent;
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            int id = int.Parse(msgId);
            if (tweet.message.ContainsKey(id))
            {
                Message messageObject = (Message)tweet.message[id];
                m_id = messageObject.messageId;
                numberOfLikes = messageObject.likes;
                messageContent = messageObject.message;
                myList.Add(m_id + "\t" + numberOfLikes+ "\t\t\t" + messageContent );
                foreach(int key in tweet.message.Keys)
                {
                    messageObject = (Message)tweet.message[key];
                    if((messageObject.replyTo == id) && (key != id) && (key!=0))
                    {
                        m_id = messageObject.messageId;
                        numberOfLikes = messageObject.likes;
                        messageContent = messageObject.message;
                        myList.Add(m_id + "\t" + numberOfLikes + "\t\t\t" + messageContent);
                    }
                }
            }
            return myList;
        }

        public List<string> listAllMessages()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            List<string> myList = new List<string>();
            myList.Add("MID\tLikeCount\t\tMSG");
            int m_id, numberOfLikes;
            string messageContent;
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            foreach(int key in tweet.message.Keys)
            {
                Message messageObject = (Message)tweet.message[key];
                if (key != 0)
                {
                    m_id = messageObject.messageId;
                    numberOfLikes = messageObject.likes;
                    messageContent = messageObject.message;
                    myList.Add(m_id + "\t" + numberOfLikes + "\t\t\t" + messageContent);
                }
            }
            return myList;
        }


        //Like message
        public string likeMessage(string msgId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            int id = int.Parse(msgId);
            List<string> myList = new List<string>();
            Message messageObject = (Message)tweet.message[id];
            myList = messageObject.userList;
            string[] users = myList.ToArray();
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i] == userId)
                {
                    return "The user Already Liked the message";
                }
            }
            //IEnumerator ie = myList.GetEnumerator();
            //while (ie.MoveNext())
            //{
            //    if (ie.ToString() == userId)
            //    {
            //        return "The user Already Liked the message";
            //    }
            //}
            UserDetais ud = (UserDetais)hashUserDetails[userId];
            ud.likeCount++;
            ud.points = ud.points + 1;
            myList.Add(userId);
            messageObject.userList = myList;
            messageObject.likes++;
            //tweet.message.Remove(id);
            //tweet.message.Add(id, messageObject);
            writeToFileUserDetails("D:\\Users.xml");
            writeToFileTweet("D:\\Tweets.xml");
            writeToFileMessage("D:\\Message.xml");
            return "Message Successfully Liked";
        }

        public string markMessageAsOffensive(string msgId)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            int id = int.Parse(msgId);
            if (tweet.message.ContainsKey(id))
            {
                Message messageObject = (Message)tweet.message[id];
                messageObject.isOffensive = true;
                tweet.message.Remove(id);
                tweet.message.Add(id, messageObject);
                writeToFileTweet("D:\\Tweets.xml");
                writeToFileMessage("D:\\Message.xml");
                return "Message Successfully marked as offensive";
            }
            return "Message Id doesn't exists";
        }

        public string back()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            string userId = getUserName();
            hashTweetUsers.Remove(userId);
            return "you are in main menu";
        }


        public List<string> statistics()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            List<string> myList = new List<string>();
            myList.Add("UID\tTCount\tMSGCount\tLikesCount");
            List<UserDetais> uList = new List<UserDetais>();
            foreach (string key in hashUserDetails.Keys)
            {
                uList.Add((UserDetais)hashUserDetails[key]);
            }
            UserDetais[] ud = uList.ToArray();
            UserDetais temp = new UserDetais();
            for (int j = 0; j < ud.Length; j++)
            {
                for (int i = 0; i < ud.Length - 1; i++)
                {
                    if (ud[i].points < ud[i + 1].points)
                    {
                        temp = ud[i];
                        ud[i] = ud[i + 1];
                        ud[i + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < 3 && i< ud.Length; i++)
            {
                string st = ud[i].userId + "\t" + ud[i].tweetCount + "\t" + ud[i].msgCount + "\t" + ud[i].likeCount;
                myList.Add(st);
            }
            return myList;
        }

        public List<string> tweetStatistics()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Set("Access-Control-Allow-Origin", "*");
            List<string> myList = new List<string>();
            myList.Add("MID\tNo.of MSG Likes");
            string userId = getUserName();
            int tweetId = (int)(hashTweetUsers[userId]);
            Tweet tweet = (Tweet)hashTweet[tweetId];
            List<Message> mList = new List<Message>();
            foreach (int key in tweet.message.Keys)
            {
                if (key != 0)
                {
                    Message newObject = (Message)tweet.message[key];
                    mList.Add(newObject);
                }
            }
            Message[] msg_array = mList.ToArray();
            Message temp = new Message();
            for (int j = 0; j < msg_array.Length; j++)
            {
                for (int i = 0; i < msg_array.Length - 1; i++)
                {
                    if (msg_array[i].likes < msg_array[i + 1].likes)
                    {
                        temp = msg_array[i];
                        msg_array[i] = msg_array[i + 1];
                        msg_array[i + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < 3 && i < msg_array.Length; i++)
            {
                string st = msg_array[i].messageId + "\t" + msg_array[i].likes;
                myList.Add(st);
            }
            return myList;
        }

        //This function is to write memory to file

        void writeToFileTweet(string fileName)
        {
            //Console.WriteLine(
            //    "Creating a Person object and serializing it.");
            FileStream writer = new FileStream(fileName, FileMode.Create);
            //FileStream writer1 = new FileStream("object.json", FileMode.Create);
            List<Tweet> myList = new List<Tweet>();
            foreach (int key in hashTweet.Keys)
            {
                Tweet tweet = (Tweet)hashTweet[key];
                myList.Add(tweet);
            }
            DataContractSerializer ser =
            new DataContractSerializer(typeof(List<Tweet>));
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<Tweet>));
            ser.WriteObject(writer, myList);
                //ser1.WriteObject(writer1, myList);
            
            //foreach(int key in hashTweet.Keys)
            //{
            //    Tweet tweet = (Tweet)hashTweet[key];
            //    foreach (int m_key in tweet.message.Keys)
            //    {
            //        Message message = (Message)tweet.message[m_key];
            //        DataContractSerializer ser =
            //        new DataContractSerializer(typeof(Message));
            //        DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(Message));
            //        ser.WriteObject(writer, message);
            //        ser1.WriteObject(writer1, message);
            //    }
            //}
            //Person p1 = new Person("Perraju", "Bendapudi", 101);
            //FileStream writer = new FileStream(fileName, FileMode.Create);
            //FileStream writer1 = new FileStream("object.json", FileMode.Create);
            //DataContractSerializer ser =
            //    new DataContractSerializer(typeof(Person));
            //DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(Person));
            //ser.WriteObject(writer, p1);
            //ser1.WriteObject(writer1, p1);
            writer.Close();
            //writer1.Close();
        }


        public void writeToFileUserDetails(string fileName)
        {
            FileStream writer = new FileStream(fileName, FileMode.Create);
            List<UserDetais> myList = new List<UserDetais>();
            foreach (string key in hashUserDetails.Keys)
            {
                UserDetais user = (UserDetais)hashUserDetails[key];
                myList.Add(user);
            }
            DataContractSerializer ser =
            new DataContractSerializer(typeof(List<UserDetais>));
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<UserDetais>));
            ser.WriteObject(writer, myList);
            writer.Close();
        }

        public void writeToFileMessage(string fileName)
        {
            FileStream writer = new FileStream(fileName, FileMode.Create);
            List<Message> myList = new List<Message>();
            foreach (int key in hashTweet.Keys)
            {
                Tweet tweet = (Tweet)hashTweet[key];
                foreach (int m_key in tweet.message.Keys)
                {
                    Message message = (Message)tweet.message[m_key];
                    myList.Add(message);
                }
            }
            DataContractSerializer ser =
            new DataContractSerializer(typeof(List<Message>));
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<Message>));
            ser.WriteObject(writer, myList);
            writer.Close();
        }
        //Reading from file

        public void readFromFile()
        {
            //Console.WriteLine("Deserializing an instance of the object.");
            FileStream fs;
            try
            {
                fs = new FileStream("D:\\Tweets.xml", FileMode.Open);
                //FileStream fs1 = new FileStream("object.json", FileMode.Open);

                XmlDictionaryReader reader =
                    XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

                //JsonReaderWriterFactory reader1 =
                //    JsonReaderWriterFactory.CreateJsonReader(fs1, new XmlDictionaryReaderQuotas());
                List<Tweet> myList = new List<Tweet>();
                DataContractSerializer ser = new DataContractSerializer(typeof(List<Tweet>));
                //DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<Tweet>));

                // Deserialize the data and read it from the instance.

                List<Tweet> deserializedTweet =
                    (List<Tweet>)ser.ReadObject(reader);
                //hashTweet.Add(deserializedTweet.tweetId, deserializedTweet);
                //IEnumerator ie = deserializedTweet.GetEnumerator();
                Tweet[] tweets = deserializedTweet.ToArray();
                for (int i = 0; i < tweets.Length; i++)
                {
                    //Tweet tweet = (Tweet)ie;
                    Tweet newTweet = new Tweet(tweets[i].tweetId, tweets[i].tweetName, tweets[i].noOfMessages, tweets[i].userId, tweets[i].isOffensive);
                    if (tweets[i].tweetName == "")
                    {
                        hashTweet.Add(0, newTweet);
                        continue;
                    }
                    hashTweet.Add(tweets[i].tweetId, newTweet);
                }

                reader.Close();
                fs.Close();
            }
            catch (FileNotFoundException)
            {
                fs = new FileStream("D:\\Tweets.xml", FileMode.Create);
                Tweet newObject = new Tweet(0, "", 0, "", false);
                List<Tweet> myList = new List<Tweet>();
                myList.Add(newObject);
                DataContractSerializer ser =
            new DataContractSerializer(typeof(List<Tweet>));
                DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<Tweet>));
                ser.WriteObject(fs, myList);
                hashTweet.Add(0, newObject);
                fs.Close();
            }
            FileStream fs_msg;
            try
            {
                // To read Messages
                fs_msg = new FileStream("D:\\Message.xml", FileMode.Open);
               if (fs_msg.Length != 0)
               {
                   XmlDictionaryReader reader_msg =
                       XmlDictionaryReader.CreateTextReader(fs_msg, new XmlDictionaryReaderQuotas());

                   DataContractSerializer ser_msg = new DataContractSerializer(typeof(List<Message>));
                   //DataContractJsonSerializer ser1 = new DataContractJsonSerializer(typeof(List<Tweet>));
                   //List<Message> 
                   List<Message> deserializedMessage =
                   (List<Message>)ser_msg.ReadObject(reader_msg);

                   Message[] message = deserializedMessage.ToArray();
                   for (int i = 0; i < message.Length; i++)
                   {
                       Message msg = new Message(message[i].tweetId, message[i].messageId, message[i].message, message[i].userId, message[i].isReply, message[i].replyTo, message[i].likes, message[i].isOffensive);
                       Tweet tweet = (Tweet)hashTweet[message[i].tweetId];
                       if (message[i].userId == "")
                       {
                           tweet.message.Add(0, msg);
                           continue;
                       }
                       string[] users = message[i].userList.ToArray();
                       for (int j = 0; j < users.Length; j++)
                       {
                           msg.userList.Add(users[j]);
                       }
                       tweet.message.Add(message[i].messageId, msg);
                   }
                   //FileStream fs1 = new FileStream("object.json", FileMode.Open);
                   fs_msg.Close();
                   reader_msg.Close();
               }
            }
            catch (FileNotFoundException)
            {
                fs_msg = new FileStream("D:\\Message.xml", FileMode.Create);
                fs_msg.Close();
            }
            FileStream fs_user;
            try
            {
                fs_user = new FileStream("D:\\Users.xml", FileMode.Open);
                if (fs_user.Length != 0)
                {
                    XmlDictionaryReader reader_user =
                       XmlDictionaryReader.CreateTextReader(fs_user, new XmlDictionaryReaderQuotas());
                    DataContractSerializer ser_user = new DataContractSerializer(typeof(List<UserDetais>));
                    List<UserDetais> deserializedUsers =
                   (List<UserDetais>)ser_user.ReadObject(reader_user);
                    UserDetais[] user = deserializedUsers.ToArray();
                    for (int i = 0; i < user.Length; i++)
                    {
                        UserDetais userObject = new UserDetais(user[i].userId, user[i].tweetCount, user[i].msgCount, user[i].likeCount, user[i].points);
                        hashUserDetails.Add(user[i].userId, userObject);
                    }

                }
                fs_user.Flush();
                fs_user.Close();
            }
            catch (FileNotFoundException)
            {
                fs_user = new FileStream("D:\\Users.xml", FileMode.Create);
                fs_user.Close();
            }
            //Tweet des = (Tweet)ser1.ReadObject(fs1);
            //Console.WriteLine(String.Format("{0} {1} {2} {3} {4}",
            //deserializedPerson.tweetId, deserializedPerson.tweetName,
            //deserializedPerson.noOfMessages, deserializedPerson.isOffensive, deserializedPerson.userId));
            //Console.WriteLine(String.Format("{0} {1}, ID: {2}", des.FirstName, des.LastName, des.ID));
                
        }
    }
}
