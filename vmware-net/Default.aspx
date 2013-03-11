<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vmware_net.Default" MasterPageFile="vmware-net.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="blueprint_div" class="container showgrid" runat="server">
        <asp:Panel ID="Login_Panel" runat="server">
            <div id="login_panel_div">
                <div id="lp_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblLoginPanelTitle" runat="server" Text="Enter Credentials" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="lp_spacer-left" class="span-6">&nbsp;</div>
                <div id="lp_label" class="span-4">
                    <!--
                    Remove the controls between these two commment blocks 
                    to have the server autoloaded from the web.config
                    -->
                        <asp:Label ID="lblSdkServer" runat="server" Text="SDK Server"></asp:Label><br />
                    <!--
                    End Here
                    -->
                        <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label><br />
                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label><br />
                        &nbsp;<br />
                </div>
                <div id="lp_control" class="span-8">
                        <asp:TextBox ID="txtSdkServer" runat="server"></asp:TextBox><br />
                        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox><br />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
                        <asp:Button ID="cmdConnect" runat="server" Text="Connect" OnClick="cmdConnect_Click" />
                </div>
                <div id="lp_spacer-right" class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Vm_Panel" runat="server" Visible="False">
            <div id="vm_panel_div">
                <div id="vm_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblVmPanelTitle" runat="server" Text="Provisioning Information" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="vm_spacer-left" class="span-6">&nbsp;</div>
                <div id="vm_label" class="span-4">
                    <asp:Label ID="lblSourceVm" runat="server" Text="Source VM"></asp:Label><br />
                    <asp:Label ID="lblTargetVm" runat="server" Text="Target VM"></asp:Label><br />
                    <asp:Label ID="lblOsCustomization" runat="server" Text=" OS Customization"></asp:Label><br />
                    <asp:Label ID="lblCluster" runat="server" Text="Cluster"></asp:Label><br />
                    <asp:Label ID="lblDatastore" runat="server" Text="Datastore"></asp:Label><br />
                    <asp:Label ID="lblPortGroups" runat="server" Text="Port Groups"></asp:Label><br />
                    <asp:Label ID="lblResourcePool" runat="server" Text="Resource Pool"></asp:Label><br />
                    <asp:Label ID="lblCpu" runat="server" Text="CPU"></asp:Label><br />
                    <asp:Label ID="lblRam" runat="server" Text="Ram"></asp:Label><br />
                    <asp:Label ID="lblIpAddress" runat="server" Text="Ip Address"></asp:Label><br />
                    <asp:Label ID="lblSubnet" runat="server" Text="Subnet"></asp:Label><br />
                    <asp:Label ID="lblGateway" runat="server" Text="Gateway"></asp:Label><br />
                    &nbsp;               
                </div>
                <div id="vm_control" class="span-8">
                    <asp:DropDownList ID="cboSourceVms" runat="server"></asp:DropDownList><br />
                    <asp:TextBox ID="txtTargetVm" runat="server"></asp:TextBox><br />
                    <asp:DropDownList ID="cboCustomizations" runat="server"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboClusters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboClusters_SelectedIndexChanged"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboDatastores" runat="server"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboPortGroups" runat="server"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboResourcePools" runat="server"></asp:DropDownList><br />
                    <asp:DropDownList ID="cboCpus" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:DropDownList ID="cboRam" runat="server">
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:TextBox ID="txtIpAddress" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtSubnet" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtGateway" runat="server"></asp:TextBox><br />
                    <asp:Button ID="cmdProvision" runat="server" Text="Provision" OnClick="cmdProvision_Click" />
                </div>
                <div id="vm_spacer-right" class="span-6 last">&nbsp;</div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Results_Panel" runat="server" Visible="False">
            <div id="results_panel_div" style="border: 1px solid">
                <div id="rp_Panel_Label" class="span-24 last" style="text-align: center">
                    <asp:Label ID="lblResultsLabel" runat="server" Text="Results" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="rp_control" class="span-24 last">
                    <asp:TextBox ID="txtResults" runat="server" TextMode="MultiLine" Width="100%" ReadOnly="True"></asp:TextBox><br />
                    <asp:Button ID="cmdClose" runat="server" Text="Close" OnClick="cmdClose_Click" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Error_Panel" runat="server" Visible="False" Height="480px" Width="640px">
            <div id="error_panel_div" style="height: 480px; width: 640px; border: 1px solid">
                <div id="ep_Panel_Label" class="span-24 last border" style="text-align: center">
                    <asp:Label ID="lblErrorPanel" runat="server" Text="Error Messages" Font-Bold="True"></asp:Label>
                    <br />
                </div>
                <div id="ep_control" class="span-24 last">
                    <asp:TextBox ID="txtErrors" runat="server" Height="60%" Width="640px" TextMode="MultiLine" ForeColor="Red" Font-Bold="True" BorderStyle="None" ReadOnly="True"></asp:TextBox><br />
                    <asp:Button ID="cmdReturn" runat="server" Text="Return" OnClick="cmdReturn_Click" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
