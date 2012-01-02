using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace JobZoom.Web
{
    public partial class GenerateICEData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Prevent caching in the browser
            Response.Clear();
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;

            // Set the content type of our response to XML
            Response.ContentType = "text/xml";

            try
            {
                Root root = CreateDataFile();

                XmlSerializer serializer =
                    new XmlSerializer(typeof(Root));

                serializer.Serialize(
                    this.Response.OutputStream, root);

                this.Response.OutputStream.Close();
            }
            catch (Exception error)
            {
                // if any error occurs, return an error message
                XmlTextWriter writer =
                    new XmlTextWriter(
                         this.Response.OutputStream,
                         Encoding.UTF8);
                writer.WriteStartElement("error");
                writer.WriteValue(error.Message);
                writer.WriteEndElement();
                writer.Close();
                return;
            }

        }

        private Root CreateDataFile()
        {
            // we create the xml structure
            Root root = new Root();
            root.currentNode = new RootCurrentNode();

            // we create two empty lists to complete the structure
            // we will use them later
            List<Node> neighbors = new List<Node>();
            List<Link> links = new List<Link>();

            // this function creates a node
            string parameter = Request.QueryString["node"];
            root.currentNode.node = CreateNodeFromObject(parameter);

            // we finalize the structure
            root.neighbors = neighbors.ToArray();
            root.currentNode.link = links.ToArray();

            return root;
        }

        private Node CreateNodeFromObject(string parameter)
        {
            return new Node { id = Guid.NewGuid().ToString(), title = parameter, type = "Profile", url = "congphuc.net" };
        }
    }
}