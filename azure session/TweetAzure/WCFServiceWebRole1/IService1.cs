using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;
using System.Xml;


namespace TweetAzure
{
    [DataContract]
    public class Tweet : IExtensibleDataObject
    {
        [DataMember]
        public int tweetId;

        [DataMember]
        public string tweetName;

        [DataMember]
        public int noOfMessages;

        //[DataMember]
        //DateTime createDate;

        public Hashtable message = new Hashtable();

        [DataMember]
        public bool isOffensive;

        [DataMember]
        public string userId;

        public Tweet(int tweetId, string tweetName, int noOfMessages, string userId, bool isOffensive)
        {
            this.tweetId = tweetId;
            this.tweetName = tweetName;
            this.noOfMessages = noOfMessages;
            this.isOffensive = isOffensive;
            this.userId = userId;
            //this.createDate = createDate;
        }
        private ExtensionDataObject extensionData_Value;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }
    }

    [DataContract]
    public class Message : IExtensibleDataObject
    {
        [DataMember]
        public int tweetId;

        [DataMember]
        public int messageId;

        [DataMember]
        public string message;

        [DataMember]
        public string userId;

        [DataMember]
        public bool isReply;

        [DataMember]
        public int replyTo;            //this contains the id of message to which this is a reply

        [DataMember]
        public int likes;

        [DataMember]
        public List<string> userList = new List<string>();

        [DataMember]
        public bool isOffensive;

        public Message()
        {
        }
        public Message(int tweetId, int messageId, string message, string userId, bool isReply, int replyTo, int likes, bool isOffensive)
        {
            this.tweetId = tweetId;
            this.messageId = messageId;
            this.message = message;
            this.userId = userId;
            this.isReply = isReply;
            this.replyTo = replyTo;
            this.likes = likes;
            this.isOffensive = isOffensive;
        }
        private ExtensionDataObject extensionData_Value;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }
    }

    [DataContract]
    class UserDetais : IExtensibleDataObject
    {
        [DataMember]
        public string userId;

        [DataMember]
        public int tweetCount;

        [DataMember]
        public int msgCount;

        [DataMember]
        public int likeCount;

        [DataMember]
        public int points;

        public UserDetais()
        {
        }
        public UserDetais(string userId, int tweetCount, int msgCount, int likeCount, int points)
        {
            this.userId = userId;
            this.tweetCount = tweetCount;
            this.msgCount = msgCount;
            this.likeCount = likeCount;
            this.points = points;
        }

        private ExtensionDataObject extensionData_Value;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }
    }


    //public class Users
    //{
    //    string Ip;
    //    int tweetId;


    //    public Users(string Ip, int tweetId)
    //    {
    //        this.Ip = Ip;
    //        this.tweetId = tweetId;
    //    }
    //}

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.

    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/authenticate/{userId}")]

        string authenticate(string userId);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/tweets/createTweet/{tweetName}")]
        string createTweet(string tweetName);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/tweets/deleteTweet/{tweetId}")]
        string deleteTweet(string tweetId);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/tweets/gotoTweet/{tweetId}")]
        string gotoTweet(string tweetId);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "/tweets/listATweet/{tweetId}")]
        List<string> getTweet(string tweetId);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "/tweets/listAllTweets")]
        List<string> getAlltweets();

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/tweets/logout/")]

        void exitFromApp();

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/tweets/markTweetAsOffensive/{tweetId}")]

        string markTweetAsOffensive(string tweetId);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/addMessage/{msg}")]

        string addMessage(string msg);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/deleteMessage/{msgId}")]

        string deleteMessage(string msgId);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/replyToMessage/{msgId}&{msg}")]

        string replyToMessage(string msgId, string msg);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "/message/listAMessage/{msgId}")]

        List<string> listAMessage(string msgId);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "/message/listAllMessages/")]

        List<string> listAllMessages();

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/likeMessages/{msgId}")]

        string likeMessage(string msgId);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/markMessageAsOffensive/{msgId}")]

        string markMessageAsOffensive(string msgId);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "/message/back/")]
        string back();

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "message/TweetStatistics/")]

        List<string> tweetStatistics();

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Xml,
                   ResponseFormat = WebMessageFormat.Xml,
                   UriTemplate = "/tweets/statistics/")]

        List<string> statistics();
    }


    //[OperationContract]
    //string GetData(int value);

    //[OperationContract]
    //CompositeType GetDataUsingDataContract(CompositeType composite);

    // TODO: Add your service operations here

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}