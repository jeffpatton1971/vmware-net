using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using VMware.Vim;

namespace vmware_net
{
    public class functions
    {
        public static string ValidateServer(string viServer)
        {
            //
            // Validate viServer for appropriate values
            //
            viServer = viServer.Trim().ToLower();
            if (viServer.Contains("://") == false)
            {
                //
                // Assuming if there is no :// someone entered flat server name
                //
                viServer = "https://" + viServer;
            }
            //
            // Convert the string into a URI for further testing
            //
            Uri uriVServer = new Uri(viServer);
            //
            // Check to see what protocol we're using
            //
            string urlScheme = uriVServer.Scheme;
            switch (urlScheme)
            {
                case "https":
                    break;
                default:
                    viServer = viServer.Replace(uriVServer.Scheme + "://", "https://");
                    break;
            }
            //
            // Check to see what that path is
            //
            if (uriVServer.AbsolutePath == "/")
            {
                viServer = viServer + "/sdk";
            }
            else if (uriVServer.AbsolutePath != "/sdk")
            {
                //
                // Some other path is listed
                //
                viServer = viServer.Replace(uriVServer.AbsolutePath, "/sdk");
            }
            return viServer;
        }
        public static List<T> GetEntities<T>(VimClient vimClient, ManagedObjectReference beginEntity, NameValueCollection filter, string[] properties)
        {
            List<T> things = new List<T>();
            List<EntityViewBase> vBase = vimClient.FindEntityViews(typeof(T), beginEntity, filter, properties);

            foreach (EntityViewBase eBase in vBase)
            {
                T thing = (T)(object)eBase;
                things.Add(thing);
            }
            return things;
        }
        public static T GetEntity<T>(VimClient vimClient, ManagedObjectReference beginEntity, NameValueCollection filter, string[] properties)
        {
            EntityViewBase vBase = vimClient.FindEntityView(typeof(T), beginEntity, filter, properties);
            T thing = (T)(object)vBase;
            return thing;
        }
        public static T GetObject<T>(VimClient vimClient, ManagedObjectReference moRef, string[] properties)
        {
            ViewBase vBase = vimClient.GetView(moRef, properties);
            T thisObject = (T)(object)vBase;
            return thisObject;
        }
        public static VimClient ConnectServer(string viServer, string viUser, string viPassword)
        {
            //
            // Establish a connetion to the provided sdk server
            //
            VimClient vimClient = new VimClient();
            ServiceContent vimServiceContent = new ServiceContent();
            UserSession vimSession = new UserSession();
            //
            // Connect over https to the /sdk page
            //
            try
            {
                vimClient.Connect(viServer);
                vimSession = vimClient.Login(viUser, viPassword);
                vimServiceContent = vimClient.ServiceContent;

                return vimClient;
            }
            catch
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
    }
}