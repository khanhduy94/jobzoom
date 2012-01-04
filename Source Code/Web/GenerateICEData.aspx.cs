using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using JobZoom.Core.Entities;
using JobZoom.Core.Taxonomy;

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
                // ensure the id argument is not null
                if (Request["id"] == null)
                {
                    throw new Exception("Not a valid \"id\" argument");
                }

                string id = Request["id"];
                Root root = CreateDataFile(id);

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

        private Root CreateDataFile(string id)
        {

            //JobZoom Core
            Tag tag = new Hierarchy().GetHierarchicalTreeByObject(new Guid(id), "JobSeekerProfile");            

            // we create the xml structure
            Root root = new Root();
            root.currentNode = new RootCurrentNode();

            // we create two empty lists to complete the structure
            // we will use them later
            List<Node> neighbors = new List<Node>();
            List<Link> links = new List<Link>();

            // this function creates a node                             
            root.currentNode.node = CreateNodeFromObject(tag.TagAttribute);

            foreach (Tag secondLevelTag in tag.Children)
            {
                TagAttribute secondLevelNode = secondLevelTag.TagAttribute;

                Node secondLevelNodeBefore = CreateNodeFromObject(secondLevelNode);
                neighbors.Add(secondLevelNodeBefore);

                Link relationWithBeforeSecondLevel = CreateLinkBetween(root.currentNode.node, secondLevelNodeBefore);
                links.Add(relationWithBeforeSecondLevel);

                foreach (Tag thirdLevelTag in secondLevelTag.Children)
                {
                    TagAttribute thirdLevelNode = thirdLevelTag.TagAttribute;
                    Node thirdLevelNodeBefore = CreateNodeFromObject(thirdLevelNode);
                    neighbors.Add(thirdLevelNodeBefore);

                    Link relationWithBeforeThirdLevel = CreateLinkBetween(secondLevelNodeBefore, thirdLevelNodeBefore);
                    links.Add(relationWithBeforeThirdLevel);
                }
            }

            // populate the neighbors list            

            //Node nodeBefore = CreateNodeFromObject("Education");
            //neighbors.Add(nodeBefore);

            // populate the links list            

            //Link relationWithBefore = CreateLinkBetween(root.currentNode.node, nodeBefore);
            //links.Add(relationWithBefore);


            // we finalize the structure
            root.neighbors = neighbors.ToArray();
            root.currentNode.link = links.ToArray();

            return root;
        }

        private Link CreateLinkBetween(Node from, Node to)
        {
            Link link = new Link();            

            // As for nodes, links have an unique ID
            link.id =
    "myDataOnIce.isAfterInAlphabeticOrder[" + from.id + "]to[" + to.id + "]";

            // We have to specify the ID reference of the two nodes
            link.from = from.id;
            link.to = to.id;

            // We must also qualify the relation to make it human readable
            link.grammar = new LinkGrammar();
            link.grammar.verb = "is after";
            link.grammar.complement = "in alphabetical order";

            // (optional) To finish the definition of a relation,
            // we COULD  quantify the relation with the strength of this relation
            // this strength is between 1 and 100
            // the higher the strength, the more the two node will try to stay together
            link.strength = 50f;

            return link;

        }

        private Node CreateNodeFromObject(string id)
        {            
            return new Node { id = Guid.NewGuid().ToString(), title = id, type = "Profile", url = "" };
        }

        private Node CreateNodeFromObject(TagAttribute tagAttribute)
        {
            return new Node { id = tagAttribute.TagId.ToString(), title = tagAttribute.TagName, type = "Profile", url = "http://congphuc.net" };
        }
    }
}