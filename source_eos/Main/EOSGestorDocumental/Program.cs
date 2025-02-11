using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace EOSGestorDocumental
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            /* var client = new RestClient("http://localhost:4883/AuthorizationList/DetailsPrint/40629");
             var request = new RestRequest(Method.GET);
             request.AddHeader("postman-token", "690422cc-289f-f66b-a63b-9d289e1fa808");
             request.AddHeader("cache-control", "no-cache");
             IRestResponse response = client.Execute(request);
             */
            var client = new RestClient("http://localhost:39457/api/GestorDocumental/Edit");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content", "multipart/form-data");
            request.AddHeader("agency-key", "P+iAbLt5LFJd5ycQGNI9jJOVUNBUIVmyYj7EzFL5sHiQFYsh1tbrOg==");
            request.AddHeader("x-token", "9A3C8AF8EE64C2D3136D4C455D7C4A25CBB20028");
            request.AddHeader("content-type", "multipart/form-data;");
            request.AddFile(" xsGcuHbAE7zQDHAA/HSJuOUHYlEg4/WbFtbgTupCuFjvTXwY7woKNUOCNOo6v0VHirNDmRf+yzAYw7xWft/p9TNlXLh2a8jW2Mcr+R+Jg12F6z5UAlHl0J8QqKs2/jqsIlSfQA+ZF8FWFjoEoOucW+hmlz9nihN14mgmHheF0XyT+iOlaGeetNKm6PeGfTeJvSlGoPWk6zkTQzlT6Zq6btbPToo1MtbDjATW/qjs0UL2AGCpotwT88E2i5wiYGQ4+aEJepz+bIGSwbUUI9LOEH/IoTaKBKOOEkIYqeL/MWeMZ42Ez4fWRp2oHwqiXjkenHGpDEzm9GgLl3srk84Q/5rtwqAhhKPVz9VR89A7cAHX0iyN4tYahsquXVY8LuVs81139glUgfcuo0xZnQ64oBalRb4rauasYAA8TnUzNpRXiPWZdS98QhPoHIhtsY8KFHSmnL2x8B8x8ZK6sol//nP4lDsDCN8cUW+vDwjwxfQhuP6ar4uEWZ7SfWev5tVIJGo8mYn//CYjTIxu3P4MqBjDvFZ+3+n1HKNMijO5gMx0FFd26yA4u52oHwqiXjkenHGpDEzm9GjxYmFgZ1tac2ZPPfslykyvSksF6IjVS6qyYpVUM1mvAWDKNXRwXsiA/+htLQAQSgA=", "bono_33073508.pdf");
            
            IRestResponse response = client.Execute(request);


        
        }
    }
}
