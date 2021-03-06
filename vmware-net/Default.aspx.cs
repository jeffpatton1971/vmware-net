﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Net;
using VMware.Vim;

namespace vmware_net
{
    public class Globals
    {
        public static string sUsername;
        public static string sPassword;
        //
        // Replace this line with the line below to populate server from web.config
        //
        public static string sViServer;
        //public static string sViServer = WebConfigurationManager.AppSettings["viServer"].ToString();
        //
        // The following was added to branch Use_MoRefs, going to start doing something smart and use
        // the morefs from the various things that I attach to, this should reduce the number of calls
        // back to the server that I need.
        //
        public static ManagedObjectReference mySourceVM;
        public static ManagedObjectReference myCustomization;
        public static ManagedObjectReference myCluster;
        public static ManagedObjectReference myDatacenter;
        public static ManagedObjectReference myDatastore;
        public static ManagedObjectReference myPortGroup;
        public static ManagedObjectReference myTask;
        public static ManagedObjectReference myClone;
    }
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Uncomment the following code to redirect this app to HTTPS once
            //  you have a valid cert installed on your IIS box.
            //
            //if (!Request.IsLocal && !Request.IsSecureConnection)
            //{
            //    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            //    Response.Redirect(redirectUrl);
            //}
            //
            // Read in the URL of the Vmware SDK Server from web.config
            // Uncomment the code below to store the sdk servername in the web.config
            //
            //txtSdkServer.Text = WebConfigurationManager.AppSettings["viServer"].ToString();
            //
            // Layout the error panel dimensions
            //
            Error_Panel.Style.Add("position", "absolute;right:auto;left:auto");
            Error_Panel.Style.Add("height", "100%");
            Error_Panel.Style.Add("width", "960px");
            //
            //
            //
            Working_Panel.Style.Add("position", "absolute;right:auto;left:auto");
            Working_Panel.Style.Add("height", "100%");
            Working_Panel.Style.Add("width", "960px");
            //
            // Layout the results panel
            //
            Results_Panel.Style.Add("position", "absolute;right:auto;left:auto");
            Results_Panel.Style.Add("height", "100%");
            Results_Panel.Style.Add("width", "960px");
        }
        protected void cmdConnect_Click(object sender, EventArgs e)
        {
            //
            // The code here establishes a connection to your Vmware Datacenter/Cluster/Standalone
            // server and populates the controls in the vm_panel.
            //
            Globals.sUsername = txtUsername.Text;
            Globals.sPassword = txtPassword.Text;
            //
            // Comment or remove the code below if you populate the sdk server from web.config
            //
            if (txtSdkServer.Text == null || txtSdkServer.Text == "")
            {
                txtErrors.Text = "Please enter a server name or IP address.";
                Error_Panel.Visible = true;
                return;
            }
            else
            {
                Globals.sViServer = functions.ValidateServer(txtSdkServer.Text);
            }
            //
            // Establish a connection with the Vmware server
            //
            VimClient vimClient = functions.ConnectServer(Globals.sViServer, Globals.sUsername, Globals.sPassword);
            if (vimClient == null)
            {
                return;
            }
            //
            // Get a list of clones
            // To populate the virtual machines to clone from with a filtered list un-comment the  line below that reads
            // from the webconfigurationmanager, and comment or remove the line below that that has no filter.
            //
            NameValueCollection filter = new NameValueCollection();
            filter.Add("name", WebConfigurationManager.AppSettings["clonePrefix"].ToString());
            List<VirtualMachine> lstVirtualMachines = functions.GetEntities<VirtualMachine>(vimClient, null, filter, null);
            filter.Remove("name");
            //
            //List<VirtualMachine> lstVirtualMachines = functions.GetEntities<VirtualMachine>(vimClient, null, null, null);
            if (lstVirtualMachines != null)
            {
                foreach (VirtualMachine itmVirtualMachine in lstVirtualMachines)
                {
                    ListItem thisVirtualMachine = new ListItem();
                    thisVirtualMachine.Text = itmVirtualMachine.Name;
                    thisVirtualMachine.Value = itmVirtualMachine.MoRef.ToString();
                    cboSourceVms.Items.Add(thisVirtualMachine);
                }
            }
            //
            // Get a list of OS Customizations
            //
            CustomizationSpecManager specManager = functions.GetObject<CustomizationSpecManager>(vimClient, vimClient.ServiceContent.CustomizationSpecManager, null);
            if (specManager != null)
            {
                foreach (CustomizationSpecInfo itmSpec in specManager.Info)
                {
                    ListItem thisSpec = new ListItem();
                    thisSpec.Text = itmSpec.Name;
                    thisSpec.Value = itmSpec.Name + "." + itmSpec.Type;
                    cboCustomizations.Items.Add(thisSpec);
                }
            }
            //
            // Get a list of clusters
            //
            List<ClusterComputeResource> lstClusters = functions.GetEntities<ClusterComputeResource>(vimClient, null, null, null);
            foreach (ClusterComputeResource itmCluster in lstClusters)
            {
                ListItem thisCluster = new ListItem();
                thisCluster.Text = itmCluster.Name;
                thisCluster.Value = itmCluster.MoRef.ToString();
                cboClusters.Items.Add(thisCluster);
            }
            //
            // Get at the ClusterResource
            //
            filter.Add("name", cboClusters.SelectedItem.Text);
            ClusterComputeResource SelectedCluster = functions.GetEntity<ClusterComputeResource>(vimClient, null, filter, null);
            filter.Remove("name");
            //
            // Need to get at the Datacenter
            //
            filter.Add("hostFolder", SelectedCluster.Parent.Value);
            Datacenter itmDatacenter = functions.GetEntity<Datacenter>(vimClient, null, filter, null);
            filter.Remove("hostFolder");
            //
            // Create a list of Datastores to populate later
            //
            List<Datastore> lstDatastores = new List<Datastore>();
            //
            // The cluster object already contains a list of datastore morefs
            // grab this list of morefs and use getview to grab the object
            // and add it to the datastore list.
            //
            foreach (ManagedObjectReference itmDs in SelectedCluster.Datastore)
            {
                ViewBase thisDsView = vimClient.GetView(itmDs, null);
                Datastore thisDatastore = (Datastore)thisDsView;
                lstDatastores.Add(thisDatastore);
            }
            //
            // Sort the list by the freespace property in descending order
            //
            lstDatastores = lstDatastores.OrderByDescending(thisStore => thisStore.Info.FreeSpace).ToList();
            foreach (Datastore itmDatastore in lstDatastores)
            {
                ListItem thisDatastore = new ListItem();
                thisDatastore.Text = itmDatastore.Name;
                thisDatastore.Value = itmDatastore.MoRef.ToString();
                cboDatastores.Items.Add(thisDatastore);
            }
            //
            // Get a list of network portgroups
            //
            filter.Add("name", WebConfigurationManager.AppSettings["portPrefix"].ToString());
            List<DistributedVirtualPortgroup> lstDVPortGroups = functions.GetEntities<DistributedVirtualPortgroup>(vimClient, itmDatacenter.MoRef, filter, null);
            filter.Remove("name");
            //
            //List<DistributedVirtualPortgroup> lstDVPortGroups = functions.GetEntities<DistributedVirtualPortgroup>(vimClient, itmDatacenter.MoRef, null, null);
            if (lstDVPortGroups != null)
            {
                foreach (DistributedVirtualPortgroup itmPortGroup in lstDVPortGroups)
                {
                    ListItem thisPortGroup = new ListItem();
                    thisPortGroup.Text = itmPortGroup.Name;
                    thisPortGroup.Value = itmPortGroup.MoRef.ToString();
                    cboPortGroups.Items.Add(thisPortGroup);
                }
            }
            //
            // Get a list of Resource Pools
            //
            Globals.myCluster = new ManagedObjectReference(cboClusters.SelectedItem.Value);
            filter.Add("parent", Globals.myCluster.Value);
            List<ResourcePool> lstResPools = functions.GetEntities<ResourcePool>(vimClient, null, filter, null);
            filter.Remove("parent");
            if (lstResPools != null)
            {
                foreach (ResourcePool itmResPool in lstResPools)
                {
                    ListItem thisResPool = new ListItem();
                    thisResPool.Text = itmResPool.Name;
                    thisResPool.Value = itmResPool.MoRef.ToString();
                    cboResourcePools.Items.Add(thisResPool);
                }
            }
            //
            // Close the session with the server
            //
            vimClient.Disconnect();
            //
            // Hide the login controls and show the vm controls
            //
            Login_Panel.Visible = false;
            Vm_Panel.Visible = true;
        }
        protected void cmdProvision_Click(object sender, EventArgs e)
        {
            //
            // Ready to provision
            //
            //
            // Populate the moRef objects with actual values
            //
            Globals.mySourceVM = new ManagedObjectReference(cboSourceVms.SelectedItem.Value);
            Globals.myCustomization = new ManagedObjectReference(cboCustomizations.SelectedItem.Value);
            Globals.myCluster = new ManagedObjectReference(cboClusters.SelectedItem.Value);
            Globals.myDatastore = new ManagedObjectReference(cboDatastores.SelectedItem.Value);
            Globals.myPortGroup = new ManagedObjectReference(cboPortGroups.SelectedItem.Value);
            //
            // The idea of using the page to do more than one VM failed rather miserably
            // I'll remove this code as I migrate to using moRefs for everything.
            //
            char[] splitChar;
            string targetVM = txtTargetVm.Text;
            Results_Panel.Visible = false;
            string targetIP = txtIpAddress.Text;
            //
            // Need a random number to pick a random virtual host to place the new vm on
            //
            Random rand = new Random();
            //
            // Validate user entries
            //
            //
            // Do we have a value for the target vm?
            //
            if (targetVM == null || targetVM == "")
            {
                txtErrors.Text = "Please enter a name for the virtual machine.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // Make sure that we don't create a machine with a longer name than what netbios supports
            //
            if (targetVM.Length > 15)
            {
                txtErrors.Text = "Please enter a NetBIOS name shorter than 15 characters for the virtual machine.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // http://kb.vmware.com/selfservice/microsites/search.do?language=en_US&cmd=displayKC&externalId=2009820
            //
            if (targetVM.Contains("_"))
            {
                txtErrors.Text = "Underscore characters not supported for cloning.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // Has a valid IP address been entered?
            //
            IPAddress theIp;
            bool ipResult = IPAddress.TryParse(targetIP, out theIp);
            if (ipResult != true)
            {
                txtErrors.Text = "Please enter a valid IP Address.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // This does some basic checking on a subnet
            //
            IPAddress theMask;
            bool mskResult = IPAddress.TryParse(txtSubnet.Text, out theMask);
            if (mskResult != true)
            {
                txtErrors.Text = "Please enter a valid Subnet Mask.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // Has a valid IP been entered for the gateway?
            //
            IPAddress theGateway;
            bool gwResult = IPAddress.TryParse(txtGateway.Text, out theGateway);
            if (gwResult != true)
            {
                txtErrors.Text = "Please entera valid IP Address for the default gateway.";
                Error_Panel.Visible = true;
                return;
            }
            //
            // Does a vm by this name already exist?
            //
            VimClient vimClient = functions.ConnectServer(Globals.sViServer, Globals.sUsername, Globals.sPassword);
            NameValueCollection filter = new NameValueCollection();
            filter.Add("name", targetVM);
            VirtualMachine chkVirtualMachine = functions.GetEntity<VirtualMachine>(vimClient, null, filter, null);
            filter.Remove("name");
            if (chkVirtualMachine != null)
            {
                vimClient.Disconnect();
                txtErrors.Text = "virtual machine " + targetVM + " already exists";
                Error_Panel.Visible = true;
                return;
            }
            //
            // Need to parse the value of the dropdown
            //
            splitChar = new char[] { '.' };
            string[] specType = cboCustomizations.SelectedValue.Split(splitChar);
            //
            // Connect to selected datacenter
            //
            ClusterComputeResource itmCluster = functions.GetObject<ClusterComputeResource>(vimClient, Globals.myCluster, null);
            filter.Add("hostFolder", itmCluster.Parent.Value);
            Datacenter itmDatacenter = functions.GetEntity<Datacenter>(vimClient, null, filter, null);
            filter.Remove("hostFolder");
            //
            // Get a list of hosts in the selected cluster
            //
            ManagedObjectReference[] lstHosts = itmCluster.Host;
            //
            // Randomly pick host
            //

            HostSystem selectedHost = functions.GetObject<HostSystem>(vimClient, lstHosts[rand.Next(0, lstHosts.Count())], null);
            txtResults.Text = "Host : " + selectedHost.Name + "\r\n";
            //
            // Connect to selected vm to clone
            //
            filter.Add("name", cboSourceVms.SelectedItem.Text);
            VirtualMachine itmVirtualMachine = functions.GetEntity<VirtualMachine>(vimClient, null, filter, null);
            filter.Remove("name");
            //
            // Make sure the spec file type matches the guest os
            //
            //
            // The commented code could be used to poweron a vm, check it's guestfamily and then turn it off.
            //
            //string GuestFamily = null;
            //if (itmVirtualMachine.Runtime.PowerState == VirtualMachinePowerState.poweredOff)
            //{
            //    //
            //    // We can power on the vm to get the guestfamily property
            //    //
            //    itmVirtualMachine.PowerOnVM(null);
            //    //
            //    // Set the GuestFamily var
            //    //
            //    while (itmVirtualMachine.Guest.GuestFamily == null)
            //    {
            //        //
            //        // Need to grab the current guest status from the vm
            //        //
            //        itmVirtualMachine.Reload();
            //        GuestFamily = itmVirtualMachine.Guest.GuestFamily;
            //    }
            //    //
            //    // Turn the VM back off
            //    //
            //    itmVirtualMachine.PowerOffVM();
            //}
            //
            // Added this test to accomodate cloning templates to vm's, per Ryan Lawrence.
            //
            if (!(chkTemplate.Checked))
            {
                if (itmVirtualMachine.Guest.GuestFamily != null)
                {
                    string GuestFamily = itmVirtualMachine.Guest.GuestFamily.ToLower();
                    string TargetType = specType[specType.GetUpperBound(0)].ToLower();
                    if (GuestFamily.Contains(TargetType) == false)
                    {
                        vimClient.Disconnect();
                        txtErrors.Text = "You specified a " + specType[specType.GetUpperBound(0)] + " spec file to clone a " + itmVirtualMachine.Guest.GuestFamily + " virtual machine.";
                        Error_Panel.Visible = true;
                        return;
                    }
                }
                else
                {
                    //
                    // Sometimes the GuestFamily property isn't populated
                    //
                    vimClient.Disconnect();
                    txtErrors.Text = "The virtual machine " + itmVirtualMachine.Name.ToString() + " has no GuestFamily property populated, please power on this VM and verify that it's a supported Guest Os.";
                    Error_Panel.Visible = true;
                    return;
                }
            }
            txtResults.Text += "Source : " + itmVirtualMachine.Name + "\r\n";
            //
            // Connect to the selected datastore
            //
            filter.Add("name", cboDatastores.SelectedItem.Text);
            Datastore itmDatastore = functions.GetEntity<Datastore>(vimClient, null, filter, null);
            filter.Remove("name");
            txtResults.Text += "Datastore : " + itmDatastore.Name + "\r\n";
            //
            // Connect to portgroup
            //
            filter.Add("name", cboPortGroups.SelectedItem.Text);
            DistributedVirtualPortgroup itmDvPortGroup = functions.GetEntity<DistributedVirtualPortgroup>(vimClient, null, filter, null);
            filter.Remove("name");
            txtResults.Text += "Portgroup : " + itmDvPortGroup.Name + "\r\n";
            //
            // Connect to the customizationspec
            //
            CustomizationSpecManager specManager = functions.GetObject<CustomizationSpecManager>(vimClient, vimClient.ServiceContent.CustomizationSpecManager, null);
            CustomizationSpecItem itmCustomizationSpecItem = specManager.GetCustomizationSpec(cboCustomizations.SelectedItem.Text);
            txtResults.Text += "Spec : " + cboCustomizations.SelectedItem.Text + "\r\n";
            //
            // Create a new VirtualMachineCloneSpec
            //
            VirtualMachineCloneSpec mySpec = new VirtualMachineCloneSpec();
            mySpec.Location = new VirtualMachineRelocateSpec();
            mySpec.Location.Datastore = itmDatastore.MoRef;
            mySpec.Location.Host = selectedHost.MoRef;
            //
            // Get resource pool for selected cluster
            //
            filter.Add("parent", itmCluster.Parent.ToString());
            ResourcePool itmResPool = functions.GetObject<ResourcePool>(vimClient, itmCluster.ResourcePool, null);
            filter.Remove("parent");
            //
            // Assign resource pool to specitem
            //
            mySpec.Location.Pool = itmResPool.MoRef;
            //
            // Add selected CloneSpec customizations to this CloneSpec
            //
            mySpec.Customization = itmCustomizationSpecItem.Spec;
            //
            // Handle hostname for either windows or linux
            //
            if (specType[specType.GetUpperBound(0)] == "Windows")
            {
                //
                // Create a windows sysprep object
                //
                CustomizationSysprep winIdent = (CustomizationSysprep)itmCustomizationSpecItem.Spec.Identity;
                CustomizationFixedName hostname = new CustomizationFixedName();
                hostname.Name = targetVM;
                winIdent.UserData.ComputerName = hostname;
                //
                // Store identity in this CloneSpec
                //
                mySpec.Customization.Identity = winIdent;
            }
            if (specType[specType.GetUpperBound(0)] == "Linux")
            {
                //
                // Create a Linux "sysprep" object
                //
                CustomizationLinuxPrep linIdent = (CustomizationLinuxPrep)itmCustomizationSpecItem.Spec.Identity;
                CustomizationFixedName hostname = new CustomizationFixedName();
                hostname.Name = targetVM;
                linIdent.HostName = hostname;
                //
                // Uncomment the line below to add a suffix to linux vm's
                //
                // linIdent.Domain = WebConfigurationManager.AppSettings["dnsSuffix"].ToString();
                //
                // Store identity in this CloneSpec
                //
                mySpec.Customization.Identity = linIdent;
            }
            //
            // Create a new ConfigSpec
            //
            mySpec.Config = new VirtualMachineConfigSpec();
            //
            // Set number of CPU's
            //
            int numCpus = new int();
            numCpus = Convert.ToInt16(cboCpus.SelectedValue);
            mySpec.Config.NumCPUs = numCpus;
            txtResults.Text += "CPU : " + numCpus + "\r\n";
            //
            // Set amount of RAM
            //
            long memoryMb = new long();
            memoryMb = (long)(Convert.ToInt16(cboRam.SelectedValue) * 1024);
            mySpec.Config.MemoryMB = memoryMb;
            txtResults.Text += "Ram : " + memoryMb + "\r\n";
            //
            // Only handle the first network card
            //
            mySpec.Customization.NicSettingMap = new CustomizationAdapterMapping[1];
            mySpec.Customization.NicSettingMap[0] = new CustomizationAdapterMapping();
            //
            // Read in the DNS from web.config and assign
            //
            string[] ipDns = new string[1];
            ipDns[0] = txtDnsServer.Text;
            mySpec.Customization.GlobalIPSettings = new CustomizationGlobalIPSettings();
            mySpec.Customization.GlobalIPSettings.DnsServerList = ipDns;
            txtResults.Text += "DNS : " + ipDns[0] + "\r\n";
            //
            // Create a new networkDevice
            //
            VirtualDevice networkDevice = new VirtualDevice();
            foreach (VirtualDevice vDevice in itmVirtualMachine.Config.Hardware.Device)
            {
                //
                // get nic on vm
                //
                if (vDevice.DeviceInfo.Label.Contains("Network"))
                {
                    networkDevice = vDevice;
                }
            }
            //
            // Create a DeviceSpec
            //
            VirtualDeviceConfigSpec[] devSpec = new VirtualDeviceConfigSpec[0];
            mySpec.Config.DeviceChange = new VirtualDeviceConfigSpec[1];
            mySpec.Config.DeviceChange[0] = new VirtualDeviceConfigSpec();
            mySpec.Config.DeviceChange[0].Operation = VirtualDeviceConfigSpecOperation.edit;
            mySpec.Config.DeviceChange[0].Device = networkDevice;
            //
            // Define network settings for the new vm
            //
            //
            // Assign IP Address
            //
            CustomizationFixedIp ipAddress = new CustomizationFixedIp();
            ipAddress.IpAddress = targetIP;
            mySpec.Customization.NicSettingMap[0].Adapter = new CustomizationIPSettings();
            txtResults.Text += "IP : " + targetIP + "\r\n";
            //
            // Assign subnet mask
            //
            mySpec.Customization.NicSettingMap[0].Adapter.Ip = ipAddress;
            mySpec.Customization.NicSettingMap[0].Adapter.SubnetMask = txtSubnet.Text;
            txtResults.Text += "Subnet : " + txtSubnet.Text + "\r\n";
            //
            // Assign default gateway
            //
            string[] ipGateway = new string[1];
            ipGateway[0] = txtGateway.Text;
            mySpec.Customization.NicSettingMap[0].Adapter.Gateway = ipGateway;
            txtResults.Text += "Gateway : " + txtGateway.Text + "\r\n";
            //
            // Create network backing information
            //
            VirtualEthernetCardDistributedVirtualPortBackingInfo nicBack = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
            nicBack.Port = new DistributedVirtualSwitchPortConnection();
            //
            // Connect to the virtual switch
            //
            VmwareDistributedVirtualSwitch dvSwitch = functions.GetObject<VmwareDistributedVirtualSwitch>(vimClient, itmDvPortGroup.Config.DistributedVirtualSwitch, null);
            //
            // Assign the proper switch port
            //
            nicBack.Port.SwitchUuid = dvSwitch.Uuid;
            //
            // Connect the network card to proper port group
            //
            nicBack.Port.PortgroupKey = itmDvPortGroup.MoRef.Value;
            mySpec.Config.DeviceChange[0].Device.Backing = nicBack;
            //
            // Enable the network card at bootup
            //
            mySpec.Config.DeviceChange[0].Device.Connectable = new VirtualDeviceConnectInfo();
            mySpec.Config.DeviceChange[0].Device.Connectable.StartConnected = true;
            mySpec.Config.DeviceChange[0].Device.Connectable.AllowGuestControl = true;
            mySpec.Config.DeviceChange[0].Device.Connectable.Connected = true;
            //
            // Get the vmfolder from the datacenter
            //
            //
            // Perform the clone
            //
            txtWorking.Text = "Currently cloning " + targetVM + " please wait.";
            Working_Panel.Visible = true;
            Globals.myTask = itmVirtualMachine.CloneVM_Task(itmDatacenter.VmFolder, targetVM, mySpec);
            Task cloneVmTask = new Task(vimClient, Globals.myTask);
            //
            // The following will make the browser appear to hang, I need to hide this panel, and show a working panel
            //
            Globals.myClone = (ManagedObjectReference)vimClient.WaitForTask(cloneVmTask.MoRef);
            ////
            //// Connect to the VM in order to set the custom fields
            //// Custom Fields are only available when connecting to Vsphere, and not to an individual esxi host
            ////
            //filter.Add("name", targetVM);
            //VirtualMachine clonedVM = functions.GetEntity<VirtualMachine>(vimClient, null, filter, null);
            //filter.Remove("name");
            ////
            //// We need to get a list of the Custom Fields from vsphere, that information is stored in ServiceContent.CustomFieldsManager
            ////
            //CustomFieldsManager fieldManager = functions.GetObject<CustomFieldsManager>(vimClient, vimClient.ServiceContent.CustomFieldsManager, null);
            ////
            //// One or more custom field names could be stored in the web.config and processed in some fashion
            ////
            //foreach (CustomFieldDef thisField in fieldManager.Field)
            //{
            //    //
            //    // These fields exist in my test environment, you will need to use your own inside the quotes
            //    //
            //    if (thisField.Name.Equals("CreatedBy"))
            //    {
            //        fieldManager.SetField(clonedVM.MoRef, thisField.Key, txtUsername.Text);
            //    }
            //    if (thisField.Name.Equals("CreatedOn"))
            //    {
            //        fieldManager.SetField(clonedVM.MoRef, thisField.Key, System.DateTime.Now.ToString());
            //    }
            //}
            vimClient.Disconnect();
            //
            // Hide the vm controls and show the result box
            //
            Working_Panel.Visible = false;
            Vm_Panel.Visible = false;
            Results_Panel.Visible = true;
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            //
            // Hide the reults and show the login panel, clear all form items
            //
            Results_Panel.Visible = false;
            Login_Panel.Visible = true;
        }
        protected void cboClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            // The selected cluster changed so we need to update the following items
            // - Datastore
            // - Portgroups
            // - Resource Pools
            //
            //
            // Establish a connection with the Vmware server
            //
            VimClient vimClient = functions.ConnectServer(Globals.sViServer, Globals.sUsername, Globals.sPassword);
            //
            // Clear out existing entries
            //
            cboDatastores.Items.Clear();
            cboPortGroups.Items.Clear();
            cboResourcePools.Items.Clear();
            //
            // Need to get at the Datacenter for the selected cluster
            //
            NameValueCollection filter = new NameValueCollection();
            filter.Add("name", cboClusters.SelectedItem.Text);
            ClusterComputeResource itmCluster = functions.GetEntity<ClusterComputeResource>(vimClient, null, filter, null);
            filter.Remove("name");
            filter.Add("hostFolder", itmCluster.Parent.Value);
            Datacenter itmDatacenter = functions.GetEntity<Datacenter>(vimClient, null, filter, null);
            filter.Remove("hostFolder");
            //
            // Update datastore list
            //
            //
            // Create a list of Datastores to populate later
            //
            List<Datastore> lstDatastores = new List<Datastore>();
            //
            // The cluster object already contains a list of datastore morefs
            // grab this list of morefs and use getview to grab the object
            // and add it to the datastore list.
            //
            foreach (ManagedObjectReference itmDs in itmCluster.Datastore)
            {
                ViewBase thisDsView = vimClient.GetView(itmDs, null);
                Datastore thisDatastore = (Datastore)thisDsView;
                lstDatastores.Add(thisDatastore);
            }
            //
            // Sort the list by the freespace property in descending order
            //
            lstDatastores = lstDatastores.OrderByDescending(thisStore => thisStore.Info.FreeSpace).ToList();
            foreach (Datastore itmDatastore in lstDatastores)
            {
                ListItem thisDatastore = new ListItem();
                thisDatastore.Text = itmDatastore.Name;
                thisDatastore.Value = itmDatastore.MoRef.ToString();
                cboDatastores.Items.Add(thisDatastore);
            }
            //
            // Update list of network portgroups
            //
            filter.Add("name", WebConfigurationManager.AppSettings["portPrefix"].ToString());
            List<DistributedVirtualPortgroup> lstDVPortGroups = functions.GetEntities<DistributedVirtualPortgroup>(vimClient, itmDatacenter.MoRef, filter, null);
            filter.Remove("name");
            //
            //List<DistributedVirtualPortgroup> lstDVPortGroups = functions.GetEntities<DistributedVirtualPortgroup>(vimClient, itmDatacenter.MoRef, null, null);
            if (lstDVPortGroups != null)
            {
                foreach (DistributedVirtualPortgroup itmPortGroup in lstDVPortGroups)
                {
                    ListItem thisPortGroup = new ListItem();
                    thisPortGroup.Text = itmPortGroup.Name;
                    thisPortGroup.Value = itmPortGroup.MoRef.ToString();
                    cboPortGroups.Items.Add(thisPortGroup);
                }
            }
            //
            // Get a list of Resource Pools
            //
            //
            // On the cluster change event, globals.mycluster should already be populated.
            //
            if (Globals.myCluster == null)
            {
                //
                // This code should never run
                //
                Globals.myCluster = new ManagedObjectReference(cboClusters.SelectedItem.Value);
            }
            filter.Add("parent", Globals.myCluster.Value);
            List<ResourcePool> lstResPools = functions.GetEntities<ResourcePool>(vimClient, null, filter, null);
            filter.Remove("parent");
            if (lstResPools != null)
            {
                foreach (ResourcePool itmResPool in lstResPools)
                {
                    ListItem thisResPool = new ListItem();
                    thisResPool.Text = itmResPool.Name;
                    thisResPool.Value = itmResPool.MoRef.ToString();
                    cboResourcePools.Items.Add(thisResPool);
                }
            }
            //
            // Close the session with the server
            //
            vimClient.Disconnect();
        }
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            txtErrors.Text = "";
            Error_Panel.Visible = false;
        }
    }
}