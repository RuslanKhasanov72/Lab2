using System;
using System.Diagnostics;
using System.Xml;

namespace Lab2
{
    public class InfoFileRead
    {
        int memorylimit;
        int processortimelimit;
        int absolutetimelimit;
        public void FileRead()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("../../../../runinfo.xml");
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {

                foreach (XmlElement xnode in xRoot)
                {


                    
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                      
                        if (childnode.ParentNode.Name == "memorylimit")
                        {
                            memorylimit = int.Parse(childnode.InnerText);
                        }
                       
                        if (childnode.ParentNode.Name == "processortimelimit")
                        {
                            processortimelimit = int.Parse(childnode.InnerText);
                        }
                        if (childnode.ParentNode.Name == "absolutetimelimit")
                        {
                            absolutetimelimit = int.Parse(childnode.InnerText);
                        }
                    }

                }
            }
        }

        public void RunApp(string path)
        {
            using (Process myProcess = Process.Start(path))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("../../../../processlog.xml");
                XmlElement? xRoot = xDoc.DocumentElement;
 
                
                Process[] procList = Process.GetProcessesByName("Google Chrome");
                while (!myProcess.HasExited)
                {
                    XmlElement eventElem = xDoc.CreateElement("event");

                    XmlElement typeElem = xDoc.CreateElement("type");
                    XmlElement processidElem = xDoc.CreateElement("processid");
                    XmlElement processElem = xDoc.CreateElement("process");
                    XmlElement messageElem = xDoc.CreateElement("message");
                    XmlElement timeElem = xDoc.CreateElement("time");
                    XmlElement exitcodeElem = xDoc.CreateElement("exitcode");

                    XmlText typeText = xDoc.CreateTextNode("notification");
                    XmlText processidText = xDoc.CreateTextNode(myProcess.Id.ToString());
                    XmlText processText = xDoc.CreateTextNode(myProcess.ProcessName);
                    XmlText messageText = xDoc.CreateTextNode($"Memory: {myProcess.WorkingSet64}; Processor Time: {myProcess.TotalProcessorTime}; Absolute time: {myProcess.UserProcessorTime}");
                    XmlText timeText = xDoc.CreateTextNode(DateTime.Now.ToString());


                    if (myProcess.WorkingSet64 > memorylimit || (int)myProcess.TotalProcessorTime.TotalMilliseconds > processortimelimit || (int)myProcess.UserProcessorTime.TotalMilliseconds > absolutetimelimit)
                    {
                        messageText = xDoc.CreateTextNode($"Error Memory: {myProcess.WorkingSet64}; Processor Time: {myProcess.TotalProcessorTime}; Absolute time: {myProcess.UserProcessorTime}");
                        myProcess.Kill();
                        typeElem.AppendChild(typeText);
                        processidElem.AppendChild(processidText);
                        processElem.AppendChild(processText);
                        messageElem.AppendChild(messageText);
                        timeElem.AppendChild(timeText);

                        eventElem.AppendChild(typeElem);
                        eventElem.AppendChild(processidElem);
                        eventElem.AppendChild(processElem);
                        eventElem.AppendChild(messageElem);
                        eventElem.AppendChild(timeElem);
                        eventElem.AppendChild(exitcodeElem);
                        xRoot?.AppendChild(eventElem);
                        xDoc.Save("../../../../processlog.xml");
                        break;
                    }
                    else if (myProcess.WorkingSet64 > (memorylimit*0.90) || (int)myProcess.TotalProcessorTime.TotalMilliseconds > (processortimelimit*0.90) || (int)myProcess.UserProcessorTime.TotalMilliseconds > (absolutetimelimit*0.90))
                    {
                        messageText = xDoc.CreateTextNode($"Warning Memory: {myProcess.WorkingSet64}; Processor Time: {myProcess.TotalProcessorTime}; Absolute time: {myProcess.UserProcessorTime}");
                    }
                    typeElem.AppendChild(typeText);
                    processidElem.AppendChild(processidText);
                    processElem.AppendChild(processText);
                    messageElem.AppendChild(messageText);
                    timeElem.AppendChild(timeText);

                    eventElem.AppendChild(typeElem);
                    eventElem.AppendChild(processidElem);
                    eventElem.AppendChild(processElem);
                    eventElem.AppendChild(messageElem);
                    eventElem.AppendChild(timeElem);

                    xRoot?.AppendChild(eventElem);
              
                    xDoc.Save("../../../../processlog.xml");
                    Thread.Sleep(3000);
                    myProcess.Refresh();
                }
                if (myProcess.HasExited)
                {
                    if (myProcess.ExitCode == 0)
                    {
                        Console.WriteLine("ExitCode: " + myProcess.ExitCode);
                    }
                    else
                    {
                        Console.WriteLine("RunTime Error " + myProcess.ExitCode);
                    }
                }

            }
        }
    }
}

