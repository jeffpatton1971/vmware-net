using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using VMware.Vim;

namespace vmware_net
{
    public class Globals
    {
        public static string sUsername;
        public static string sPassword;
        public static string sViServer = WebConfigurationManager.AppSettings["viServer"].ToString();
        public static string adServer = WebConfigurationManager.AppSettings["adServer"].ToString();
        public static string adRootPath = WebConfigurationManager.AppSettings["adRootPath"].ToString();
    }
    public partial class Default : System.Web.UI.Page
    {
        protected VimClient ConnectServer(string viServer, string viUser, string viPassword)
        {
            VimClient vimClient = new VimClient();
            ServiceContent vimServiceContent = new ServiceContent();
            UserSession vimSession = new UserSession();

            vimClient.Connect("https://" + viServer.Trim() + "/sdk");
            vimSession = vimClient.Login(viUser, viPassword);
            vimServiceContent = vimClient.ServiceContent;

            return vimClient;
        }
        protected List<Datacenter> GetDataCenter(VimClient vimClient, string dcName = null)
        {
            List<Datacenter> lstDatacenters = new List<Datacenter>();
            List<EntityViewBase> appDatacenters = new List<EntityViewBase>();

            if (dcName == null)
            {
                appDatacenters = vimClient.FindEntityViews(typeof(Datacenter), null, null, null);
            }
            else
            {
                NameValueCollection dcFilter = new NameValueCollection();
                dcFilter.Add("name", dcName);
                appDatacenters = vimClient.FindEntityViews(typeof(Datacenter), null, dcFilter, null);
            }
            foreach (EntityViewBase appDatacenter in appDatacenters)
            {
                Datacenter thisDatacenter = (Datacenter)appDatacenter;
                lstDatacenters.Add(thisDatacenter);
            }

            return lstDatacenters;
        }
        protected List<Datastore> GetDataStore(VimClient vimClient, Datacenter selectedDC = null, string dsName = null)
        {
            List<Datastore> lstDatastores = new List<Datastore>();
            NameValueCollection dsFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (dsName != null)
            {
                dsFilter.Add("name", dsName);
            }
            else
            {
                dsFilter = null;
            }

            if (selectedDC != null)
            {
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }

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
        protected VmwareDistributedVirtualSwitch GetDvSwitch(VimClient vimClient, ManagedObjectReference dvportGroupSwitch)
        {
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
        protected List<DistributedVirtualPortgroup> GetDVPortGroups(VimClient vimClient, Datacenter selectedDC = null, string pgName = null)
        {
            List<DistributedVirtualPortgroup> lstPortGroups = new List<DistributedVirtualPortgroup>();
            NameValueCollection pgFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (pgName != null)
            {
                pgFilter.Add("name", pgName);
            }
            else
            {
                pgFilter = null;
            }

            if (selectedDC != null)
            {
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }

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
        protected List<Network> GetPortGroups(VimClient vimClient, Datacenter selectedDC = null, string pgName = null)
        {
            List<Network> lstPortGroups = new List<Network>();
            NameValueCollection pgFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (pgName != null)
            {
                pgFilter.Add("name", pgName);
            }
            else
            {
                pgFilter = null;
            }

            if (selectedDC != null)
            {
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }

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
        protected List<VirtualMachine> GetVirtualMachines(VimClient vimClient, Datacenter selectedDC = null, string vmName = null)
        {
            List<VirtualMachine> lstVirtualMachines = new List<VirtualMachine>();
            NameValueCollection vmFilter = new NameValueCollection();
            ManagedObjectReference DcMoRef = new ManagedObjectReference();

            if (vmName != null)
            {
                vmFilter.Add("name", vmName);
            }
            else
            {
                vmFilter = null;
            }

            if (selectedDC != null)
            {
                DcMoRef = selectedDC.MoRef;
            }
            else
            {
                DcMoRef = null;
            }

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
        protected List<CustomizationSpecInfo> GetCustomizationSpecs(VimClient vimClient)
        {
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
        protected CustomizationSpecItem GetCustomizationSpecItem(VimClient vimClient, string specName = null)
        {
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
        protected List<ClusterComputeResource> GetClusters(VimClient vimClient, string clusterName = null)
        {
            List<ClusterComputeResource> lstClusters = new List<ClusterComputeResource>();
            List<EntityViewBase> appClusters = new List<EntityViewBase>();

            if (clusterName == null)
            {
                appClusters = vimClient.FindEntityViews(typeof(ClusterComputeResource), null, null, null);
            }
            else
            {
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
        protected List<HostSystem> GetHosts(VimClient vimClient, string hostParent = null)
        {
            List<HostSystem> lstHosts = new List<HostSystem>();
            List<EntityViewBase> appHosts = new List<EntityViewBase>();
            if (hostParent == null)
            {
                appHosts = vimClient.FindEntityViews(typeof(HostSystem), null, null, null);
            }
            else
            {
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
        protected List<Datacenter> GetDcFromCluster(VimClient vimClient, string clusterParent)
        {
            List<Datacenter> lstDataCenters = new List<Datacenter>();
            NameValueCollection parentFilter = new NameValueCollection();
            parentFilter.Add("hostFolder", clusterParent);

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
        protected List<ResourcePool> GetResPools(VimClient vimClient, string ClusterMoRefVal)
        {
            List<ResourcePool> lstResPools = new List<ResourcePool>();
            NameValueCollection clusterFilter = new NameValueCollection();
            clusterFilter.Add("parent", ClusterMoRefVal);

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
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}