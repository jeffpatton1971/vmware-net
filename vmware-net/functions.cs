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
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
            catch (Exception e)
            {
                //
                // Regular Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + e.GetType().Name + " with message '" + e.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                vimClient = null;
                return vimClient;
            }
        }
        public static List<Datastore> GetDataStore(VimClient vimClient, Datacenter selectedDC = null, string dsName = null)
        {
            //
            // Get a list of datastores from a specific datacenter
            //
            List<Datastore> lstDatastores = new List<Datastore>();
            NameValueCollection dsFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (dsName != null)
            {
                //
                // The name of a specific datastore
                //
                dsFilter.Add("name", dsName);
            }
            else
            {
                dsFilter = null;
            }

            if (selectedDC != null)
            {
                //
                // A specific datacenter to get datastores from
                //
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }
            try
            {
                //
                // if DcMoref and dsFilter are empty return all datastores
                //
                List<EntityViewBase> appDatastores = vimClient.FindEntityViews(typeof(Datastore), DcMoRef, dsFilter, null);
                if (appDatastores != null)
                {
                    foreach (EntityViewBase appDatastore in appDatastores)
                    {
                        Datastore thisDatastore = (Datastore)appDatastore;
                        lstDatastores.Add(thisDatastore);
                    }
                    return lstDatastores;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static VmwareDistributedVirtualSwitch GetDvSwitch(VimClient vimClient, ManagedObjectReference dvportGroupSwitch)
        {
            //
            // Get a specific distributed switch
            //
            try
            {
                //
                // Get the switch associated with the moref
                //
                ViewBase appSwitch = vimClient.GetView(dvportGroupSwitch, null);
                if (appSwitch != null)
                {
                    VmwareDistributedVirtualSwitch thisDvSwitch = (VmwareDistributedVirtualSwitch)appSwitch;
                    return thisDvSwitch;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<DistributedVirtualPortgroup> GetDVPortGroups(VimClient vimClient, Datacenter selectedDC = null, string pgName = null)
        {
            //
            // Get a list of Distributed Portgroups
            //
            List<DistributedVirtualPortgroup> lstPortGroups = new List<DistributedVirtualPortgroup>();
            NameValueCollection pgFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (pgName != null)
            {
                //
                // A specific portgroup
                //
                pgFilter.Add("name", pgName);
            }
            else
            {
                pgFilter = null;
            }

            if (selectedDC != null)
            {
                //
                // A specific datacenter
                //
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }
            try
            {
                //
                // If DcMoref and pgFilter are null return all Portgroups, otherwise return selected Portgroup
                //
                List<EntityViewBase> appPortGroups = vimClient.FindEntityViews(typeof(DistributedVirtualPortgroup), DcMoRef, pgFilter, null);
                if (appPortGroups != null)
                {
                    foreach (EntityViewBase appPortGroup in appPortGroups)
                    {
                        DistributedVirtualPortgroup thisPortGroup = (DistributedVirtualPortgroup)appPortGroup;
                        lstPortGroups.Add(thisPortGroup);
                    }
                    return lstPortGroups;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<Network> GetPortGroups(VimClient vimClient, Datacenter selectedDC = null, string pgName = null)
        {
            //
            // Get a list of Portgroups
            //
            List<Network> lstPortGroups = new List<Network>();
            NameValueCollection pgFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (pgName != null)
            {
                //
                // Name of a specific portgroup
                //
                pgFilter.Add("name", pgName);
            }
            else
            {
                pgFilter = null;
            }

            if (selectedDC != null)
            {
                //
                // A specific datacenter
                //
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }
            try
            {
                //
                // If DcMoRef and pgFilter are null return all portgroups, otherwise return the selected portgroup
                //
                List<EntityViewBase> appPortGroups = vimClient.FindEntityViews(typeof(Network), DcMoRef, pgFilter, null);
                if (appPortGroups != null)
                {
                    foreach (EntityViewBase appPortGroup in appPortGroups)
                    {
                        Network thisPortGroup = (Network)appPortGroup;
                        lstPortGroups.Add(thisPortGroup);
                    }
                    return lstPortGroups;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<VirtualMachine> GetVirtualMachines(VimClient vimClient, Datacenter selectedDC = null, string vmName = null)
        {
            //
            // Get a list of virtual machines
            //
            List<VirtualMachine> lstVirtualMachines = new List<VirtualMachine>();
            NameValueCollection vmFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (vmName != null)
            {
                //
                // A specific virtual machine
                //
                vmFilter.Add("name", vmName);
            }
            else
            {
                vmFilter = null;
            }

            if (selectedDC != null)
            {
                //
                // A specific datacenter
                //
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }
            try
            {
                //
                // If DcMoRef and vmFilter are null return all Vm's, otherwise return specific vm's
                //
                List<EntityViewBase> appVirtualMachines = vimClient.FindEntityViews(typeof(VirtualMachine), DcMoRef, vmFilter, null);
                if (appVirtualMachines != null)
                {
                    foreach (EntityViewBase appVirtualMachine in appVirtualMachines)
                    {
                        VirtualMachine thisVirtualMachine = (VirtualMachine)appVirtualMachine;
                        lstVirtualMachines.Add(thisVirtualMachine);
                    }
                    return lstVirtualMachines;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<CustomizationSpecInfo> GetCustomizationSpecs(VimClient vimClient)
        {
            //
            // Get a list of Customization Spec Info items
            //
            try
            {
                //
                // Get all Spec Info items
                //
                List<CustomizationSpecInfo> lstSpecInfo = new List<CustomizationSpecInfo>();
                CustomizationSpecManager specManager = (CustomizationSpecManager)vimClient.GetView(vimClient.ServiceContent.CustomizationSpecManager, null);
                if (specManager != null)
                {
                    foreach (CustomizationSpecInfo specInfo in specManager.Info)
                    {
                        lstSpecInfo.Add(specInfo);
                    }
                    return lstSpecInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static CustomizationSpecItem GetCustomizationSpecItem(VimClient vimClient, string specName = null)
        {
            //
            // Get one or more Customization Spec Items
            //
            try
            {
                //
                // Need a manager to collect the spec items
                //
                CustomizationSpecManager specManager = (CustomizationSpecManager)vimClient.GetView(vimClient.ServiceContent.CustomizationSpecManager, null);
                if (specManager != null)
                {
                    CustomizationSpecItem itmCustomizationSpecItem = specManager.GetCustomizationSpec(specName);
                    return itmCustomizationSpecItem;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<ClusterComputeResource> GetClusters(VimClient vimClient, string clusterName = null)
        {
            //
            // Get one or more clusters
            //
            List<ClusterComputeResource> lstClusters = new List<ClusterComputeResource>();
            List<EntityViewBase> appClusters = new List<EntityViewBase>();
            try
            {
                if (clusterName == null)
                {
                    //
                    // Get all the clusters
                    //
                    appClusters = vimClient.FindEntityViews(typeof(ClusterComputeResource), null, null, null);
                }
                else
                {
                    //
                    // Get a specific cluster
                    //
                    NameValueCollection clusterFilter = new NameValueCollection();
                    clusterFilter.Add("name", clusterName);
                    appClusters = vimClient.FindEntityViews(typeof(ClusterComputeResource), null, clusterFilter, null);
                }
                if (appClusters != null)
                {
                    foreach (EntityViewBase appCluster in appClusters)
                    {
                        ClusterComputeResource thisCluster = (ClusterComputeResource)appCluster;
                        lstClusters.Add(thisCluster);
                    }
                    return lstClusters;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<HostSystem> GetHosts(VimClient vimClient, string hostParent = null)
        {
            //
            // Get one or more virtual hosts
            //
            List<HostSystem> lstHosts = new List<HostSystem>();
            List<EntityViewBase> appHosts = new List<EntityViewBase>();
            try
            {
                if (hostParent == null)
                {
                    //
                    // Get all the hosts
                    //
                    appHosts = vimClient.FindEntityViews(typeof(HostSystem), null, null, null);
                }
                else
                {
                    //
                    // Get all the hosts in a cluster
                    //
                    NameValueCollection hostFilter = new NameValueCollection();
                    hostFilter.Add("parent", hostParent);

                    appHosts = vimClient.FindEntityViews(typeof(HostSystem), null, hostFilter, null);
                }
                if (appHosts != null)
                {
                    foreach (EntityViewBase appHost in appHosts)
                    {
                        HostSystem thisHost = (HostSystem)appHost;
                        lstHosts.Add(thisHost);
                    }
                    return lstHosts;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<Datacenter> GetDcFromCluster(VimClient vimClient, string clusterParent)
        {
            //
            // Get a datacenter based on the cluster
            //
            List<Datacenter> lstDataCenters = new List<Datacenter>();
            NameValueCollection parentFilter = new NameValueCollection();
            parentFilter.Add("hostFolder", clusterParent);
            try
            {
                //
                // Get a specific datacenter based on parentFilter
                //
                List<EntityViewBase> arrDataCenters = vimClient.FindEntityViews(typeof(Datacenter), null, parentFilter, null);
                if (arrDataCenters != null)
                {
                    foreach (EntityViewBase arrDatacenter in arrDataCenters)
                    {
                        Datacenter thisDatacenter = (Datacenter)arrDatacenter;
                        lstDataCenters.Add(thisDatacenter);
                    }
                    return lstDataCenters;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
        public static List<ResourcePool> GetResPools(VimClient vimClient, string ClusterMoRefVal)
        {
            //
            // Get resource pools from a specific cluster
            //
            List<ResourcePool> lstResPools = new List<ResourcePool>();
            NameValueCollection clusterFilter = new NameValueCollection();
            clusterFilter.Add("parent", ClusterMoRefVal);
            try
            {
                List<EntityViewBase> arrResPools = vimClient.FindEntityViews(typeof(ResourcePool), null, clusterFilter, null);
                if (arrResPools != null)
                {
                    foreach (EntityViewBase arrResPool in arrResPools)
                    {
                        ResourcePool thisResPool = (ResourcePool)arrResPool;
                        lstResPools.Add(thisResPool);
                    }
                    return lstResPools;
                }
                else
                {
                    return null;
                }
            }
            catch (VimException ex)
            {
                //
                // VMware Exception occurred
                //
                //txtErrors.Text = "A server fault of type " + ex.MethodFault.GetType().Name + " with message '" + ex.Message + "' occured while performing requested operation.";
                //Error_Panel.Visible = true;
                return null;
            }
        }
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
    }
}