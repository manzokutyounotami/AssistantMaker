using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CortanaCommand.Core.Cortana
{
    public class CortanaXmlGenerator
    {
        XDocument doc;
        XElement commandSet;
        public CortanaXmlGenerator(string prefix,string example)
        {
            doc = new XDocument();
            XDeclaration dec = new XDeclaration("1.0", "utf-8", "no");
            doc.Declaration = dec;
            XElement root = new XElement("VoiceCommands");
            root.SetAttributeValue("def", "defVal");
            doc.Add(root);
            commandSet = new XElement("CommandSet");
            commandSet.SetAttributeValue("xmllang", "ja-JP");
            commandSet.Add(new XElement("CommandPrefix", prefix));
            commandSet.Add(new XElement("Example", example));
            root.Add(commandSet);
        }

        public void AddCommandService(string name,string example,string[] listenFor,string feedback,string target)
        {
            XElement command = new XElement("Command");
            command.SetAttributeValue("Name",name);
            command.Add(new XElement("Example",example));
            foreach(var listen in listenFor)
            {
                command.Add(new XElement("ListenFor", listen));
            }
            
            command.Add(new XElement("Feedback", feedback));
            XElement service = new XElement("VoiceCommandService");
            service.SetAttributeValue("Target",target);
            command.Add(service);
            commandSet.Add(command);
        }

        public void AddCommandNavigate(string name, string example, string[] listenFor, string feedback,string target)
        {
            XElement command = new XElement("Command");
            command.SetAttributeValue("Name", name);
            command.Add(new XElement("Example", example));
            foreach (var listen in listenFor)
            {
                command.Add(new XElement("ListenFor", listen));
            }
            command.Add(new XElement("Feedback", feedback));
            XElement service = new XElement("Navigate");
            service.SetAttributeValue("Target", target);
            command.Add(service);
            commandSet.Add(command);
        }

        public string GenerateXml()
        {
            StringWriter writer = new StringWriter();
            doc.Save(writer);
            string xml = writer.ToString();
            xml = xml.Replace("xmllang", "xml:lang");
            xml = xml.Replace("def=\"defVal\"", "xmlns=\"http://schemas.microsoft.com/voicecommands/1.2\"");
            xml = xml.Replace("utf-16","utf-8");
            xml = xml.Replace("standalone=\"no\"","");
            return xml;
        }
    }
}
